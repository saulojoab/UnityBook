using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public long Tempo;
    public bool pararJogo = false;
    Text textObject;
    String textHolder; // Só da pra alterar o texto da UI no método principal, então essa variável só guarda o retorno da função - Saulo.
    Thread t;

    void Start()
    {
        textObject = GameObject.Find("contadorTexto").GetComponent<Text>();

        // Criando a Thread do Contador.
        t = new Thread(Contador);
        t.Start();
    }

    void Update()
    {
        // Atualizando a contagem na tela.
        textObject.text = textHolder;
    }

    /// <summary>
    /// Esse método cria um cronômetro que é usado pra saber quanto tempo a partida durou - Saulo.
    /// </summary>
    void Contador()
    {
        // Laço de repetição eterno.
        while (true)
        {
            // Se o usuário pausar o jogo, o contador é pausado.
            if (pararJogo)
            {
                // Pausa o contador.
            }
            else
            {
                // Formatando o tempo.
                if (Tempo < 60)
                {
                    textHolder = (String.Format("{0}s", Tempo));
                }
                else
                {
                    int segundos = Convert.ToInt32(Tempo - 60);
                    int minutos = Convert.ToInt32(Tempo / 60);

                    textHolder = (String.Format("{0}min {1}s", minutos, segundos));
                }

                // Adicionando mais um aos segundos.
                Tempo += 1;

                // Esperando um segundo.
                Thread.Sleep(1000);
            }
        }
    }

    // Parando a Thread do Contador quando o programa fechar - Saulo.
    private void OnApplicationQuit()
    {
        t.Abort();
    }
}

