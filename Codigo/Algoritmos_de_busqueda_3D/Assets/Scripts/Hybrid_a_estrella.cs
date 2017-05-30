using UnityEngine;
using System.Collections;

public class Hybrid_a_estrella : ControladorCoche {

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
