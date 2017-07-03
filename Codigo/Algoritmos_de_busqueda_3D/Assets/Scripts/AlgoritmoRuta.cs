using UnityEngine;
using System.Collections;

public abstract class AlgoritmoRuta{

	public abstract Vector3[] CalcularRuta (Vector3 inicio, Vector3 meta, float v_angulo_coche, ObtenerMapa mapa, Parrilla parrilla, float p_peso, int tam_parrilla, int ancho, int largo, bool dibujar_casillas);
	public abstract void iniciarCalcularRuta (Vector3 v_inicio, Vector3 v_meta, float v_angulo_coche, ObtenerMapa v_mapa, Parrilla v_parrilla, float p_peso, int tam_parrilla, int ancho, int largo, bool dibujar_casillas);
	public abstract bool pasoCalcularRuta (out bool error);
	public abstract Vector3[] getTrayectoria ();
	public abstract Nodo[] getTrayectoriaNodos ();
	public abstract int [,,] getMapaObstaculos ();
	public abstract int [,,] getMapaDistancias ();
}
