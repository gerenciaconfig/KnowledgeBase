using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTimer : MonoBehaviour
{
    public static HintTimer instance;
    public float timeToShowHint;
    public List<Animator> hintAnimators;
    [Space(10)]

    public float timer;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        ResetHintTimer();
    }

    // Update is called once per frame
    void Update()
    {
        HintTimerCounter();
    }

    public void HintTimerCounter()
    {
        timer += Time.deltaTime;


        if (timer >= timeToShowHint)
        {
            foreach (var item in hintAnimators)
            {
                item.SetTrigger("hint");
            }

            ResetHintTimer();
        }

    }

    public void HintClick(Animator hintAnim)
    {
        ResetHintTimer();
        hintAnim.SetTrigger("click");
    }

    public void ResetHintTimer()
    { 
        timer = 0;
    }

    public void RebindHintAnimators()
    {
        foreach (var item in hintAnimators)
        {
            item.Rebind();
        }
    }

    public void PlayHintSound()
    {
        AudioManager.instance.PlaySound("Hint");
    }
}