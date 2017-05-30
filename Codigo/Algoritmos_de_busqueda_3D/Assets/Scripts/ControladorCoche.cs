using UnityEngine;
using System.Collections;

public abstract class ControladorCoche : MonoBehaviour {

	public abstract Vector3[] CalcularRuta (Vector3 inicio, Vector3 meta, ObtenerMapa mapa, Parrilla parrilla, float p_peso, int tam_parrilla);
	public abstract void iniciarCalcularRuta (Vector3 v_inicio, Vector3 v_meta, ObtenerMapa v_mapa, Parrilla v_parrilla, float p_peso, int tam_parrilla);
	public abstract bool pasoCalcularRuta (out bool error);
	public abstract Vector3[] getTrayectoria ();

}
