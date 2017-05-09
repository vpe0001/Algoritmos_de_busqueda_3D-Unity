using UnityEngine;
using System.Collections;


public class MoverCoche : MonoBehaviour {
	public bool a_estrella;
	public bool hybrid_a_estrella;

	// Para poder aplicar la fuerza y el giro a las ruedas
	[SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
	// Para que los modelos de las ruedas giren
	[SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
	[SerializeField] private LineRenderer m_LineRenderer;

	private ControladorCoche script_algoritmo;

	/*
	private void Awake() 	{
		// get the car controller reference
		if (hybrid_a_estrella) {
			script_algoritmo = GetComponent<Hybrid_a_estrella> ();
		} else {
			script_algoritmo = GetComponent<A_estrella> ();
		}
	}
	*/

	// Use this for initialization
	void Start () {
		Vector3[] trayectoria;

		// get the car controller reference
		if (hybrid_a_estrella) {
			script_algoritmo = GetComponent<Hybrid_a_estrella> ();
		} else {
			script_algoritmo = GetComponent<A_estrella> ();
		}


		Vector3[] vertices = new Vector3[5];

		// Sumamos 0.1f a la altura del suelo para que no se produzca flickering
		// al dibujar la linea
		//vertices [0] = new Vector3 (0.0f, 0.0f + 0.1f, 0.0f);
		//vertices [1] = new Vector3 (-5.0f, 0.0f + 0.1f, 10.0f);
		//vertices [2] = new Vector3 (-10.0f, 0.0f + 0.1f, 20.0f);
		//vertices [3] = new Vector3 (-5.0f, 0.0f + 0.1f, 30.0f);
		//vertices [4] = new Vector3 (0.0f, 0.0f + 0.1f, 40.0f);

		//DibujarTrayectoria (vertices, 5);

		trayectoria = script_algoritmo.CalcularRuta (new Vector3 (0.0f, 0.0f, 0.0f), new Vector3 (0.0f, 0.0f, 4.0f), 5.0f, -5.0f, 5.0f, -5.0f);

		Debug.Log (trayectoria.Length);

		DibujarTrayectoria (trayectoria, trayectoria.Length);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		script_algoritmo.MoverCoche (m_WheelColliders, m_WheelMeshes);
	}

	void Update () {
		
	}

	// Dibujamos la trayectoria
	void DibujarTrayectoria (Vector3[] vertices, int num_vertices) {
		
		//m_LineRenderer.
		m_LineRenderer.SetVertexCount(num_vertices);
		m_LineRenderer.SetPositions (vertices);
	}
}
