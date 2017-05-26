using UnityEngine;
using System.Collections;
//using UnityEditor;


//public class ObtenerMapa : MonoBehaviour  {
public class ObtenerMapa  {
	private Vector3[] vertices;
	private float radio;

	// Use this for initialization
	public ObtenerMapa () {
		NavMeshTriangulation mapa;

		mapa = NavMesh.CalculateTriangulation ();
		vertices = mapa.vertices;
		radio = 0.01f;
	}

	public Vector3[] getVertices (){
		return vertices;
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

	public bool lineaVision (Vector3 inicio, Vector3 fin) {
		bool linea = false;
		NavMeshHit hit;

		linea = !(NavMesh.Raycast (inicio, fin, out hit, NavMesh.AllAreas));

		return linea;
	}


}
