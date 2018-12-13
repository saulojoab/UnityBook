using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paginas : MonoBehaviour {

	public Button btProxPag;
	public GameObject pagina3;

	// Use this for initialization
	void Start () 
	{
		btProxPag = GameObject.Find("btPassarPag").GetComponent<Button>();	
		pagina3 = GameObject.Find("p3");

		btProxPag.onClick.AddListener(PassarPagina);
	}
	
	private void Update() {
		
	}

	void PassarPagina()
	{
		GameObject.Find("p1").SetActive(false);
		Debug.Log("Passando página!");
		pagina3.GetComponent<Animator>().Play("turningPage");
	}
}
