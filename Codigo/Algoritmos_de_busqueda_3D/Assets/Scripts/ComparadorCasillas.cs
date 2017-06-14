using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComparadorCasillas : IComparer<Casilla> {
	// 0 si son iguales 
	// -1 si x es menor
	// 1 si x es mayor

	public int Compare(Casilla casilla_1, Casilla casilla_2) {
		int mayor = 0;

		if ( Mathf.Approximately (casilla_1.vector.x, casilla_2.vector.x) && Mathf.Approximately (casilla_1.vector.y, casilla_2.vector.y) && Mathf.Approximately (casilla_1.vector.z, casilla_2.vector.z) ) {
			mayor = 0;
		}else {
			if (casilla_1.vector.magnitude < casilla_2.vector.magnitude) {
				mayor = -1;
			} else {
				mayor = 1;
			}
		}

		return mayor;
	}
}
