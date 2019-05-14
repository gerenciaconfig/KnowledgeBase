using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace Arcolabs.Home { 
    public class LoginDisney : MonoBehaviour
    {
        [Header("Telas de Login")]
        public GameObject tela_1;

        [Header("Tela 1")]
        public TMP_InputField emailField;
        public TMP_InputField passField;

        public LoadingScriptHelper loadHelper;

        private Int64 userId;

        public async void ValidateTela_1()
        {
            if (await ValidateLogin(emailField.text, passField.text) == false)
                return;

            if (await ValidateLogin(userId) == false)
                return;

            loadHelper.GoToHome();
            //SCREEN 1 AUTH SUCESS
            //tela_1.SetActive(false);
            //tela_2.SetActive(true);
        }

        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                Message.instance.Show(MessageClass.ERROR_EMAIL_EMPTY);
                return false;
            }

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                Message.instance.Show(MessageClass.ERROR_EMAIL_INVALID);
                return false;
            }
            catch (ArgumentException)
            {
                Message.instance.Show(MessageClass.ERROR_EMAIL_INVALID);
                return false;
            }

            try
            {
                if (!Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
                {
                    Message.instance.Show(MessageClass.ERROR_EMAIL_INVALID);
                    return false;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                Message.instance.Show(MessageClass.ERROR_EMAIL_INVALID);
                return false;
            }
            /*
            var requisicaoWeb = WebRequest.CreateHttp(ConstantClass.SERVER_URL + "Person/Email?email=" + email);

            requisicaoWeb.Method = "GET";
            requisicaoWeb.ContentType = "text";
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();

                if (bool.Parse(objResponse.ToString()) == true)
                {
                    Message.instance.Show(MessageClass.ERROR_EMAIL_DONT_EXIST);
                    return false;
                }

                streamDados.Close();
                resposta.Close();
            }
            */
            return true;
        }

        public bool ValidatePassword(string pass)
        {
            if (string.IsNullOrWhiteSpace(pass))
            {
                Message.instance.Show(MessageClass.ERROR_PASSWORD_EMPTY);
                return false;
            }
            else
            {
                return true;
            }
        }


        public async Task<bool> ValidateLogin(string email, string pass)
        {
            if (ValidateEmail(emailField.text) == false)
                return false;

            if (ValidatePassword(passField.text) == false)
                return false;

            Login login = new Login
            {
                username = email,
                password = pass
            };

            var dadosPOST = JsonConvert.SerializeObject(login);
            Debug.Log(dadosPOST);

            var dados = Encoding.UTF8.GetBytes(dadosPOST);
            var requisicaoWeb = WebRequest.CreateHttp(ConstantClass.SERVER_URL + "auth/login");

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
                
                    userId = Int64.Parse(objResponse.ToString());
                    Debug.Log(userId);

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
                    else if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Debug.LogError("WebException, HttpWebResponse 401 Unauthorized");
                        Message.instance.Show(MessageClass.ERROR_UNAUTHORIZED);
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

            //TODO VALIDAR LOGIN COM O BANCO DE DADOS
            Debug.Log("USUÁRIO LOGADO COM SUCESSO");
            return true;
        }

        public async Task<bool> ValidateLogin(Int64 _userId)
        {
            var requisicaoWeb = WebRequest.CreateHttp(ConstantClass.SERVER_URL + "Person/" + _userId);

            requisicaoWeb.Method = "GET";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            using (var resposta = await requisicaoWeb.GetResponseAsync())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();

                CurrentStatsInfo.currentUser = JsonConvert.DeserializeObject<PersonDTO>(objResponse.ToString());
                PlayerPrefs.SetString(ConstantClass.CURRENT_USER, JsonConvert.SerializeObject(CurrentStatsInfo.currentUser));

                streamDados.Close();
                resposta.Close();
            }

            return true;
        }

        public class Login
        {
            public string username;
            public string password;
        }
    }
}
