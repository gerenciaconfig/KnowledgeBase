using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Fungus;

public class FramesOptionsShaker : MonoBehaviour
{
    //TODO: Verificar performace |Código com performace em tempo de execução questionável...|
    //[SerializeField] private 
    public static FramesOptionsShaker instance;

    private UnityAction action;
    //private UnityAction tutorialInterrupt;

    [SerializeField] private GameObject canvasBlocker;
    [SerializeField] private Button audioButtonToEnable;
    [SerializeField] private List<ShakerObject> targets;

    private IEnumerator tutorial;

    private void OnEnable()
    {
        canvasBlocker.SetActive(true);

        instance = this;

        //Debug.Log("whata");

        foreach (ShakerObject shakObj in targets)//transform.GetComponentsInChildren<NewButtonBehaviour>())
        {
            shakObj.Setup();
            //Animator btAmtr = bt.GetComponent<Animator>();
            //Button btBt = bt.GetComponent<Button>();
            action = null;

            action = delegate { shakObj.amtr.SetTrigger("shake"); InterruptTutorial(); };

            if (shakObj.nBtBhavior.buttonType == NewButtonBehaviour.ButtonType.RightButton)
            {
                action += delegate { instance.FallWrongs(); };
            }
            
            //action += delegate { shakObj.bt.onClick.RemoveListener(action); };

            //Debug.Log(btBt);
            shakObj.bt.onClick.AddListener(action);

        }

    }

    public void FallWrongs()
    {
        StartCoroutine(Fall());
    }

    public IEnumerator Fall()
    {
        yield return new WaitForSeconds(0);
        yield return new WaitForSeconds(0);
        foreach (ShakerObject shakObj in targets)
            shakObj.bt.onClick.RemoveAllListeners();

        foreach (ShakerObject shakObj in targets)//transform.GetComponentsInChildren<NewButtonBehaviour>())
        {
            //targets[0].bt.onClick.RemoveAllListeners();
            if (shakObj.nBtBhavior.buttonType == NewButtonBehaviour.ButtonType.WrongButton)
            {
                yield return new WaitForSeconds(0.1f);
                shakObj.nBtBhavior.transform.SetAsLastSibling();
                shakObj.amtr.SetTrigger("fall");
            }
        }
    }

    // private void UnityAction() { }

    [Button]
    public void Setup()
    {
        targets = new List<ShakerObject>();
        foreach (NewButtonBehaviour nBtBhavior in transform.GetComponentsInChildren<NewButtonBehaviour>())
        {
            targets.Add(new ShakerObject(nBtBhavior));
        }

    }

    public void MakeTutorial()
    {
        tutorial = Tutorial();
        StartCoroutine(tutorial);
    }

    public IEnumerator Tutorial()
    {
        canvasBlocker.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].amtr.SetTrigger("shake");
            yield return new WaitForSeconds(AudioManager.instance.PlaySound(targets[i].audioName) + 1);
        }

        audioButtonToEnable.interactable = true;
    }

    public void InterruptTutorial()
    {
        StopCoroutine(tutorial);

        //foreach (ShakerObject shakObj in targets)
        //    shakObj.bt.onClick.RemoveListener(tutorialInterrupt);
    }

}

[System.Serializable]
public class ShakerObject
{
    public ShakerObject(NewButtonBehaviour nBtBhavior)
    {
        this.nBtBhavior = nBtBhavior;
    }

    public NewButtonBehaviour nBtBhavior;
    public string audioName;
    [HideInInspector] public Animator amtr;
    [HideInInspector] public Button bt;

    public void Setup()
    {
        amtr = nBtBhavior.GetComponent<Animator>();
        bt = nBtBhavior.GetComponent<Button>();
    }
}
