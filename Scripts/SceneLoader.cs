using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mkey
{
    public class SceneLoader : MonoBehaviour
    {

        public SimpleSlider simpleSlider;
        public GuiFader_v2 LoadGroup;
        public Action LoadingCallBack;
        public static SceneLoader Instance;
        public bool showLoader;

        #region sprite sequence as loader
        // public Image simpleLoader;
        // public int Samples;
        // public Sprite[] simpleLoaderSprites;
        // public bool showsimpleLoader;
        #endregion sprite sequence as loader

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); }
            else
            {
                Instance = this;
            }
        }

        public void LoadScene(int sceneIndex)
        {
                StartCoroutine(AsyncLoadBeaty(sceneIndex));
            /*
            if (showsimpleLoader)
            {
                StartCoroutine(ShowSimpleLoader());
            }
            */
        }

        public void LoadScene(int sceneIndex, Action LoadingCallBack)
        {
            this.LoadingCallBack = LoadingCallBack;
            StartCoroutine(AsyncLoadBeaty(sceneIndex));
            /*
            if (showsimpleLoader)
            {
                StartCoroutine(ShowSimpleLoader());
            }
            */
        }

        public void LoadScene(string sceneName)
        {
            int scene = SceneManager.GetSceneByName(sceneName).buildIndex;
            StartCoroutine(AsyncLoadBeaty(scene));
            /*
            if (showsimpleLoader)
            {
                StartCoroutine(ShowSimpleLoader());
            }
            */
        }

        public void ReLoadCurrentScene()
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(AsyncLoadBeaty(scene));
        }

        float loadProgress;
        IEnumerator AsyncLoad(int scene)
        {
            float minLoadTime = 2.0f;
            float steps = 100f;
            float stepWait = minLoadTime / steps;
            WaitForSeconds wfsStepWait = new WaitForSeconds(stepWait);
            float loadTime = 0.0f;

            loadProgress = 0;
            if (simpleSlider && showLoader) simpleSlider.value = loadProgress;

            bool fin = false;
            if (LoadGroup && showLoader)
            {
                LoadGroup.gameObject.SetActive(true);
                LoadGroup.FadeIn(0, () => { fin = true; });
            }
            while (LoadGroup && showLoader && !fin)
            {
                yield return new WaitForSeconds(0.1f);
            }

            AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
            ao.allowSceneActivation = false;

            while (!ao.isDone && loadTime < minLoadTime)
            {
                loadTime += stepWait;
                loadProgress = Mathf.Clamp01(loadProgress + 1.0f / steps);
                if (simpleSlider && showLoader) simpleSlider.value = loadProgress;
                if (ao.progress >= 0.9f && !ao.allowSceneActivation && loadTime >= 0.9f * minLoadTime)
                {
                    ao.allowSceneActivation = true;
                }
                Debug.Log("waite scene: " + loadTime);
                yield return wfsStepWait;
            }

            if (LoadGroup && showLoader) LoadGroup.FadeOut(0, null);
            if (LoadingCallBack != null) LoadingCallBack();
        }

        IEnumerator AsyncLoadBeaty(int scene)
        {
            float apprLoadTime = 1f;
            float steps = 100f;
            float loadTime = 0.0f;
            loadProgress = 0;
            if (simpleSlider && showLoader) simpleSlider.value = loadProgress;

            bool fin = false;
            if (LoadGroup && showLoader)
            {
                LoadGroup.gameObject.SetActive(true);
                LoadGroup.FadeIn(0, () => { fin = true; });
            }
            while (LoadGroup && showLoader && !fin)
            {
                yield return null;
            }

            AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
            ao.allowSceneActivation = false;
            float lastTime = Time.time;
            while (!ao.isDone && loadProgress < 0.99f)
            {
                loadTime += (Time.time - lastTime);
                lastTime = Time.time;
                loadProgress = Mathf.Clamp01(loadProgress + 0.01f);
                if (simpleSlider && showLoader) simpleSlider.value = loadProgress;

                if (loadTime >= 0.5f * apprLoadTime && (ao.progress < 0.5f))
                {
                    apprLoadTime *= 1.1f;
                }
                else if (loadTime >= 0.5f * apprLoadTime && (ao.progress > 0.5f))
                {
                    apprLoadTime /= 1.1f;
                }

                if (ao.progress >= 0.90f && !ao.allowSceneActivation && loadProgress >= 0.99f)
                {
                    ao.allowSceneActivation = true;
                }

                // Debug.Log("waite scene: " + loadTime + "ao.progress : " + ao.progress);
                yield return new WaitForSeconds(apprLoadTime / steps); ;
            }

            animate = false;
            // StopCoroutine("ShowSimpleLoader");
            if (LoadGroup && showLoader) LoadGroup.FadeOut(0, null);
            if (LoadingCallBack != null) LoadingCallBack();
        }


        #region sprite sequence as loader
        bool animate = true;
        IEnumerator ShowSimpleLoader()
        {
            yield return null;
            /*
            float t = 1.0f / Samples;
            int i = 0;
            WaitForSeconds wft = new WaitForSeconds(t);
            int length = simpleLoaderSprites.Length;
            while (animate)
            {
                i++;
                i =(int) Mathf.Repeat(i, length);// Debug.Log(i);
                if (simpleLoader) simpleLoader.sprite = simpleLoaderSprites[i];
                yield return wft;
            }
            animate = true;
            */
        }
        #endregion sprite sequence as loader
    }
}