using System;
using System.Threading.Tasks;

namespace CippSharp.Core.Tasks
{
    /// <summary>
    /// Listener is a 'tracking' class
    /// Use this to track a variable whenever it changes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="F"></typeparam>
    public class Listener<T, F> : ListenerBase
    {
        /// <summary>
        /// Cached Context
        /// </summary>
        private T context = default(T);

        /// <summary>
        /// Cached Variable
        /// </summary>
        private F variable = default(F);

        /// <summary>
        /// Milliseconds to check variable every X of them
        /// </summary>
        private int ticks;

        /// <summary>
        /// Selector
        /// </summary>
        private Func<T, F> selector = null;

        /// <summary>
        /// I can listen while this is true! Null means true!
        /// </summary>
        private Predicate<T> predicate = null;

        //Runtime:
        private bool stopped = true;
        private bool paused = false;

        #region Constructors
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">not null</param>
        /// <param name="selector">not null</param>
        /// <param name="ticks">milliseconds</param>
        public Listener(T context, Func<T, F> selector, int ticks = TaskUtils.oneSecond)
        {
            this.context = context;
            this.variable = selector.Invoke(context);
            this.selector = selector;
            this.ticks = ticks;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">not null</param>
        /// <param name="predicate">can be null</param>
        /// <param name="selector">not null</param>
        /// <param name="ticks"></param>
        public Listener(T context, Predicate<T> predicate, Func<T, F> selector, int ticks = TaskUtils.oneSecond) : this(context, selector, ticks)
        {
            this.predicate = predicate;
        }
        
        #endregion

        /// <summary>
        /// Starts the listening
        /// </summary>
        public void Start()
        {
            stopped = false;
            Pause(false);
            ListeningAsync();
            
            this.onDestroy -= Stop;
            this.onDestroy += Stop;
        }

        private async void ListeningAsync()
        {
            //while until context is != null, if this has predicate predicate must be verified
            while (context != null && ((predicate != null && predicate.Invoke(context)) || predicate == null))
            {
                //wait that ticks
                await Task.Delay(ticks);
                //if paused continues to wait
                if (paused)
                {
                    continue;
                }

                //if stopped, exit!
                if (stopped)
                {
                    break;
                }

                //if context is null, exit!
                if (context == null)
                {
                    break;
                }

                F newVariableValue = selector.Invoke(context);
                if (!Equals(variable, newVariableValue))
                {
                    variable = newVariableValue;
                    InvokeOnVariableChanged(variable);
                }
            }

            InvokeOnListeningEnd();
        }

        /// <summary>
        /// Pause the listening
        /// </summary>
        /// <param name="value"></param>
        public void Pause(bool value)
        {
            paused = value;
        }

        /// <summary>
        /// Stops the listening
        /// </summary>
        public void Stop()
        {
            stopped = true;
        }
    }

}