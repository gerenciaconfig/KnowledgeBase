using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RolloutEndMenu : MonoBehaviour
{
    [SerializeField] private float TransitionTime;
    [SerializeField] private List<CanvasGroupController> fadeInElements;
    //[SerializeField] private List<CanvasGroupController> fadeOutElements;
    [SerializeField] private List<RolloutObject> rollObjects;
    //[SerializeField] private UnityEvent<float> timeBasedEvents;

    private Animator amtr;

    private void Awake()
    {
        amtr = GetComponent<Animator>();

        foreach (RolloutObject rollObj in rollObjects)
            rollObj.SetStartValues();
    }

    public void EndGame()
    {
        foreach (CanvasGroupController fadeInE in fadeInElements)
            fadeInE.FadeCanvasGroup(true, TransitionTime);

        amtr.SetFloat("EndDuration", TransitionTime);
        amtr.SetTrigger("EndGame");

        foreach (RolloutObject rollObj in rollObjects)
            StartCoroutine(GoToTargetPosition(rollObj, TransitionTime));

        //timeBasedEvents.Invoke(TransitionTime);
    }

    private void OnDisable()
    {
        foreach (RolloutObject rollObj in rollObjects)
            rollObj.ResetPosition();
    }

    //TODO: Checar performace
    public static IEnumerator GoToTargetPosition(RolloutObject rollObj, float flyTime)
    {

        float timer = 0;
        float timingPercent = 0;

        Vector3 originalPos = rollObj.objectToRoll.localPosition;
        Vector3 targetPos = rollObj.targetTransform.localPosition;

        Vector3 originalScale = rollObj.objectToRoll.localScale;
        Vector3 targetScale = rollObj.targetTransform.localScale;

        do
        {
            yield return new WaitForSeconds(0);
            timer += Time.deltaTime;

            timingPercent = Mathf.InverseLerp(0, flyTime, timer);

            rollObj.objectToRoll.localPosition = Vector3.Lerp(originalPos, targetPos, timingPercent);
            rollObj.objectToRoll.localScale = Vector3.Lerp(originalScale, targetScale, timingPercent);

        } while (timingPercent < 1f);

    }

}

[System.Serializable]
public class RolloutObject
{
    public Transform objectToRoll;
    public Transform targetTransform;
    //public bool changeScale;

    [HideInInspector]
    public Vector3 startPosition;
    [HideInInspector]
    public Vector3 startScale;

    public void SetStartValues()
    {
        startPosition = objectToRoll.localPosition;
        startScale = objectToRoll.localScale;
    }

    public void ResetPosition()
    {
        objectToRoll.localPosition = startPosition;
        objectToRoll.localScale = startScale;
    }

}
