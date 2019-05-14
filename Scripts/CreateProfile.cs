using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class CreateProfile : MonoBehaviour
{
    public static Int64 choosenImageId;
    public static string genre;

    [Header("Telas")]
    public GameObject tela1;
    public GameObject tela2;
    public GameObject nextTela;

    [Header("Tela 1")]
    public TMP_InputField nomeCriancaField;
    public TMP_InputField dataField;


    public void ValidateTela_1()
    {
        if (ValidateNomeCrianca(nomeCriancaField.text) == false)
            return;

        if (ValidateData(dataField.text) == false)
            return;

        tela1.SetActive(false);
        tela2.SetActive(true);
    }

    public async void ValidateTela_2()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Message.instance.Show(MessageClass.ERROR_INTERNET_NO);
            return;
        }

        if (await PutCreateChild(nomeCriancaField.text, dataField.text, genre, choosenImageId) == false)
        {
            return;
        }

        tela2.SetActive(false);
        nextTela.SetActive(true);

        Debug.Log("Criança cadastrada com sucesso!");
    }

    #region Validations
    public bool ValidateNomeCrianca(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            Message.instance.Show(MessageClass.ERROR_NOME_CRIANCA_EMPTY);
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool ValidateData(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            Message.instance.Show(MessageClass.ERROR_DATA_NASCIMENTO_EMPTY);
            return false;
        }

        DateTime dataParse = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        if (dataParse > DateTime.Today.Date)
        {
            Message.instance.Show(MessageClass.ERROR_DATA_NASCIMENTO_GREATER);
            return false;
        }

        return true;
    }
    #endregion

    #region Server Methods

    public async Task<bool> PutCreateChild(string nomeCrianca, string dataNascimento, string genero, Int64 imageId)
    {
        PersonDTO currentUser = new PersonDTO();
        try
        {
            currentUser = JsonConvert.DeserializeObject<PersonDTO>(PlayerPrefs.GetString(ConstantClass.CURRENT_USER));
            Debug.Log(PlayerPrefs.GetString(ConstantClass.CURRENT_USER));

            //Atualizamos o objeto do usuário adicionando mais uma criança ao objeto do usuário.
            if (currentUser.kids == null)
            {
                currentUser.kids = new List<KidProfile>();
            }

            currentUser.kids.Add(new KidProfile
            {
                name = nomeCrianca,
                birthDate = dataNascimento,
                genre = genero,
                imageId = imageId,
                franchise = FavoriteController.favoriteFranchiseIdList
            });


            //Tentamos subir o objeto através de um PUT.
            var dadosPOST = JsonConvert.SerializeObject(currentUser);
            Debug.Log(dadosPOST);

            var dados = Encoding.UTF8.GetBytes(dadosPOST);
            var requisicaoWeb = WebRequest.CreateHttp(ConstantClass.SERVER_URL + "Person/");

            requisicaoWeb.Method = "PUT";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.ContentLength = dados.Length;
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            Loading.instance.StartLoading();

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

                KidProfile kid = userConvert.kids[userConvert.kids.Count - 1];
                var currentKid = JsonConvert.SerializeObject(kid);
                PlayerPrefs.SetString(ConstantClass.CURRENT_KID, currentKid.ToString());
                CurrentStatsInfo.currentKid = JsonConvert.DeserializeObject<KidProfile>(objResponse.ToString());
                Debug.Log(PlayerPrefs.GetString(ConstantClass.CURRENT_KID));

                PlayerPrefs.SetString(ConstantClass.CURRENT_USER, objResponse.ToString());
                CurrentStatsInfo.currentUser = JsonConvert.DeserializeObject<PersonDTO>(objResponse.ToString());
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
        finally
        {
            Loading.instance.StopLoading();
        }

        return true;
    }

    #endregion
}
public class KidProfile
{
    public Int64 id { get; set; }
    public string birthDate { get; set; }
    public string genre { get; set; }
    public Int64 imageId { get; set; }
    public string name { get; set; }
    public List<Int64> franchise { get; set; }
}
