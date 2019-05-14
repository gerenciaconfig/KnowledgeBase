using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using Arcolabs.Home;

[System.Serializable]
public class ServerActivitiesList : SavedClass
{
    public static List<LevelDTO> serverActivities;

    public override void UpdateClass()
    {
        var requisicaoWeb = WebRequest.CreateHttp(ConstantClass.SERVER_URL + "Level");
        requisicaoWeb.Method = "GET";
        requisicaoWeb.UserAgent = "RequisicaoWebDemo";

        try
        {
            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();

                serverActivities = JsonConvert.DeserializeObject<List<LevelDTO>>(objResponse.ToString());

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
                    Debug.Log("Tipo ou parâmetro do objeto não corresponde ao servidor. 404 Bad Request");
                    return;
                }
                else
                {
                    throw;
                }
            }

        }
        finally
        {
            CurrentStatsInfo.isDownloading = false;
        }
    }
}

