using UnityEngine;
using System.Collections;


public class MoverCoche : MonoBehaviour {
	public bool a_estrella;
	public bool hybrid_a_estrella;

	// Para poder aplicar la fuerza y el giro a las ruedas
	[SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
	// Para que los modelos de las ruedas giren
	[SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];

	private ControladorCoche script_algoritmo;

	private void Awake() 	{
		// get the car controller reference
		if (hybrid_a_estrella) {
			script_algoritmo = GetComponent<Hybrid_a_estrella> ();
		} else {
			script_algoritmo = GetComponent<A_estrella> ();
		}
	}

	// Use this for initialization
	void Start () {
		// get the car controller reference
		if (hybrid_a_estrella) {
			script_algoritmo = GetComponent<Hybrid_a_estrella> ();
		} else {
			script_algoritmo = GetComponent<A_estrella> ();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		script_algoritmo.MoverCoche (m_WheelColliders, m_WheelMeshes);
	}
}
