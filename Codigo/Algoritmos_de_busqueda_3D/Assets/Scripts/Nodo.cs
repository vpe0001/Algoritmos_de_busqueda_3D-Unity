using UnityEngine;
using System.Collections;
using Priority_Queue;

public class Nodo : FastPriorityQueueNode {
	public Vector3 vector;
	public Nodo padre;
	public float coste;
	public float costeH;
	public float costeG;

	public Nodo (){}

	public Nodo (Vector3 _vector, Nodo _padre, float _coste, float _costeG, float _costeH){
		vector = _vector;
		padre = _padre;
		coste = _coste;
		costeH = _costeH;
		costeG = _costeG;
	}
}
