using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Arcolabs.Home;

public class RegisterDisney : MonoBehaviour
{
    [Header("Telas de Cadastro")]
    public GameObject tela_1;
    public GameObject tela_2;
    public GameObject tela_3;
    public GameObject tela_4;
    public GameObject tela_5;

    [Header("Tela 1")]
    public TMP_InputField nomeField;
    public TMP_InputField emailField;

    [Header("Tela 2")]
    public TMP_InputField passField;
    public TMP_InputField repeatPassField;

    [Header("Tela 3")]
    public TMP_InputField pinField;
    public TMP_InputField repeatPinField;

    [Header("Tela 4")]
    public TMP_InputField cepField;
    public TMP_InputField complementField;
    public TMP_InputField cpfField;
    [Space(10)]
    public TMP_InputField uf;
    public TMP_InputField cidade;
    public TMP_InputField endereco;

    [Header("Tela 5")]
    public TMP_InputField telefoneField;
    public TMP_InputField cupomField;
    public Toggle termsToggle;
    public string nextSceneName;

    private void Start()
    {
        //TestesAuth.GetAllPeople();
        //TestesAuth.PostCreatePerson();
    }


    #region Screen Validations
    public void ValidateTela_1()
    {
        if (ValidateNome(nomeField.text) == false)
            return;

        if (ValidateEmail(emailField.text) == false)
            return;

        //SCREEN 1 AUTH SUCESS
        tela_1.SetActive(false);
        tela_2.SetActive(true);
    }

    public void ValidateTela_2()
    {
        if (ValidatePassword(passField.text, repeatPassField.text) == false)
            return;

        //SCREEN 2 AUTH SUCESS
        tela_2.SetActive(false);
        tela_3.SetActive(true);
    }

    public void ValidateTela_3()
    {
        if (ValidatePin(pinField.text, repeatPinField.text) == false)
            return;

        //SCREEN 3 AUTH SUCESS
        tela_3.SetActive(false);
        tela_4.SetActive(true);
    }

    public async void ValidateTela_4()
    {
        if (await ValidateCep(cepField.text) == false)
            return;

        if (ValidateComplement(complementField.text) == false)
            return;

        if (ValidateCpf(cpfField.text) == false)
            return;

        //SCREEN 4 AUTH SUCESS
        tela_4.SetActive(false);
        tela_5.SetActive(true);
    }

    public async void ValidateTela_5()
    {
        if (ValidateTelefone(telefoneField.text) == false)
            return;

        if (ValidateCupom(cupomField.text) == false)
            return;

        if (ValidateUserTerms(termsToggle) == false)
            return;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Message.instance.Show(MessageClass.ERROR_INTERNET_NO);
            return;
        }

        if (await PostCreatePerson() == false)
            return;


        //StartCoroutine(PostCreatePersonCoroutine());

        //SCREEN 5 AUTH SUCESS
        Debug.Log("SUCESSO NO CADASTRO!");
        Arcolabs.Home.LoadingScript.nextScene = ConstantClass.HOME;
        SceneManager.LoadScene(ConstantClass.LOADING);
    }
    #endregion

    #region Validation
    public async void ValidateCepOnEndEdit(TMP_InputField cep)
    {
        if (string.IsNullOrEmpty(cep.text))
        {
            return;
        }

        var requisitionWeb = WebRequest.CreateHttp("https://viacep.com.br/ws/" + cep.text + "/json/");
        requisitionWeb.Method = "GET";
        requisitionWeb.UserAgent = "RequisicaoWebDemo";

        Loading.instance.StartLoading();

        try
        {
            using (var resposta = await requisitionWeb.GetResponseAsync())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();

                CepResponseObject cepResponseObject = JsonConvert.DeserializeObject<CepResponseObject>(objResponse.ToString());

                if (objResponse.ToString().Contains("erro"))
                {
                    Message.instance.Show(MessageClass.ERROR_CEP_INVALID);

                    endereco.text = null;
                    cidade.text = null;
                    uf.text = null;
                    return;
                }

                //TODO Data binding no formulário das informações recuperadas pelo CEP.
                cepField.text = cepResponseObject.cep;
                endereco.text = cepResponseObject.logradouro + ", " + cepResponseObject.bairro;
                cidade.text = cepResponseObject.localidade;
                uf.text = cepResponseObject.uf;

                streamDados.Close();
                resposta.Close();

                return;
            }
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.BadRequest)
                {
                    Message.instance.Show(MessageClass.ERROR_CEP_INVALID);
                }
            }

            endereco.text = null;
            cidade.text = null;
            uf.text = null;

            return;
        }
        finally
        {
            Loading.instance.StopLoading();
        }
    }

    public async Task<bool> ValidateCep(string cep)
    {
        if (string.IsNullOrEmpty(cep))
        {
            Message.instance.Show(MessageClass.ERROR_CEP_EMPTY);
            return false;
        }

        var requisitionWeb = WebRequest.CreateHttp("https://viacep.com.br/ws/" + cep + "/json/");
        requisitionWeb.Method = "GET";
        requisitionWeb.UserAgent = "RequisicaoWebDemo";

        Loading.instance.StartLoading();

        try
        {
            using (var resposta = await requisitionWeb.GetResponseAsync())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();

                CepResponseObject viaCepBrazil = JsonConvert.DeserializeObject<CepResponseObject>(objResponse.ToString());

                if (objResponse.ToString().Contains("erro"))
                {
                    Message.instance.Show(MessageClass.ERROR_CEP_INVALID);
                    return false;
                }

                //TODO Data binding no formulário das informações recuperadas pelo CEP.
                cepField.text = viaCepBrazil.cep;
                endereco.text = viaCepBrazil.logradouro + ", " + viaCepBrazil.bairro;
                cidade.text = viaCepBrazil.localidade;
                uf.text = viaCepBrazil.uf;

                streamDados.Close();
                resposta.Close();

                return true;
            }
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.BadRequest)
                {
                    Message.instance.Show(MessageClass.ERROR_CEP_INVALID);
                }
            }
            return false;
        }
        finally
        {
            Loading.instance.StopLoading();
        }
    }

    public bool ValidateComplement(string complement)
    {
        if (string.IsNullOrWhiteSpace(complement))
        {
            Message.instance.Show(MessageClass.ERROR_COMPLEMENT_EMPTY);
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool ValidateCpf(string cpf)
    {
        if (string.IsNullOrEmpty(cpf))
        {
            Message.instance.Show(MessageClass.ERROR_CPF_EMPTY);
            return false;
        }

        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf;
        string digito;
        int soma;
        int resto;

        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");

        if (cpf.Length != 11)
        {
            Message.instance.Show(MessageClass.ERROR_CPF_INVALID);
            return false;
        }

        switch (cpf)
        {
            case "12345678909":
                Message.instance.Show(MessageClass.ERROR_CPF_INVALID);
                return false;
            case "11111111111":
                Message.instance.Show(MessageClass.ERROR_CPF_INVALID);
                return false;
            case "00000000000":
                Message.instance.Show(MessageClass.ERROR_CPF_INVALID);
                return false;
            case "2222222222":
                Message.instance.Show(MessageClass.ERROR_CPF_INVALID);
                return false;
            case "33333333333":
                Message.instance.Show(MessageClass.ERROR_CPF_INVALID);
                return false;
            case "44444444444":
                Message.instance.Show(MessageClass.ERROR_CPF_INVALID);
                return false;
            case "55555555555":
                Message.instance.Show(MessageClass.ERROR_CPF_INVALID);
                return false;
            case "66666666666":
                Message.instance.Show(MessageClass.ERROR_CPF_INVALID);
                return false;
            case "77777777777":
                Message.instance.Show(MessageClass.ERROR_CPF_INVALID);
                return false;
            case "88888888888":
                Message.instance.Show(MessageClass.ERROR_CPF_INVALID);
                return false;
            case "99999999999":
                Message.instance.Show(MessageClass.ERROR_CPF_INVALID);
                return false;
        }

        tempCpf = cpf.Substring(0, 9);
        soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = resto.ToString();

        tempCpf = tempCpf + digito;

        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = digito + resto.ToString();

        if (!cpf.EndsWith(digito) == true)
        {
            Message.instance.Show(MessageClass.ERROR_CPF_INVALID);
            return false;
        }

        //TODO Trocar CPF?email=  para CPF?cpf=
        var requisicaoWeb = WebRequest.CreateHttp(ConstantClass.SERVER_URL + "Person/CPF?cpf=" + cpf);

        requisicaoWeb.Method = "GET";
        requisicaoWeb.ContentType = "text";
        requisicaoWeb.UserAgent = "RequisicaoWebDemo";

        using (var resposta = requisicaoWeb.GetResponse())
        {
            var streamDados = resposta.GetResponseStream();
            StreamReader reader = new StreamReader(streamDados);
            object objResponse = reader.ReadToEnd();

            if (bool.Parse(objResponse.ToString()) == false)
            {
                Message.instance.Show(MessageClass.ERROR_CPF_ALREADY_EXISTS);
                return false;
            }

            streamDados.Close();
            resposta.Close();
        }

        return true;
    }

    public bool ValidateNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            Message.instance.Show(MessageClass.ERROR_NOME_EMPTY);
            return false;
        }
        else
        {
            return true;
        }
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

        var requisicaoWeb = WebRequest.CreateHttp(ConstantClass.SERVER_URL + "Person/Email?email=" + email);

        requisicaoWeb.Method = "GET";
        requisicaoWeb.ContentType = "text";
        requisicaoWeb.UserAgent = "RequisicaoWebDemo";

        using (var resposta = requisicaoWeb.GetResponse())
        {
            var streamDados = resposta.GetResponseStream();
            StreamReader reader = new StreamReader(streamDados);
            object objResponse = reader.ReadToEnd();

            if (bool.Parse(objResponse.ToString()) == false)
            {
                Message.instance.Show(MessageClass.ERROR_EMAIL_ALREADY_EXISTS);
                return false;
            }

            streamDados.Close();
            resposta.Close();
        }

        return true;
    }

    public bool ValidateTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
        {
            Message.instance.Show(MessageClass.ERROR_TELEFONE_EMPTY);
            return false;
        }

        return true;
    }

    public bool ValidateCupom(string cupom)
    {
        if (string.IsNullOrWhiteSpace(cupom) == false)
        {
            //TODO VERIFICAR CUPOM NO BANCO
            if (cupom == "123456789")
            {
                Debug.Log("SUCESS: VALID CUPOM");
                return true;
            }
            else
            {
                Message.instance.Show(MessageClass.ERROR_CUPOM_INVALID);
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public bool ValidatePassword(string pass, string repeatPass)
    {
        if (string.IsNullOrWhiteSpace(pass))
        {
            Message.instance.Show(MessageClass.ERROR_PASSWORD_EMPTY);
            return false;
        }

        if (pass.Length < 6 || pass.Length > 18)
        {
            Message.instance.Show(MessageClass.ERROR_PASSWORD_WORNG_PATTERN);
            return false;
        }

        if (string.IsNullOrWhiteSpace(repeatPass))
        {
            Message.instance.Show(MessageClass.ERROR_CONFIRM_PASSWORD_EMPTY);
            return false;
        }

        if (pass != repeatPass)
        {
            Message.instance.Show(MessageClass.ERROR_PASSWORD_DONT_MATCH);
            return false;
        }

        return true;
    }

    public bool ValidateUserTerms(Toggle terms)
    {
        if (terms.isOn)
        {
            return true;
        }
        else
        {
            Message.instance.Show(MessageClass.ERROR_USER_TERMS_NOT_CHECKED);
            return false;
        }
    }

    public bool ValidatePin(string pin, string repeatPin)
    {
        if (string.IsNullOrWhiteSpace(pin))
        {
            Message.instance.Show(MessageClass.ERROR_PIN_EMPTY);
            return false;
        }

        if (pin.Length != 4)
        {
            Message.instance.Show(MessageClass.ERROR_CONFIRM_NEED_FOUR_DIGITS);
            return false;
        }

        if (string.IsNullOrWhiteSpace(repeatPin))
        {
            Message.instance.Show(MessageClass.ERROR_CONFIRM_PIN_EMPTY);
            return false;
        }

        if (pin == repeatPin)
        {
            return true;
        }
        else
        {
            Message.instance.Show(MessageClass.ERROR_CONFIRM_NEED_FOUR_DIGITS);
            return false;
        }
    }
    #endregion

    #region Server Methods
    public async Task<bool> PostCreatePerson()
    {
        //TODO Remove unused parameters from PersonDTO Class

        PersonDTO user = new PersonDTO
        {
            contact = new ContactDTO
            {
                address = endereco.text,
                complement = complementField.text,
                city = cidade.text,
                email = emailField.text,
                phone = telefoneField.text,
                postalCode = cepField.text,
                uf = uf.text
            },
            name = nomeField.text,
            origin = Application.platform.ToString(),
            cpf = cpfField.text,
            pin = pinField.text,
            password = passField.text,
            kids = null,
        };

        var dadosPOST = JsonConvert.SerializeObject(user);
        Debug.Log(dadosPOST);

        var dados = Encoding.UTF8.GetBytes(dadosPOST);
        var requisicaoWeb = WebRequest.CreateHttp(ConstantClass.SERVER_URL + "Person/");

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
                CurrentStatsInfo.currentUser = JsonConvert.DeserializeObject<PersonDTO>(objResponse.ToString());

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

#region Classes

public class Login
{

}

public class PersonDTO
{
    public Int64 id { get; set; }
    public ContactDTO contact { get; set; }
    public string cpf { get; set; }
    public List<KidProfile> kids { get; set; }
    public string name { get; set; }
    public string origin { get; set; }
    public string password { get; set; }
    public string pin { get; set; }
}

public class ContactDTO
{
    public string address { get; set; }
    public string city { get; set; }
    public string complement { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string postalCode { get; set; }
    public string uf { get; set; }
}

public class CepResponseObject
{
    public string cep;
    public string logradouro;
    public string complemento;
    public string bairro;
    public string localidade;
    public string uf;
    public string unidade;
    public string ibge;
    public string gia;
}

public class ErrorObj
{
    public int code { get; set; }
    public string message { get; set; }
}

public enum GenreDisney
{
    M,
    F
}

public enum PersonTypeDisney
{
    Owner,
    Dependent
}
#endregion