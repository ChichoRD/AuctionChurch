using AgnosticPhysics.Rigidbody;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ModularPlatforming.Movement.Applier.Jump
{
    internal class JumpCancelCompositeMovementApplier : MonoBehaviour, IMovementApplier<Vector3>
    {
        private IMovementApplier<Vector3> _jumpCancelMovementApplier;

        [SerializeField]
        private GameObject _jumpAscentMovementApplierObject;
        private ICancellableMovementApplier _jumpAscentMovementApplier;

        private void Awake()
        {
            _jumpCancelMovementApplier = GetComponents<IMovementApplier<Vector3>>().FirstOrDefault(m => m != (IMovementApplier<Vector3>)this);
            _jumpAscentMovementApplier = _jumpAscentMovementApplierObject.GetComponent<ICancellableMovementApplier>();
        }

        public async Task<bool> TryApply(IReadOnlyRigidbody readOnlyRigidbody, IRigidbody rigidbody, Vector3 input) =>
            _jumpAscentMovementApplier.TryCancel()
            & await _jumpCancelMovementApplier.TryApply(readOnlyRigidbody, rigidbody, input);

    }
}
