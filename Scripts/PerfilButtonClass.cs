using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;

public class PerfilButtonClass : MonoBehaviour
{
    public KidProfile kid;
    public TextMeshProUGUI perfilName;
    public Image perfilImage;  

    public void PerfilAcess()
    {
        PlayerPrefs.SetString(ConstantClass.CURRENT_KID, JsonConvert.SerializeObject(kid));
        CurrentStatsInfo.currentKid = kid;
        HudManager.SetStaticObjs();
        HudManager.ClickPerfil();
    }

    public void PerfilAcessParent()
    {
        PlayerPrefs.SetString(ConstantClass.CURRENT_KID, JsonConvert.SerializeObject(kid));
        CurrentStatsInfo.currentKid = kid;
        HudManager.ClickPerfilParent();
    }
}
