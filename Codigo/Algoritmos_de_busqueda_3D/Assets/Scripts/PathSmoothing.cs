using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathSmoothing {
	private ObtenerMapa  mapa;
	private Vector3[] trayectoria;
	private float peso_trayectoria;
	private float peso_suavizado;
	private float tolerancia;
	private float num_puntos_bezier;

	public PathSmoothing (ObtenerMapa p_mapa, Vector3[] p_trayectoria){
		mapa = p_mapa;
		trayectoria = p_trayectoria;

		peso_trayectoria = 0.5f;
		peso_suavizado = 0.1f;
		tolerancia = 0.000001f;

		num_puntos_bezier = 10.0f;
	}

	public Vector3 [] getTrayectoriaSuavizadaCurvasBezier () {
		Vector3[] suavizada;
		Vector3[] sinzigzag;

		//Hacemos dos pasadas para mejorar la eliminacion del zigzag
		sinzigzag = eliminarZigZag (trayectoria);
		sinzigzag = eliminarZigZag (sinzigzag);

		suavizada = curvaBezier(sinzigzag);

		return suavizada;
	}

	public Vector3 [] getTrayectoriaDescensoGradiente () {
		Vector3[] suavizada;

		suavizada = descensoGradiente (trayectoria);

		return suavizada;
	}

	public Vector3 [] eliminarZigZag (Vector3[] p_trayectoria) {
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

		return temp;
	}

	//
	public Vector3 [] descensoGradiente (Vector3[] p_trayectoria){
		Vector3[] nueva_trayectoria = (Vector3[]) p_trayectoria.Clone();
		float error = tolerancia * 10.0f;

		while (error > tolerancia) {
			//Vector3[] temporal = (Vector3[]) nueva_trayectoria.Clone();
			error = 0.0f;

			for (int i = 1; i < (nueva_trayectoria.Length -1) ; i++) {
				
				Vector3 aux = nueva_trayectoria [i];

				nueva_trayectoria [i].x += peso_trayectoria * (trayectoria[i].x - nueva_trayectoria[i].x);	
				nueva_trayectoria [i].y += peso_trayectoria * (trayectoria[i].y - nueva_trayectoria[i].y);	
				nueva_trayectoria [i].z += peso_trayectoria * (trayectoria[i].z - nueva_trayectoria[i].z);

				nueva_trayectoria [i].x += peso_suavizado * (nueva_trayectoria[i+1].x + nueva_trayectoria[i-1].x - (2 * nueva_trayectoria[i].x));	
				nueva_trayectoria [i].y += peso_suavizado * (nueva_trayectoria[i+1].y + nueva_trayectoria[i-1].y - (2 * nueva_trayectoria[i].y));	
				nueva_trayectoria [i].z += peso_suavizado * (nueva_trayectoria[i+1].z + nueva_trayectoria[i-1].z - (2 * nueva_trayectoria[i].z));

				error += Mathf.Abs(aux.x - nueva_trayectoria [i].x) + Mathf.Abs(aux.y - nueva_trayectoria [i].y) + Mathf.Abs(aux.z - nueva_trayectoria [i].z);
			}
		}

		return nueva_trayectoria;
	}

	public Vector3 [] curvaBezier(Vector3[] sinzigzag) {
		Vector3[] suavizada;
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

		return suavizada;
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
