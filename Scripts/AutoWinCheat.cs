using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using Fungus;

public class AutoWinCheat : MonoBehaviour
{
	[TextArea] [SerializeField] private string howTo = "Durante qualquer jogo pressione a tecla escolhida abaixo para pular pro próximo jogo!";
	[Space]
	[SerializeField] private KeyCode input = KeyCode.W;

	private GameManager gManager;

	private void Awake()
	{
		gManager = FindObjectOfType<GameManager>();
		Debug.LogError("Atenção! Esse componente é para auxiliar nos testes apenas, remova-o antes de fazer uma build! (AutoWinCheat atrealado à " + gameObject.name + " !)");
	}

	private void Update()
	{
		if (Input.GetKeyDown(input))
		{
			foreach (Flowchart fc in FindObjectsOfType<Flowchart>())
			{
				fc.StopAllBlocks();
			}

			AudioManager.instance.StopAllSounds();
			gManager.GoToNextGame();
		}
	}

}
