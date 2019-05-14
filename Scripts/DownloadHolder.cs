using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DownloadHolder : MonoBehaviour
{
    public DownloadState downloadState;
    public float downloadProgress;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public IEnumerator DownloadBundle(string gameId, string bundleUrl)
    {
        Debug.Log("DownloadBundle " + gameId + " -- " + bundleUrl);

        downloadState = DownloadState.Downloading;

        WWW request = new WWW(ConstantClass.SERVER_DOWNLOAD_URL + bundleUrl);
        Debug.Log(ConstantClass.SERVER_DOWNLOAD_URL + bundleUrl);

        while (!request.isDone)
        {
            downloadProgress = request.progress;
            yield return null;
        }

        if (request.error == null)
        {
            downloadState = DownloadState.Downloaded;
        }
        else
        {
            downloadState = DownloadState.NotDownloaded;
            yield break;
        }

        System.IO.File.WriteAllBytes(Application.persistentDataPath + @"/" + gameId, request.bytes);

        yield return new WaitForSeconds(1);

        request.Dispose();

        Debug.Log("EndOf " + gameId + " -- " + bundleUrl);

        Destroy(this.gameObject);
    }
}

public enum DownloadState
{
    NotDownloaded,
    Downloaded,
    Downloading,
    Excluding
}
