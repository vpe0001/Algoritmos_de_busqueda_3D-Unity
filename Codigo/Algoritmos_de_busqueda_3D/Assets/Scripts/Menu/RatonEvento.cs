using UnityEngine;
using System.Collections;

public class RatonEvento : MonoBehaviour {
	private Color color;

	void Start(){
		color = GetComponent<Renderer>().material.color;
	}

	void OnMouseEnter(){
		GetComponent<Renderer>().material.color = Color.blue;
	}

	void OnMouseExit() {
		GetComponent<Renderer>().material.color = color;
	}
}
