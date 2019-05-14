using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class MenuStateController : SerializedMonoBehaviour 
{

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public enum MenuState{
		Intro = 0, Login = 1, Cadastro1 = 2, Cadastro2 = 3, Codigo = 4, CriarPerfil = 5, EscolherPerfil = 6, Home = 7, JogosPesquisa = 8, AreaDosPaisAcesso = 9, AreaDosPais = 10, JogosUniverso = 11
	};

	public MenuState actualState;

	[SerializeField]
	public Dictionary<MenuState, GameObject> menus;
	
	public void ChangeState(string newState)
	{
		actualState = ParseEnum(newState);
		ResetMenu();
	}

	public void ChangeState2(MenuState newState)
	{
		//actualState = NewState;
		ResetMenu();
	}

	public MenuState ParseEnum(string myString)
    {
        try
        {
            MenuState enumerable = (MenuState)System.Enum.Parse(typeof(MenuState), myString);
			return enumerable;
            //Foo(enumerable); //Now you have your enum, do whatever you want.
        }
        catch (System.Exception)
        {
            Debug.LogErrorFormat("Parse: Can't convert {0} to enum, please check the spell.", myString);
			return MenuState.Intro;
        }
    }

	private void ResetMenu()
	{
		foreach(var item in menus.Values)
		{
			item.gameObject.SetActive(false);
		}

		menus[actualState].SetActive(true);
	}


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}