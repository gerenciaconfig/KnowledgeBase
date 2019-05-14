using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasGroup))]

public class DragBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    Vector3 startPosition;
    Vector3 newPos;
    Vector3 currentPosition;

    public bool startDragging=true;

    private bool canDrag = false;

    public enum DropType { dropStay, dropSmooth, dropSmoothChangeImage };
    public DropType dropType;

    public bool dragging;

    public Transform pivotObj;

    public Transform startPivotPosition;

    public bool havePivot;

    public bool select;
    
    float offsetX;
    float offsetY;

    RectTransform rect;

    private Sprite settedSprite;
    public string audioName;

    bool isSet;
    Vector3 lastMousePos;

    private ActivityAnalytics activityAnalytics;

    public Sprite SettedSprite
    {
        get
        {
            return settedSprite;
        }

        set
        {
            settedSprite = value;
        }
    }

    // Transform canvas;


    void Awake ()
    {
        try
        {
            activityAnalytics = GameObject.FindGameObjectWithTag("Analytics").GetComponent<ActivityAnalytics>();
        }
        catch
        {

        }

        rect = GetComponent<RectTransform>();

        if(startPivotPosition != null)
        {
            startPosition = startPivotPosition.position;
            transform.position = startPosition;
        }
        else
        {
            startPosition = transform.position;
            transform.position = startPosition;
        }
    }

    public void OnEnable()
    {
        GetComponent<Image>().raycastTarget = true;
        ResetGame();        
    }

    public void SetDragObject(DropType dropType, Sprite sprite)
    {
        this.dropType = dropType;
        GetComponent<Image>().sprite = sprite;
    }

    public void SetDragObject(DropType dropType, Sprite sprite, Sprite settedSprite)
    {
        this.dropType = dropType;
        GetComponent<Image>().sprite = sprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canDrag) {
            //Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastMousePos = Input.mousePosition;
            //offsetX = rect.anchoredPosition.x - Input.mousePosition.x;
            //offsetY = rect.anchoredPosition.y - Input.mousePosition.y;

            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {        

        if(!isSet && !dragging && canDrag)
        {
            rect.SetAsLastSibling();
            Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 pos = transform.position;
            pos.y = delta.y;
            pos.x = delta.x;
            transform.position = pos;
            //lastMousePos = Input.mousePosition;
            //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //newPos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition.x) + offsetX;
            //newPos.x = Input.mousePosition.x + offsetX;
            //newPos.y = Input.mousePosition.y + offsetY;
            //newPos.z = 0;
            //rect.anchoredPosition3D = newPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (canDrag)
        {
                    
            GetComponent<CanvasGroup>().blocksRaycasts = true;

            if (isSet && dropType == DropType.dropStay)
            {
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                activityAnalytics.AddWrong();
            }
            else if (isSet && dropType == DropType.dropSmooth)
            {
                StartCoroutine(GoToPosition(1, currentPosition));
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                activityAnalytics.AddRight();
            }
            else if (isSet && dropType == DropType.dropSmoothChangeImage)
            {
                StartCoroutine(GoToPosition(1, currentPosition, SettedSprite));
                activityAnalytics.AddRight();
            }
            else
            {
                StartCoroutine(GoToPosition(1, currentPosition));
            }

            Debug.Log(dropType);
        }
    }

    public IEnumerator GoToPosition(float seconds, Vector3 destination)
    {
        if(havePivot)
        {
            if(pivotObj != null)
            {
                yield return StartCoroutine(GoToPivotPosition(pivotObj.position,0.4f));
            }
            if(select && isSet)
            {
                rect.SetAsFirstSibling();
            }
        }

        float elapsedTime = 0;
        dragging = true;
        while (elapsedTime < seconds)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, (elapsedTime / seconds));
           
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        dragging = false;
    }

    public IEnumerator GoToPosition(float seconds, Vector3 destination, Sprite newImage)
    {
        float elapsedTime = 0;
        dragging = true;
        while (elapsedTime < seconds)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, (elapsedTime / seconds));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        //GetComponent<Image>().sprite = newImage;
        rect.SetAsFirstSibling();
        dragging = false;
    }

    public void ObjectIsSet(Vector3 newPos)
    {
        isSet = true;
        currentPosition = newPos;
    }

    public void PlayWrongAudio()
    {
        AudioManager.instance.StopAllSounds();
        AudioManager.instance.PlaySound(audioName);
    }

    public void PlayRandomWrongAudio()
    {
        AudioManager.instance.StopAllSounds();
        AudioManager.instance.PlayRandomFailSound();
    }

    public void PlayRightAudio()
    {
        AudioManager.instance.StopAllSounds();
        AudioManager.instance.PlaySound(audioName);
    }

    public void PlaySortedAudio()
    {
        AudioManager.instance.StopAllSounds();
        AudioManager.instance.PlayScaleSound();
    }

    public void ResetGame()
    {
        isSet = false;

        if (startDragging) { 
            canDrag = true;
        }
        if (havePivot && startPivotPosition != null)
        {
            currentPosition = startPivotPosition.position;
            transform.position = currentPosition;
        }
        else
        {
            currentPosition = startPosition;
            transform.position = currentPosition;
        }

        GetComponent<Image>().raycastTarget = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void SetNewStartPos(Vector3 newStartPos)
    {
        startPosition = newStartPos;
        transform.position = startPosition;
    }

    public IEnumerator GoToPivotPosition(Vector3 pivotPos, float sec)
    {
        if(havePivot && select && isSet)
        {
            float elapsedTime = 0;
            dragging = true;
            while (elapsedTime < sec)
            {
                transform.position = Vector3.MoveTowards(transform.position, pivotPos, (elapsedTime / sec));
                
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public bool GetCanDrag()
    {
        return canDrag;
    }

    public void EnableCanDrag()
    {
        canDrag = true;
    }

    public void DisableCanDrag()
    {
        canDrag = false;
    }
}