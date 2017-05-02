using UnityEngine;
using System.Collections;

public class MoverCubo : MonoBehaviour {
	public float fuerza;
	private Rigidbody rb_cubo;
	// Use this for initialization
	void Start () {
		rb_cubo = GetComponent <Rigidbody> ();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 movimiento = new Vector3 (fuerza, 0.0f, 0.0f);

		rb_cubo.AddForce (movimiento);
	
	}
}
