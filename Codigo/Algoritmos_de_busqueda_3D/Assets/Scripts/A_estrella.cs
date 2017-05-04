using UnityEngine;
using System.Collections;

public class A_estrella : ControladorCoche {

	public override void MoverCoche (WheelCollider[] m_WheelColliders, GameObject[] m_WheelMeshes) {
		float thrustTorque = 5000000f; 
		m_WheelColliders [0].motorTorque = thrustTorque;
		m_WheelColliders [1].motorTorque = thrustTorque;
		m_WheelColliders [2].motorTorque = thrustTorque;
		m_WheelColliders [3].motorTorque = thrustTorque;

		m_WheelColliders [0].steerAngle = 0f;
		m_WheelColliders [1].steerAngle = 0f;
		m_WheelColliders [2].steerAngle = 0f;
		m_WheelColliders [3].steerAngle = 0f;
	}

	public override bool cierto () {
		return true;
	}
}
