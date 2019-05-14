using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;




public class DataManager : MonoBehaviour {
    
   
    int dataExtra = 0 ;
    
       [SerializeField] TextMeshProUGUI diaAniversario;
       [SerializeField] TextMeshProUGUI mesAniversario;
       [SerializeField] TextMeshProUGUI anoAniversario;
        

    void Start()
    {
        print("oi");
        BirthdayCheck();
    }

    private void BirthdayCheck()
    {

        if (PlayerPrefs.HasKey("anoAniversario"))
        {
            int month = PlayerPrefs.GetInt("mesAniversario");
            int day = PlayerPrefs.GetInt("diaAniversario");
            int year = PlayerPrefs.GetInt("anoAniversario");
            print(day + " " + month + " " + year);
            DateTime d = new DateTime(year, month, day);
            if (d.Month == DateTime.Now.Month)
            {
                if (d.Day >= DateTime.Now.Day - 4 && d.Day <= DateTime.Now.Day)
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
                print(PlayerPrefs.GetInt("Parabens"));

        }
    }




     public void InclusaoDeDia()
     {
        
        string diaAniversario = this.diaAniversario.text;
        int dia;
        int.TryParse(diaAniversario, out dia);
        if (PlayerPrefs.HasKey("diaAniversario")){
            PlayerPrefs.SetInt("diaAniversario", dia);
            print(dia);
        }
     }

    public void InclusaoDeMes()
    {
        string mesAniversario = this.mesAniversario.text;
        int mes;
        int.TryParse(mesAniversario, out mes);
        if (PlayerPrefs.HasKey("mesAniversario"))
        {
            PlayerPrefs.SetInt("mesAniversario", mes);
            print(mes);
        }
    }
    public void InclusaoDeAno()
    {
        string anoAniversario = this.anoAniversario.text;
        int ano;
        int.TryParse(anoAniversario, out ano);
        if (PlayerPrefs.HasKey("anoAniversario"))
        {
            PlayerPrefs.SetInt("anoAniversario", ano);
            print(ano);
        }
    }

}

