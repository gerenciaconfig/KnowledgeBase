using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotOrderChecker : MonoBehaviour
{
    public Game gameReference;
    public List<GameObject> slotImages;
    public List<Sprite> correctImage;
    public List<string> successBlocks;
    public List<string> failBlocks;
    //Define todos pelo inspetor NA ORDEM CORRETA do índice

    public Flowchart flow;
    public string successBlockName;
    public string failBlockName;

    public bool CheckOrder() //Checa todos os slots e verifica se a ordem esta correta
    {
        for (int i = 0; i <= slotImages.Count - 1; i++)
        {
            if (slotImages[i].GetComponent<Image>().sprite != correctImage[i])
            {
                Debug.Log("Wrong!");
                return false;
            } else if (i == slotImages.Count - 1)
            {
                Debug.Log("Correct!");
            }
        }
        return true;
    }

    public void CheckSlot(int index) //Checa um slot em específico, reabilitando seu drag dalí caso errado.
    {
        if (slotImages[index].GetComponent<Image>().sprite != correctImage[index])
        {
            Debug.Log("Not Checked!");
            slotImages[index].GetComponent<DragAndDropSlot>().ResetSlot();
            if (slotImages[index].GetComponent<DragAndDropSlot>().receivedSlot != null)
            {
                slotImages[index].GetComponent<DragAndDropSlot>().receivedSlot.transform.position = slotImages[index].transform.position;
                slotImages[index].GetComponent<DragAndDropSlot>().receivedSlot.GetComponent<Image>().enabled = true;
                slotImages[index].GetComponent<DragAndDropSlot>().receivedSlot.GetComponent<Image>().raycastTarget = true;
            }
            if (failBlocks[index] != null)
            {
                flow.ExecuteBlock(failBlocks[index]); //Toca blocos específicos de fracasso se tiver algum
            }
        } else
        {
            Debug.Log("Checked!");
            AudioManager.instance.StopAllSounds();
            if (!CheckOrder()) //Checa se tudo já ta certo ou não
            {
                if (successBlocks[index] != null)
                {
                    flow.ExecuteBlock(successBlocks[index]); //Toca blocos específicos de sucesso se tiver algum
                }
            } else
            {
                if (successBlocks[index] != null)
                {
                    flow.ExecuteBlock(successBlocks[index]); //Toca blocos específicos de sucesso se tiver algum
                }
                flow.ExecuteBlock(successBlockName); //Toca o bloco de sucesso do game todo
            }
        }
    }
}
