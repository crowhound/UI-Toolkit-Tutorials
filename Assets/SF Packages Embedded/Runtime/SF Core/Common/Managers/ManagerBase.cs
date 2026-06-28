using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace SF
{
    /// <summary>
    /// Base class for manager types in scene that don't have their static instance cleaned up between
    /// entering and exiting playmode.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ManagerBase<T> : MonoBehaviour
    {
        [NoAutoStaticsCleanup]
        protected static T _instance;

        public static T  Manager
        {
            get => _instance;
            set => _instance = value;
        }
        
    }
    
    /// <summary>
    /// Base class for manager types in scene that cleans their static instance between entering and exiting playmode.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class ManagerBaseStaticCleanUp<T> : MonoBehaviour where T : Object
    {
        [AutoStaticsCleanup]
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindAnyObjectByType<T>();
                
                return _instance;
            } 
            set => _instance = value;
        }
    }
}
