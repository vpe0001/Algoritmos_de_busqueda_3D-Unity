using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cerrados {
	private SortedDictionary <Vector3, Nodo> cerrados;

	public Cerrados (){
		cerrados = new SortedDictionary <Vector3, Nodo> (new ComparadorVectores());
	}

	public bool add (Nodo nodo) {
		bool existe = false;

		existe = comprobar (nodo);

		if (!existe) {
			cerrados.Add (nodo.vector, nodo);
		}

		return existe;
	}

	public bool comprobar (Nodo nodo) {
		return cerrados.ContainsKey(nodo.vector);
	}

	public bool delete (Nodo nodo) {
		return cerrados.Remove(nodo.vector);
	}

	public bool find (Nodo nodo, out Nodo encontrado) {
		return cerrados.TryGetValue(nodo.vector, out encontrado);
	}

	public SortedDictionary <Vector3, Nodo>.Enumerator GetEnumerator(){
		return cerrados.GetEnumerator();
	}

	public int count (){
		return cerrados.Count;
	}

	public void getEmpty (){
		cerrados.Clear ();
	}
}
