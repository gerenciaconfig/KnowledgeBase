using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using Sirenix.OdinInspector;

public class CorrelatedButtons : MonoBehaviour
{
    public static List<CorrelatedButtons> correlatedOptions;

    [SerializeField] private Button firstOption;
    [SerializeField] private Button correlatedOption;
    [SerializeField] private bool punchScaleOnClick;

    [SerializeField] private BlockReference blockOnRight;
    [SerializeField] private BlockReference blockOnWrong;

    public static void ResetAllCorrelates()
    {
        foreach (CorrelatedButtons cBts in correlatedOptions)
        {
            cBts.ResetInteractions();
        }
    }

    public void ResetInteractions()
    {
        firstOption.interactable = true;
        correlatedOption.interactable = false;

        firstOption.onClick.AddListener(delegate { FirstBTClicked(); });
    }

    public void FirstBTClicked()
    {
        Punch(firstOption);

        SetThisOption();
    }

    public void SetThisOption()
    {

        foreach (CorrelatedButtons cbt in correlatedOptions)
        {
            cbt.firstOption.interactable = false;
            cbt.correlatedOption.interactable = true;

            cbt.correlatedOption.onClick.RemoveAllListeners();

            if (cbt != this)
            {
                cbt.correlatedOption.onClick.AddListener(delegate 
				{
					blockOnWrong.block.GetFlowchart().StopAllBlocks();
					blockOnWrong.Execute(); Punch(cbt.correlatedOption);
					ResetAllCorrelates();
				});
            }
            else
            {
                cbt.correlatedOption.onClick.AddListener(delegate 
                {
                    blockOnRight.Execute();
                    Punch(cbt.correlatedOption);
                    correlatedOptions.Remove(cbt);
                    cbt.correlatedOption.interactable = false;
                    ResetAllCorrelates();
                });
            }
        }
    }

    private void Punch(Button btTPunch)
    {
        if(punchScaleOnClick)
            iTween.PunchScale(btTPunch.gameObject, new Vector3(0.15f, 0.15f, 0), 1);
    }

    [Button]
    private void Setup()
    {
        firstOption = GetComponentsInChildren<Button>()[0];
        correlatedOption = GetComponentsInChildren<Button>()[1];
    }

}
