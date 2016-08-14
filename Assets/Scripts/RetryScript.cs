using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RetryScript : MonoBehaviour {
	public TextMesh texto;
	// Use this for initialization
	void OnMouseEnter(){
		texto.fontSize = 20;
	}

	void OnMouseExit(){
		texto.fontSize = 15;
	}

	void OnMouseDown(){
        SceneManager.LoadScene(0);
	}
}
