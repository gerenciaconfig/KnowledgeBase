using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadAssetFileBundle : MonoBehaviour
{
    public string gameCode;
    public string bundleUrl;
    public bool isLocalGame;

    [Space(10)]
    public Image progressImage;

    private LevelDTO activity;
    private DownloadState gameButtonState;
    private DownloadHolder downloadHolder;

    public void SetActivity(LevelDTO value)
    {
        activity = value;
        gameCode = value.code;
        bundleUrl = value.pathAndroid;


#if UNITY_ENGINE
        bundleUrl = value.pathAndroid;
#elif UNITY_ANDROID
        bundleUrl = value.pathAndroid;
#elif UNITY_IOS
        //bundleUrl = value.pathIOS;
#endif
    }

    public void OnEnable()
    {
        CheckDownloadedGames();
        List<GameObject> downloadArray = new List<GameObject>(GameObject.FindGameObjectsWithTag(ConstantClass.DOWNLOAD_OBJ_TAG));

        foreach (var item in downloadArray)
        {
            if (item.name == gameCode + ConstantClass.DOWNLOAD_OBJ_NAME)
            {
                downloadHolder = GetComponent<DownloadHolder>();
            }
        }
    }

    private void Update()
    {
        if (downloadHolder != null && progressImage != null)
        {
            progressImage.fillAmount = 1 - downloadHolder.downloadProgress;
        }
    }

    public void SetGameButtonState(DownloadState value)
    {
        switch (value)
        {
            case DownloadState.NotDownloaded:
                //progressImage.gameObject.SetActive(true);
                break;
            case DownloadState.Downloaded:
                //progressImage.gameObject.SetActive(false);
                break;
        }
    }

    public enum DownloadState
    {
        NotDownloaded,
        Downloaded,
        Downloading,
        Excluding
    }

    public enum ImageType
    {
        Icon,
        Thumbnail
    }

    public void CheckDownloadedGames()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + @"/" + gameCode))
        {
            SetGameButtonState(DownloadState.Downloaded);
        }
        else
        {
            SetGameButtonState(DownloadState.NotDownloaded);
        }
    }

    public void LoadGame(bool loadGame)
    {
        if (isLocalGame)
        {
            StartCoroutine(LoadGameFromStreamingAssets());
        }
        else if (System.IO.File.Exists(Application.persistentDataPath + @"/" + gameCode))
        {
            StartCoroutine(LoadGameFromFile());
        }
        else
        {
            //StartCoroutine(DownloadBundle(loadGame));
            CreateDownload();
        }
    }

    public void DeleteGame()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + @"/" + gameCode))
        {
            System.IO.File.Delete(Application.persistentDataPath + @"/" + gameCode);
            SetGameButtonState(DownloadState.NotDownloaded);
        }
    }

    public IEnumerator LoadGameFromFile()
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(Application.persistentDataPath + @"/" + gameCode);

        string[] scenePaths = bundle.GetAllScenePaths();
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePaths[0]);

        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    public IEnumerator LoadGameFromStreamingAssets()
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + @"/" + gameCode);

        string[] scenePaths = bundle.GetAllScenePaths();
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePaths[0]);

        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    IEnumerator DownloadBundle(bool loadGame)
    {
        SetGameButtonState(DownloadState.Downloading);

        WWW request = new WWW(bundleUrl);

        while (!request.isDone)
        {
            progressImage.fillAmount = 1 - request.progress;
            yield return null;
        }

        if (request.error == null)
        {
            SetGameButtonState(DownloadState.Downloaded);
        }
        else
        {
            SetGameButtonState(DownloadState.NotDownloaded);
            yield break;
        }

        System.IO.File.WriteAllBytes(Application.persistentDataPath + @"/" + gameCode, request.bytes);

        yield return new WaitForSeconds(1);

        request.Dispose();

        if (loadGame)
        {
            yield return LoadGameFromFile();
        }
    }

    public void CreateDownload()
    {
        GameObject downloadObj = new GameObject
        {
            name = gameCode + ConstantClass.DOWNLOAD_OBJ_NAME,
            tag = ConstantClass.DOWNLOAD_OBJ_TAG
        };

        DownloadHolder holder = downloadObj.AddComponent<DownloadHolder>();
        downloadHolder = holder;

        StartCoroutine(holder.DownloadBundle(gameCode, bundleUrl));
    }
}
