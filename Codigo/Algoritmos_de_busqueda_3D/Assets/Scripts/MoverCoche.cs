using UnityEngine;
using System.Collections;


public class MoverCoche : MonoBehaviour {
	public bool a_estrella;
	public bool hybrid_a_estrella;
	private ObtenerMapa mapa;
	private GameObject coche;
	private float contador_frames;
	int contador_vector;

	// Para poder aplicar la fuerza y el giro a las ruedas
	[SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
	// Para que los modelos de las ruedas giren
	[SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
	// Para dibujar la trayectoria
	[SerializeField] private LineRenderer m_LineRenderer;
	// Para usar un algorutmo u otro
	private ControladorCoche script_algoritmo;
	private Vector3[] trayectoria;

	// Use this for initialization
	void Start () {
		Vector3 salida_coche;
		Vector3 meta;
		float radio = 0.25f;

		contador_frames = 0.0f;

		mapa = new ObtenerMapa ();

		// get the car controller reference
		if (hybrid_a_estrella) {
			script_algoritmo = GetComponent<Hybrid_a_estrella> ();
		} else {
			script_algoritmo = GetComponent<A_estrella> ();
		}
			
		coche = GameObject.FindGameObjectWithTag ("Coche");
		salida_coche = coche.transform.position;
		meta = GameObject.FindGameObjectWithTag ("Meta").transform.position;

		trayectoria = script_algoritmo.CalcularRuta (salida_coche, meta, mapa);

		//Debug.Log (trayectoria.Length);
		while (trayectoria.Length == 0 && radio < 1.0f) {
			mapa.setRadio (radio);
			trayectoria = script_algoritmo.CalcularRuta (salida_coche, meta, mapa);
			radio += 0.1f;
		}

		Debug.Log ("Radio: " + radio);
		DibujarTrayectoria (trayectoria, trayectoria.Length);
		contador_vector = trayectoria.Length - 1;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//script_algoritmo.MoverCoche (m_WheelColliders, m_WheelMeshes);
		float offset = 5.0f;

		if (contador_frames%offset == 0 && contador_vector>=0) {
			script_algoritmo.MoverCoche (coche, trayectoria[contador_vector]);
			contador_vector -= 1;
		}

		contador_frames += 1.0f;
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


/*
 * Ejemplo de casos de prueba para los puntos recorribles del mapa
	Debug.Log ("Centro de la rotanda: 0.0f, 0.0f, -10.0f");
		if (mapa.esRecorrible (new Vector3 (0.0f, 0.0f, -10.0f))) {
			Debug.LogError ("Error: devuelve recorrible");
		} else {
			Debug.Log ("Correcto: no devuelve recorrible");
		}

		Debug.Log ("lado de la rotanda: 0.0f, 0.0f, -20.0f");
		if (mapa.esRecorrible (new Vector3 (0.0f, 0.0f, -20.0f))) {
			Debug.LogError ("Error: devuelve recorrible");
		} else {
			Debug.Log ("Correcto: no devuelve recorrible");
		}

		Debug.Log ("lado de la rotanda: 0.0f, 0.0f, 0.0f");
		if (mapa.esRecorrible (new Vector3 (0.0f, 0.0f, 0.0f))) {
			Debug.LogError ("Error: devuelve recorrible");
		} else {
			Debug.Log ("Correcto: no devuelve recorrible");
		}

		Debug.Log ("lado de la rotanda: 10.0f, 0.0f, -10.0f");
		if (mapa.esRecorrible (new Vector3 (10.0f, 0.0f, -10.0f))) {
			Debug.LogError ("Error: devuelve recorrible");
		} else {
			Debug.Log ("Correcto: no devuelve recorrible");
		}

		Debug.Log ("lado de la rotanda: -10.0f, 0.0f, -10.0f");
		if (mapa.esRecorrible (new Vector3 (-10.0f, 0.0f, -10.0f))) {
			Debug.LogError ("Error: devuelve recorrible");
		} else {
			Debug.Log ("Correcto: no devuelve recorrible");
		}

		Debug.Log ("Caja 1: 0.0f, 0.0f, 20.0f");
		if (mapa.esRecorrible (new Vector3 (0.0f, 0.0f, 20.0f))) {
			Debug.LogError ("Error: devuelve recorrible");
		} else {
			Debug.Log ("Correcto: no devuelve recorrible");
		}

		Debug.Log ("Caja 2: 10.0f, 0.0f, 10.0f");
		if (mapa.esRecorrible (new Vector3 (10.0f, 0.0f, 10.0f))) {
			Debug.LogError ("Error: devuelve recorrible");
		} else {
			Debug.Log ("Correcto: no devuelve recorrible");
		}

		Debug.Log ("Caja 3: -10.0f, 0.0f, 10.0f");
		if (mapa.esRecorrible (new Vector3 (-10.0f, 0.0f, 10.0f))) {
			Debug.LogError ("Error: devuelve recorrible");
		} else {
			Debug.Log ("Correcto: no devuelve recorrible");
		}

		Debug.Log ("Punto recorrible: 30.0f, 0.0f, -30.0f");
		if (mapa.esRecorrible (new Vector3 (30.0f, 0.0f, -30.0f))) {
			Debug.Log ("Correcto: devuelve recorrible");
		} else {
			Debug.LogError ("Error: no devuelve recorrible");
		}

		Debug.Log ("Meta: 0.0f, 0.0f, 40.0f");
		if (mapa.esRecorrible (new Vector3 (0.0f, 0.0f, 40.0f))) {
			Debug.Log ("Correcto: devuelve recorrible");
		} else {
			Debug.LogError ("Error: no devuelve recorrible");
		}
*/