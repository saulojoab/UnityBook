using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using UnityEngine.UI;
using System.Linq;

public class Cards : MonoBehaviour {

    public Transform gameScene, winScene;

    GameObject objeto2, objeto1;
    UIScript scriptUI;
    Database database;
    RandomizingCards generatorScript;
    GameObject objetoAntigo;
    
    public int clique;
    public int Pontos = 0;

    void Start()
    {
        scriptUI = FindObjectOfType<UIScript>();
        database = FindObjectOfType<Database>();
        generatorScript = FindObjectOfType<RandomizingCards>();

        // Inicializa com o primeiro clique
        clique = 1;

        // Exemplo: database.AdicionarCartaAlbum(1, 1, 2);
        // Exemplo: Debug.Log("Carta sorteada: " + SortearCarta());
        // VerificarResultado();
        Debug.Log("Usuário logado: " + database.usuarioLogado.Nome);
    }

    void Update()
    {
        // Atualizando a contagem de pontos na tela - Saulo.
        GameObject.Find("pontosTexto").GetComponent<Text>().text = "Pontos: " + Pontos;

        // Verificando se o tempo acabou ou o usuário venceu - Saulo.
        if (VerificarVitoria(Pontos) || scriptUI.Tempo >= 180)
        {
            // transform.position = winScene.position;
            
            // Pausando o contador - Saulo.
            scriptUI.pararJogo = true;

            // Verifica se o usuário vai ganhar alguma carta pro álbum - Saulo.
            VerificarResultado();

            // Mostrando as cartas do álbum - Saulo.
            Debug.Log("Era pra ir");
            Camera.main.transform.position = GameObject.Find("AlbumScene").transform.position;
        } 

        // Quando o usuário clicar.
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Objeto hitado: " + hit.transform.gameObject.transform.name);

                // Gira a carta clicada em 180 graus no eixo Y.
                hit.transform.gameObject.transform.Rotate(0, 180, 0);

                // Desativa o material do facecard (FASE 1) - Saulo.
                hit.transform.gameObject.transform.Find("verso").GetComponent<Renderer>().enabled = false;
                // Reativa o material da parte da frente da carta (FASE 1) - Saulo.
                hit.transform.gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;

                // Verifica o primeiro clique.
                testClick(ray, hit);

            }
        }
    }

    void testClick(Ray ray, RaycastHit hit)
    {
        if (clique < 2)
        {
            // Seta o objeto a variavel objeto1                        
            objeto1 = hit.transform.gameObject;  // Jozivan mexeu aqui

            // Desativa o Collider da primeira carta
            objeto1.GetComponent<Collider>().enabled = false;  // Jozivan mexeu aqui

            // Adiciona um a contagem de cliques
            clique++;
        }
        // Segundo clique
        else if (clique == 2)
        {
            // Seta o objeto a variavel objeto2.
            objeto2 = hit.transform.gameObject;  // Jozivan mexeu aqui

            // Reativa o material da frente da carta (FASE 1) - Saulo.
            hit.transform.gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;

            // Desativa o material do facecard (FASE 1) - Saulo.
            hit.transform.gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;

            Debug.Log(objeto1.transform.GetChild(1).gameObject.name + " e " + objeto2.transform.GetChild(1).gameObject.name);  // Jozivan mexeu aqui

            // Compara se o ID das cartas é o mesmo.
            // Caso as cartas sejam iguais.
            if (comparaCartas(Convert.ToInt32(objeto1.transform.GetChild(1).gameObject.name), Convert.ToInt32(objeto2.transform.GetChild(1).gameObject.name)))  // Jozivan mexeu aqui
            {
                // Desativa as duas cartas - Saulo.
                FindObjectOfType<RandomizingCards>().carta.Remove(objeto1);
                FindObjectOfType<RandomizingCards>().carta.Remove(objeto2);
                objeto2.GetComponent<Collider>().enabled = false;

                // Adicionando mais um ponto ao usuário - Saulo.
                Pontos++;
            }
            // Caso as cartas sejam diferentes.
            else
            {
                // Esperando 1 segundo e reorganizando as cartas - Saulo.
                StartCoroutine(waiter(objeto1, objeto2));
            }

            // Reseta as variaveis.
            clique = 1;

            objeto1 = null;
            objeto2 = null;
        }
        // Essa condição serve pra garantir que o usuário não consiga clicar em 3 cartas - Saulo.
        else
        {
            clique = 1;
        }
    }

    /// <summary>
    /// Essa função verifica se o usuário atingiu o número de pontos necessários pra vencer o jogo. - Saulo
    /// </summary>
    private bool VerificarVitoria(int pontos)
    {
        // Se o jogo não estiver pausado.
        if (scriptUI.pararJogo == false)
        { 
            // Se o usuário atingir o número de pontos necessário.
            if (pontos == 12)
            {
                return true;
            }
            // Enquanto ele não atingir o número de pontos necessário.
            else
            {
                return false;
            }
        } 
        // Se o jogo estiver pausado.
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Função de comparar DUAS cartas selecionadas.
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <returns></returns>
    private bool comparaCartas(int c1, int c2)
    {
        // Verifica se o ID da primeira carta menos o da segunda é igual a 1 ou -1, se for então são iguais
        if (c1 == c2)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Essa função mostra as duas cartas que o usuário selecionou por 1 segundo, e depois vira elas de volta.
    /// By: Saulo.
    /// </summary>
    /// <param name="objeto1"></param>
    /// <param name="objeto2"></param>
    /// <returns></returns>
    IEnumerator waiter(GameObject objeto1, GameObject objeto2)
    {
        // Desativando o collider de todas as cartas pra evitar do player clicar nelas - Saulo.
        // Por isso que temos que remover as cartas que o usuário acertou na linha 82 - Saulo.
        foreach (GameObject c in FindObjectOfType<RandomizingCards>().carta)
        {
            c.GetComponent<Collider>().enabled = false;
        }

        // Esperando um segundo pro usuário ver as duas cartas que ele clicou - Saulo.
        yield return new WaitForSeconds(0.5f);

        // Reativando o collider de todas as cartas - Saulo.
        foreach (GameObject c in FindObjectOfType<RandomizingCards>().carta)
        {
            c.GetComponent<Collider>().enabled = true;
        }

        // Desativando o materal da frente da carta (FASE 1) - Saulo.
        objeto1.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        objeto2.transform.GetChild(1).GetComponent<Renderer>().enabled = false;

        // Reativa o material do facecard (FASE 1) - Saulo.
        objeto1.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        objeto2.transform.GetChild(0).GetComponent<Renderer>().enabled = true;

        // Reverte as posições às iniciais.
        objeto1.transform.Rotate(0, 180, 0);
        objeto2.transform.Rotate(0, 180, 0);

        // Reativa o Collider da primeira.
        objeto1.GetComponent<Collider>().enabled = true;

        StopCoroutine(waiter(objeto1, objeto2));
    }

    /// <summary>
    /// Essa função verifica em quanto tempo o jogador terminou a partida e dá seus prêmios - Saulo.
    /// </summary>
    private void VerificarResultado()
    {
        // Essa lista irá guardar as cartas que foram sorteadas dessa vez - Saulo.
        List<int> cartasNovas = new List<int>();

        // Se o usuário terminar em menos de 60 segundos - Saulo.
        if (scriptUI.Tempo < 60)
        {
            Debug.Log("VITÓRIA: O usuário terminou em menos de 60 segundos!");
            Debug.Log("RECOMPENSAS: Uma carta nova e duas repetidas!");
            
            // Adicionando uma carta nova - Saulo.
            bool resultado = false;

            // Se na primeira vez a carta sorteada for repetida - Saulo.
            if (database.AdicionarCartaAlbum(database.usuarioLogado.idAlbum, 1, SortearCarta()) == false)
            {
                // Enquanto não der uma carta nova - Saulo.
                while (resultado == false)
                {
                    // Sorteamos uma carta diferente até dar uma carta nova - Saulo.
                    int cartaSorteada = SortearCarta();
                    resultado = database.AdicionarCartaAlbum(database.usuarioLogado.idAlbum, 1, cartaSorteada);

                    // Se der uma carta nova, adicionamos ela à nossa lista - Saulo.
                    if (resultado)
                    {
                        cartasNovas.Add(cartaSorteada);
                    }
                }
            }

            // Adicionando duas cartas repetidas - Saulo.
            resultado = false;

            // Sorteando duas cartas que o usuário já possui no álbum - Saulo.
            System.Random rnd = new System.Random();
            List<string> cartasUsuarioPossui = (database.RetornarCartasAlbum(database.usuarioLogado.idAlbum, 1));

            // Se o usuário possuir alguma carta.
            if (cartasUsuarioPossui.Any())
            {
                for (int i = 0; i <= 1; i++)
                {
                    // Adicionando uma carta que o usuário já possui no álbum (ESSA LINHA FICOU ENORME) - Saulo.
                    cartasNovas.Add(Convert.ToInt32(cartasUsuarioPossui[rnd.Next(0, database.RetornarCartasAlbum(database.usuarioLogado.idAlbum, 1).Count - 1)].ToString()));
                }
            }
        }
        // Se o usuário terminar entre 1 e 2 minutos - Saulo.
        else if (120 > scriptUI.Tempo && scriptUI.Tempo > 60)
        {
            Debug.Log("VITÓRIA: O usuário terminou entre 1 e 2 minutos!");
            Debug.Log("RECOMPENSAS: Duas cartas aleatórias (podem ser repetidas ou não)!");

            // Sorteando duas cartas aleatórias.
            for (int i = 0; i <= 1; i++)
            {
                // Adicionando (ou não) a carta aleatória no álbum - Saulo.
                int sorteada = SortearCarta();
                database.AdicionarCartaAlbum(database.usuarioLogado.idAlbum, 1, sorteada);

                // Adicionando ela a lista de cartas sorteadas dessa vez - Saulo.
                cartasNovas.Add(sorteada);
            }
        }
        // Se ele terminar entre 2 ou 3 minutos.
        else if (180 > scriptUI.Tempo && scriptUI.Tempo > 120)
        {
            Debug.Log("VITÓRIA: O usuário terminou entre 2 e 3 minutos!");
            Debug.Log("RECOMPENSAS: Uma carta nova!");

            // Adicionando uma carta nova - Saulo.
            bool resultado = false;

            // Se na primeira vez a carta sorteada for repetida - Saulo.
            if (database.AdicionarCartaAlbum(database.usuarioLogado.idAlbum, 1, SortearCarta()) == false)
            {
                // Enquanto não der uma carta nova - Saulo.
                while (resultado == false)
                {
                    // Sorteamos uma carta diferente até dar uma carta nova - Saulo.
                    int cartaSorteada = SortearCarta();
                    resultado = database.AdicionarCartaAlbum(database.usuarioLogado.idAlbum, 1, cartaSorteada);

                    // Se der uma carta nova, adicionamos ela à nossa lista - Saulo.
                    if (resultado)
                    {
                        cartasNovas.Add(cartaSorteada);
                    }
                }
            }
        }
        // Se ele terminar em mais de 3 minutos.
        else
        {
            Debug.Log("VITÓRIA: O usuário terminou entre 2 e 3 minutos!");
            Debug.Log("RECOMPENSAS: Nada...");
        }

        /*
        Debug.Log("Cartas que foram sorteadas dessa vez: \n");
        foreach (int i in cartasNovas)
        {
            Debug.Log(i);
        }
        */     
    }

    /// <summary>
    /// Esse método sorteia uma das cartas que está aparecendo no jogo - Saulo.
    /// </summary>
    /// <returns></returns>
    public int SortearCarta()
    {
        System.Random random = new System.Random();
        int cartaRandom = random.Next(0, generatorScript.listaDeMateriaisRandomizada.Count - 1);
        int cartaGerada = Convert.ToInt32(generatorScript.listaDeMateriaisRandomizada[cartaRandom].name);

        return cartaGerada;
    }
}
    
