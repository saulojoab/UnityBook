using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour {

    Button botaoLogin;
    Database database;

	// Use this for initialization
	void Start ()
    {
        botaoLogin = GameObject.Find("btLogin").GetComponent<Button>();
        database = FindObjectOfType<Database>();

        // Adicionando o OnClick do botão de login - Saulo.
        botaoLogin.onClick.AddListener(Login);
	}
	
    /// <summary>
    /// Esse método chama o método de login do banco de dados - Saulo.
    /// </summary>
    void Login()
    {
        // Pegando os valores do login e da senha.
        string login = GameObject.Find("inpEmail").GetComponent<InputField>().text;
        string senha = GameObject.Find("inpSenha").GetComponent<InputField>().text;

        // Chamando o método de logar e jogando os valores que o usuário informou.
        // Caso dê certo.
        if(database.LogarUsuario(login, senha))
        {
            // Mantendo o objeto que gerencia o banco de dados quando mudar de cena.
            DontDestroyOnLoad(GameObject.Find("DatabaseManager").gameObject);
            SceneManager.LoadScene("main");
        }
        // Caso dê errado.
        else
        {
            Debug.Log("DATABASE: Ocorreu algum erro no login!");
        }
    }
}

public class Usuario
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public int idAlbum { get; set; }
}
