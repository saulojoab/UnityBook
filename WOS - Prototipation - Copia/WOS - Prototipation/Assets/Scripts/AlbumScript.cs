using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlbumScript : MonoBehaviour {

    Database database;
    public List<GameObject> listaObjetosAlbum;
    RandomizingCards scriptGerador;

	// Use this for initialization
	void Start ()
    {
        database = FindObjectOfType<Database>();
        scriptGerador = FindObjectOfType<RandomizingCards>();

        foreach (GameObject i in listaObjetosAlbum)
        {
            i.transform.GetChild(0).GetComponent<Renderer>().material = scriptGerador.faceCard;
            i.transform.Rotate(0, 0, 180);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        List<string> list = database.RetornarCartasAlbum(database.usuarioLogado.idAlbum, 1);
        List<Material> materiais = Resources.LoadAll(@"Nivel1\Materiais", typeof(Material)).Cast<Material>().ToList();
        
        foreach (string i in list)
        {
            foreach (GameObject g in listaObjetosAlbum)
            {
                if (g.name == "ab_" + i)
                {
                    g.transform.GetChild(0).GetComponent<Renderer>().material = materiais[Convert.ToInt32(i)];
                }
            }
        }
	}
}
