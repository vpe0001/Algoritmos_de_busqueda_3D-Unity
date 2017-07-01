using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PID_control_hybrid {
	private GameObject coche;
	private GameObject eje_delantero;
	private GameObject eje_trasero;
	private Nodo[] trayectoria;
	private Rigidbody rb_coche;
	private float orientacion_coche;
	private int punto_actual;
	private int punto_meta;
	private float error_anterior;
	private Parrilla parrilla;
	private float distancia_anterior;
	private bool cambio_sentido;
	private int sentido_anterior;
	private bool fin;

	public PID_control_hybrid (GameObject p_coche, Nodo [] p_trayectoria) {
		Vector3 primer_destino;

		fin = false;

		coche = p_coche;
		trayectoria = p_trayectoria;
		rb_coche = coche.GetComponent<Rigidbody> ();
		eje_delantero = GameObject.FindGameObjectWithTag ("EjeDelantero");
		eje_trasero = GameObject.FindGameObjectWithTag ("EjeTrasero");


		punto_actual = 1;
		punto_meta = trayectoria.Length - 1;

		if (trayectoria [punto_actual].sentido == Constantes.hacia_adelante) {
			primer_destino = trayectoria [punto_actual].vector_hybrid - eje_trasero.transform.position;
		} else {
			primer_destino = trayectoria [punto_actual].vector_hybrid - eje_delantero.transform.position;
		}

		sentido_anterior = trayectoria [punto_actual].sentido;
		cambio_sentido = false;
		error_anterior = anguloGiro(primer_destino, trayectoria [punto_actual].sentido);

		distancia_anterior = 9999.9f;
	}

	public float[] pasoPID (float parametro_p, float parametro_i, float parametro_d) {
		float distancia = 0.0f;
		float fuerza_motor = 0.0f;
		float angulo_giro = 0.0f;
		float[] retorno = new float[2];
		float angulo_error = 0.0f;
		float diferencial_error;
		float error_interno = 0.0f;
		Vector3 destino;
		float distancia_cambio = 1.9f;
		bool ya_cambiado = false;


		if (!fin) {
			if (sentido_anterior != trayectoria [punto_actual].sentido) {
				cambio_sentido = true;
			}

			if (cambio_sentido) {
				if (rb_coche.velocity.magnitude > 0.0001f) {
					fuerza_motor = 0.0f;
					angulo_giro = 0.0f;
				} else {
					cambio_sentido = false;
					sentido_anterior = trayectoria [punto_actual].sentido;
				}


			} else {
				if (punto_actual < punto_meta) {
				
					if (trayectoria [punto_actual].sentido == Constantes.hacia_adelante) {
						//distancia = Vector3.Distance (eje_trasero.transform.position, trayectoria [punto_actual].vector_hybrid);
						distancia = Vector3.Distance (eje_delantero.transform.position, trayectoria [punto_actual].vector_hybrid);
						destino = trayectoria [punto_actual].vector_hybrid - eje_trasero.transform.position;
					} else if (trayectoria [punto_actual].sentido != trayectoria [punto_actual + 1].sentido) {
						//distancia = Vector3.Distance (eje_trasero.transform.position, trayectoria [punto_actual].vector_hybrid);
						distancia = Vector3.Distance (eje_delantero.transform.position, trayectoria [punto_actual].vector_hybrid);
						destino = trayectoria [punto_actual].vector_hybrid - eje_delantero.transform.position;
					} else {
						distancia = Vector3.Distance (eje_trasero.transform.position, trayectoria [punto_actual].vector_hybrid);
						//distancia = Vector3.Distance (eje_delantero.transform.position, trayectoria [punto_actual].vector_hybrid);
						destino = trayectoria [punto_actual].vector_hybrid - eje_delantero.transform.position;
					}

				} else {
					if (trayectoria [punto_actual].sentido == Constantes.hacia_adelante) {
						//distancia = Vector3.Distance (eje_trasero.transform.position, trayectoria [punto_actual].vector_hybrid);
						distancia = Vector3.Distance (eje_delantero.transform.position, trayectoria [punto_actual].vector_hybrid);
						destino = trayectoria [punto_actual].vector_hybrid - eje_trasero.transform.position;
					} else {
						distancia = Vector3.Distance (eje_trasero.transform.position, trayectoria [punto_actual].vector_hybrid);
						//distancia = Vector3.Distance (eje_delantero.transform.position, trayectoria [punto_actual].vector_hybrid);
						destino = trayectoria [punto_actual].vector_hybrid - eje_delantero.transform.position;
					}
				}
			

				if (punto_actual == punto_meta) {
					distancia_cambio = 1.6f;
				} else {
					if (trayectoria [punto_actual].sentido == Constantes.hacia_adelante) {
						distancia_cambio = 1.5f;
					} else {
						distancia_cambio = 0.2f;
					}
				}

				if (distancia > distancia_cambio) { //Si la distancia al punto es pequeña pasamos al siguiente
					angulo_error = anguloGiro (destino, trayectoria [punto_actual].sentido);
					//angulo_error = coche.transform.rotation.eulerAngles.y - trayectoria[punto_actual].angulo_hybrid;
					//angulo_error = trayectoria[punto_actual].angulo_hybrid - coche.transform.rotation.eulerAngles.y;

					diferencial_error = angulo_error - error_anterior;
					error_anterior = angulo_error;
					error_interno += angulo_error;

					angulo_giro = (angulo_error * parametro_p) - (diferencial_error * parametro_d) - (error_interno * parametro_i);

					fuerza_motor = (20.0f * distancia) + (0.5f * Mathf.Abs (angulo_giro));


					if (Mathf.Abs (fuerza_motor) > 50.0f) {
						fuerza_motor = 50.0f;// + (0.5f * Mathf.Abs (angulo_giro));
					}

					if (trayectoria [punto_actual].sentido == Constantes.hacia_atras) {
						fuerza_motor *= trayectoria [punto_actual].sentido; 
						fuerza_motor *= 0.5f; //Mas despacio por ir hacia atras

						angulo_giro *= trayectoria [punto_actual].sentido; 
					}
				
				} else {
					if (punto_actual == punto_meta) { //Si hemos llegado a la meta
						fuerza_motor = 0.0f;
						angulo_giro = 360.0f;	
						fin = true;
						Debug.Log ("Destino alcanzado");
					} else { //Si estamos en un destino parcial pero aun no hemos llegado a la meta
						punto_actual++;	
						ya_cambiado = true;
						distancia_anterior = 9999.9f;
						parrilla.crearCasilla (trayectoria [punto_actual].vector_hybrid, Constantes._ABIERTOS);
					}
				}

				if (distancia_anterior < (distancia - 0.0005f) && !fin) {
					punto_actual++;
					distancia_anterior = 9999.9f;
					parrilla.crearCasilla (trayectoria [punto_actual].vector_hybrid, Constantes._ABIERTOS);
				} else {
					if (!ya_cambiado) {
						distancia_anterior = distancia;
					} else {
						ya_cambiado = false;
					}
				}
					
			}

			if (punto_actual > punto_meta) {
				punto_actual = punto_meta;
			}
		} else {
			fuerza_motor = 0.0f;
			angulo_giro = 360.0f;
		}
			
		retorno [0] = fuerza_motor;
		retorno [1] = angulo_giro;

		return retorno;
	}


	//
	public float anguloGiro (Vector3 p_vector, int sentido) {
		//Vector3 vector_coche = coche.transform.position;
		Vector3 vector = p_vector;
		Vector3 vector_coche;
		Vector3 normal = Vector3.up;
		Vector3 v_aux;
		float f_aux1;
		float f_aux2;
		float angulo = 0.0f;

		if (sentido == Constantes.hacia_adelante) {
			vector_coche = eje_delantero.transform.position - eje_trasero.transform.position;
		} else {
			vector_coche = eje_trasero.transform.position - eje_delantero.transform.position;
		}

		vector.y = 0.0f;

		v_aux = Vector3.Cross (vector_coche, vector);
		f_aux1 = Vector3.Dot (normal, v_aux);
		f_aux2 = Vector3.Dot (vector_coche, vector);

		angulo = Mathf.Atan2 (f_aux1, f_aux2) * Mathf.Rad2Deg;

		return angulo;
	}

	public void setParrilla (Parrilla p_parrilla){
		parrilla = p_parrilla;

		parrilla.crearCasilla (trayectoria[punto_actual].vector_hybrid, 0);
	}

}
