using UnityEngine;
using System.Collections;

public class Camara : MonoBehaviour {
	public GameObject cubo;
	private Vector3 offset; //La distancia entre el cubo y la camara
	// Use this for initialization
	void Start () {
		offset = transform.position - cubo.transform.position;
	}
	
	// LateUpdate is called once per frame despues de que se hayan calculado el resto de cosas
	void LateUpdate () {
		transform.position = cubo.transform.position + offset;
	}
}
