using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Arcolabs.Home { 
    public class QualityReduceByMemory : MonoBehaviour
    {
        AsyncOperation ao;
    
        private int memorySizeToReduceQuality = 1536;
        private int memorySizeMinimumQuality = 1024;

        void Start()
        {
            //Debug.Log("DEBUG_UTC - QUALITY BEFORE: " + QualitySettings.GetQualityLevel());
            MemoryCheck();
            //Debug.Log("DEBUG_UTC - QUALITY AFTER: " + QualitySettings.GetQualityLevel());
            LoadingScript.lastScene = ConstantClass.NONE;
            StartCoroutine(LoadLevel(ConstantClass.LOADING));
        }

        void PlayScene()
        {
            ao.allowSceneActivation = true;
        }

        private void MemoryCheck()
        {
            var memorySize = SystemInfo.systemMemorySize;
            //Debug.Log("DEBUG_UTC - MEMORY SIZE: " + memorySize);

            if (memorySize < memorySizeToReduceQuality)
            {

                if (memorySize <= memorySizeMinimumQuality)
                {
                    QualitySettings.SetQualityLevel(2);
                }
                else
                {
                    QualitySettings.SetQualityLevel(0);
                }

            }
            else
            {
                QualitySettings.SetQualityLevel(1);
            }

        }


        public IEnumerator LoadLevel(string nameScene)
        {
            ao = SceneManager.LoadSceneAsync(nameScene);
            ao.allowSceneActivation = false;
            yield return new WaitForEndOfFrame();
        }
    }
}
