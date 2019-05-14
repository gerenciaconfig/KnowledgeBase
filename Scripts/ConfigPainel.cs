using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class ConfigPainel : MonoBehaviour
 {
	 public float timeCount;
	 public bool startCount = false;
	public GameObject configbtn;
	public GameObject searchbtn;
	 public GameObject painel;

	void Update()
	{
		if(startCount)
		{
			timeCount += Time.deltaTime;
		}

		if(timeCount >= 1.3f)
		{
			startCount = false;
			timeCount = 0;
			configbtn.SetActive(true);
			searchbtn.SetActive(true);
			if(this.gameObject.tag == "closePainel")
			{
				this.gameObject.GetComponent<Button>().interactable = true;
			}
		}

	}
	public void Cliked()
	{
		if(this.gameObject.tag == "openPainel")
		{
			painel.GetComponent<Animator>().SetTrigger("OpenP");
			configbtn.SetActive(false);
			searchbtn.SetActive(false);
		}
		if(this.gameObject.tag == "closePainel")
		{
			painel.GetComponent<Animator>().SetTrigger("CloseP");
			this.gameObject.GetComponent<Button>().interactable = false;
			startCount = true;
		}
	}

	public void ResetPainel()
	{
		painel.GetComponent<Animator>().SetTrigger("CloseP");
		configbtn.SetActive(true);
		searchbtn.SetActive(true);
	}
}