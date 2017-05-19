using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathSmoothing {
	private ObtenerMapa  mapa;
	private Vector3[] trayectoria;
	private Vector3[] suavizada;
	private Vector3[] sinzigzag;

	public PathSmoothing (ObtenerMapa p_mapa, Vector3[] p_trayectoria){
		mapa = p_mapa;
		trayectoria = p_trayectoria;
	}

	public Vector3 [] getTrayectoriaSuavizada () {
		//Hacemos dos pasadas para mejorar la eliminacion del zigzag
		eliminarZigZag (trayectoria);
		eliminarZigZag (sinzigzag);
		redondearCurvas ();
		//añadirVectoresIntermedios();

		return suavizada;
	}

	private void eliminarZigZag (Vector3[] p_trayectoria) {
		int size_trayectoria = p_trayectoria.Length;
		LinkedList<Vector3> nueva = new LinkedList<Vector3> ();
		Vector3 actual;
		Vector3 siguiente;
		Vector3 visible;
		int i = 2;

		actual = p_trayectoria [0];
		visible = p_trayectoria [1];
		siguiente = p_trayectoria [2];

		nueva.AddLast(actual);

		while ( i < size_trayectoria ) {
			siguiente = p_trayectoria [i];

			if (mapa.lineaVision(actual, siguiente)) {
				visible = siguiente;
				i++;
			} else {
				nueva.AddLast (visible);

				actual = visible;
			}
		}

		nueva.AddLast(siguiente);

		Vector3[] temp = new Vector3[nueva.Count];

		i = 0;
		foreach (Vector3 vector in nueva) {
			temp [i] = vector;
			i++;
		}

		sinzigzag = temp;

	}

	private void redondearCurvas (){
		suavizada = sinzigzag;
	}
}
