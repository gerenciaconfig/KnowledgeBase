using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class Manager : MonoBehaviour 
{
	public List <GameObject> allStars;//todas as estrelas que existem na cena

    public Flowchart fc;

    public Game g;

	public int totalStars;

	public int connections;

	public StarBehaviour selectedStar;

    public AudioManager am;
    
    public bool hasSelected;

    public bool end;
    public bool whiteLine = false;
    public GameObject lvl;

     GameObject myLine;

     public GameObject linePrefab;

     private List <GameObject> lines = new List<GameObject>();

     public GameObject previousManager;

     private GameObject pointerLine;

     private bool isInstatiatedLine;

     Vector3 pos;

    public bool points;

    public bool lab;

    public bool endGame;

	[Space]
	[SerializeField] private bool permanentLines;

    public void OnEnable()
    {
        Input.multiTouchEnabled = false;
        lvl.SetActive(true);
        ResetManager();
    }

	void Update()
	{
		if(connections == totalStars)
        {
            end = true;
            EndGame();
        }

        if(hasSelected)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            myLine.GetComponent<LineRenderer>().SetPosition(1,Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetMouseButtonUp(0))
		{
            DesselectAndDestroyLine();
		}

        if(lab && endGame)
        {
            EndGame();    
        }
    }

    public void SelectStar(StarBehaviour star)
    {
        hasSelected = true;
        selectedStar = star;
        if (whiteLine)
        {
            DrawLine(selectedStar.transform.position, pos, Color.white);
        } else
        {
            DrawLine(selectedStar.transform.position, pos, Color.yellow);
        }
        isInstatiatedLine = false;
    }

    public void DesselectStar()
    {
        hasSelected = false;
    }

    public void DesselectAndDestroyLine()
    {
        if(selectedStar!= null)
        {
            selectedStar.Desselect();
        }
        
        if(!isInstatiatedLine)
        {
            DeleteLine();
        }
        hasSelected = false;
    }

    public void MouseUp()
    {
        if(hasSelected)
        {
            hasSelected = false;
            DeleteLine();
        }
    }

    public void SelectSecondStar(StarBehaviour star)
    {
        if(selectedStar.SeekAndSelect(star))
        {
            am.StopAllSounds();
            star.SeekAndSelect(selectedStar);
            myLine.GetComponent<LineRenderer>().SetPosition(1,star.transform.position);
            isInstatiatedLine = true;
            am.PlayScaleSound();


            if(star.canSelect)
            {
                selectedStar.Desselect();
                star.SelectStar();
            }

            else
            {
                selectedStar.Desselect();           
            }
        }
        else
        {
            DeleteLine();
            if(star.canSelect)
            {
                selectedStar.Desselect();
                star.SelectStar();
            }
            else
            {
                selectedStar.Desselect();
            }
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        
        myLine = Instantiate(linePrefab);
        //myLine = new GameObject();
        myLine.transform.parent = lvl.transform;
        myLine.transform.position = start;
        //myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.alignment = LineAlignment.TransformZ;
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.sortingLayerName = "UI";
        lr.sortingOrder = 1;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lines.Add(myLine.gameObject);
		// /\ Esses valores terem sido fixos até o momento está atrasando o projeto.

/* 
        myLine = new GameObject();
        myLine.transform.parent = lvl.transform;
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr2 = myLine.GetComponent<LineRenderer>();
        lr2.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr2.alignment = LineAlignment.TransformZ;
        lr2.startColor = color;
        lr2.endColor = color;
        lr2.startWidth = 0.1f;
        lr2.sortingLayerName = "UI";
        lr2.sortingOrder = 1;
        lr2.SetPosition(0, start);
        lr2.SetPosition(1, end);
        lines.Add(myLine.gameObject);*/
    }

    public void DeleteLine()
    {
        Destroy(myLine);
    }

    void EndGame()
    {
        am.ResetNoteIndex();
        g.AddVictory(true);
        ResetManager();
    }

    public void ResetManager()
    {
		if (!permanentLines)
			ClearAllLines();

		hasSelected = false;
        connections = 0;

        foreach (GameObject s in allStars)
        {
            s.GetComponent<StarBehaviour>().canSelect = true;

            for (int i = 0; i < s.GetComponent<StarBehaviour>().neibours.Count; i++)
            {
                if(s.GetComponent<StarBehaviour>().neibours[i].completed)
                {
                    s.GetComponent<StarBehaviour>().neibours[i].completed = false;
                }
            }     
        }

        if(lab)
        {
            foreach (GameObject s in allStars)
            {
                if(!s.GetComponent<StarBehaviour>().first)
                {
                    s.GetComponent<StarBehaviour>().canSelect = false;
                    s.GetComponent<StarBehaviour>().interact = false;   
                }

                 if(s.GetComponent<StarBehaviour>().first)
                {
                    s.GetComponent<StarBehaviour>().canSelect = true;
                    s.GetComponent<StarBehaviour>().interact = true;   
                }
            }

            endGame = false;
        }
    }

	public void ClearAllLines()
	{
		foreach (GameObject l in previousManager.GetComponent<Manager>().lines)
		{
			Destroy(l);
		}
	}
}