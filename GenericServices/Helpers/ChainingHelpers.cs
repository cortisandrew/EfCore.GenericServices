using System;
using System.Collections.Generic;
using System.Text;

namespace GenericServices.Helpers
{
    /// <summary>
    /// Extension methods to support chaining
    /// </summary>
    public static class ChainingHelpers
    {
        /// <summary>
        /// Chain an action only when a condition is true
        /// </summary>
        /// <typeparam name="T">Type parameter for the extension method</typeparam>
        /// <param name="obj">The instance that will be used for chaining</param>
        /// <param name="condition">The condition whether to call the action or otherwise</param>
        /// <param name="action">The action to be called when the condition is true</param>
        /// <returns>The instance for which the action may be called</returns>
        public static T When<T>(this T obj, bool condition, Func<T, T> action)
        {
            return condition ? action(obj) : obj;
        }
    }
}
