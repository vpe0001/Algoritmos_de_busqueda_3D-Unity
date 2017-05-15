using UnityEngine;
using System.Collections;
//using UnityEditor;


//public class ObtenerMapa : MonoBehaviour  {
public class ObtenerMapa  {
	private Vector3[] vectores;
	private Vector3[] bordes;
	private float radio;

	// Use this for initialization
	public ObtenerMapa () {
		NavMeshTriangulation mapa;

		mapa = NavMesh.CalculateTriangulation ();
		vectores = mapa.vertices;
		radio = 0.1f;
	}

	public Vector3[] getBordes (){
		return vectores;
	}

	public bool esRecorrible (Vector3 posicion) {
		bool recorrible = false;
		NavMeshHit punto_cercano;
		//float radio = 0.25f; // debe ser lo mas pequeño posible 0.17f

		recorrible = NavMesh.SamplePosition (posicion, out punto_cercano, radio, NavMesh.AllAreas);

		return recorrible;
	}

	public void setRadio (float nuevo_radio) {
		radio = nuevo_radio;
	}


}
