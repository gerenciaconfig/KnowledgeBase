
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarBehaviour : MonoBehaviour 
{
	public Manager m;

    public Sprite normalSprite;
    public Sprite selectedSprite;

    public List<NeibourStar> neibours;

    [Space]

    private bool isSelected;
    

    public bool canSelect;

    public int completedStars;

    public bool interact;
    public bool first;
    public bool last;
    public StarBehaviour next;
    public StarBehaviour previous;

    private void OnEnable()
    {
        completedStars = 0;
        //canSelect = true;
        if(m.points)
        {
            canSelect = true;
        }

        if(m.lab && interact)//se for o jogo do labirinto e esse for o primeiro marcador
        {
            canSelect = true;
        }

        if(m.lab && !interact)
        {
            canSelect = false;
        }
    }

    void Update()
    {
        if(m.lab && interact)
        {
            canSelect = true;
        }

        if(m.lab && this.interact && this.last)//fim do jogo do labirinto
        {
            canSelect = false;
            interact = false;
            m.endGame = true;
        }
    }
    
    public void SelectStar()
    {
       isSelected = true;
       m.SelectStar(this);

       GetComponent<Image>().sprite = selectedSprite;

       if(m.lab)
       { 
            this.interact = true;
            ActivateNext();
       }  
    }

    public void Desselect()
    {
        isSelected = false;
        GetComponent<Image>().sprite = normalSprite;
        m.DesselectStar();
        DeactivateNext();
    }

    public void Clicked()
	{
		if(canSelect)//se a estrela nao fez todas as possiveis conexoes ela pode ser clicada
		{
            SelectStar();
            ActivateNext();
        }
	}

	public void Entered()
	{
		if(canSelect && !isSelected && m.hasSelected)
		{
			if(Input.GetMouseButton(0))//se o "mouse down" permanecer e interagir com outro obj
			{
                if(m.hasSelected)
                {
                    m.SelectSecondStar(this);

                    if(m.lab)
                    { 
                        this.interact = true;
                        ActivateNext();
                    }  
                }
                else
                {
                    SelectStar();
                }
			}
		}
	}
    
    public bool SeekAndSelect(StarBehaviour searched)
    {
        if(m.lab)
        {
            this.canSelect = true;
            this.interact = false;
            if(!first)
            {
                previous.canSelect = false;
                previous.interact = false;
            }
        }

        for (int i = 0; i < neibours.Count; i++)
        {
            if(neibours[i].neibour == searched && !neibours[i].completed)
            {
                neibours[i].completed = true;
                
                completedStars++;

                if (completedStars == neibours.Count)
                {
                    m.connections++;
                    canSelect = false;
                }
                return true;
            }
        }
        return false;
    }

    void ActivateNext()
    {
        if(m.lab && !last)
        {
            next.canSelect = true;
        }
    }

    void DeactivateNext()
    {
        if(m.lab && !last)
        { 
            next.canSelect = false;
        }
    }

}