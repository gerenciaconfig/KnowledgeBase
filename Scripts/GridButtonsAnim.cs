using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo("Animation", "Grid Buttons Anim", "")]
public class GridButtonsAnim : Command
{
    public TypeOfAnim animType;

    public string enterAnimTrigger;
    public string exitAnimTrigger;
    [Space(10)]
    public string popEnterSound;
    public string popExitSound;
    [Space(10)]
    public GameObject gridButtons;
    public float timeBetweenAnim;
    public float timeBetweenExitAudios;

    public enum TypeOfAnim
    {
        EnterAnim,
        ExitAnim
    }

    public override void OnEnter()
    {
        switch (animType)
        {
            case TypeOfAnim.EnterAnim:
                StartCoroutine(ExecuteAnim(gridButtons.transform, enterAnimTrigger));
                break;
            case TypeOfAnim.ExitAnim:
                StartCoroutine(ExecuteExitAnim(gridButtons.transform, exitAnimTrigger));


                break;
        }

        Continue();
    }

    public IEnumerator ExecuteAnim(Transform gridTransforms, string animTrigger)
    {
        List<Animator> animList = new List<Animator>();

        foreach (Transform item in gridTransforms)
        {
            animList.Add(item.GetComponent<Animator>());
        }

        foreach (var item in animList)
        {
            item.SetTrigger(animTrigger);

            if (!string.IsNullOrEmpty(animTrigger))
            {
                switch (animType)
                {
                    case TypeOfAnim.EnterAnim:
                        AudioManager.instance.PlaySound(popEnterSound);
                        break;
                    case TypeOfAnim.ExitAnim:
                        AudioManager.instance.PlaySound(popExitSound);
                        break;
                }
            }


            yield return new WaitForSeconds(timeBetweenAnim);
        }
    }

    public IEnumerator ExecuteExitAnim(Transform gridTransforms, string animTrigger)
    {
        foreach (Transform item in gridButtons.transform)
        {
            item.GetComponent<Animator>().SetTrigger(exitAnimTrigger);
        }

        AudioManager.instance.PlaySound(popExitSound);
        yield return new WaitForSeconds(timeBetweenExitAudios);
        AudioManager.instance.PlaySound(popExitSound);
    }
}
