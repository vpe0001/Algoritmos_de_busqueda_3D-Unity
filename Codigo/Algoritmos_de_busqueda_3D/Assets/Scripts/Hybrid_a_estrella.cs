using UnityEngine;
using System.Collections;

public class Hybrid_a_estrella : ControladorCoche {

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

	public override bool MoverCoche (GameObject coche, Vector3 posicion, float velocidad){
		return true;
	}

	public override void iniciarCalcularRuta(Vector3 v_inicio, Vector3 v_meta, ObtenerMapa v_mapa, Parrilla v_parrilla, float p_peso, int tam_parrilla) {
	}

	public override bool pasoCalcularRuta (out bool error) {
		error = false;

		return false;
	}

	public override Vector3[] getTrayectoria (){
		return new Vector3[1];
	}


	public override Vector3[] CalcularRuta (Vector3 inicio, Vector3 meta, ObtenerMapa mapa, Parrilla parrilla, float p_peso, int tam_parrilla) {
		
		return new Vector3[1];
	}
}
