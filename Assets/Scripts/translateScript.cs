using UnityEngine;
using System.Collections;

public class translateScript : MonoBehaviour {
	float lerpPosition = 0.0f;
	float lerpTime = 0.05f; // This is the number of seconds the Lerp will take
	bool translating = false;
	Vector3 start = new Vector3();
	Vector3 end = new Vector3();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(translating){
			SmoothTranslate();
		}
	}
	void SmoothTranslate(){
		lerpPosition += Time.deltaTime/lerpTime;
		transform.position = Vector3.Lerp(start,end,lerpPosition);
		if(lerpPosition>=1.0){
			translating = false;
		}
	}
	public void MoveTo(Vector3 where){
		lerpPosition = 0;
		translating = true;
		start = transform.position;
		end = where;
	}
	public void MoveThis(Vector3 T){
		lerpPosition = 0;
		translating = true;
		start = transform.position;
		end = transform.position + T;
	}
}
