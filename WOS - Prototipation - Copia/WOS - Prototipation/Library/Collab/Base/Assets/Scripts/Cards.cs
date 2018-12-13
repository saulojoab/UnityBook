using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Cards : MonoBehaviour {
    GameObject objeto2, objeto1;
    public int clique;
    public int Pontos = 0;
    
	void Start ()
    {
        // Inicializa com o primeiro clique
        clique = 1;
    }

    void Update ()
    {
        // Verificando se o usuário venceu o jogo - Saulo.
        VerificarVitoria();

        // Quando o usuário clicar.
        if (Input.GetMouseButtonDown(0)){
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Gira a carta clicada em 180 graus no eixo Y.
                hit.transform.gameObject.transform.Rotate(0, 180, 0);

                // Reativa o material da parte da frente da carta (FASE 1) - Saulo.
                hit.transform.gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;
                // Desativa o material do facecard (FASE 1) - Saulo.
                hit.transform.gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;

                // Verifica o primeiro clique.
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
                        // Desativa o Collider da segunda carta.
                        objeto2.GetComponent<Collider>().enabled = false;
                        Pontos++;
                    }
                    // Caso as cartas sejam diferentes.
                    else
                    {
                        // Reverte as posições às iniciais.
                        objeto1.transform.Rotate(0, 180, 0);
                        objeto2.transform.Rotate(0, 180, 0);

                        // Reativa o Collider da primeira.
                        objeto1.GetComponent<Collider>().enabled = true;

                        // Desativa o material da frente das cartas (FASE 1) - Saulo.
                        objeto1.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
                        objeto2.transform.GetChild(1).GetComponent<Renderer>().enabled = false;

                        // Reativa o material do facecard (FASE 1) - Saulo.
                        objeto1.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                        objeto2.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
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
        }
    }

    /// <summary>
    /// Essa função verifica se o usuário atingiu o número de pontos necessários pra vencer o jogo. - Saulo
    /// </summary>
    void VerificarVitoria()
    {
        if (Pontos == 12)
        {
            Debug.Log("GANHOUUUUUUUUUUUUUUU");
        }
    }

    /// <summary>
    /// Função de comparar DUAS cartas selecionadas.
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <returns></returns>
    bool comparaCartas(int c1, int c2)
    {
        // Verifica se o ID da primeira carta menos o da segunda é igual a 1 ou -1, se for então são iguais
        if (c1 == c2)
        {
            return true;
        }

        return false;
    } 
}
