﻿using UnityEngine;
using System.Collections;

public abstract class ControladorCoche : MonoBehaviour {

	public abstract void MoverCoche (WheelCollider[] m_WheelColliders, GameObject[] m_WheelMeshes);
	public abstract bool MoverCoche (GameObject coche, Vector3 posicion, float velocidad);

	public abstract Vector3[] CalcularRuta (Vector3 inicio, Vector3 meta, ObtenerMapa mapa, Parrilla parrilla, float p_peso);
	public abstract void iniciarPasoAestrella (Vector3 v_inicio, Vector3 v_meta, ObtenerMapa v_mapa, Parrilla v_parrilla, float p_peso);
	public abstract bool pasoAestrella (out bool error);
	public abstract Vector3[] getTrayectoria ();

}
