using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Arcolabs.Home { 
    public class LoadingScript : MonoBehaviour
    {
        AsyncOperation ao;
        public static string lastScene;
        public static string nextScene;
        private bool isLoading;

        private void Start()
        {
            isLoading = false;
            if (lastScene==null)
            {
                lastScene = ConstantClass.NONE;
            }        
            if (lastScene == ConstantClass.NONE && HomeIlhasHelper.firstAcess)
            {
                nextScene = ConstantClass.LOGIN;
            }
            lastScene = nextScene;
        }

        private void Update()
        {
            if (ao != null)
            {
                if (ao.progress >= 0.9f)
                {
                    LoadFinish();
                }
                return;
            }
            return;
        }

        public void LoadFinish()
        {
            if (ao != null)
            {
                ao.allowSceneActivation = true;
            }
        }

        public void StartLoading()
        {
            if (!isLoading)
            {
                isLoading = true;
                ao = SceneManager.LoadSceneAsync(nextScene);
                ao.allowSceneActivation = false;
            }
            return;
        }

    }
}
