using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Fungus;

public class Slots : MonoBehaviour, IDropHandler
{
    [SerializeField] List<GameObject> droppableObjects = new List<GameObject>();
    [SerializeField] Game game;

    public Vector3 ScaleTo;

    public bool scale;

    public bool randomFail;

    public bool isSortedAudio;

    public List<Transform> positionSnapList = new List<Transform>();

    public UnityEvent OnComplete;

    private ActivityAnalytics activityAnalytics;

    public void Start()
    {
        if (positionSnapList.Count == 0)
        {
            positionSnapList.Add(this.transform);
        }

        try
        {
            activityAnalytics = GameObject.FindGameObjectWithTag("Analytics").GetComponent<ActivityAnalytics>();
        }
        catch
        {

        }
    }

    public void OnDrop(PointerEventData eventData)
    {

        GameObject droppedObject = eventData.pointerDrag;

        if (droppableObjects.Contains(droppedObject))
        {
            DragBehaviour dragBehaviour = droppedObject.GetComponent<DragBehaviour>();

            if (dragBehaviour.GetCanDrag())
            {
                dragBehaviour.ObjectIsSet((positionSnapList[droppableObjects.IndexOf(droppedObject)].position));
                dragBehaviour.PlayRightAudio();
                

                if (isSortedAudio)
                {
                    droppedObject.GetComponent<Image>().sprite = droppedObject.GetComponent<DragBehaviour>().SettedSprite;
                    droppedObject.GetComponent<Image>().raycastTarget = false;
                    dragBehaviour.PlaySortedAudio();
                }

                if (scale)
                {
                    if (ScaleTo != Vector3.zero)
                    {
                        droppedObject.GetComponent<RectTransform>().localScale = ScaleTo;
                    }
                    else
                    {
                        droppedObject.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    }

                }

                OnComplete.Invoke();

                // particleManager.StartParticles();
                if (activityAnalytics != null)
                {
                    activityAnalytics.AddRight();
                }
                game.AddVictory(true);
            }

            
        }
        else
        {
            if (activityAnalytics != null)
            {
                activityAnalytics.AddWrong();
            }
            if (randomFail)
            {
                droppedObject.GetComponent<DragBehaviour>().PlayRandomWrongAudio();
            }
            else
            {
                droppedObject.GetComponent<DragBehaviour>().PlayWrongAudio();
            }
        }
    }

    public void AddDroppableObject(GameObject gameObject)
    {
        droppableObjects.Add(gameObject);
    }

    public void ClearList()
    {
        // droppableObjects.Clear();
        droppableObjects = new List<GameObject>();
    }
}