using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading;

public class RandomizingCards : MonoBehaviour {
    // Array com todas as cartas.
    public GameObject [] carta;

    [Space(20)]
    // Material FaceCard.
    public Material faceCard;

    // Material AllCards.
    public Material [] matCartas;

    /// <summary>
    /// Método pra randomizar ordem dos valores de uma lista - Saulo.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<T> Randomize<T>(List<T> list)
    {
        List<T> randomizedList = new List<T>();
        System.Random rnd = new System.Random();
        while (list.Count > 0)
        {
            int index = rnd.Next(0, list.Count); // Pega um valor aleatório da lista mãe.
            randomizedList.Add(list[index]); // Colocando esse valor na lista randomizada.
            list.RemoveAt(index);
        }

        return randomizedList;
    }
   
    void Start () {
        // Convertendo o vetor de cartas para uma lista - Saulo.
        //List<GameObject> listaDeCartasOriginal = carta.ToList();
        //List<Material> listaDeMateriaisOriginal = matCartas.ToList();
        List<Material> listaDeMateriaisRandomizada = matCartas.ToList();

        // Pegando todos os materiais da pasta de Materiais.
        Material[] materiais = Resources.LoadAll(@"Nivel1\Materiais", typeof(Material)).Cast<Material>().ToArray();
        Debug.Log("Elementos na lista: " + materiais.Count());

        listaDeMateriaisRandomizada = Randomize<Material>(materiais.ToList());

        // Sorteando 12 materiais e colocando esses materiais nas cartas - Saulo e Jozivan.
        int b = 0;
        for (int a = 0; a < 12; a++)
        {
            matCartas[b] = (listaDeMateriaisRandomizada[a]);
            matCartas[b + 1] = (listaDeMateriaisRandomizada[a]);
            b += 2;
        }

        // Randomizando a ordem dos objetos na lista - Saulo.
        //List<String> generatedRandom = new List<String>();
        listaDeMateriaisRandomizada = Randomize<Material>(matCartas.ToList());
     
        // Alterando o nome dos objetos filhos, de acordo com a lista randomizada.
        for (int a = 0; a < listaDeMateriaisRandomizada.Count; a++)
        {
            // Mudando o nome dos objetos para o nome dos materiais - Saulo e Jozivan.
            carta[a].transform.GetChild(1).gameObject.name = listaDeMateriaisRandomizada[a].name;
            // Mundando o material dos objetos - Saulo e Jozivan.
            carta[a].transform.GetChild(1).GetComponent<Renderer>().material = listaDeMateriaisRandomizada[a];
            //Debug.Log(carta[a].transform.GetChild(1).gameObject.name);
        }
        
        // Fix da orientação do facecard e adiciona o material do mesmo
        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if(gameObj.tag == "versoCarta")
            {
                gameObj.transform.Rotate(0, 180, 0);
                gameObj.transform.GetComponent<Renderer>().material = faceCard;
            }
        }
        
    }
	
	void Update () {
		
	}
}
