using UnityEngine;
using System.Collections;

public abstract class ControladorCoche : MonoBehaviour {

	public abstract void MoverCoche (WheelCollider[] m_WheelColliders, GameObject[] m_WheelMeshes);
	public abstract void MoverCoche (GameObject coche, Vector3 posicion);

	public abstract Vector3[] CalcularRuta (Vector3 inicio, Vector3 meta, ObtenerMapa mapa);

}
