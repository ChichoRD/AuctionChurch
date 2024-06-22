using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class RTGraphicRaycaster : GraphicRaycaster
    {
        private Canvas _canvas;
        private Canvas Canvas
        {
            get
            {
                if (_canvas != null)
                    return _canvas;

                _canvas = GetComponent<Canvas>();
                return _canvas;
            }
        }

        [NonSerialized] private List<Graphic> _raycastResults = new List<Graphic>();

        /// <summary>
        /// Perform the raycast against the list of graphics associated with the Canvas.
        /// </summary>
        /// <param name="eventData">Current event data</param>
        /// <param name="resultAppendList">List of hit objects to append new results to.</param>
        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            if (Canvas == null)
                return;

            var canvasGraphics = GraphicRegistry.GetRaycastableGraphicsForCanvas(Canvas);
            if (canvasGraphics == null || canvasGraphics.Count == 0)
                return;

            int displayIndex;
            var currentEventCamera = eventCamera; // Property can call Camera.main, so cache the reference

            if (Canvas.renderMode == RenderMode.ScreenSpaceOverlay || currentEventCamera == null)
                displayIndex = Canvas.targetDisplay;
            else
                displayIndex = currentEventCamera.targetDisplay;

            var eventPosition = MultipleDisplayUtilities.RelativeMouseAtScaled(eventData.position);
            if (eventPosition != Vector3.zero)
            {
                // We support multiple display and display identification based on event position.

                int eventDisplayIndex = (int)eventPosition.z;

                // Discard events that are not part of this display so the user does not interact with multiple displays at once.
                if (eventDisplayIndex != displayIndex)
                    return;
            }
            else
            {
                // The multiple display system is not supported on all platforms, when it is not supported the returned position
                // will be all zeros so when the returned index is 0 we will default to the event data to be safe.
                eventPosition = eventData.position;

#if UNITY_EDITOR
                if (Display.activeEditorGameViewTarget != displayIndex)
                    return;
                eventPosition.z = Display.activeEditorGameViewTarget;
#endif

                // We dont really know in which display the event occured. We will process the event assuming it occured in our display.
            }

            // Convert to view space
            Vector2 pos;
            if (currentEventCamera == null)
            {
                // Multiple display support only when not the main display. For display 0 the reported
                // resolution is always the desktops resolution since its part of the display API,
                // so we use the standard none multiple display method. (case 741751)
                float w = Screen.width;
                float h = Screen.height;
                if (displayIndex > 0 && displayIndex < Display.displays.Length)
                {
                    w = Display.displays[displayIndex].systemWidth;
                    h = Display.displays[displayIndex].systemHeight;
                }
                pos = new Vector2(eventPosition.x / w, eventPosition.y / h);
            }
            else
            {
                pos = currentEventCamera.ScreenToViewportPoint(eventPosition);
                float displayWidth = Screen.width;
                float displayHeight = Screen.height;
                if (displayIndex > 0 && displayIndex < Display.displays.Length)
                {
                    displayWidth = Display.displays[displayIndex].systemWidth;
                    displayHeight = Display.displays[displayIndex].systemHeight;
                }

                if (currentEventCamera.targetTexture != null)
                    pos = new Vector2(pos.x * currentEventCamera.targetTexture.width / displayWidth, pos.y * currentEventCamera.targetTexture.height / displayHeight);

                eventPosition = currentEventCamera.ViewportToScreenPoint(pos);
            }

            // If it's outside the camera's viewport, do nothing
            if (pos.x < 0f || pos.x > 1f || pos.y < 0f || pos.y > 1f)
                return;

            float hitDistance = float.MaxValue;

            Ray ray = new Ray();
            if (currentEventCamera != null)
                ray = currentEventCamera.ViewportPointToRay(pos);

            if (Canvas.renderMode != RenderMode.ScreenSpaceOverlay && blockingObjects != BlockingObjects.None)
            {
                float distanceToClipPlane = 100.0f;

                if (currentEventCamera != null)
                {
                    float projectionDirection = ray.direction.z;
                    distanceToClipPlane = Mathf.Approximately(0.0f, projectionDirection)
                        ? Mathf.Infinity
                        : Mathf.Abs((currentEventCamera.farClipPlane - currentEventCamera.nearClipPlane) / projectionDirection);
                }
#if PACKAGE_PHYSICS
                if (blockingObjects == BlockingObjects.ThreeD || blockingObjects == BlockingObjects.All)
                {
                    if (ReflectionMethodsCache.Singleton.raycast3D != null)
                    {
                        var hits = ReflectionMethodsCache.Singleton.raycast3DAll(ray, distanceToClipPlane, (int)m_BlockingMask);
                        if (hits.Length > 0)
                            hitDistance = hits[0].distance;
                    }
                }
#endif
#if PACKAGE_PHYSICS2D
                if (blockingObjects == BlockingObjects.TwoD || blockingObjects == BlockingObjects.All)
                {
                    if (ReflectionMethodsCache.Singleton.raycast2D != null)
                    {
                        var hits = ReflectionMethodsCache.Singleton.getRayIntersectionAll(ray, distanceToClipPlane, (int)m_BlockingMask);
                        if (hits.Length > 0)
                            hitDistance = hits[0].distance;
                    }
                }
#endif
            }

            _raycastResults.Clear();

            Raycast(Canvas, currentEventCamera, eventPosition, canvasGraphics, _raycastResults);

            int totalCount = _raycastResults.Count;
            for (var index = 0; index < totalCount; index++)
            {
                var go = _raycastResults[index].gameObject;
                bool appendGraphic = true;

                if (ignoreReversedGraphics)
                {
                    if (currentEventCamera == null)
                    {
                        // If we dont have a camera we know that we should always be facing forward
                        var dir = go.transform.rotation * Vector3.forward;
                        appendGraphic = Vector3.Dot(Vector3.forward, dir) > 0;
                    }
                    else
                    {
                        // If we have a camera compare the direction against the cameras forward.
                        var cameraForward = currentEventCamera.transform.rotation * Vector3.forward * currentEventCamera.nearClipPlane;
                        appendGraphic = Vector3.Dot(go.transform.position - currentEventCamera.transform.position - cameraForward, go.transform.forward) >= 0;
                    }
                }

                if (appendGraphic)
                {
                    float distance = 0;
                    Transform trans = go.transform;
                    Vector3 transForward = trans.forward;

                    if (currentEventCamera == null || Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                        distance = 0;
                    else
                    {
                        // http://geomalgorithms.com/a06-_intersect-2.html
                        distance = (Vector3.Dot(transForward, trans.position - ray.origin) / Vector3.Dot(transForward, ray.direction));

                        // Check to see if the go is behind the camera.
                        if (distance < 0)
                            continue;
                    }

                    if (distance >= hitDistance)
                        continue;

                    var castResult = new RaycastResult
                    {
                        gameObject = go,
                        module = this,
                        distance = distance,
                        screenPosition = eventPosition,
                        displayIndex = displayIndex,
                        index = resultAppendList.Count,
                        depth = _raycastResults[index].depth,
                        sortingLayer = Canvas.sortingLayerID,
                        sortingOrder = Canvas.sortingOrder,
                        worldPosition = ray.origin + ray.direction * distance,
                        worldNormal = -transForward
                    };
                    resultAppendList.Add(castResult);
                }
            }

        }

        /// <summary>
        /// Perform a raycast into the screen and collect all graphics underneath it.
        /// </summary>
        [NonSerialized] static readonly List<Graphic> s_sortedGraphics = new List<Graphic>();
        private static void Raycast(Canvas canvas, Camera eventCamera, Vector2 pointerPosition, IList<Graphic> foundGraphics, List<Graphic> results)
        {
            // Necessary for the event system
            int totalCount = foundGraphics.Count;
            for (int i = 0; i < totalCount; ++i)
            {
                Graphic graphic = foundGraphics[i];

                // -1 means it hasn't been processed by the canvas, which means it isn't actually drawn
                if (!graphic.raycastTarget || graphic.canvasRenderer.cull || graphic.depth == -1)
                    continue;

                if (!RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, pointerPosition, eventCamera, graphic.raycastPadding))
                    continue;

                if (eventCamera != null && eventCamera.WorldToScreenPoint(graphic.rectTransform.position).z > eventCamera.farClipPlane)
                    continue;

                if (graphic.Raycast(pointerPosition, eventCamera))
                {
                    s_sortedGraphics.Add(graphic);
                }
            }

            s_sortedGraphics.Sort((g1, g2) => g2.depth.CompareTo(g1.depth));
            totalCount = s_sortedGraphics.Count;
            for (int i = 0; i < totalCount; ++i)
                results.Add(s_sortedGraphics[i]);

            s_sortedGraphics.Clear();
        }

        internal static class MultipleDisplayUtilities
        {
            /// <summary>
            /// Converts the current drag position into a relative position for the display.
            /// </summary>
            /// <param name="eventData"></param>
            /// <param name="position"></param>
            /// <returns>Returns true except when the drag operation is not on the same display as it originated</returns>
            public static bool GetRelativeMousePositionForDrag(PointerEventData eventData, ref Vector2 position)
            {
#if UNITY_EDITOR
                position = eventData.position;
#else
            int pressDisplayIndex = eventData.pointerPressRaycast.displayIndex;
            var relativePosition = RelativeMouseAtScaled(eventData.position);
            int currentDisplayIndex = (int)relativePosition.z;

            // Discard events on a different display.
            if (currentDisplayIndex != pressDisplayIndex)
                return false;

            // If we are not on the main display then we must use the relative position.
            position = pressDisplayIndex != 0 ? (Vector2)relativePosition : eventData.position;
#endif
                return true;
            }

            /// <summary>
            /// A version of Display.RelativeMouseAt that scales the position when the main display has a different rendering resolution to the system resolution.
            /// By default, the mouse position is relative to the main render area, we need to adjust this so it is relative to the system resolution
            /// in order to correctly determine the position on other displays.
            /// </summary>
            /// <returns></returns>
            public static Vector3 RelativeMouseAtScaled(Vector2 position)
            {
#if !UNITY_EDITOR && !UNITY_WSA
            // If the main display is now the same resolution as the system then we need to scale the mouse position. (case 1141732)
            if (Display.main.renderingWidth != Display.main.systemWidth || Display.main.renderingHeight != Display.main.systemHeight)
            {
                // The system will add padding when in full-screen and using a non-native aspect ratio. (case UUM-7893)
                // For example Rendering 1920x1080 with a systeem resolution of 3440x1440 would create black bars on each side that are 330 pixels wide.
                // we need to account for this or it will offset our coordinates when we are not on the main display.
                var systemAspectRatio = Display.main.systemWidth / (float)Display.main.systemHeight;

                var sizePlusPadding = new Vector2(Display.main.renderingWidth, Display.main.renderingHeight);
                var padding = Vector2.zero;
                if (Screen.fullScreen)
                {
                    var aspectRatio = Screen.width / (float)Screen.height;
                    if (Display.main.systemHeight * aspectRatio < Display.main.systemWidth)
                    {
                        // Horizontal padding
                        sizePlusPadding.x = Display.main.renderingHeight * systemAspectRatio;
                        padding.x = (sizePlusPadding.x - Display.main.renderingWidth) * 0.5f;
                    }
                    else
                    {
                        // Vertical padding
                        sizePlusPadding.y = Display.main.renderingWidth / systemAspectRatio;
                        padding.y = (sizePlusPadding.y - Display.main.renderingHeight) * 0.5f;
                    }
                }

                var sizePlusPositivePadding = sizePlusPadding - padding;

                // If we are not inside of the main display then we must adjust the mouse position so it is scaled by
                // the main display and adjusted for any padding that may have been added due to different aspect ratios.
                if (position.y < -padding.y || position.y > sizePlusPositivePadding.y ||
                     position.x < -padding.x || position.x > sizePlusPositivePadding.x)
                {
                    var adjustedPosition = position;

                    if (!Screen.fullScreen)
                    {
                        // When in windowed mode, the window will be centered with the 0,0 coordinate at the top left, we need to adjust so it is relative to the screen instead.
                        adjustedPosition.x -= (Display.main.renderingWidth - Display.main.systemWidth) * 0.5f;
                        adjustedPosition.y -= (Display.main.renderingHeight - Display.main.systemHeight) * 0.5f;
                    }
                    else
                    {
                        // Scale the mouse position to account for the black bars when in a non-native aspect ratio.
                        adjustedPosition += padding;
                        adjustedPosition.x *= Display.main.systemWidth / sizePlusPadding.x;
                        adjustedPosition.y *= Display.main.systemHeight / sizePlusPadding.y;
                    }

                    var relativePos = Display.RelativeMouseAt(adjustedPosition);

                    // If we are not on the main display then return the adjusted position.
                    if (relativePos.z != 0)
                        return relativePos;
                }

                // We are using the main display.
                return new Vector3(position.x, position.y, 0);
            }
#endif
                return Display.RelativeMouseAt(position);
            }
        }
    }
}