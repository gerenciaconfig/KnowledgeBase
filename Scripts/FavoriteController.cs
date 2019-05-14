using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;


[Serializable]
public class Franchise
{
    public string descricao;
    public Int64 id;
}

public class Favorites
{
    public Int64 kidID { get; set; }
    public List<Int64> leveis { get; set; }
}

public class FavoriteController : MonoBehaviour
{
    public FranchiseImages frachiseImagesData;
    public GameObject favButtonPrefab;
    public Transform favGrid;

    public ActivitiesFranchises activitiesFranchises;

    public static List<Int64> favoriteFranchiseIdList = new List<long>();

    private GameObject thisCanvas;
    private GameObject nextCanvas;


    private void OnEnable()
    {
        LoadAllFranchises();
    }

    public void LoadAllFranchises()
    {
        if (favGrid.childCount != 0)
        {
            return;
        }


        foreach (var item in ActivitiesFranchises.franchises)
        {
            var buttonComponents = Instantiate(favButtonPrefab, favGrid).GetComponent<FavoriteButton>();
            buttonComponents.favoriteFranchise = item;

            if (frachiseImagesData.imageList.ContainsKey(item.id.ToString()))
            {
                buttonComponents.favImage.sprite = frachiseImagesData.imageList[item.id.ToString()];
            }
            else
            {
                buttonComponents.favImage.sprite = frachiseImagesData.defaultImage;
            }
        }
    }

    public async void PostFavorites()
    {
        var currentKid = PlayerPrefs.GetString(ConstantClass.CURRENT_KID);

        Favorites fav = new Favorites
        {
            kidID = JsonConvert.DeserializeObject<KidProfile>(currentKid).id,
            leveis = favoriteFranchiseIdList
        };

        var dadosPOST = JsonConvert.SerializeObject(fav);
        Debug.Log(dadosPOST);

        var dados = Encoding.UTF8.GetBytes(dadosPOST);
        var requisicaoWeb = WebRequest.CreateHttp("http://187.102.147.18:1050/LevelResult/Favorites");

        requisicaoWeb.Method = "POST";
        requisicaoWeb.ContentType = "application/json";
        requisicaoWeb.ContentLength = dados.Length;
        requisicaoWeb.UserAgent = "RequisicaoWebDemo";

        Loading.instance.StartLoading();

        try
        {
            //precisamos escrever os dados post para o stream
            using (var stream = requisicaoWeb.GetRequestStream())
            {
                stream.Write(dados, 0, dados.Length);
                stream.Close();
            }

            using (var resposta = await requisicaoWeb.GetResponseAsync())
            {
                Debug.Log(resposta);
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();

                var userConvert = JsonConvert.DeserializeObject<PersonDTO>(objResponse.ToString());

                PlayerPrefs.SetString(ConstantClass.CURRENT_USER, objResponse.ToString());
                Debug.Log(PlayerPrefs.GetString(ConstantClass.CURRENT_USER));
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
                    //return false;
                }
                else if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.InternalServerError)
                {
                    Debug.LogError("WebException, HttpWebResponse 500 Internal Server Error");
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
            Loading.instance.StopLoading();

            thisCanvas.SetActive(false);
            nextCanvas.SetActive(true);
        }

        //return true;
    }
}
