using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arcolabs.Home { 
    public class LoadingScriptHelper : MonoBehaviour
    {

        AsyncOperation ao;
        public void GoToRegister()
        {
            LoadingScript.nextScene = ConstantClass.REGISTER;
            StartCoroutine(LoadLevel(ConstantClass.LOADING));
        }

        public void BackFromRegister()
        {
            LoadingScript.nextScene = ConstantClass.LOGIN;
            StartCoroutine(LoadLevel(ConstantClass.LOADING));
        }

        public void GoToHome()
        {
            LoadingScript.nextScene = ConstantClass.HOME;
            StartCoroutine(LoadLevel(ConstantClass.LOADING));
        }

        public void GoToPayment()
        {
            LoadingScript.lastScene = ConstantClass.HOME;
            LoadingScript.nextScene = ConstantClass.PAYMENT;
            StartCoroutine(LoadLevel(ConstantClass.LOADING));
        }

        public IEnumerator LoadLevel(string nameScene)
        {
            ao = SceneManager.LoadSceneAsync(nameScene);
            ao.allowSceneActivation = true;
            yield return new WaitForEndOfFrame();
        }
    }
}
