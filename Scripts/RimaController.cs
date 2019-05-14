using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RimaController : MonoBehaviour
{
    //public string rimaDescription;
    public string afterGameDescription;
    public GameObject afterGamePanel;
    public RandomPositionSetter gridButton;
    public Flowchart gameFlow;
    public string changeGameBlockString;

    private void OnEnable()
    {
        //AudioManager.instance.PlayAudioDescription(rimaDescription);
        gridButton.RandomizeObjPosition();
    }

    private void OnDisable()
    {
        afterGamePanel.SetActive(false);
    }

    public void RightButton()
    {
        StartCoroutine(Sucess());
    }

    IEnumerator Sucess()
    {
        //MuppetsRimarGameController.instance.currentRimaChar.OnRimaWin.Invoke();

        AudioManager.instance.StopAllSounds();

        yield return new WaitForSeconds(0.5f);

        FadeController.instance.SetOpen(afterGamePanel);
        FadeController.instance.FadeScreen(FadeController.FadeTypes.fadeOpen);

        yield return new WaitForSeconds(2);

        AudioManager.instance.PlaySound(afterGameDescription);
        yield return new WaitForSeconds(AudioManager.instance.GetAudioSource(afterGameDescription).clip.length);

        yield return new WaitForSeconds(1);

        //FadeController.instance.SetClose(afterGamePanel);
        //FadeController.instance.FadeScreen(FadeController.FadeTypes.fadeClose);

        //yield return new WaitForSeconds(1.5f);
        //this.gameObject.SetActive(false);

        gameFlow.ExecuteBlock(changeGameBlockString);

        AudioManager.instance.SetLastAudioDescription(MuppetsRimarGameController.instance.introRima);
        //MuppetsRimarGameController.instance.currentRimaChar.AddRimaVictory();
    }
}
