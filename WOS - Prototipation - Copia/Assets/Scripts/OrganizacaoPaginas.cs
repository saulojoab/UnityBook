using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrganizacaoPaginas : MonoBehaviour
{

    public long nPaginas = 20;
    public long folhaAtual;

    public List<long> paginasAtuais;

    public float folhaOffset = 0.1f;
    public List<GameObject> Paginas;

    public List<GameObject> Anteriores;

    void Start()
    {
        // Settando a página inicial - Saulo.
        folhaAtual = 1;
        paginasAtuais = new List<long>() { (nPaginas * 2 - 1) - 1, (nPaginas * 2) - 1 };
        Debug.Log("CAPA");

        Debug.Log("=============GERANDO PÁGINAS=================");
        // Criando páginas de acordo com o número de páginas desejado - Saulo.
        for (int i = 0; i < nPaginas - 1; i++)
        {
            SpawnarPaginas(i + 1);
        }
        Debug.Log("=============PÁGINAS GERADAS================");

        GameObject.Find("Pivot").gameObject.SetActive(false);

        // Resetando o offset das folhas - Saulo.
        folhaOffset = 0.1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            passarPagina();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            voltarPagina();
        }
    }

    void passarPagina()
    {
        // Se o usuário estiver no fim do livro - Saulo.
        if (folhaAtual + 1 == nPaginas)
        {
            Debug.Log("VOCÊ ESTÁ NO FIM DO LIVRO");
        }
        // Caso não, passar a página - Saulo. 
        else
        {
            // Se a página atual + 1 for menor que o número total de páginas.
            if (folhaAtual + 1 < nPaginas)
            {
                Debug.Log("Pagina atual + 1 é maior que o número de páginas!");

                // Pegamos o valor da página que queremos animar.
                GameObject pAtual = GameObject.Find("folha" + (folhaAtual));
                Debug.Log("pAtual =" + pAtual.name);
                //pAtual.GetComponent<Animator>().speed = 2;

                // Se a página atual for maior do que 1.
                if (folhaAtual > 1)
                {
                    // Pegamos o objeto da página anterior.
                    GameObject pAnterior = GameObject.Find("folha" + (folhaAtual - 1));

                    // Se a nossa lista de páginas anteriores não conter a página anterior atual.
                    if (!Anteriores.Contains(pAnterior))
                    {
                        // Adicionamos ela na lista.
                        Anteriores.Add(pAnterior);
                    }

                    // Se a página atual e a página anterior não estiverem se movendo.
                    if (pAtual.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && pAnterior.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                    {
                        // Recuando as páginas anteriores pra evitar que elas entrem uma dentro das outras.
                        Debug.Log("==== RECUANDO ANTERIORES");
                        foreach (var g in Anteriores)
                        {
                            Debug.Log(g.name);
                            g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z + (folhaOffset*2));
                        }
                        Debug.Log("=====");

                        // Animando a página atual.
                        pAtual.GetComponent<Animator>().Play("virarPagina");

                        // Incrementando a folha atual.
                        folhaAtual++;
                    }
                } 
                else 
                {
                    // Se a página atual e a página anterior não estiverem se movendo.
                    if (pAtual.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                    {
                        // Animando a página atual.
                        pAtual.GetComponent<Animator>().Play("virarPagina");

                        // Adicionando ela à lista de páginas anteriores.
                        Anteriores.Add(pAtual);

                        // Incrementando a folha atual.
                        folhaAtual++;
                    }
                }
            }

            Debug.Log(folhaAtual + " | " + (nPaginas - folhaAtual));
        }
    }

    void voltarPagina()
    {
		// Se a folha atual não for zero.
        if (folhaAtual > 1)
        {
			// Criando o objeto da página atual.
            GameObject pAtual = GameObject.Find("folha" + (folhaAtual - 1));

			// Se incrementarmos a folha atual, ela não passar do limite de folhas.
            if (folhaAtual <= nPaginas - 2)
            {
                // O objeto da proxima página.
				GameObject proxPag = GameObject.Find("folha" + (folhaAtual));

				// Se nenhuma das páginas estiver se movendo.
                if (proxPag.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && pAtual.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && pAtual.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    // Avançando a página atual.
                    pAtual.transform.position = new Vector3(pAtual.transform.position.x, pAtual.transform.position.y, pAtual.transform.position.z - (folhaOffset*2));
					
                    // Movemos a página atual.
                    pAtual.GetComponent<Animator>().Play("voltarPagina");
					
                    // Avançamos as páginas anteriores.
					Debug.Log("==== AVANÇANDO ANTERIORES");
                    foreach (var g in Anteriores)
                    {
                        Debug.Log(g.name);
                        g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z - (folhaOffset*2));
                    }
                    Debug.Log("=====");
                    
                    // Se a lista de páginas anteriores conterem alguma página.
                    if (Anteriores.Count > 0)
                    {
                        Anteriores.Remove(Anteriores[Anteriores.Count - 1]);
                    }

					// Reduzimos a folha atual.
					folhaAtual--;
                } 
                else
				{
					Debug.Log("A folha atual (" + folhaAtual + ") não pode se mover pois alguma página está se movendo.");
				}
            }
            // Se estivermos na última página do livro.
            else if (folhaAtual == nPaginas-1)
            {
                GameObject folha = GameObject.Find("folha" + (Paginas.Count-1));
                // Se nenhuma das páginas estiver se movendo.
                if (folha.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    folha.transform.position = new Vector3(folha.transform.position.x, folha.transform.position.y, folha.transform.position.z - (folhaOffset));
					
                    // Movemos a página atual.
                    folha.GetComponent<Animator>().Play("voltarPagina");
					
                    // Avançamos as páginas anteriores.
					Debug.Log("==== AVANÇANDO ANTERIORES");
                    foreach (var g in Anteriores)
                    {
                        Debug.Log(g.name);
                        g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z - (folhaOffset));
                    }
                    Debug.Log("=====");
                    
                    Anteriores.Remove(Anteriores[Anteriores.Count - 1]);

					// Reduzimos a folha atual.
					folhaAtual -= 1;
                } 
                else
				{
					Debug.Log("A folha atual (" + folhaAtual + ") não pode se mover pois alguma página está se movendo.");
				}
            }
        }

        Debug.Log(folhaAtual*2 + " | " + (nPaginas - folhaAtual)*2);
    }


    void SpawnarPaginas(int nPag)
    {
        // Procurando o objeto à ser clonado - Saulo.
        GameObject folha = GameObject.Find("Pivot");

        // Arrumando a rotação das páginas pra deixar elas bonitinhas - Saulo.
        Quaternion q = Quaternion.Euler(88.46101f, folha.transform.rotation.y, (folha.transform.rotation.z - (folhaOffset * 2) / nPaginas));

        // Clonando esse objeto - Saulo.
        // Caso você remova esses cálculos de doido, as páginas irão nascer uma em cima da outra - Saulo.
        GameObject novaPag = (GameObject)Instantiate(folha, new Vector3(folha.transform.position.x + 35.5f - ((folhaOffset / 2) * 0.2f), folha.transform.position.y, folha.transform.position.z - (folhaOffset / 2) - 1.9f), q);

        // Dando um novo nome à esse objeto - Saulo.
        novaPag.name = "folha" + (nPaginas - nPag);

        Debug.Log("==FOLHA GERADA: " + "folha" + (nPaginas - nPag));

        novaPag.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (paginasAtuais[0]-1).ToString();
        novaPag.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (paginasAtuais[1]-1).ToString();

        Debug.Log("=PÁGINAS GERADAS=");
        Debug.Log(paginasAtuais[0] + " e " + paginasAtuais[1]);

        // Adicionando a nova pagina na lista de páginas.
        Paginas.Add(novaPag);

        paginasAtuais[0] -= 2;
        paginasAtuais[1] -= 2;

        // Aumentando o offset para a próxima página criada - Saulo.
        folhaOffset += 0.1f;
    }
}

/*
        // Posicionando corretamente a primeira folha - Saulo.
        // Isso é uma traquinagem bem louca, cuidado na hora de mexer - Saulo.
        GameObject primeiraFolha = GameObject.Find("Pivot");
        Destroy(primeiraFolha);
        
        primeiraFolha = (GameObject)Instantiate(Paginas[1]);
        primeiraFolha.name = "folha0";
        primeiraFolha.transform.position = new Vector3(-33.8f, primeiraFolha.transform.position.y, primeiraFolha.transform.position.z + 0.5f);
        primeiraFolha.transform.rotation = Quaternion.Euler(primeiraFolha.transform.rotation.eulerAngles.x, primeiraFolha.transform.rotation.eulerAngles.y, primeiraFolha.transform.rotation.eulerAngles.z);
        primeiraFolha.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "1";
        Paginas[0] = primeiraFolha;

        // Se a folha atual for a folha 0.
                else
                {
                    Debug.Log("Folha atual é 0!");

                    GameObject pzero = GameObject.Find("folha0");

                    // Adicionando a página 0 à lista de anteriores.
                    Anteriores.Add(pzero);

                    // Se a página 1 estiver estática.
                    if (pAtual.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                    {
						// Recuamos a página zero.
						Debug.Log("=== RECUANDO PAGINA ZERO");
                        pzero.transform.position = new Vector3(pzero.transform.position.x, pzero.transform.position.y, pzero.transform.position.z + (folhaOffset));
                        Debug.Log("=== RECUADA");
                        
						// Animamos a página 1.
						pAtual.GetComponent<Animator>().Play("virarPagina");
                        
						// Incrementamos a página atual.
						folhaAtual += 1;
                    }
					// Se a página 1 não estiver estática, nada acontece, então removemos a página 0 da lista.
                    else
                    {
                        Anteriores.Remove(pzero);
                    }
                }
 */
