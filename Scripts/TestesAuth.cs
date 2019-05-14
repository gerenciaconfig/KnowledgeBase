using UnityEngine;
using System.IO;
using System.Net;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Text;

public class User
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string Sexo { get; set; }
}

public class TestesAuth : MonoBehaviour
{
    public InputField inputId;
    public Text jsonText;
    public Text responseText;

    // Start is called before the first frame update
    void Start()
    {
        GetAllPeople();
    }

    public void GetUserById()
    {
        var requisicaoWeb = WebRequest.CreateHttp("http://jsonplaceholder.typicode.com/posts/" + inputId.text);
        requisicaoWeb.Method = "GET";
        requisicaoWeb.UserAgent = "RequisicaoWebDemo";

        using (var resposta = requisicaoWeb.GetResponse())
        {
            var streamDados = resposta.GetResponseStream();
            StreamReader reader = new StreamReader(streamDados);
            object objResponse = reader.ReadToEnd();

            var user = JsonConvert.DeserializeObject<User>(objResponse.ToString());

            var didnei = JsonConvert.SerializeObject(user);
            Debug.Log(didnei);

            jsonText.text = ("User ID: " + user.UserId +
                "\n\nMain ID: " + user.Id +
                "\n\nTitle: " + user.Title +
                "\n\nBody: " + user.Body);

            responseText.text = objResponse.ToString();

            streamDados.Close();
            resposta.Close();
        }
    }

    public void EnviaRequisicaoPOST()
    {
        User user1 = new User
        {
            UserId = 1,
            Title = "Lorem Ipsum Dolor Siamente.",
            Body = "Meu corpo minhas regras."
        };

        string dadosPOST = JsonConvert.SerializeObject(user1);
        var dados = Encoding.UTF8.GetBytes(dadosPOST);
        var requisicaoWeb = WebRequest.CreateHttp("http://jsonplaceholder.typicode.com/posts");

        requisicaoWeb.Method = "POST";
        requisicaoWeb.ContentType = "application/x-www-form-urlencoded";
        requisicaoWeb.ContentLength = dados.Length;
        requisicaoWeb.UserAgent = "RequisicaoWebDemo";

        //precisamos escrever os dados post para o stream
        using (var stream = requisicaoWeb.GetRequestStream())
        {
            stream.Write(dados, 0, dados.Length);
            stream.Close();
        }

        //ler e exibir a resposta
        using (var resposta = requisicaoWeb.GetResponse())
        {
            var streamDados = resposta.GetResponseStream();
            StreamReader reader = new StreamReader(streamDados);
            object objResponse = reader.ReadToEnd();

            var user = JsonConvert.DeserializeObject<User>(objResponse.ToString());
            Debug.Log("User ID:" + user.Id + "Title: " + user.Title + "Body: " + user.Body);

            streamDados.Close();
            resposta.Close();
        }
    }

    #region ServidorDisney
    public static void GetAllPeople()
    {
        var requisicaoWeb = WebRequest.CreateHttp(ConstantClass.SERVER_URL + "Person");
        requisicaoWeb.Method = "GET";
        requisicaoWeb.UserAgent = "RequisicaoWebDemo";

        using (var resposta = requisicaoWeb.GetResponse())
        {
            var streamDados = resposta.GetResponseStream();
            StreamReader reader = new StreamReader(streamDados);
            object objResponse = reader.ReadToEnd();
            Debug.Log(objResponse.ToString());

            streamDados.Close();
            resposta.Close();
        }
    }

    public static void PostCreatePerson()
    {
        PersonDTO user = new PersonDTO
        {
            contact = new ContactDTO
            {
                address = "123123123213",
                complement = "asdasdasdadsd",
                city = "adad",
                email = "asdasdads",
                phone = "122132132132",
                postalCode = "54546546546",
                uf = "asdasdadsa"
            },
            name = "asdasda",
            origin = "asdadasdads",
            cpf = "123131231231",
            pin = "1234",
            //password = passField.text,
            kids = null
        };

        var dadosPOST = JsonConvert.SerializeObject(user);

        Debug.Log(dadosPOST.ToString());

        var dados = Encoding.UTF8.GetBytes(dadosPOST);
        var requisicaoWeb = WebRequest.CreateHttp(ConstantClass.SERVER_URL + "Person");

        requisicaoWeb.Method = "POST";
        requisicaoWeb.ContentType = "application/json";
        requisicaoWeb.ContentLength = dados.Length;
        requisicaoWeb.UserAgent = "RequisicaoWebDemo";

        //precisamos escrever os dados post para o stream
        using (var stream = requisicaoWeb.GetRequestStream())
        {
            stream.Write(dados, 0, dados.Length);
            stream.Close();
        }
 

        //ler e exibir a resposta
        using (var resposta = requisicaoWeb.GetResponse())
        {
            Debug.Log(resposta);
            var streamDados = resposta.GetResponseStream();
            StreamReader reader = new StreamReader(streamDados);
            object objResponse = reader.ReadToEnd();

            var user2 = JsonConvert.DeserializeObject<PersonDTO>(objResponse.ToString());

            streamDados.Close();
            resposta.Close();
        }
        Debug.Log(dadosPOST);
    }

    #endregion
}
