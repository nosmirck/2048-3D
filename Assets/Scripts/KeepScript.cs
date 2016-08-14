using UnityEngine;
using System.Collections;

public class KeepScript : MonoBehaviour {
	public TextMesh texto;
	// Use this for initialization
	void OnMouseEnter(){
		texto.fontSize = 20;
	}

	void OnMouseExit(){
		texto.fontSize = 15;
	}

	void OnMouseDown(){
		GameObject.Find("GameManager").GetComponent<GameManager>().keep = true;
		Destroy(transform.parent.gameObject, 0.1f);
	}
}
