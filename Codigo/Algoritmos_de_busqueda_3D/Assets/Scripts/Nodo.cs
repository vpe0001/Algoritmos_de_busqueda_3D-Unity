using UnityEngine;
using System.Collections;
using Priority_Queue;

public class Nodo : FastPriorityQueueNode {
	public Vector3 vector;
	public Nodo padre;
	public float coste;
	public float costeH;
	public float costeG;
	public Vector3 vector_hybrid;
	public float angulo_hybrid;
	public int sentido;

	public Nodo (){}

	public Nodo (Vector3 _vector, Nodo _padre, float _coste, float _costeG, float _costeH, Vector3 _vector_hybrid, float _angulo_hybrid, int _sentido){
		vector = _vector;
		padre = _padre;
		coste = _coste;
		costeH = _costeH;
		costeG = _costeG;
		vector_hybrid = _vector_hybrid;
		angulo_hybrid = _angulo_hybrid;
		sentido = _sentido;
	}
}
