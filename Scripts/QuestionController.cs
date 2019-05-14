using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestionPanel
{
    public GameObject panel;
    public bool wasQueued;
}

public class QuestionController : MonoBehaviour
{
    public static QuestionController instance;
    public Button questionButton;
    public List<QuestionPanel> questionPanelList;
    public List<GameObject> enqueuedQuestionList;

    private Queue<GameObject> QuestionPanelQueue = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        ResetQuestions();
    }

    // Activate And Deactivate Question button.
    public void ActivateQuestion()
    {
        if (questionPanelList[questionPanelList.Count - 1].wasQueued)
        {
            return;
        }

        questionButton.gameObject.SetActive(true);

        foreach (var item in questionPanelList)
        {
            if (item.wasQueued == false)
            {
                item.wasQueued = true;
                QuestionPanelQueue.Enqueue(item.panel);
                break;
            }
        }
    }

    public void ActivateEnqueuedQuestion(int questionId)
    {
        enqueuedQuestionList[questionId].SetActive(true);
    }

    // Shows Question Panels sorted;
    public void ShowQuestionPanel()
    {
        QuestionPanelQueue.Peek().SetActive(true);
    }

    public void CloseActivePanel()
    {
        QuestionPanelQueue.Peek().SetActive(false);
    }

    public void CloseActivePanelOnRight()
    {
        QuestionPanelQueue.Dequeue().SetActive(false);

        if (QuestionPanelQueue.Count == 0)
        {
            questionButton.gameObject.SetActive(false);
        }
    }

    private void ResetQuestions()
    {
        questionButton.gameObject.SetActive(false);

        foreach (var item in questionPanelList)
        {
            item.panel.SetActive(false);
            item.wasQueued = false;
        }

        foreach (var item in enqueuedQuestionList)
        {
            item.SetActive(false);
        }

        QuestionPanelQueue.Clear();
    }
}