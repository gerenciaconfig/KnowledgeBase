using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class ActivityAnalytics : MonoBehaviour
{
    private int rights;
    private int wrongs;
    private DateTime startTime;

    public bool isMasterClass;

    private void Awake()
    {
        if(isMasterClass)
        {
            LoadAndPostAll();
        }
    }

    #region Activities Functions
    private void ResetAnalytics()
    {
        rights = 0;
        wrongs = 0;
    }

    public void StartActivity()
    {
        ResetAnalytics();
        startTime = DateTime.Now;
    }

    public void AddRight()
    {
        rights++;
    }

    public void AddWrong()
    {
        wrongs++;
    }

    public void FinishActivity()
    {
        DateTime finishTime = DateTime.Now;

        LevelResults levelResults = new LevelResults(finishTime, startTime, rights, CurrentStatsInfo.currentActivity.levelDTO.id, wrongs, CurrentStatsInfo.currentKid.id);

        ConvertJsonAndSave(levelResults);
    }
    #endregion

    private async void ConvertJsonAndSave(LevelResults levelResults)
    {
        //Cria o json a partir do levelResults
        var json = JsonConvert.SerializeObject(levelResults);

        string backup = PlayerPrefs.GetString("AnalyticsBackup");
        backup += ((string)json + "&");

        //Tenta postar o json no servidor. Caso não tenha sucesso, salva no player prefs
        bool result = await Post(json);
        if (!result)
        {
            PlayerPrefs.SetString("AnalyticsBackup", backup);
        }
    }

    private async void LoadAndPostAll()
    {
        string[] jsons = LoadAll();

        string backup = PlayerPrefs.GetString("AnalyticsBackup");

        for (int i = 0; i < jsons.Length; i++)
        {
            if (!jsons[i].Equals(""))
            {
                bool result = await Post(jsons[i]);
                if(result)
                {
                    string toRemove = jsons[i] + "&";
                    backup = backup.Remove(backup.IndexOf(toRemove), toRemove.Length);
                }
            }
        }
        PlayerPrefs.SetString("AnalyticsBackup", backup);
    }

    //Carrega todos os jsons de LevelResult que estavam guardados no 
    private string[] LoadAll()
    {
        string[] jsonList = PlayerPrefs.GetString("AnalyticsBackup").Split('&');
        return jsonList;
    }

    //Posta o json do LevelResult no banco, retorna false se não conseguir
    private async Task<bool> Post(string json)
    {
        bool response;

        print("mandou");
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }

        try
        {
            var data = Encoding.UTF8.GetBytes(json);
            var webRequest = WebRequest.CreateHttp(ConstantClass.SERVER_URL + "LevelResult");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";
            webRequest.ContentLength = data.Length;
            webRequest.UserAgent = "RequisicaoWebDemo";

            //precisamos escrever os dados post para o stream
            using (var stream = webRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
                stream.Close();
            }

            //ler e exibir a resposta
            using (var resposta = await webRequest.GetResponseAsync())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();

                response = bool.Parse(objResponse.ToString());

                streamDados.Close();
                resposta.Close();
            }
        }
        catch (WebException ex)
        {
            return false;
        }        
        return response;
    }
}