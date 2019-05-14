using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Teste : MonoBehaviour
 {
	 public TextMeshProUGUI nomeInput;
	 public TextMeshProUGUI sobrenomeInput;
     public TextMeshProUGUI diaInput;
     public TextMeshProUGUI mesInput;
     public TextMeshProUGUI anoInput;

    SerializerChild child;

	void Start ()
	{
		SerializerChild exampleClass = Serializer.Load<SerializerChild>("crianca1");
        
        //if (PlayerPrefs.HasKey("Aniversario"))
     //   {
      //      print("tem key");
            if (exampleClass.mes == DateTime.Now.Month)
            {
                print("mes do seu aniv");
                if (exampleClass.dia >= DateTime.Now.Day - 4 && exampleClass.dia <= DateTime.Now.Day)
                {
                    if (PlayerPrefs.HasKey("Parabens"))
                    {
                        if (PlayerPrefs.GetInt("Parabens") == 0)
                        {
                            PlayerPrefs.SetInt("Parabens", 1);
                            print("Hoje é seu aniversario fique em casa");
                        }
                    }
                }
                else
                    PlayerPrefs.SetInt("Parabens", 0);


            }
            else
                PlayerPrefs.SetInt("Parabens", 0);
    //    }
     


  

    }
	
	void Update () 
	{
		
	}

	public void SaveChild()
	{
		child = new SerializerChild(nomeInput.text,sobrenomeInput.text, int.Parse(diaInput.text), int.Parse(mesInput.text),int.Parse(anoInput.text));
		Serializer.Save<SerializerChild>("crianca1", child);
	}

    public void BirthDayCheck()
    {
            
        
    }
}