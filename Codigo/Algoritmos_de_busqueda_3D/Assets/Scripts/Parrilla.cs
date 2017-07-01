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
	private GameObject distancias_0;
	private GameObject distancias_1;
	private GameObject distancias_2;
	private GameObject distancias_3;
	private GameObject distancias_4;
	private GameObject distancias_5;
	private GameObject distancias_6;
	private Casilla [,,] array_casillas;

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

	public Parrilla (GameObject c_abiertos, GameObject c_cerrados, int c_ancho, int c_largo) {
		casilla_abiertos = c_abiertos;
		casilla_cerrados = c_cerrados;
		//casillas = new List<GameObject> ();

		//casillas2 = new SortedDictionary<Vector3, GameObject[]> (new ComparadorVectores());
		casillas = new List<Casilla> ();
		comparador_casillas = new ComparadorCasillas ();

		Ancho = c_ancho;
		Largo = c_largo;

		array_casillas = new Casilla[Ancho+1, Largo+1, 1];
	}

	public Parrilla (GameObject c_abiertos, GameObject c_cerrados, int c_ancho, int c_largo, GameObject c_distancias_0, GameObject c_distancias_1, GameObject c_distancias_2, GameObject c_distancias_3, GameObject c_distancias_4, GameObject c_distancias_5, GameObject c_distancias_6 ) {
		casilla_abiertos = c_abiertos;
		casilla_cerrados = c_cerrados;
		distancias_0 = c_distancias_0;
		distancias_1 = c_distancias_1;
		distancias_2 = c_distancias_2;
		distancias_3 = c_distancias_3;
		distancias_4 = c_distancias_4;
		distancias_5 = c_distancias_5;
		distancias_6 = c_distancias_6;

		//casillas = new List<GameObject> ();

		//casillas2 = new SortedDictionary<Vector3, GameObject[]> (new ComparadorVectores());
		casillas = new List<Casilla> ();
		comparador_casillas = new ComparadorCasillas ();

		Ancho = c_ancho;
		Largo = c_largo;

		array_casillas = new Casilla[Ancho+1, Largo+1, 1];
	}

	// tipo = 1 -> cerrados
	// tipo = 0 -> abiertos
	public void crearCasilla (Vector3 posicion, int tipo) {
		GameObject casilla;
		//GameObject casilla_antigua;

		if (tipo == Constantes._ABIERTOS) {
			casilla = GameObject.Instantiate<GameObject> (casilla_abiertos);
		} else {
			casilla = GameObject.Instantiate<GameObject> (casilla_cerrados);
		}

		addCasilla ( posicion, casilla, tipo );

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

		for (int i=0; i<=Ancho; i++){
			for (int j=0 ;j<=Largo; j++){
				if (array_casillas [i, j, 0] != null) {
					GameObject.Destroy (array_casillas [i, j, 0].go_casilla);
				}
			}
			
		}

		casillas.Clear ();
	}
		
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

	public void crearCasillaDistancias (Vector3 posicion, int tipo) {
		GameObject casilla;
		//GameObject casilla_antigua;

		switch (tipo) {
			case 0:
				casilla = GameObject.Instantiate<GameObject> (distancias_0);
				break;
			case 1:
				casilla = GameObject.Instantiate<GameObject> (distancias_1);
				break;
			case 2:
				casilla = GameObject.Instantiate<GameObject> (distancias_2);
				break;
			case 3:
				casilla = GameObject.Instantiate<GameObject> (distancias_3);
				break;
			case 4:
				casilla = GameObject.Instantiate<GameObject> (distancias_4);
				break;
			case 5:
				casilla = GameObject.Instantiate<GameObject> (distancias_5);
				break;
			default:
				casilla = GameObject.Instantiate<GameObject> (distancias_6);
				break;
		}
			

		addCasilla ( posicion, casilla, tipo );

		//Debug.Log ("Tamanio parrilla: " + casillas.Count);
	}

	private void addCasilla ( Vector3 posicion, GameObject casilla, int tipo ) {
		Vector3 temp;

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
	}


	private Casilla crearCasillaArray (Vector3 posicion, int tipo){
		GameObject go_casilla;
		Casilla casilla;
		Vector3 temp;

		if (tipo == Constantes._ABIERTOS) {
			go_casilla = GameObject.Instantiate<GameObject> (casilla_abiertos);
		} else {
			go_casilla = GameObject.Instantiate<GameObject> (casilla_cerrados);
		}

		//La altura de la casilla independiente de la del vector posicion
		temp = posicion;
		temp.y = 0.01f;
		go_casilla.transform.position = temp;

		//Hacemos la casilla visible poniendo y a 1
		temp = go_casilla.transform.localScale;
		temp.y = 1;
		go_casilla.transform.localScale = temp;

		casilla = new Casilla (posicion, tipo, go_casilla);

		return casilla;
	}

	private void destruirCasillaArray (Vector3 posicion){
		//GameObject go_casilla;
		Casilla casilla;
		int x = Mathf.RoundToInt ( posicion.x + (Ancho/2));
		int z = Mathf.RoundToInt (posicion.z + (Largo/2));

		casilla = array_casillas[x,z,0];

		GameObject.Destroy (casilla.go_casilla);

		//return casilla;
	}

	public void visualizarCasilla (Vector3 posicion, int tipo) {
		//Vector3 temp;
		int x = Mathf.RoundToInt (posicion.x + (Ancho / 2));
		int z = Mathf.RoundToInt (posicion.z + (Largo / 2));
		Casilla casilla;

		casilla = array_casillas [x, z, 0];

		if (casilla != null) {
			destruirCasillaArray (posicion);

			casilla = crearCasillaArray (posicion, tipo);

			array_casillas [x, z, 0] = casilla;
		} else {
			casilla = crearCasillaArray (posicion, tipo);

			array_casillas [x, z, 0] = casilla;
		}
	}
}
