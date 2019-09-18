using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PSG.OrderImportPane.ClientTool
{
    /// <summary>
    /// Class that manages the execution of tasks sometime in the future.
    /// </summary>
    public static class At
    {
        #region Members

        /// <summary>
        /// Specifies the method that will be fired to execute the delayed anonymous method.
        /// </summary>
        private readonly static TimerCallback timer = new TimerCallback(At.ExecuteDelayedAction);

        #endregion Members

        #region Methods

        /// <summary>
        /// Method that executes an anonymous method after a delay period.
        /// </summary>
        /// <param name="action">The anonymous method that needs to be executed.</param>
        /// <param name="delay">The period of delay to wait before executing.</param>
        /// <param name="interval">The period (in milliseconds) to delay before executing the anonymous method again (Timeout.Infinite to disable).</param>
        public static void Do(Action action, TimeSpan delay, int interval = Timeout.Infinite)
        {
            // create a new thread timer to execute the method after the delay
            new Timer(timer, action, Convert.ToInt32(delay.TotalMilliseconds), interval);

            return;
        }

        /// <summary>
        /// Method that executes an anonymous method after a delay period.
        /// </summary>
        /// <param name="action">The anonymous method that needs to be executed.</param>
        /// <param name="delay">The period of delay (in milliseconds) to wait before executing.</param>
        /// <param name="interval">The period (in milliseconds) to delay before executing the anonymous method again (Timeout.Infinite to disable).</param>
        public static void Do(Action action, int delay, int interval = Timeout.Infinite)
        {
            Do(action, TimeSpan.FromMilliseconds(delay), interval);

            return;
        }

        /// <summary>
        /// Method that executes an anonymous method after a delay period.
        /// </summary>
        /// <param name="action">The anonymous method that needs to be executed.</param>
        /// <param name="dueTime">The due time when this method needs to be executed.</param>
        /// <param name="interval">The period (in milliseconds) to delay before executing the anonymous method again (Timeout.Infinite to disable).</param>
        public static void Do(Action action, DateTime dueTime, int interval = Timeout.Infinite)
        {
            if (dueTime < DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("dueTime", "The specified due time has already elapsed.");
            }

            Do(action, dueTime - DateTime.Now, interval);

            return;
        }

        /// <summary>
        /// Method that executes a delayed action after a specific interval.
        /// </summary>
        /// <param name="o">The Action delegate that is to be executed.</param>
        /// <remarks>This method is invoked on its own thread.</remarks>
        private static void ExecuteDelayedAction(object o)
        {
            // invoke the anonymous method
            (o as Action).Invoke();

            return;
        }

        #endregion Methods
    }
}