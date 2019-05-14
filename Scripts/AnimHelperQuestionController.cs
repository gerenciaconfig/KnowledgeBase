using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHelperQuestionController : MonoBehaviour
{
    public void ActivateQuestion()
    {
        QuestionController.instance.ActivateQuestion();
    }

    public void ActivateInstantQuestion(int questionId)
    {
        QuestionController.instance.ActivateEnqueuedQuestion(questionId);
    }
}
