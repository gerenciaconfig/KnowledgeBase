using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Fungus;
using Sirenix.OdinInspector;

public class CorrelateButtonsController : MonoBehaviour
{
    //[SerializeField] private BlockReference globl;
    //[SerializeField] private BlockReference globalOnWrongBlock;

    [SerializeField] private List<CorrelatedButtons> correlations;

    private void OnEnable()
    {
		Reset();
    }

	public void Reset()
	{
		Setup();
		CorrelatedButtons.correlatedOptions = correlations;
		CorrelatedButtons.ResetAllCorrelates();
	}

	[Button]
    public void Setup()
    {
        foreach (CorrelatedButtons cbuttons in GetComponentsInChildren<CorrelatedButtons>())
        {
            correlations.Add(cbuttons);
        }
    }

}