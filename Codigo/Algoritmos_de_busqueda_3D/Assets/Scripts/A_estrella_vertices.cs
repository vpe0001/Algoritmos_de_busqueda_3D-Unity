using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class A_estrella_vertices : A_estrella {
	
	protected override List <Nodo> CalcularSucesores (Nodo n_actual, Vector3 meta, ObtenerMapa mapa) {
		List <Nodo> sucesores = new List <Nodo> ();

		foreach (Vector3 vector in vertices) {
			// La version comprobando las linea de vision en los dos sentidos no funciona bien con los vertices
			//  porque no encuentra los mejores caminos
			//if (mapa.lineaVision (n_actual.vector, vector) && mapa.lineaVision (vector, n_actual.vector)) {
			if (mapa.lineaVision (n_actual.vector, vector)) {
				Nodo nuevo_sucesor = new Nodo ();

				nuevo_sucesor.vector = vector;
				nuevo_sucesor.padre = n_actual;

				nuevo_sucesor.costeG = funcionG (nuevo_sucesor) + nuevo_sucesor.padre.costeG;
				nuevo_sucesor.costeH = funcionH (nuevo_sucesor, meta);
				nuevo_sucesor.coste = (peso * nuevo_sucesor.costeG) + nuevo_sucesor.costeH;

				sucesores.Add (nuevo_sucesor);
			}
		}

		return sucesores;
	}

}
