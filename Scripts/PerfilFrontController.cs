using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class PerfilFrontController : MonoBehaviour
{
    public GameObject perfilPrefab;
    public GameObject criarPerfilButton;
    public GameObject gridPerfil;

    [Space(10)]
    public PerfilImageScriptable perfilImageList;

    private void OnEnable()
    {
        Debug.Log(this.gameObject.name);

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Message.instance.Show(MessageClass.ERROR_INTERNET_NO);
            return;
        }

        ClearGridPerfil();
        CarregarPerfis();
        criarPerfilButton.transform.SetAsLastSibling();
    }

    public virtual bool CarregarPerfis()
    {
        Debug.Log("Entrou aqui 1");
        PersonDTO currentUser = new PersonDTO();
        List<KidProfile> kidList = new List<KidProfile>();

        Debug.Log(PlayerPrefs.GetString(ConstantClass.CURRENT_USER));
        CurrentStatsInfo.currentUser = JsonConvert.DeserializeObject<PersonDTO>(PlayerPrefs.GetString(ConstantClass.CURRENT_USER));
        currentUser = CurrentStatsInfo.currentUser;

        var requisicaoWeb = WebRequest.CreateHttp(ConstantClass.SERVER_URL + "Person/GetChildren?parentID=" + currentUser.id);

        requisicaoWeb.Method = "GET";
        requisicaoWeb.ContentType = "application/json";
        requisicaoWeb.UserAgent = "RequisicaoWebDemo";

        try
        {
            using (var resposta = requisicaoWeb.GetResponse())
            {
                Debug.Log("Entrou aqui 2");
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();

                kidList = JsonConvert.DeserializeObject<List<KidProfile>>(objResponse.ToString());

                streamDados.Close();
                resposta.Close();
            }
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.BadRequest)
                {
                    Debug.LogError("Tipo ou parâmetro do objeto não corresponde ao servidor. 400 Bad Request");
                    Message.instance.Show(MessageClass.ERROR_BAD_REQUEST);

                    return false;
                }
                else if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.InternalServerError)
                {
                    Debug.LogError("WebException, HttpWebResponse 500 Internal Server Error");
                    Message.instance.Show(MessageClass.ERROR_INTERNAL_SERVER);
                    return false;
                }
                else if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.GatewayTimeout)
                {
                    Debug.LogError("WebException, HttpWebResponse 504 Gateway Timeout");
                    Message.instance.Show(MessageClass.ERROR_GATEWAY_TIMEOUT);
                    return false;
                }
                else
                {
                    Debug.Log(((HttpWebResponse)ex.Response).StatusCode);
                    return false;
                }
            }
        }

        if (kidList != null)
        {
            foreach (var item in kidList)
            {
                PerfilButtonClass perfilButton = Instantiate(perfilPrefab, gridPerfil.transform).GetComponent<PerfilButtonClass>();
                perfilButton.kid = item;
                perfilButton.perfilName.text = item.name;

                foreach (var image in perfilImageList.perfilImageList)
                {
                    if (image.Key == item.imageId)
                    {
                        perfilButton.perfilImage.sprite = image.Value;
                        break;
                    }
                }
            }
        }
        return true;
    }

    public void ClearGridPerfil()
    {
        foreach (Transform item in gridPerfil.transform)
        {
            if (item.name != criarPerfilButton.name)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
