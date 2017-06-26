using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Theta_estrella  : A_estrella {
	//Cuando se calculan los sucesores, theta * mira si:
	// de un sucesor al nodo actual, hay una linea de vision al padre del nodo actual
	// si la hay considera esta camino en lugar desde el nodo actual
	// Ejemplo: nodo_actual.vector = (1,0,0)
	//          nodo_actual.padre = (0,0,0)
	//          sucesor.vector = (2,0,0)
	// Si hay linea de vision:
	//          sucesor.padre = nodo_actual.padre
	// Si no:
	//          sucesor.padre = nodo_actual;

	protected override Nodo [] rellenarSucesores (int num_sucesores, Nodo n_actual, Vector3[] movimientos){
		Nodo[] sucesor = new Nodo[num_sucesores];

		for (int i=0; i < num_sucesores; i++){
			sucesor [i] = new Nodo ();
			sucesor [i].vector = n_actual.vector + movimientos[i];

			if (n_actual.padre != null) { //El nodo inicial, su padre es null
				//if (mapa.lineaVision (sucesor [i].vector, n_actual.padre.vector) && mapa.lineaVision (n_actual.padre.vector, sucesor [i].vector)) {
				if (mapa.lineaVision (n_actual.padre.vector, sucesor[i].vector)){
					sucesor [i].padre = n_actual.padre;		
				} else {
					sucesor [i].padre = n_actual;		
				}
			} else {
				sucesor [i].padre = n_actual;		
			}


		}

		return sucesor;
	}

}
