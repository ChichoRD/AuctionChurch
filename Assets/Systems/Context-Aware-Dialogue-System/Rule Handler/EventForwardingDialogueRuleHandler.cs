using ContextualDialogueSystem.Rule;
using ContextualDialogueSystem.Rule.Criteria;
using RequireAttributes;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace ContextualDialogueSystem.RuleHandler
{
    //[Obsolete]
    internal class EventForwardingDialogueRuleHandler : MonoBehaviour, IDialogueRuleHandler<object>
    {
        [RequireInterface(typeof(IDialogueRuleHandler))]
        [SerializeField]
        private Object _eventDialogueHandlerObject;
        private IDialogueRuleHandler _eventDialogueHandler;

        [field: SerializeField]
        public UnityEvent<IDialogueRule<object, ICriteria>> DialogueHandlingStarted { get; private set; }

        [field: SerializeField]
        public UnityEvent<IDialogueRule<object, ICriteria>> DialogueHandlingFinished { get; private set; }

        private void Awake()
        {
            _eventDialogueHandler = _eventDialogueHandlerObject as IDialogueRuleHandler;
        }

        public async Task<bool> HandleRule(IDialogueRule<object, ICriteria> dialogueRule)
        {
            DialogueHandlingStarted?.Invoke(dialogueRule);
            await _eventDialogueHandler.HandleRule(dialogueRule);
            DialogueHandlingFinished?.Invoke(dialogueRule);
            return true;
        }
    }
}