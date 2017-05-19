using UnityEngine;
using System.Collections;

//https://code.tutsplus.com/tutorials/unity3d-third-person-cameras--mobile-11230

public class Camara : MonoBehaviour {
	// Camara en tercera persona
	// Use this for initialization
	public GameObject coche;
	public float damping = 1;

	Vector3 distancia_camara;
	//Vector3 rotacion_camara;

	void Start () {
		distancia_camara = coche.transform.position - transform.position;
		//distancia_camara = coche.transform.rotation - transform.rotation;
	}
	
	// Despues de que las fisicas y el resto de elementos se hayan calculado
	void LateUpdate () {
		
		float currentAngle = transform.eulerAngles.y;
		float desiredAngle = coche.transform.eulerAngles.y;
		float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);


		Quaternion rotation = Quaternion.Euler(0.0f, angle, 0);
		transform.position = coche.transform.position - (rotation * distancia_camara);

		transform.LookAt(coche.transform);

		//transform.rotation.x = 30.0f;
		//Quaternion.
		//transform.position = coche.transform.position + distancia_camara;
		//transform.rotation = coche.transform.rotation + rotacion_camara;
	}
}
