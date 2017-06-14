using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Parrilla {
	//private SortedDictionary <Vector3, GameObject[]> casillas2;
	//private SortedDictionary <Vector3, GameObject> casillas;
	private List <Casilla> casillas;
	private IComparer<Casilla> comparador_casillas;
	private GameObject casilla_abiertos;
	private GameObject casilla_cerrados;

	public int Ancho { get; set;} 
	public int Largo { get; set;} 

	public Parrilla (GameObject c_abiertos, GameObject c_cerrados) {
		casilla_abiertos = c_abiertos;
		casilla_cerrados = c_cerrados;
		//casillas = new List<GameObject> ();

		//casillas2 = new SortedDictionary<Vector3, GameObject[]> (new ComparadorVectores());
		casillas = new List<Casilla> ();
		comparador_casillas = new ComparadorCasillas ();

		Ancho = 0;
		Largo = 0;
	}

	// tipo = 1 -> cerrados
	// tipo = 0 -> abiertos
	public void crearCasilla (Vector3 posicion, int tipo) {
		GameObject casilla;
		//GameObject casilla_antigua;
		Vector3 temp;

		if (tipo == Constantes._ABIERTOS) {
			casilla = GameObject.Instantiate<GameObject> (casilla_abiertos);
		} else {
			casilla = GameObject.Instantiate<GameObject> (casilla_cerrados);
		}

		//La altura de la casilla independiente de la del vector posicion
		temp = posicion;
		temp.y = 0.01f;
		casilla.transform.position = temp;

		//Hacemos la casilla visible poniendo y a 1
		temp = casilla.transform.localScale;
		temp.y = 1;
		casilla.transform.localScale = temp;

		Casilla nueva_casilla = new Casilla (posicion, tipo, casilla);

		borrarCasilla (nueva_casilla); //si ya existe la borramos
			
		casillas.Add (nueva_casilla);

		//Debug.Log ("Tamanio parrilla: " + casillas.Count);
	}

	public void borrarCasilla (Vector3 posicion) {
		Casilla nueva_casilla = new Casilla (posicion, Constantes._ABIERTOS, null);

		borrarCasilla (nueva_casilla);
	}

	public void borrarCasilla (Casilla nueva_casilla) {
		int pos_casilla = casillas.BinarySearch (nueva_casilla, comparador_casillas);

		if ( pos_casilla >= 0 ) {
			Casilla anterior_casilla = casillas[pos_casilla];

			GameObject.Destroy ( anterior_casilla.go_casilla );
			casillas.Remove (anterior_casilla);
		}
	}

	public void borrarTodasCasillas () {
		foreach (Casilla c in casillas){
			GameObject.Destroy (c.go_casilla);
		}

		casillas.Clear ();
	}

	/*
	public void crearTodasCasilla () {
		Vector3 temp;
		GameObject casilla;
		int inicio_ancho = (Ancho / 2) * (-1);
		int inicio_largo = (Largo / 2) * (-1);

		for (int i=inicio_ancho; i<=(Ancho/2); i++) {
			for (int j=inicio_largo; j<=(Largo/2); j++) {
				Vector3 vector = new Vector3 (i, 0.0f, j);
				GameObject[] casillas_vector = new GameObject[2];

				temp = vector;
				temp.y = 0.01f;

				casilla = GameObject.Instantiate<GameObject> (casilla_abiertos);
				casilla.transform.position = temp;
				casillas_vector[1] = casilla;

				casilla = GameObject.Instantiate<GameObject> (casilla_cerrados);
				casilla.transform.position = temp;
				casillas_vector[0] = casilla;

				casillas.Add (vector, casillas_vector);
			}
		}

		
	}
	*/

	public void dibujarTodas (Abiertos abiertos, Cerrados cerrados) {
		SortedDictionary <Vector3, Nodo>.Enumerator enu_cerrados = cerrados.GetEnumerator();

		borrarTodasCasillas ();

		while ( enu_cerrados.MoveNext() ) {
			crearCasilla (enu_cerrados.Current.Value.vector, Constantes._CERRADOS);
		}

		foreach (Nodo n in abiertos) {
			crearCasilla (n.vector, Constantes._ABIERTOS);
		}
	}

}
