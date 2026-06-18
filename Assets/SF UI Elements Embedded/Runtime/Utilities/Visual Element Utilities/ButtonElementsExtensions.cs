using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SF.UIModule
{
    /// <summary>
    /// Set of extension methods for Unity's built in <see cref="Button"/>
    /// </summary>
    public static class ButtonElementsExtensions
    {
   
#region Event Registering Extensions

        /// <summary>
        /// Add a <see cref="Action"/> to the target <see cref="Button"/>.
        /// Even if the target is null it returns the null Button reference to allow continuing a set of calls in a method chain.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="callback"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T OnClick<T>(this T target, Action callback)
            where T : Button
        {
            if (target is null)
                Debug.LogWarning("When trying to register a Button OnClick event, the target Button element was null.");
            
            target?.RegisterCallback<MouseUpEvent>((evt) => callback());
            return target;
        }

#endregion
       
    }
}
