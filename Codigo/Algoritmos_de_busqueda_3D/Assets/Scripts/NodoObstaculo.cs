using UnityEngine;
using System.Collections;
using Priority_Queue;

public class NodoObstaculo {
	public Vector3 vector;
	public int obstaculo;

	public NodoObstaculo (){}

	public NodoObstaculo (Vector3 _vector, int _obstaculo){
		vector = _vector;
		obstaculo = _obstaculo;
	}
}
