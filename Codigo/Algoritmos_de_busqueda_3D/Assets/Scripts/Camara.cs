using UnityEngine;
using System.Collections;

//https://code.tutsplus.com/tutorials/unity3d-third-person-cameras--mobile-11230

public class Camara : MonoBehaviour {
	// Camara en tercera persona
	// Use this for initialization
	public GameObject coche;
	public float damping = 1;

	Vector3 distancia_camara;

	void Start () {
		distancia_camara = coche.transform.position - transform.position;
	}
	
	// Despues de que las fisicas y el resto de elementos se hayan calculado
	void LateUpdate () {
		float currentAngle = transform.eulerAngles.y;
		float desiredAngle = coche.transform.eulerAngles.y;
		float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);

		Quaternion rotation = Quaternion.Euler(0, angle, 0);
		transform.position = coche.transform.position - (rotation * distancia_camara);

		transform.LookAt(coche.transform);
	}
}
