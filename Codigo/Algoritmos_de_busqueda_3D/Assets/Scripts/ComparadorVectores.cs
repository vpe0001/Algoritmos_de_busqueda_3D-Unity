using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComparadorVectores : IComparer<Vector3> {
	// 0 si son iguales 
	// -1 si x es menor
	// 1 si x es mayor

	public int Compare(Vector3 vector_1, Vector3 vector_2) {
		int mayor = 0;

		if (vector_1.x == vector_2.x && vector_1.y == vector_2.y && vector_1.z == vector_2.z){
			mayor = 0;
		}else {
			if (vector_1.magnitude < vector_2.magnitude) {
				mayor = -1;
			} else {
				mayor = 1;
			}
		}

		return mayor;
	}
}
