using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupController : MonoBehaviour
{
    private CanvasGroup cG;
    [SerializeField] private float fadeDuration;
    [SerializeField] private CanvasGroupData enableSettings = new CanvasGroupData(1,true,true);
    [SerializeField] private CanvasGroupData disableSettings = new CanvasGroupData(0,false,false);

    private void Awake()
    {
        cG = GetComponent<CanvasGroup>();
    }

    public void EnableCanvasGroup(bool enable)
    {
        CanvasGroupData cachedData = enable ? enableSettings : disableSettings;

        cG.alpha = cachedData.alpha;
        cG.interactable = cachedData.interactable;
        cG.blocksRaycasts = cachedData.blocksRaycasts;
    }

    public void FadeCanvasGroup(bool fadeIn)
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvas(fadeIn,fadeDuration));
    }

    public void FadeCanvasGroup(bool fadeIn, float time)
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvas(fadeIn, time));
    }

    private IEnumerator FadeCanvas(bool fadeIn, float durTime)
    {
        float timer = 0;
        float scaledCurrentTime = 0;

        do
        {
            yield return new WaitForSeconds(0);
            timer += Time.deltaTime;

            scaledCurrentTime = Mathf.InverseLerp(0, durTime, timer);
            cG.alpha = Mathf.Abs( (fadeIn ? 0 : 1) - scaledCurrentTime );

        } while (scaledCurrentTime < 1);

        EnableCanvasGroup(fadeIn);
    }

}

[System.Serializable]
public class CanvasGroupData
{
    public CanvasGroupData(float alpha, bool interactable, bool blocksRaycasts)
    {
        this.alpha = alpha;
        this.interactable = interactable;
        this.blocksRaycasts = blocksRaycasts;
    }

    [Range(0, 1)] public float alpha;
    public bool interactable;
    public bool blocksRaycasts;
}
