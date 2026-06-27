using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

#if UNITY_EDITOR
using Object = UnityEngine.Object;
using UnityEditor;
#endif

namespace SF.DataModule
{
    [CreateAssetMenu(fileName = nameof(DatabaseRegistry), menuName = "SF/Data/Database Registry")]
    public partial class DatabaseRegistry : ScriptableObject
    {
        /// <summary>
        /// A list of databases needing to be preloaded when the runtime player first starts up.
        /// Any database set in here will have the 
        /// </summary>
        public List<SFDatabase> PreloadedDatabase = new List<SFDatabase>();
        [NonSerialized] public Dictionary<Type, SFDatabase> RegisteredDatabases = new();

        private static DatabaseRegistry _registry;

        public static DatabaseRegistry Registry
        {
            get => _registry;
            private set => _registry = value;
        }

        private void Awake()
        {
            // If this is the first time we created a registry set it as the default to prevent null values.
            if (_registry != null)
                return;

            _registry = this;
        }

        private void OnEnable()
        {
            if(_registry == null)
                _registry = this;
            
            List<SFDatabase> nullSetDatabases = new List<SFDatabase>();
            for (int i = 0; i < PreloadedDatabase.Count; i++)
            {
                if (PreloadedDatabase[i] == null)
                {
                    /* If for some reason a database was originally added to the Database Registry scriptable object
                     * than that database object was deleted from the project it would leave a null value that can
                     * cause a null reference error in the RegisterDatabase method call.*/
                    nullSetDatabases.Add(PreloadedDatabase[i]);
                    continue;
                }

                RegisterDatabase(PreloadedDatabase[i]);
            }
            
            if(nullSetDatabases.Count < 1)
                return;

            // Remove any previously found databases that were set to null.
            foreach (var database in nullSetDatabases)
            {
                PreloadedDatabase.Remove(database);
            }
        }
        
        private void OnDestroy()
        {
            for (int i = 0; i < PreloadedDatabase.Count; i++)
            {
                DeregisterDatabase(PreloadedDatabase[i]);
            }
        }
        
        public static bool Contains(Type databaseType)
        {
            return _registry.RegisteredDatabases.ContainsKey(databaseType);
        }

        public static TDatabase GetDatabase<TDatabase>() where TDatabase : SFDatabase
        {
            if (_registry == null)
                return null;
            
            _registry.RegisteredDatabases.TryGetValue(typeof(TDatabase), out var database);
            return (TDatabase)database;
        }
        
        public static bool TryGetDatabase<TDatabase>(out TDatabase foundDatabase) where TDatabase : SFDatabase
        {
            foundDatabase = null;
            
            if (_registry == null)
                return false;

            if (!_registry.RegisteredDatabases.TryGetValue(typeof(TDatabase), out var database)) 
                return false;
            
            foundDatabase = (TDatabase)database;
            return true;
        }
        
        public static bool TryGetDatabase<TDatabase>(Type databaseType, out TDatabase foundDatabase) where TDatabase : SFDatabase
        {
            foundDatabase = null;
            
            if (_registry == null)
                return false;

            if (!_registry.RegisteredDatabases.TryGetValue(databaseType, out var database)) 
                return false;
            
            foundDatabase = (TDatabase)database;
            return true;
        }


        public static void RegisterDatabase<TDatabase>(TDatabase database) where TDatabase : SFDatabase
        {
            if (_registry == null || database == null)
                return;
            
            if (_registry.RegisteredDatabases.TryAdd(database.GetType(),database))
            { 
                database.OnRegisterDatabase();
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"When registering a database of type: {database.GetType()}, there was one already registered. Only one of each type can be registered at once.");
#endif
            }
        }
        
        public static void DeregisterDatabase<TDatabase>(TDatabase database) where TDatabase : SFDatabase
        {
            if (_registry == null || database == null)
                return;
            
            if (_registry.RegisteredDatabases.Remove(typeof(TDatabase)))
            {
                database.OnDeregisterDatabase();
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"When unregistering a database of type: {typeof(TDatabase)}, there was no registered database of that type.");
#endif
            }
        }
        
        
#if UNITY_EDITOR
        [AutoStaticsCleanup] private static string _cachedPath;
    
        /// <summary>
        /// Returns the first asset found using the search glob filter if any is passed in. If no filter string is passed in it will just find the first type without worrying about any filters being applied.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchGlobFilter"></param>
        /// <returns></returns>
        public static T FindFirstAssetOfType<T>(string searchGlobFilter = "") where T : Object
        {
            string[] guids = AssetDatabase.FindAssets($"{searchGlobFilter} t:{typeof(T).Name}");
            
            if (guids == null || guids.Length < 1 || string.IsNullOrEmpty(guids[0]))
                return null;
            
            _cachedPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<T>(_cachedPath);
        }
    
        /// <summary>
        /// Tries and set the <see cref="_registry"/> instance after a code reload from Unity where the managed objects have already finished being restored.
        /// if one is not already been set. If one is set we make sure to add it to the PlayerSettings.SetPreloadedAssets array to make sure the databases all are initialized before the first frame in scenes.
        /// </summary>
        [OnCodeInitializing]
        static void InitializeDatabaseRegistry()
        {
            _registry ??= FindFirstAssetOfType<DatabaseRegistry>();

            // TODO: Add example of how to auto create a folder and make a Database Registry in there if one was not already made.
            if (_registry == null)
                return;
            
            PreloadDatabases_Internal();
        }
        
        /// <summary>
        /// Preloads all <see cref="SFDatabase{TDTOBase}"/> set inside of the <see cref="DatabaseRegistry"/> to make sure they are ready before the first frame of the game.
        /// </summary>
        private static void PreloadDatabases_Internal()
        {
            if (_registry == null)
            {
                Debug.Log("There was not DatabaseRegistry set as the active registry.");
                return;
            }

            // Add the config asset to the build
            var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
            
            // Don't set it if it already is in the PreloadedAssets list.
            if (preloadedAssets.Contains(_registry))
                return;
            
            preloadedAssets.Add(_registry);
            UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }
        
        // Context menu needs a public method so we just use it to call into the internal static method.
        [ContextMenu("Register Preloaded Databases")]
        public void PreloadDatabases()
        {
            PreloadDatabases_Internal();
        }
#endif
    }
}