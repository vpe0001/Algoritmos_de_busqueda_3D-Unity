using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComparadorNodos : IComparer<Nodo>  {
	// 0 si son iguales 
	// -1 si x es menor
	// 1 si x es mayor

	public int Compare(Nodo x, Nodo y) {
		int mayor = 0;

		if (x.coste < y.coste){
			mayor = -1;
		}else if (x.coste > y.coste) {
			mayor = 1;
		}

		return mayor;
	}
}
