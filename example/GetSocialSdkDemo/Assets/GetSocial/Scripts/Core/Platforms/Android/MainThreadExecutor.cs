#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    class MainThreadExecutor : MonoBehaviour
    {
        static MainThreadExecutor _instance;
        static System.Object _initLock = new System.Object();

        readonly System.Object _queueLock = new System.Object();
        readonly List<Action> _queuedActions = new List<Action>();
        readonly List<Action> _executingActions = new List<Action>();

        internal static void Init()
        {
            lock (_initLock)
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<MainThreadExecutor>();

                    if (instances.Length > 1)
                    {
                        Debug.LogError("[MainThreadExecutor] Something went really wrong " +
                                       " - there should never be more than 1 MainThreadExecutor!" +
                                       " Reopening the scene might fix it.");
                    }
                    else if (instances.Length == 0)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<MainThreadExecutor>();
                        singleton.name = "[singleton] " + typeof(MainThreadExecutor);

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(MainThreadExecutor) +
                                  " is needed in the scene, so '" + singleton +
                                  "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        _instance = instances[0];
                        Debug.Log("[Singleton] Using instance already created: " + _instance.gameObject.name);
                    }
                }
            }
        }

        MainThreadExecutor()
        {
        }

        internal static void Queue(Action action)
        {
            if (action == null)
            {
                Debug.LogWarning("Trying to queue null action");
                return;
            }

            lock (_instance._queueLock)
            {
                _instance._queuedActions.Add(action);
            }
        }

        void Update()
        {
            MoveQueuedActionsToExecuting();

            while (_executingActions.Count > 0)
            {
                Action action = _executingActions[0];
                _executingActions.RemoveAt(0);
                action();
            }
        }

        void MoveQueuedActionsToExecuting()
        {
            lock (_queueLock)
            {
                while (_queuedActions.Count > 0)
                {
                    Action action = _queuedActions[0];
                    _executingActions.Add(action);
                    _queuedActions.RemoveAt(0);
                }
            }
        }
    }
}
#endif