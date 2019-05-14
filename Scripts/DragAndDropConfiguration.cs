using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Game", "Drag and Drop Configuration", "Bla bla bla")]
public class DragAndDropConfiguration : Command
{
    public List<Slots> slots = new List<Slots>();
    public List<DragBehaviour> draggableObjects = new List<DragBehaviour>();
    public List<DragImage> dragImages = new List<DragImage>();

    public bool hasOrder;
    //public bool isSortedAudio;

    public override void OnEnter()
    {

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].ClearList();
        }


        for (int i = 0; i < dragImages.Count; i++)
        {
            draggableObjects[i].SetDragObject(dragImages[i].dropType, dragImages[i].image);

            if (dragImages[i].settedSprite != null)
            {
                draggableObjects[i].SettedSprite = dragImages[i].settedSprite;
            }

            draggableObjects[i].SetNewStartPos(draggableObjects[i].transform.position);
            draggableObjects[i].ResetGame();

            if (dragImages[i].mustSelect)
            {
                if (hasOrder)
                {
                    slots[i].AddDroppableObject(draggableObjects[i].gameObject);
                }
                else
                {
                    slots[0].AddDroppableObject(draggableObjects[i].gameObject);
                }

                draggableObjects[i].audioName = dragImages[i].rightSound;

            }
            else if (!dragImages[i].mustSelect)
            {
                draggableObjects[i].audioName = dragImages[i].wrongSound;
            }
        }
        Continue();
    }
}