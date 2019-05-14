using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializerChild
 {

	 public string nome;
	 public string sobrenome;
	 public int idade;
	 public int mes;
	 public int ano;
	 public int dia;

	public SerializerChild(string nomeG,string sobrenomeG, int diaG, int mesG, int anoG)
	{
		nome = nomeG;
		sobrenome = sobrenomeG;
		mes = mesG;
		ano = anoG;
		dia = diaG;
	}

}