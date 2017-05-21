using UnityEngine;
using System.Collections;


public class MoverCoche : MonoBehaviour {
	public bool a_estrella;
	public bool hybrid_a_estrella;
	private ObtenerMapa mapa;
	private PathSmoothing path_smoothing;
	private GameObject coche;
	int contador_vector;
	private Parrilla parrilla;
	private bool encontrada_meta;
	private float inicio;
	private float final;
	public float peso;// = 0.5f; //con 1 encuentra el camino mas corto, pero es mas lento. Valores menores intenta acercars a la meta mas rapido aunque no sea en mejor camino
	private bool error;

	[SerializeField] private float velocidad = 1.0f;
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
	private float radio = 0.35f;
	private Vector3 salida_coche;
	private Vector3 meta;

	// Use this for initialization
	void Start () {
		error = false;
		encontrada_meta = false;

		parrilla = new Parrilla (casilla_abiertos, casilla_cerrados);

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
		meta.y = meta.y - 0.01f; // Esto es porque esta posicionada elevada por moticos esteticos

		//trayectoria = script_algoritmo.CalcularRuta (salida_coche, meta, mapa);
		encontrada_meta = false;

		inicio = Time.realtimeSinceStartup;
		script_algoritmo.iniciarPasoAestrella(salida_coche, meta, mapa, parrilla, peso);



	}
	
	// 
	void FixedUpdate () {
		bool fin;

		if (error) {
			
			Debug.Log ("Error: no se ha encontrado un camino");

			parrilla.borrarCasillas ();

		} else if ( !encontrada_meta ) {
			//script_algoritmo.MoverCoche (m_WheelColliders, m_WheelMeshes);
			fin = script_algoritmo.pasoAestrella (out error);

			if (fin) {
				final = Time.realtimeSinceStartup;
				encontrada_meta = true;
				Debug.Log ("A* terminado en " + (final - inicio));

				trayectoria = script_algoritmo.getTrayectoria ();

				//contador_vector = trayectoria.Length - 1;

				parrilla.borrarCasillas ();
				//DibujarTrayectoria (trayectoria, trayectoria.Length);

				path_smoothing = new PathSmoothing (mapa, trayectoria);

				trayectoria = path_smoothing.getTrayectoriaSuavizada ();

				contador_vector = trayectoria.Length - 1;
				DibujarTrayectoria (trayectoria, trayectoria.Length);
			} 
		}
	}

	//
	void Update (){
		if (encontrada_meta == true) {
			
			//if (contador_frames % offset == 0 && contador_vector >= 0) {
			if (contador_vector >= 0) {
				bool llegado = false;

				llegado = script_algoritmo.MoverCoche (coche, trayectoria [contador_vector], velocidad);
				//script_algoritmo.MoverCoche (m_WheelColliders, m_WheelMeshes);
				if (llegado){
					contador_vector -= 1;			
				}
			}
		}
	}

	// Dibujamos la trayectoria
	void DibujarTrayectoria (Vector3[] vertices, int num_vertices) {
		
		//m_LineRenderer.
		m_LineRenderer.SetVertexCount(num_vertices);
		m_LineRenderer.SetPositions (vertices);
	}
}
