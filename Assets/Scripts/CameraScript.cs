using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public float speed;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate(Vector3.right * Time.deltaTime*speed);
		transform.LookAt(Vector3.zero);
	}

}
