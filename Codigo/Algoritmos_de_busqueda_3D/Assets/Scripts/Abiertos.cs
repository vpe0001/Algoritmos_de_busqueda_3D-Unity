using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

public class Abiertos {
	private FastPriorityQueue <Nodo> abiertos;
	private int [,,] abiertos_comprobar;
	private int ancho;
	private int largo;

	public Abiertos (int max_num_nodos, int p_ancho, int p_largo){
		ancho = p_ancho;
		largo = p_largo;
		
		abiertos = new FastPriorityQueue<Nodo>(max_num_nodos);
		abiertos_comprobar = new int[ancho+1, largo+1, 1];

		for (int i=0; i<= ancho; i++){
			for (int j=0; j<= largo; j++){
				abiertos_comprobar [i, j, 0] = Constantes._LIBRE;
			}
		}
	}

	public void add (Nodo nodo) {
		int x = Mathf.RoundToInt (nodo.vector.x + (ancho/2));
		int z = Mathf.RoundToInt (nodo.vector.z + (largo/2));

		abiertos.Enqueue(nodo, nodo.coste);
		abiertos_comprobar [x, z, 0] = Constantes._OCUPADO;

	}

	public bool comprobar (Nodo nodo) {
		bool encontrado = false;
		int x = Mathf.RoundToInt (nodo.vector.x + (ancho/2));
		int z = Mathf.RoundToInt (nodo.vector.z + (largo/2));


		if (abiertos_comprobar[x,z,0] == Constantes._OCUPADO) {
			encontrado = true;
		}

		return encontrado;
	}

	public bool delete (Nodo nodo) {
		bool existe = false;
		int x = Mathf.RoundToInt (nodo.vector.x + (ancho/2));
		int z = Mathf.RoundToInt (nodo.vector.z + (largo/2));

		existe = comprobar (nodo);

		if (existe) {
			abiertos.Remove (nodo);
			abiertos_comprobar[x,z,0] = Constantes._LIBRE;
		}

		return existe;
	}

	public bool find (Nodo nodo, out Nodo encontrado) {
		bool existe = false;
		encontrado = null;

		existe = comprobar (nodo);

		if (existe) {
			foreach (Nodo n in abiertos) {
				if (n.vector == nodo.vector) {
					encontrado = n;
					existe = true;

					break;
				}
			}
		}

		return existe;
	}


	public Nodo getFirst (){
		Nodo primero = abiertos.Dequeue ();
		int x = Mathf.RoundToInt (primero.vector.x + (ancho/2));
		int z = Mathf.RoundToInt (primero.vector.z + (largo/2));

		abiertos_comprobar[x,z,0] = Constantes._LIBRE;

		return primero;
	}


	public IEnumerator GetEnumerator(){
		return abiertos.GetEnumerator ();
	}

	public int count (){
		return abiertos.Count;
	}

	public void updatePrioridad (Nodo nodo, float prioridad){
		abiertos.UpdatePriority (nodo, prioridad);
	}

	public void getEmpty (){
		abiertos.Clear ();
	}
}
