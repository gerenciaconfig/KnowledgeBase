using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Fungus;

public class FindObjectLogic : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Game thisGameLogic;
	[SerializeField] private List<string> audioOnCopyClicked;
	[SerializeField] private List<FindableObject> objectsToFind;
    private List<FindableObject> _objectsToFind = new List<FindableObject>();
	

    [Header("Hint System")]
    [SerializeField] private float timeToHint = 3;
    //[SerializeField] private bool permanetHint = true;
    [SerializeField] private float hintSpeed = 0.5f;
    [SerializeField] private string hintAudio;
    private bool randomizeHint = false;
    //[SerializeField] private List<Animator> possibleHints;
    [SerializeField] private BlockReference blockExecuteOnHint;

    private AudioManager aManager;
    private bool hinted;

	private int lastIndexPlayed = 0;

    private void OnDisable()
    {
        _objectsToFind.Clear();
        hinted = false;
    }

    public void StartGame()
    {
        aManager = FindObjectOfType<AudioManager>();

        foreach (FindableObject findObj in objectsToFind)
        {
            _objectsToFind.Add(findObj);

			findObj.findableButton.onClick.RemoveAllListeners();
            findObj.findableButton.onClick.AddListener(delegate { TryToFindMultiples(findObj); });
        }

        //foreach (FindableObject findObj in _objectsToFind)
            
        StartCoroutine(WaitingForHint());
    }

	public void TryToFindMultiples(FindableObject findObj)
	{
		//if (findObj.multipleElement)
		//{
		for (int i = _objectsToFind.Count-1; i >= 0; i--)
		{
			if (_objectsToFind[i] != findObj && _objectsToFind[i].OnFindExecute.Equals(findObj.OnFindExecute))
			{
				Debug.Log("Teste");
				//_objectsToFind[i].hintAnimator.enabled = ;
				_objectsToFind[i].findableButton.onClick.RemoveAllListeners();
				_objectsToFind[i].findableButton.onClick.AddListener(delegate { RandomWarningAudio(); Debug.Log("Objeto com copias!"); });
				_objectsToFind[i].hintAnimator.SetTrigger("Exit");
				_objectsToFind.RemoveAt(i);
			}
		}
		//}

		ObjectFinded(findObj);
	}

    public void ObjectFinded(FindableObject findObj)
    {
        findObj.findableButton.onClick.RemoveAllListeners();
        //aManager.PlayAudioDescription(findObj.OnFindAudio);
        findObj.OnFindExecute.Execute();
        findObj.hintAnimator.SetTrigger("Win");
        _objectsToFind.Remove(findObj);
        ResetHintTime();
        thisGameLogic.AddVictory(true);
    }

	private void RandomWarningAudio()
	{
        AudioManager.instance.StopAllSounds();
		AudioManager.instance.PlaySound(audioOnCopyClicked[lastIndexPlayed]);
		lastIndexPlayed++;

		if (lastIndexPlayed >= audioOnCopyClicked.Count)
			lastIndexPlayed = 0;
	}

	public void ResetHintTime()
    {
        StopAllCoroutines();
        StartCoroutine(WaitingForHint());
    }

    private IEnumerator WaitingForHint()
    {
        if (hinted)
            yield break;

        Animator currentHint;

        yield return new WaitForSeconds(timeToHint);

        if (randomizeHint)
        {
            currentHint = _objectsToFind[Random.Range(0, _objectsToFind.Count)].hintAnimator;
            currentHint.SetTrigger("Hint");
        }
        else
        {
            foreach (FindableObject findObj in _objectsToFind)
            {
                findObj.hintAnimator.SetFloat("HintTime", 1/hintSpeed);
                findObj.hintAnimator.SetTrigger("Hint");
            }
        }

        blockExecuteOnHint.Execute();
        hinted = true;
        
    }

    [Button]
    public void Setup()
    {
        objectsToFind = new List<FindableObject>();

        foreach (Button bt in transform.GetComponentsInChildren<Button>(true))
        {
            objectsToFind.Add(new FindableObject(bt, bt.GetComponent<Animator>()));
        }
    }




}

[System.Serializable]
public class FindableObject
{
	public FindableObject(Button bt, Animator amtr)
	{
		findableButton = bt;
		hintAnimator = amtr;
	}

	public Button findableButton;
	public Animator hintAnimator;
	public BlockReference OnFindExecute;
	//[Tooltip("Se tiver mais que um objeto desse tipo coloque aqui o nome compartilhado desses objetos")]

	//[HideInInspector]
	//public bool multipleElement;
}
