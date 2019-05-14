using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcolabs.Home;

public class HudManagerBoolSetter
{
    public HudStates ilhasObject;
    public HudStates canvasEscolha;
    public HudStates bgHud;
    public HudStates canvasHome;
    public HudStates backButton;
    public HudStates vulcan;

    public HudManagerBoolSetter(HudStates ilhasObject, HudStates canvasEscolha, HudStates bgHud,
        HudStates canvasHome, HudStates backButton, HudStates vulcan)
    {
        this.ilhasObject = ilhasObject;
        this.canvasEscolha = canvasEscolha;
        this.bgHud = bgHud;
        this.canvasHome = canvasHome;
        this.backButton = backButton;
        this.vulcan = vulcan;
    }
}

public enum HudStates
{
    DoNothing,
    Activate,
    Deactivate
}

public class HudManager : MonoBehaviour
{

    public static GameObject ilhasObject;
    public static GameObject canvasEscolha;
    public static GameObject bgHud;
    public static GameObject canvasHome;
    public static GameObject backButton;
    public static GameObject canvasAdP;
    public static GameObject canvasPerfilKid;
    public static GameObject canvasPin;
    public static GameObject canvasFade;
    public GameObject vulcanWorld;
    public GameObject GlaciusWorld;
    public GameObject ForestWorld;
    public GameObject TechWorld;
    public GameObject BeachWorld;
    public static Camera cam;

    public static GameObject host;
    public static GameObject clouds;
    public static GameObject worlds;

    private void Awake()
    {
        SetStaticObjs();
    }

    private void Start()
    {
        if (HomeIlhasHelper.firstAcess)
        {
            ilhasObject.gameObject.SetActive(false);
            canvasHome.gameObject.SetActive(false);

            canvasAdP.gameObject.SetActive(false);
            canvasPerfilKid.gameObject.SetActive(false);
            canvasPin.gameObject.SetActive(false);
            //canvasFade.gameObject.SetActive(false);

            PlayerPrefs.SetInt("MusicOff", 0);
        }
        else
        {
            BackFromActivity();
        }
    }

    public static void SetStaticObjs()
    {
        if (canvasEscolha == null)
        {
            canvasEscolha = GameObject.FindGameObjectWithTag("CanvasEscolha");
        }
        if (bgHud == null)
        {
            bgHud = GameObject.FindGameObjectWithTag("BgHud");
        }
        if (ilhasObject == null)
        {
            ilhasObject = GameObject.FindGameObjectWithTag("HomeIlhas");
        }
        if (canvasHome == null)
        {
            canvasHome = GameObject.FindGameObjectWithTag("CanvasHome");
        }
        if (backButton == null)
        {
            backButton = GameObject.FindGameObjectWithTag("BackButton");
        }
        if (canvasAdP == null)
        {
            canvasAdP = GameObject.FindGameObjectWithTag("CanvasAdP");
        }
        if (canvasPerfilKid == null)
        {
            canvasPerfilKid = GameObject.FindGameObjectWithTag("CanvasPerfilKid");
        }
        if (canvasPin == null)
        {
            canvasPin = GameObject.FindGameObjectWithTag("CanvasPin");
        }
        if (host == null)
        {
            host = GameObject.FindGameObjectWithTag("Host");
        }
        if (clouds == null)
        {
            clouds = GameObject.FindGameObjectWithTag("Clouds");
        }
        if (worlds == null)
        {
            worlds = GameObject.FindGameObjectWithTag("Worlds");
        }
        if (canvasFade == null)
        {
            canvasFade = GameObject.FindGameObjectWithTag("CanvasFade");
        }
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    public static void BackFromActivity()
    {
        SetStaticObjs();
        bgHud.gameObject.SetActive(false);
        canvasEscolha.gameObject.SetActive(false);
        canvasAdP.gameObject.SetActive(false);
        canvasPerfilKid.gameObject.SetActive(false);
        canvasPin.gameObject.SetActive(false);
        ilhasObject.gameObject.SetActive(false);
        cam.GetComponent<NewCameraMovement>().enabled = true;
        //canvasFade.gameObject.SetActive(false);
    }

    public static void ClickPerfil()
    {
        backButton.gameObject.SetActive(false);
        bgHud.gameObject.SetActive(false);
        canvasEscolha.gameObject.SetActive(false);
        canvasHome.gameObject.SetActive(true);
        host.gameObject.SetActive(true);
        IntroEnable();
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicVolumeChecker>().CheckVolume();
        DisableWorlds();
        HomeIlhasHelper.reEnter = true;
        ilhasObject.gameObject.SetActive(true);
    }

    public static void IntroEnable()
    {
        canvasHome.gameObject.SetActive(false);
        cam.GetComponent<Animator>().enabled = true;        
        host.GetComponent<Animator>().enabled = true;
        clouds.GetComponent<Animator>().enabled = true;
        cam.GetComponent<Animator>().SetTrigger("Intro");
    }

    public static void ClickPerfilParent()
    {
        canvasAdP.gameObject.SetActive(false);
        canvasPerfilKid.gameObject.SetActive(true);
    }

    public void BackFromIsland()
    {
        ilhasObject.gameObject.GetComponent<HomeIlhasHelper>().ToggleGameCamera();
        cam.gameObject.GetComponent<NewCameraMovement>().enabled = false;
        ilhasObject.gameObject.SetActive(true);

        DisableWorlds();

        Camera.main.GetComponent<IntroHelper>().EnableColliders();

        host.gameObject.SetActive(true);
        host.GetComponent<Animator>().enabled = true;
        cam.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        cam.gameObject.GetComponent<CameraParallax>().enabled = true;
        backButton.gameObject.SetActive(false);

        if (canvasHome.transform.GetChild(0).GetComponent<Animator>().GetInteger("state") == 1)
        {
            canvasHome.transform.GetChild(0).GetComponent<Animator>().SetInteger("state", 2);
        }
        
        for (int i = 0; i < cam.GetComponent<CamLightsHelper>().camWorldLights.transform.childCount; i++)
        {
            Destroy(cam.GetComponent<CamLightsHelper>().camWorldLights.transform.GetChild(i).transform.gameObject);
        }
    }

    public static void GoToIsland()
    {
        cam.gameObject.GetComponent<CameraParallax>().enabled = false;
        cam.gameObject.GetComponent<Camera>().fieldOfView = 60;
        cam.gameObject.transform.position = new Vector3(0, 0, -10);
        cam.gameObject.GetComponent<NewCameraMovement>().enabled = true;
        ilhasObject.gameObject.SetActive(false);

        //host.gameObject.SetActive(false);

        cam.gameObject.transform.GetChild(0).gameObject.SetActive(true);

    }

    public void AcessAdP()
    {

        canvasEscolha.gameObject.SetActive(false);
        bgHud.gameObject.SetActive(true);

        vulcanWorld.gameObject.SetActive(false);
        GlaciusWorld.gameObject.SetActive(false);
        ForestWorld.gameObject.SetActive(false);
        BeachWorld.gameObject.SetActive(false);
        TechWorld.gameObject.SetActive(false);

        ilhasObject.gameObject.SetActive(false);
        canvasHome.gameObject.SetActive(false);
        canvasAdP.gameObject.SetActive(true);

        /*
        SetHudActivation(new HudManagerBoolSetter(HudStates.DoNothing, HudStates.DoNothing, HudStates.Activate, 
            HudStates.Activate, HudStates.Activate, HudStates.Activate));
        */
    }

    public static void PinAcess()
    {
        canvasEscolha.gameObject.SetActive(false);
        bgHud.gameObject.SetActive(true);

        ilhasObject.gameObject.SetActive(false);
        canvasHome.gameObject.SetActive(false);
        canvasAdP.gameObject.SetActive(true);
        canvasPin.gameObject.SetActive(false);
        host.gameObject.SetActive(false);
        cam.GetComponent<Animator>().enabled = false;
        canvasHome.transform.GetChild(0).GetComponent<BtConfigHelper>().ChangeClicked();

        cam.transform.GetChild(0).gameObject.SetActive(false);
    }

    public static void DisableWorlds()
    {
        for (int i = 0; i < worlds.transform.childCount; i++)
        {
            worlds.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void SetHudActivation(HudManagerBoolSetter boolSetter)
    {
        switch (boolSetter.ilhasObject)
        {
            case HudStates.DoNothing:
                break;
            case HudStates.Activate:
                ilhasObject.SetActive(true);
                break;
            case HudStates.Deactivate:
                ilhasObject.SetActive(false);
                break;
        }
    }
}
