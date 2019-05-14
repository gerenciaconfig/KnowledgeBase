using UnityEngine;
using UnityEngine.UI;
using System;
using Mkey;
using Fungus;

public class VictoryWindowController : PopUpsController
{
    [SerializeField]
    private SceneCurve curveLeft;
    [SerializeField]
    private SceneCurve curveMiddle;
    [SerializeField]
    private SceneCurve curveRight;
    [SerializeField]
    private float speed = 5;

    [Space(8)]
    public Text scoreText;
    public Text missionDescriptionText;

    [Space(16)]
    public GameObject starLeftFull;
    public GameObject starMiddleFull;
    public GameObject starRightFull;
    [Space(8)]
    public GameObject starLeftEmpty;
    public GameObject starMiddleEmpty;
    public GameObject starRightEmpty;

    bool starLeftSet = false;
    bool starMiddleSet = false;
    bool starRightSet = false;
    TweenSeq ts;
    private void SetStars()
    {
        if (!starLeftSet) starLeftFull.SetActive(BubblesPlayer.Instance.StarCount >= 1);
        if (!starMiddleSet) starMiddleFull.SetActive(BubblesPlayer.Instance.StarCount >= 2);
        if (!starRightSet) starRightFull.SetActive(BubblesPlayer.Instance.StarCount >= 3);

        ts = new TweenSeq();
        if (BubblesPlayer.Instance.StarCount >= 1 && !starLeftSet)
        {
            starLeftSet = true;

            ts.Add((callBack) =>
            {
                if (curveLeft)
                {
                    float time = curveLeft.Length / speed;
                    curveLeft.MoveAlongPath(starLeftFull.gameObject, starLeftEmpty.transform, time, 0f, EaseAnim.EaseInOutSine, callBack);
                }
                else
                {
                    SimpleTween.Move(starLeftFull.gameObject, starLeftFull.transform.position, starLeftEmpty.transform.position, 0.5f).AddCompleteCallBack(() =>
                    {
                        callBack();
                    });
                }

            });
        }
        if (BubblesPlayer.Instance.StarCount >= 2 && !starMiddleSet)
        {
            starMiddleSet = true;
            ts.Add((callBack) =>
            {
                if (curveMiddle)
                {
                    float time = curveMiddle.Length / speed;
                    curveMiddle.MoveAlongPath(starMiddleFull.gameObject, starMiddleEmpty.transform, time, 0f, EaseAnim.EaseInOutSine, callBack);
                }
                else
                {
                    SimpleTween.Move(starMiddleFull.gameObject, starMiddleFull.transform.position, starMiddleEmpty.transform.position, 0.5f).AddCompleteCallBack(() =>
                    {
                        callBack();
                    });
                }
            });
        }
        if (BubblesPlayer.Instance.StarCount >= 3 && !starRightSet)
        {
            starRightSet = true;
            ts.Add((callBack) =>
            {
                if (curveRight)
                {
                    float time = curveRight.Length / speed;
                    curveRight.MoveAlongPath(starRightFull.gameObject, starRightEmpty.transform, time, 0f, EaseAnim.EaseInOutSine, callBack);
                }
                else
                {
                    SimpleTween.Move(starRightFull.gameObject, starRightFull.transform.position, starRightEmpty.transform.position, 0.5f).AddCompleteCallBack(() =>
                    {
                        callBack();
                    });
                }
            });
        }
        ts.Start();
    }

    private void SetScore()
    {
        scoreText.text = BubblesPlayer.Instance.LevelScore.ToString();
    }

    public override void RefreshWindow()
    {
        string description = (GameBoard.LcSet) ? GameBoard.LcSet.levelMission.Description : "";
        missionDescriptionText.text = description;
        missionDescriptionText.enabled = !string.IsNullOrEmpty(description);
        SetStars();
        SetScore();
        base.RefreshWindow();
    }


    public void Replay_Click()
    {
        SoundMasterController.Instance.SoundPlayClick(0, null);
        CloseWindow();
        Mkey.SceneLoader.Instance.LoadScene("90 - Divertidamente (Game)");
    }

    public void Next_Click()
    {
        BubblesPlayer.CurrentLevel += 1;
        CloseWindow();
        Mkey.SceneLoader.Instance.LoadScene("90 - Divertidamente (Game)");
    }

    public void Cancel_Click()
    {
        CloseWindow();
        Mkey.SceneLoader.Instance.LoadScene("90 - Divertidamente (Menu)");
    }

    public void TestLeftStar()
    {
        BubblesPlayer.Instance.SetStarsCount(1);
    }

    public void TestMiddleStar()
    {
        BubblesPlayer.Instance.SetStarsCount(2);
    }

    public void TestRightStar()
    {
        BubblesPlayer.Instance.SetStarsCount(3);
    }

    private void OnDestroy()
    {
        BubblesPlayer.Instance.ChangeScoreEvent -= SetScore;
        BubblesPlayer.Instance.ChangeStarsEvent -= SetStars;
        ts.Break();
        SimpleTween.Cancel(gameObject, false);
        SimpleTween.Cancel(starRightFull.gameObject, false);
        SimpleTween.Cancel(starLeftFull.gameObject, false);
        SimpleTween.Cancel(starMiddleFull.gameObject, false);
    }

    private void Start()
    {
        BubblesPlayer.Instance.ChangeScoreEvent += SetScore;
        BubblesPlayer.Instance.ChangeStarsEvent += SetStars;
    }
}
