/**
 *     Copyright 2015-2016 GetSocial B.V.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    internal class MainThreadExecutor : MonoBehaviour
    {
        private static MainThreadExecutor instance;
        private static System.Object initLock = new System.Object();

        private System.Object queueLock = new System.Object();
        private List<Action> queuedActions = new List<Action>();
        private List<Action> executingActions = new List<Action>();

        internal static void Init()
        {
            lock(initLock)
            {
                if(instance == null)
                {
                    var instances = FindObjectsOfType<MainThreadExecutor>();

                    if(instances.Length > 1)
                    {
                        Debug.LogError("[MainThreadExecutor] Something went really wrong " +
                            " - there should never be more than 1 MainThreadExecutor!" +
                            " Reopening the scene might fix it.");
                    }
                    else if(instances.Length == 0)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<MainThreadExecutor>();
                        singleton.name = "[singleton] " + typeof(MainThreadExecutor);

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(MainThreadExecutor) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        instance = instances[0];
                        Debug.Log("[Singleton] Using instance already created: " + instance.gameObject.name);
                    }
                }
            }
        }

        private MainThreadExecutor()
        {
        }

        internal static void Queue(Action action)
        {
            if(action == null)
            {
                Debug.LogWarning("Trying to queue null action");
                return;
            }

            lock(instance.queueLock)
            {
                instance.queuedActions.Add(action);
            }
        }

        void Update()
        {
            MoveQueuedActionsToExecuting();

            while(executingActions.Count > 0)
            {
                Action action = executingActions[0];
                executingActions.RemoveAt(0);
                action();
            }
        }

        private void MoveQueuedActionsToExecuting()
        {
            lock(queueLock)
            {
                while(queuedActions.Count > 0)
                {
                    Action action = queuedActions[0];
                    executingActions.Add(action);
                    queuedActions.RemoveAt(0);
                }
            }
        }
    }
}
