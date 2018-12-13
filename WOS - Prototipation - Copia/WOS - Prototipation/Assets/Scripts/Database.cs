using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public class Database : MonoBehaviour {

    private String connString;
    private MySqlConnection connection;
    private MySqlCommand command;
    private LoginScript login;
    public Usuario usuarioLogado = null;

	void Start ()
    {
        ConnectDatabase();
        login = FindObjectOfType<LoginScript>();
    }

    /// <summary>
    /// Esse método faz a conexão com o banco de dados - Saulo.
    /// Database = nome do banco; UID = nome de usuario; Pwd = Senha.
    /// </summary>
    private void ConnectDatabase()
    {
        Debug.Log("DATABASE: Conectando ao banco...");
        connString = "Server=localhost;Database=wos;Uid=root;Pwd=";
        connection = new MySqlConnection(connString);
        command = connection.CreateCommand();
        Debug.Log("DATABASE: Conectado!");
    }
	
    /// <summary>
    /// Esse método adiciona uma nova carta ao álbum (se ele não tiver essa carta ainda, claro) - Saulo.
    /// </summary>
    /// <param name="idAlbum"></param>
    /// <param name="nAlbum"></param>
    /// <param name="idCarta"></param>
    public bool AdicionarCartaAlbum(int idAlbum, int nAlbum, int idCarta)
    {
        // Recebendo as cartas que o usuário tem no álbum.
        List<string> listaDeCartasNoAlbum = RetornarCartasAlbum(idAlbum, nAlbum);

        if (listaDeCartasNoAlbum.Contains(idCarta.ToString()))
        {
            Debug.Log(String.Format("DATABASE: O usuário já possui a carta selecionada! (ID da carta: {0} [ERA {1}])", idCarta, nAlbum));
            return false;
        }
        // Se ele não tiver a carta selecionada, adicionamos ela.
        else
        {
            Debug.Log(String.Format("DATABASE: O usuário não possui a carta {0} no álbum {1}!", idCarta, nAlbum));

            // Criando uma string com os valores antigos.
            string stringValoresAntigos = "";

            foreach (string c in listaDeCartasNoAlbum)
            {
                stringValoresAntigos += c + " ";
            }

            // Criando uma string que contém os valores antigos + a nova carta.
            string valorNovo = stringValoresAntigos += idCarta + " ";

            try
            {
                // Fazendo o Update no banco de dados.
                connection.Open();
                command.CommandText = String.Format("UPDATE album SET ab_{0}='{1}' WHERE id={2}", nAlbum, valorNovo, idAlbum);
                command.ExecuteNonQuery();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    // Informando ao usuário que a carta foi adicionada.
                    Debug.Log(String.Format("DATABASE: Nova carta adicionada ao álbum! (ID da carta: {0} [ERA {1}])", idCarta, nAlbum));
                    connection.Close();                    
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Esse método faz o login de um usuário no sistema - Saulo.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="senha"></param>
    /// <returns></returns>
    public bool LogarUsuario(string email, string senha)
    {
        try
        {
            // Procurando algum usuário com o e-mail informado - Saulo.
            connection.Open();
            command.CommandText = String.Format("SELECT * FROM usuario WHERE email='{0}'", email);
            command.ExecuteNonQuery();

            MySqlDataReader dr = command.ExecuteReader();

            Usuario usuario = new Usuario();

            while (dr.Read())
            {
                // Recuperando a senha cadastrada no banco - Saulo.
                usuario.Id = Convert.ToInt64(dr["id"]);
                usuario.Nome = dr["nome"].ToString();
                usuario.Email = dr["email"].ToString();
                usuario.Senha = dr["senha"].ToString();
                usuario.idAlbum = Convert.ToInt32(dr["idAlbum"].ToString());
            }

            // Se a senha que o usuário informou bater - Saulo.
            if (usuario.Senha == senha)
            {
                usuarioLogado = usuario;
                return true;
            }
            // Se não bater - Saulo.
            else
            {
                return false;
            }
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }

    /// <summary>
    /// Esse método retorna uma lista com todas as cartas que o usuário possui em um álbum - Saulo.
    /// </summary>
    /// <param name="idAlbum"></param>
    /// <param name="nAlbum"></param>
    /// <returns></returns>
    public List<String> RetornarCartasAlbum(int idAlbum, int nAlbum)
    {
        String cartas = "";

        try
        {
            // Query que irá selecionar a tabela das cartas.
            connection.Open();
            command.CommandText = "SELECT * FROM album WHERE id=" + idAlbum;
            command.ExecuteNonQuery();

            MySqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            { 
                // Adicionando o valor da tabela na variável cartas.
                cartas = dr["ab_" + nAlbum].ToString();
            }

            // Se o valor não for nulo.
            if (cartas != null)
            {
                // Transforma a string de cartas em uma lista com cada uma das cartas.
                List<string> listaCartasUsuario = new List<string>();
                listaCartasUsuario = cartas.Split(' ').ToList();

                // Retorna a lista;
                return listaCartasUsuario;
            }
            // Se o valor for nulo.
            else
            {
                // Retorna nulo.
                return null;
            } 
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }

    /*public void exemploInserirValores()
    {
        try
        {
            connection.Open();
            command.CommandText = "INSERT INTO TABELA1 (CAMPO1) VALUES ('VALOR1')";
            command.ExecuteNonQuery();
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }
    */

    /*public void exemploPegarValores()
    {
        try
        {
            connection.Open();
            command.CommandText = "Select * from blablabla";
            command.ExecuteNonQuery();

            MySqlDataReader dr = command.ExecuteReader();
            
            while (dr.Read())
            {
                //stud.Studid = Convert.ToInt32(dr["StudId"]);
                //stud.StudName = dr["StudName"].ToString();
                //stud.StudentDept = dr["StudentDept"].ToString();
            }
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }
    */
}
