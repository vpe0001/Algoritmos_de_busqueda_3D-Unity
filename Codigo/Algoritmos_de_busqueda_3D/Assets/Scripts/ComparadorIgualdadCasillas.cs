using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComparadorIgualdadCasillas : IEqualityComparer<Casilla> {
	// 0 si son iguales 
	// -1 si x es menor
	// 1 si x es mayor

	public bool Equals (Casilla casilla_1, Casilla casilla_2) {
		bool iguales = false;

		if ( Mathf.Approximately (casilla_1.vector.x, casilla_2.vector.x) && Mathf.Approximately (casilla_1.vector.y, casilla_2.vector.y) && Mathf.Approximately (casilla_1.vector.z, casilla_2.vector.z) ) {
			iguales = true;
		}

		return iguales;
	}

	public int GetHashCode (Casilla casilla) {
		return casilla.GetHashCode ();
	}
}
