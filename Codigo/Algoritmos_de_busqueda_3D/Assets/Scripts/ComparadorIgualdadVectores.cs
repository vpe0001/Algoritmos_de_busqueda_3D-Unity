using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComparadorIgualdadVectores : IEqualityComparer<Vector3> {

	public bool Equals (Vector3 vector_1, Vector3 vector_2) {
		bool iguales = false;

		if (vector_1.x == vector_2.x && vector_1.y == vector_2.y && vector_1.z == vector_2.z) {
			iguales = true;
		}

		return iguales;
	}

	public int GetHashCode (Vector3 vector) {
		return vector.GetHashCode ();
	}
}
