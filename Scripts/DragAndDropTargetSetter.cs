using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropTargetSetter : MonoBehaviour
{

    private DragAndDropObj dragObj;

    [SerializeField] private GameObject target;
    [SerializeField] private GameObject fakeTarget;
    [SerializeField] private bool switchToTargetOnEnable;
    [Tooltip("Toca ou não áudio de falha se o jogador errar o objeto enquanto estiver preparado para chegar no alvo")]
    [SerializeField] private bool soundOnMissTarget;
    [Tooltip("Toca ou não áudio de falha se o jogador largar o objeto enquanto não estiver preparado para chegar no alvo")]
    [SerializeField] private bool soundOnFakeTarget = true;

    private void Awake()
    {
        dragObj = GetComponent<DragAndDropObj>();
    }

    private void OnEnable()
    {
        SetRightTarget(switchToTargetOnEnable);
    }

    public void SetRightTarget(bool setRight)
    {
        dragObj.slot = setRight ? target : fakeTarget;
        dragObj.playFailSound = setRight ? soundOnMissTarget : soundOnFakeTarget;
    }

}
