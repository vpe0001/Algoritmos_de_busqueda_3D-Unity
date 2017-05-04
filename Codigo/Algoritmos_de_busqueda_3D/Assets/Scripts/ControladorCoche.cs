using UnityEngine;
using System.Collections;

public abstract class ControladorCoche : MonoBehaviour {

	public abstract void MoverCoche (WheelCollider[] m_WheelColliders, GameObject[] m_WheelMeshes);

	public abstract bool cierto ();

}
