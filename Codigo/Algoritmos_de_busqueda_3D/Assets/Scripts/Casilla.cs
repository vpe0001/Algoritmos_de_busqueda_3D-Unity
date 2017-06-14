using UnityEngine;
using System.Collections;

public class Casilla {
	public Vector3 vector { get; set;}
	public int tipo { get; set;}
	public GameObject go_casilla { get; set;}

	public Casilla () {
		vector = new Vector3 (0.0f, 0.0f, 0.0f);
		tipo = Constantes._ABIERTOS;
		go_casilla = null;
	}

	public Casilla (Vector3 _vector, int _tipo, GameObject _go_casilla) {
		vector = _vector;
		tipo = _tipo;
		go_casilla = _go_casilla;
	}
}
