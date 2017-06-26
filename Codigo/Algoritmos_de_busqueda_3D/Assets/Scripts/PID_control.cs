using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PID_control {
	private GameObject coche;
	private GameObject eje_delantero;
	private GameObject eje_trasero;
	private Vector3[] trayectoria;
	//private Rigidbody rb_coche;
	private float orientacion_coche;
	private int punto_actual;
	private int punto_meta;
	private float error_anterior;
	private Parrilla parrilla;

	public PID_control (GameObject p_coche, Vector3 [] p_trayectoria) {
		Vector3 primer_destino;

		coche = p_coche;
		trayectoria = p_trayectoria;
		//rb_coche = coche.GetComponent<Rigidbody> ();
		eje_delantero = GameObject.FindGameObjectWithTag ("EjeDelantero");
		eje_trasero = GameObject.FindGameObjectWithTag ("EjeTrasero");
		//Debug.Log ("Posicion eje delantero: " + eje_delantero.transform.position.z);
		//rb_coche.velocity.magnitude;

		punto_actual = 1;
		punto_meta = trayectoria.Length - 1;

		primer_destino = trayectoria [punto_actual] - eje_trasero.transform.position;
		error_anterior = anguloGiro(primer_destino);
	}

	public float[] pasoPID (float parametro_p, float parametro_i, float parametro_d) {
		float distancia = 0.0f;
		//float distancia2 = 0.0f;
		float fuerza_motor = 0.0f;
		float angulo_giro = 0.0f;
		float[] retorno = new float[2];
		float angulo_error;
		float diferencial_error;
		float error_interno = 0.0f;
		//float angulo_aux;
		Vector3 destino;
		//Vector3 posicion_coche;
		float distancia_cambio = 1.9f;

		//destino = trayectoria [punto_actual] - eje_delantero.transform.position;
		//destino = trayectoria [punto_actual] - eje_trasero.transform.position;
		//destino = trayectoria [punto_actual];
		//posicion_coche = eje_delantero.transform.position - eje_trasero.transform.position;

		//Debug.Log ("Posicion coche: " + posicion_coche  + " | Destino: " + destino);

		distancia = Vector3.Distance (eje_delantero.transform.position, trayectoria [punto_actual]);
		destino = trayectoria [punto_actual] - eje_trasero.transform.position;

		if (punto_actual == punto_meta) {
			distancia_cambio = 2.9f;
		} else {
			distancia_cambio = 1.9f;
		}

		if (distancia > distancia_cambio) { //Si la distancia al punto es pequeña pasamos al siguiente
			//angulo_error = calcularAnguloEjeCoche(trayectoria[punto_actual]);
			//angulo_error *= calcularSentidoGiro (trayectoria[punto_actual]);
			angulo_error = anguloGiro(destino);
			diferencial_error = angulo_error - error_anterior;
			error_anterior = angulo_error;
			error_interno += angulo_error;

			angulo_giro = (angulo_error * parametro_p) - (diferencial_error * parametro_d) - (error_interno * parametro_i);

			fuerza_motor = (7.0f * distancia) + (0.5f * Mathf.Abs(angulo_giro));

			if (fuerza_motor > 40.0f) {
				fuerza_motor = 40.0f + (0.5f * Mathf.Abs(angulo_giro));
			}
				
		}else{
			if (punto_actual == punto_meta) { //Si hemos llegado a la meta
				fuerza_motor = 0.0f;
				angulo_giro = 360.0f;	
				Debug.Log ("Destino alcanzado");
			}else{ //Si estamos en un destino parcial pero aun no hemos llegado a la meta
				punto_actual++;	
				parrilla.crearCasilla (trayectoria[punto_actual], 0);
			}
		}

		//Debug.Log ("Angulo giro: " + angulo_giro + " || motor: " + fuerza_motor); 

		retorno [0] = fuerza_motor;
		retorno [1] = angulo_giro;

		return retorno;
	}

	public float calcularAnguloEjeCoche (Vector3 vector){
		float angulo = 0.0f;
		//Vector3 eje_coche = coche.transform.position;
		Vector3 eje_coche = eje_delantero.transform.position;

		angulo = Vector3.Angle (vector, eje_coche);

		return angulo;
	}

	public int calcularSentidoGiro (Vector3 vector){
		int sentido = 0; //1 sera derecha; -1 izquierda
		//float angulo = 0.0f;
		float angulo_trayectoria = 0.0f;
		float angulo_respecto_coche = 0.0f;

		angulo_trayectoria = calcularAnguloEjeZ (vector);
		angulo_respecto_coche = anguloRespectoCoche (angulo_trayectoria);

		if (angulo_respecto_coche >= 0.0f) {
			sentido = 1;
		} else {
			sentido = -1;
		}

		return sentido;
	}

	public float calcularAnguloEjeZ (Vector3 vector){
		float angulo = 0.0f;
		Vector3 eje_Z = Vector3.forward; //Eje Z

		angulo = Vector3.Angle (vector, eje_Z);

		if (vector.x < 0) {
			angulo = 360.0f - angulo;
		}

		return angulo;
	}

	//Positivo: giro hacia la derecha
	//Negativo: giro hacia la izquierda
	public float anguloRespectoCoche (float p_angulo) {
		float angulo_giro = 0.0f;
		float orientacion_coche = coche.transform.rotation.eulerAngles.y;
		//float aux = 0.0f;
		float angulo = p_angulo;
		//int float_abs = 0;

		angulo_giro = angulo - orientacion_coche;

		if (Mathf.Approximately (angulo, orientacion_coche)) {
			angulo_giro = 0.0f;
		} else {
			angulo_giro = Mathf.DeltaAngle (orientacion_coche, angulo);
		}
		
		return angulo_giro;
	}

	//
	public float anguloGiro (Vector3 p_vector) {
		//Vector3 vector_coche = coche.transform.position;
		Vector3 vector = p_vector;
		Vector3 vector_coche = eje_delantero.transform.position - eje_trasero.transform.position;
		Vector3 normal = Vector3.up;
		Vector3 v_aux;
		float f_aux1;
		float f_aux2;
		float angulo = 0.0f;

		vector.y = 0.0f;

		v_aux = Vector3.Cross (vector_coche, vector);
		f_aux1 = Vector3.Dot (normal, v_aux);
		f_aux2 = Vector3.Dot (vector_coche, vector);

		angulo = Mathf.Atan2 (f_aux1, f_aux2) * Mathf.Rad2Deg;

		return angulo;
	}

	public void setParrilla (Parrilla p_parrilla){
		parrilla = p_parrilla;

		parrilla.crearCasilla (trayectoria[punto_actual], 0);
	}

}
