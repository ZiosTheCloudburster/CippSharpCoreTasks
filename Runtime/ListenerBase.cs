using System;

namespace CippSharp.Core.Tasks
{
    /// <summary>
    /// Abstract class for 'Listener'
    /// </summary>
    public abstract class ListenerBase
    {
        public event Action<object> onVariableChanged = null;
        public event Action onListeningEnd = null;
        public event Action onDestroy = null;

        protected virtual void InvokeOnVariableChanged(object parameter)
        {
            onVariableChanged?.Invoke(parameter);
        }

        protected virtual void InvokeOnListeningEnd()
        {
            onListeningEnd?.Invoke();
        }

        ~ListenerBase()
        {
            onDestroy?.Invoke();
        }
    }
}
