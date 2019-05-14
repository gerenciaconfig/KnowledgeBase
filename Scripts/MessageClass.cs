using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MessageClass
{
    //public const string 
    public const string ERROR_CEP_EMPTY = "O campo 'CEP' está vazio!";
    public const string ERROR_CEP_INVALID = "O 'CEP' informado é inválido!";
    public const string ERROR_COMPLEMENT_EMPTY = "O campo 'Complemento' está vazio!";
    public const string ERROR_CPF_EMPTY = "O campo 'CPF' está vazio!";
    public const string ERROR_CPF_INVALID = "O 'CPF' informado é inválido!";
    public const string ERROR_CPF_ALREADY_EXISTS = "O 'CPF' informado já está cadastrado!";
    public const string ERROR_NOME_EMPTY = "O campo 'Nome' está vazio!";
    public const string ERROR_EMAIL_EMPTY = "O campo 'Email' está vazio!";
    public const string ERROR_EMAIL_INVALID = "O 'Email' informado é inválido!";
    public const string ERROR_EMAIL_ALREADY_EXISTS = "O 'Email' informado já está cadastrado!";
    public const string ERROR_TELEFONE_EMPTY = "O campo 'Telefone' está vazio!";
    public const string ERROR_CUPOM_INVALID = "O 'Cupom' informado é inválido!";
    public const string ERROR_PASSWORD_EMPTY = "O campo 'Senha' está vazio!";
    public const string ERROR_CONFIRM_PASSWORD_EMPTY = "O campo 'Confirmar Senha' está vazio!";
    public const string ERROR_PASSWORD_DONT_MATCH = "As 'Senhas' informadas não são iguais!";
    public const string ERROR_PASSWORD_WORNG_PATTERN = "A senha informada deve ter de 6 a 18 dígitos!";
    public const string ERROR_USER_TERMS_NOT_CHECKED = "É necessário aceitar os 'Termos de Serviço' para prosseguir!";
    public const string ERROR_PIN_EMPTY = "O campo 'PIN' está vazio!";
    public const string ERROR_CONFIRM_PIN_EMPTY = "O campo 'Confirmar PIN' está vazio!";
    public const string ERROR_CONFIRM_NEED_FOUR_DIGITS = "O 'PIN' informado necessita ter 4 dígitos!";
    public const string ERROR_PIN_DONT_MATCH = "Os 'PINs' informadas não são iguais!";
    public const string ERROR_NOME_CRIANCA_EMPTY = "O campo 'Nome da criança' está vazio!";
    public const string ERROR_DATA_NASCIMENTO_EMPTY = "O campo 'Data do nascimento' está vazio!";
    public const string ERROR_DATA_NASCIMENTO_GREATER = "A 'Data do nascimento' informada é maior que o dia atual!";
    public const string ERROR_NO_IMAGE_SELECT = "É necessário escolher uma imagem para o perfil!";
    public const string ERROR_EMAIL_DONT_EXIST = "O 'Email' informado não está cadastrado!";
    public const string ERROR_PASSWORD_WRONG = "A senha informada está incorreta!";
    public static string ERROR_INTERNET_NO = "Sem acesso a internet!";
    public static string ERROR_INTERNAL_SERVER = "Erro interno no servidor, favor tentar novamente!";
    public static string ERROR_GATEWAY_TIMEOUT = "Erro timeout no servidor, favor tentar novamente!";
    public static string ERROR_BAD_REQUEST = "Bad Request Error!";
    public static string ERROR_INCORRECT_PIN = "PIN incorreto, favor tentar novamente!";
    internal static string ERROR_UNAUTHORIZED = "Usuário não autorizado!";
}