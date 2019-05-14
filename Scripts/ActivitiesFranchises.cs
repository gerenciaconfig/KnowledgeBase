using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

[System.Serializable]
public class ActivitiesFranchises : SavedClass
{
    [SerializeField]
    public static List<Franchise> franchises;

    public override async void UpdateClass()
    {
        var requisicaoWeb = WebRequest.CreateHttp(ConstantClass.SERVER_URL + "Level/Franchise");

        requisicaoWeb.Method = "GET";
        requisicaoWeb.ContentType = "application/json";
        requisicaoWeb.UserAgent = "RequisicaoWebDemo";

        try
        {
            using (var resposta = await requisicaoWeb.GetResponseAsync())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();

                franchises = JsonConvert.DeserializeObject<List<Franchise>>(objResponse.ToString());

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

                    //return false;
                }
                else if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.InternalServerError)
                {
                    Debug.LogError("WebException, HttpWebResponse 500 Internal Server Error");
                    Message.instance.Show(MessageClass.ERROR_INTERNAL_SERVER);
                    //return false;
                }
                else if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.GatewayTimeout)
                {
                    Debug.LogError("WebException, HttpWebResponse 504 Gateway Timeout");
                    Message.instance.Show(MessageClass.ERROR_GATEWAY_TIMEOUT);
                    //return false;
                }
                else
                {
                    Debug.Log(((HttpWebResponse)ex.Response).StatusCode);
                    //return false;
                }
            }
        }
        finally
        {
            //Loading.instance.StopLoading();
            CurrentStatsInfo.isDownloading = false;
        }
    }
}
