using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadImagesAssetBundleFile : MonoBehaviour
{
    public float downloadProgress;
    public string bundleUrl;

    private void OnEnable()
    {
        LoadOrDownloadImageBundle();
    }

    public void LoadOrDownloadImageBundle()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + @"/" + ConstantClass.IMAGE_BUNDLE_NAME))
        {
            LoadImageBundle(AssetBundle.LoadFromFile(Application.persistentDataPath + @"/" + ConstantClass.IMAGE_BUNDLE_NAME));
        }
        else
        {
            StartCoroutine(DownloadBundle(bundleUrl));
        }
    }

    public IEnumerator DownloadBundle(string bundleUrl)
    {
        WWW request = new WWW(bundleUrl);

        while (!request.isDone)
        {
            downloadProgress = request.progress;
            Debug.Log("ENTROU AQUI NO IMAGE DOWNLOAD");
            yield return null;
        }

        System.IO.File.WriteAllBytes(Application.persistentDataPath + @"/" + ConstantClass.IMAGE_BUNDLE_NAME, request.bytes);

        yield return new WaitForSeconds(1);

        request.Dispose();

        AssetBundle bundle = AssetBundle.LoadFromFile(Application.persistentDataPath + @"/" + ConstantClass.IMAGE_BUNDLE_NAME);
    }

    public void LoadImageBundle(AssetBundle bundle)
    {
        if (CurrentStatsInfo.ActivityImagesList == null || CurrentStatsInfo.ActivityImagesList.Count == 0)
        {
            CurrentStatsInfo.ActivityImagesList = new List<Sprite>(bundle.LoadAllAssets<Sprite>());
        }
    }
}