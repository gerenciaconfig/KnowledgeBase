using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using Sirenix.OdinInspector;

public class SelectRightOption : MonoBehaviour
{
    [SerializeField] private List<SelectableOption> options;
    [SerializeField] private BlockReference rightOptionBlock;
    [SerializeField] private bool randomizeElements = true;
    [SerializeField] private bool disabledOnEnter;

    [Header("Feedbacks")]
    [SerializeField] private string audioOnClick = "Button Click";
    [SerializeField] private bool playFailSounds = true;
    [SerializeField] private bool punchOnClick = true;

    private void OnEnable()
    {
        foreach (SelectableOption sOpt in options)
        {
            sOpt.AutoSetup();

            if (audioOnClick != "")
            {
                sOpt.button.onClick.AddListener(delegate { AudioManager.instance.StopAllSounds(); AudioManager.instance.PlaySound(audioOnClick); });
            }

            if (playFailSounds && sOpt.response == SelectableOption.option.wrong)
            {
                sOpt.button.onClick.AddListener(delegate { PlayFailSound(); });
            }
            else if (sOpt.response == SelectableOption.option.right)
            {
                sOpt.button.onClick.AddListener(delegate {
                    rightOptionBlock.Execute();
                    DisableButtons();
                });
            }

            if (punchOnClick)
                sOpt.button.onClick.AddListener(delegate { iTween.PunchScale(sOpt.button.gameObject, new Vector3(-0.15f, -0.15f, 0f), 1f); } );

            if (randomizeElements)
                if (Random.Range(0f, 1f) < 0.5f)
                {
                    sOpt.button.transform.SetAsLastSibling();
                }

            if (disabledOnEnter)
            {
                sOpt.button.interactable = false;
            }
            else
                sOpt.button.interactable = true;

        }
    }

    public void EnableButtons()
    {
        foreach (SelectableOption sOpt in options)
        {
            sOpt.button.interactable = true;
        }
    }

    public void DisableButtons()
    {
        foreach (SelectableOption sOpt in options)
        {
            sOpt.button.interactable = false;
        }
    }

    public void PlayFailSound()
    {
        StopAllCoroutines();
        StartCoroutine(PlayFail());
    }

    private IEnumerator PlayFail()
    {
        if (audioOnClick != "")
            yield return new WaitForSeconds(0.5f);

        AudioManager.instance.StopAllSounds();
        AudioManager.instance.PlayRandomFailSound();
    }

    [Button]
    public void SetOptions()
    {
        options = new List<SelectableOption>();

        foreach (Button bt in GetComponentsInChildren<Button>())
        {
            options.Add(new SelectableOption(bt));
        }
    }

}

[System.Serializable]
public class SelectableOption
{
    public SelectableOption(Button bt)
    {
        button = bt;
        amtr = bt.GetComponent<Animator>();
    }

    public enum option { right, wrong }

    public Button button;
    public option response = option.wrong;
    //public BlockReference onClickBlock;

    [HideInInspector] public Animator amtr;

    public void AutoSetup()
    {
        button.interactable = true;
        button.onClick.RemoveAllListeners();

        if (response == option.right)
            button.onClick.AddListener(delegate { amtr.SetTrigger("right"); } );
        else
            button.onClick.AddListener(delegate { amtr.SetTrigger("wrong"); } );

    }

}
