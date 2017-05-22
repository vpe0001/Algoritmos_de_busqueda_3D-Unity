using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathSmoothing {
	private ObtenerMapa  mapa;
	private Vector3[] trayectoria;
	private Vector3[] suavizada;
	private Vector3[] sinzigzag;
	private float peso_trayectoria;
	private float peso_suavizado;
	private float tolerancia;
	private float num_puntos_bezier;

	public PathSmoothing (ObtenerMapa p_mapa, Vector3[] p_trayectoria){
		mapa = p_mapa;
		trayectoria = p_trayectoria;

		peso_trayectoria = 0.9f;
		peso_suavizado = 0.1f;
		tolerancia = 0.000001f;

		num_puntos_bezier = 10.0f;
	}

	public Vector3 [] getTrayectoriaSuavizada () {
		//Hacemos dos pasadas para mejorar la eliminacion del zigzag
		eliminarZigZag (trayectoria);
		eliminarZigZag (sinzigzag);

		//descensoGradiente ();
		curvaBezier();
		//suavizada = sinzigzag;

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

	//
	private void descensoGradiente (){
		//Vector3[] temporal = (Vector3[])sinzigzag.Clone();
		Vector3[] temporal = (Vector3[]) trayectoria.Clone();
		float error = tolerancia * 10.0f;

		while (error > tolerancia) {
			error = 0.0f;

			for (int i = 1; i < (temporal.Length -1) ; i++) {
				Vector3 aux = temporal [i];

				/*
				temporal [i].x += peso_trayectoria * (sinzigzag[i].x - temporal[i].x);	
				temporal [i].y += peso_trayectoria * (sinzigzag[i].y - temporal[i].y);	
				temporal [i].z += peso_trayectoria * (sinzigzag[i].z - temporal[i].z);
				*/

				temporal [i].x += peso_trayectoria * (trayectoria[i].x - temporal[i].x);	
				temporal [i].y += peso_trayectoria * (trayectoria[i].y - temporal[i].y);	
				temporal [i].z += peso_trayectoria * (trayectoria[i].z - temporal[i].z);


				temporal [i].x += peso_suavizado * (temporal[i+1].x - temporal[i-1].x - (2 * temporal[i].x));	
				temporal [i].y += peso_suavizado * (temporal[i+1].y - temporal[i-1].y - (2 * temporal[i].y));	
				temporal [i].z += peso_suavizado * (temporal[i+1].z - temporal[i-1].z - (2 * temporal[i].z));

				error += Mathf.Abs(aux.x - temporal [i].x) + Mathf.Abs(aux.y - temporal [i].y) + Mathf.Abs(aux.z - temporal [i].z);
			}
		}

		suavizada = temporal;
	}

	private void curvaBezier() {
		LinkedList <Vector3> trayectoriaBezier = new LinkedList<Vector3> ();
		int size = sinzigzag.Length;
		int i = 0;
		Vector3 punto1;
		Vector3 punto2;
		Vector3 punto3;

		//Recorremos la ruta de tres en tres puntos para redondear las esquinas que forman
		while (i < size && (i+2) < size){
			punto1 = sinzigzag [i];
			punto2 = sinzigzag [i+1];
			punto3 = sinzigzag [i+2];

			//Creamos nuevo vectores de la curva
			for (int j = 0; j <= num_puntos_bezier; j++){
				Vector3 aux;
				aux = vectorBezier ((j/num_puntos_bezier), punto1, punto2, punto3);

				trayectoriaBezier.AddLast (aux);
			}

			i += 2; //Solo 2 porque si no no tenemos en cuenta el ultimo que debe iniciar el siguiente sector
		}

		while (i < size) { //Cuando queda algun nodo suelto sin añadir porque no se ha podido hacer una curva con el
			trayectoriaBezier.AddLast (sinzigzag [i]);
			i++;
		}

		suavizada = new Vector3[trayectoriaBezier.Count];

		i = 0;
		foreach (Vector3 vector_nuevo in trayectoriaBezier) {
			suavizada [i] = vector_nuevo;
			i++;
		}
	}

	private Vector3 vectorBezier (float bezierT, Vector3 punto1, Vector3 punto2, Vector3 punto3) {
		Vector3 vectorBezier = new Vector3 ();

		vectorBezier.x = coordenadaBezier (bezierT, punto1.x, punto2.x, punto3.x);
		vectorBezier.y = coordenadaBezier (bezierT, punto1.y, punto2.y, punto3.y);
		vectorBezier.z = coordenadaBezier (bezierT, punto1.z, punto2.z, punto3.z);

		return vectorBezier;
	}

	private float coordenadaBezier (float bezierT, float punto1x, float punto2x, float punto3x){
		float coordenada_bezier = 0.0f;

		coordenada_bezier = (Mathf.Pow((1 - bezierT), 2) * punto1x);
		coordenada_bezier += 2 * (1 - bezierT) * bezierT * punto2x;
		coordenada_bezier += Mathf.Pow (bezierT, 2) * punto3x;

		return coordenada_bezier;
	}

	public void setPesoTrayectoria (float nuevo_peso_trayectoria){
		peso_trayectoria = nuevo_peso_trayectoria;	
	}

	public void setPesoSuavizado (float nuevo_peso_suavizado){
		peso_suavizado = nuevo_peso_suavizado;
	}

	public void setTolerancia (float nuevo_tolerancia){
		tolerancia = nuevo_tolerancia;
	}

	public void setNumPuntosBezier (float nuevo_num_puntos_bezier){
		num_puntos_bezier = nuevo_num_puntos_bezier;
	}
}
