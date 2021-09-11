using System;
using System.Threading.Tasks;

namespace CippSharp.Core.Tasks
{
    /// <summary>
    /// Tasks Utils
    /// </summary>
    public static class TaskUtils
    {
        /// <summary>
        /// One second in milliseconds
        /// </summary>
        public const int oneSecond = 1000;

        #region Wait (Time)
        
        /// <summary>
        /// Action to invoke something after X milliseconds
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="milliseconds"></param>
        public static void Wait(Action callback, int milliseconds = oneSecond)
        {
            WaitAsync(callback, milliseconds);
        }

        /// <summary>
        /// Async Action to invoke something after X milliseconds
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="milliseconds"></param>
        private static async void WaitAsync(Action callback, int milliseconds)
        {
            await Task.Delay(milliseconds);
            callback.Invoke();
        }

        #endregion

        #region Wait Until
        
        /// <summary>
        /// Wait Until for code 
        /// </summary>
        /// <param name="context">predicate context</param>
        /// <param name="predicate">condition that needs to be verified</param>
        /// <param name="callback">invoked when predicate is verified</param>
        /// <param name="milliseconds">check predicate every X milliseconds</param>
        /// <typeparam name="T"></typeparam>
        public static void WaitUntil<T>(T context, Predicate<T> predicate, Action callback, int milliseconds = oneSecond)
        {
            WaitUntilAsync(context, predicate, callback, milliseconds);
        }

        /// <summary>
        /// Async Wait Until for code 
        /// </summary>
        /// <param name="context">predicate context</param>
        /// <param name="predicate">condition that needs to be verified</param>
        /// <param name="callback">invoked when predicate is verified</param>
        /// <param name="milliseconds">check predicate every X milliseconds</param>
        /// <typeparam name="T"></typeparam>
        private static async void WaitUntilAsync<T>(T context, Predicate<T> predicate, Action callback, int milliseconds)
        {
            while (!predicate.Invoke(context))
            {
                await Task.Delay(milliseconds);
            }
            
            callback.Invoke();
        }

        #endregion
    }
}
