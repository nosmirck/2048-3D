using UnityEngine;
using System.Collections;

public class TileController : MonoBehaviour {
	public TextMesh value;
	public MeshRenderer tilemesh;
	int fsize;
	Animator animator;
	void Start(){
		animator = GetComponent<Animator>();
		Born();
	}
	public void SetValue(int val){
		value.fontSize = 100;
		if(val>8)
			value.fontSize = 85;
		if(val>64)
			value.fontSize = 70;
		if(val>512)
			value.fontSize = 60;
		value.text = val.ToString();
		tilemesh.material.color = SetColorValue(val);
	}
	public void Fusion(){
		animator = GetComponent<Animator>();
		animator.SetTrigger("fusion");
	}
	public void Born(){
		animator = GetComponent<Animator>();
		animator.SetTrigger("born");
	}
	Color SetColorValue(int val){
		switch(val){
		case 2:
			return new Color(0.933f, 0.894f, 0.855f);
		case 4:
			return new Color(0.929f, 0.878f, 0.784f);
		case 8:
			return new Color(0.949f, 0.694f, 0.475f);
		case 16:
			return new Color(0.961f, 0.584f, 0.388f);
		case 32:
			return new Color(0.965f, 0.486f, 0.373f);
		case 64:
			return new Color(0.965f, 0.369f, 0.231f);
		case 128:
			return new Color(0.929f, 0.812f, 0.447f);
		case 256:
			return new Color(0.929f, 0.8f, 0.38f);
		case 512:
			return new Color(0.929f, 0.784f, 0.314f);
		case 1024:
			return new Color(0.929f, 0.773f, 0.247f);
		case 2048:
			return new Color(0.929f, 0.761f, 0.18f);
		case 4096:
			return new Color(0.8f, 0.8f, 0.8f);
		case 8192:
			return new Color(1,1,1);
		}
		return Color.black;
	}
}
