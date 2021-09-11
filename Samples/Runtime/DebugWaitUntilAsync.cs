using UnityEngine;

namespace CippSharp.Core.Tasks.Samples
{
    public class DebugWaitUntilAsync : MonoBehaviour
    {
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
            Debug.Log("Wait Until → Condition was changed to true.", this);
        }
        
        private void WheneverConditionIsChanged(object o)
        {
            Debug.Log($"Listener → Condition was changed to {((bool)o).ToString()}.", this);
        }
    }
}
