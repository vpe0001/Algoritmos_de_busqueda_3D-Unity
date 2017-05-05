using UnityEngine;
using System.Collections;

public abstract class ControladorCoche : MonoBehaviour {

	public abstract void MoverCoche (WheelCollider[] m_WheelColliders, GameObject[] m_WheelMeshes);

	public abstract Vector3[] CalcularRuta (Vector3 inicio, Vector3 meta, float x_pos, float x_neg, float z_pos, float z_neg);

}
