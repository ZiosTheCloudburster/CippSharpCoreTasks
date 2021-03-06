#if UNITY_EDITOR
using UnityEngine;

namespace CippSharp.Core.Tasks.Samples
{
    internal class DebugWaitUntilAsync : MonoBehaviour
    {
#pragma warning disable 414
        [SerializeField] private string tooltip = "Change condition at runtime to see what happens.";
#pragma warning restore 414
        [SerializeField] private bool condition = false;

        private Listener<DebugWaitUntilAsync, bool> listener = null;
        
        private void Start()
        {
            TaskUtils.WaitUntil(this, (i) => i.condition, OnConditionChangedToTrue);
            
            listener = new Listener<DebugWaitUntilAsync, bool>(this, (i) => Application.isPlaying, (i) => i.condition);
            listener.onVariableChanged += WheneverConditionIsChanged;
            listener.Start();
        }
        
        private void OnConditionChangedToTrue()
        {
            Debug.Log($"[{gameObject.name}]: Wait Until → Condition was changed to true.", this);
        }
        
        private void WheneverConditionIsChanged(object o)
        {
            Debug.Log($"[{gameObject.name}/Listener]: → Condition was changed to {((bool)o).ToString()}.", this);
        }
    }
}
#endif
