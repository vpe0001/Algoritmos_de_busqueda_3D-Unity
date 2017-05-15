using UnityEngine;
using System.Collections;


public class MoverCoche : MonoBehaviour {
	public bool a_estrella;
	public bool hybrid_a_estrella;
	private ObtenerMapa mapa;
	private GameObject coche;
	private float contador_frames;
	int contador_vector;
	private Parrilla parrilla;
	private float offset = 4.0f;
	private bool encontrada_meta;

	[SerializeField] private GameObject casilla_abiertos;
	[SerializeField] private GameObject casilla_cerrados;

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

		parrilla = new Parrilla (casilla_abiertos, casilla_cerrados);

		contador_frames = 0.0f;

		mapa = new ObtenerMapa ();
		mapa.setRadio (radio);

		// get the car controller reference
		if (hybrid_a_estrella) {
			script_algoritmo = GetComponent<Hybrid_a_estrella> ();
		} else {
			script_algoritmo = GetComponent<A_estrella> ();
		}
			
		coche = GameObject.FindGameObjectWithTag ("Coche");
		salida_coche = coche.transform.position;
		meta = GameObject.FindGameObjectWithTag ("Meta").transform.position;

		//trayectoria = script_algoritmo.CalcularRuta (salida_coche, meta, mapa);
		encontrada_meta = false;
		script_algoritmo.iniciarPasoAestrella(salida_coche, meta, mapa, parrilla);



	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//script_algoritmo.MoverCoche (m_WheelColliders, m_WheelMeshes);
		if (script_algoritmo.pasoAestrella () && encontrada_meta == false) {
			trayectoria = script_algoritmo.getTrayectoria ();
			contador_vector = trayectoria.Length - 1;
			//parrilla.borrarCasillas ();
			DibujarTrayectoria (trayectoria, trayectoria.Length);
			encontrada_meta = true;

		} 

		if (encontrada_meta == true) {
			if (contador_frames % offset == 0 && contador_vector >= 0) {
				script_algoritmo.MoverCoche (coche, trayectoria [contador_vector]);
				contador_vector -= 1;
				contador_frames = 0.0f;
			}
				
			contador_frames += 1.0f;
		}
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
