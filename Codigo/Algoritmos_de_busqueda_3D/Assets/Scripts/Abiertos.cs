using UnityEngine;
using System.Collections;
using Priority_Queue;

public class Abiertos {
	//private List <Nodo> abiertos;
	//private IComparer <Nodo> comparador;
	private FastPriorityQueue <Nodo> abiertos;
	//private SortedList <Nodo, Nodo> abiertos;
	//Queue <Nodo> prueba = new Queue<Nodo> ();

	public Abiertos (int max_num_nodos){
		//abiertos = new List <Nodo> ();
		//comparador = new ComparadorNodos ();
		abiertos = new FastPriorityQueue<Nodo>(max_num_nodos);

		//abiertos = new SortedList <Nodo, Nodo> (new ComparadorNodosParaSorted());
	}

	public void add (Nodo nodo) {
		abiertos.Enqueue(nodo, nodo.coste);
	}

	public bool comprobar (Nodo nodo) {
		Nodo encontrado;

		return find (nodo, out encontrado);
		//return abiertos.Contains(nodo);
	}

	public bool delete (Nodo nodo) {
		bool existe = false;

		existe = comprobar (nodo);

		if (existe) {
			abiertos.Remove (nodo);
		}

		return existe;
	}

	public bool find (Nodo nodo, out Nodo encontrado) {
		bool existe = false;
		encontrado = null;

		//existe = comprobar (nodo);

		foreach (Nodo n in abiertos){
			if (n.vector == nodo.vector) {
				encontrado = n;
				existe = true;

				break;
			}
		}

		return existe;
	}

	public Nodo getFirst (){
		/*
			Nodo primero = null;

			if (abiertos.Count > 0) {
				primero = abiertos.Keys [0];
				abiertos.RemoveAt (0);
			}
			*/

		return abiertos.Dequeue ();
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
