using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Parrilla {
	private List <GameObject> casillas;
	private GameObject casilla_abiertos;
	private GameObject casilla_cerrados;

	public Parrilla (GameObject c_abiertos, GameObject c_cerrados) {
		casilla_abiertos = c_abiertos;
		casilla_cerrados = c_cerrados;
		casillas = new List<GameObject> ();
	}

	// tipo = 1 -> cerrados
	// tipo = 0 -> abiertos
	public void crearCasilla (Vector3 posicion, int tipo) {
		GameObject casilla;
		Vector3 temp;

		if (tipo == 1) {
			casilla = GameObject.Instantiate<GameObject> (casilla_abiertos);
		} else {
			casilla = GameObject.Instantiate<GameObject> (casilla_cerrados);
		}

		temp = posicion;
		temp.y = 0.01f;
		casilla.transform.position = temp;

		temp = casilla.transform.localScale;
		temp.y = 1;
		casilla.transform.localScale = temp;

		casillas.Add (casilla);
	}

	public void borrarCasillas () {
		foreach (GameObject casilla in casillas){
			GameObject.Destroy(casilla);
		}
	}
}
