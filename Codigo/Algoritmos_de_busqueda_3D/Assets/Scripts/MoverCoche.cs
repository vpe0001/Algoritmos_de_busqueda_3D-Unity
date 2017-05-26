using UnityEngine;
using System.Collections;


public class MoverCoche : MonoBehaviour {
	//Compos de la clase
	private ObtenerMapa mapa;
	private PathSmoothing path_smoothing;
	private GameObject coche;
	private int contador_vector;
	private Parrilla parrilla;
	private bool encontrada_meta;
	private float inicio;
	private float final;
	private bool error;
	private ControladorCoche script_algoritmo;
	private Vector3[] trayectoria;
	private Vector3 salida_coche;
	private Vector3 meta;

	//Campor para el editor
	[SerializeField] private bool a_estrella;
	[SerializeField] private bool hybrid_a_estrella;
	[SerializeField] private bool a_estrella_vertices;

	[SerializeField] private bool path_smoothing_activado;
	[SerializeField] private bool ps_bezier;
	[SerializeField] private bool ps_descenso_gradiente;

	[SerializeField] private float velocidad = 10.0f;
	[SerializeField] private float peso_heuristica = 0.5f;
	[SerializeField] private GameObject casilla_abiertos;
	[SerializeField] private GameObject casilla_cerrados;

	// Para poder aplicar la fuerza y el giro a las ruedas
	[SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
	// Para que los modelos de las ruedas giren
	[SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
	// Para dibujar la trayectoria
	[SerializeField] private LineRenderer m_LineRenderer;
	// Para usar un algorutmo u otro


	// Inicializacion
	void Start () {
		error = false;
		encontrada_meta = false;

		parrilla = new Parrilla (casilla_abiertos, casilla_cerrados);

		mapa = new ObtenerMapa ();

		// Elegir algoritmo
		if (hybrid_a_estrella) {
			Debug.Log ("Usando Hybrid A Estrella");
			script_algoritmo = GetComponent<Hybrid_a_estrella> ();
		} else if (a_estrella) {
			Debug.Log ("Usando A Estrella");
			script_algoritmo = GetComponent<A_estrella> ();
		} else if (a_estrella_vertices) {
			Debug.Log ("Usando Hybrid A Estrella con vertices");
			script_algoritmo = GetComponent<A_estrella_vertices> ();
		}
			
		coche = GameObject.FindGameObjectWithTag ("Coche");
		salida_coche = coche.transform.position;
		meta = GameObject.FindGameObjectWithTag ("Meta").transform.position;
		meta.y = meta.y - 0.01f; // Esto es porque esta posicionada elevada por motivos esteticos

		//trayectoria = script_algoritmo.CalcularRuta (salida_coche, meta, mapa);
		encontrada_meta = false;

		inicio = Time.realtimeSinceStartup;
		script_algoritmo.iniciarCalcularRuta(salida_coche, meta, mapa, parrilla, peso_heuristica);
	}
	
	// Se ejecuta cada vez que se calculan las fisicas. Suele ser mas de una vez por frame
	void FixedUpdate () {
		bool fin;

		if (error) {
			
			Debug.Log ("Error: no se ha encontrado un camino");

			parrilla.borrarCasillas ();

		} else if ( !encontrada_meta ) {
			//script_algoritmo.MoverCoche (m_WheelColliders, m_WheelMeshes);
			fin = script_algoritmo.pasoCalcularRuta (out error);

			if (fin) {
				final = Time.realtimeSinceStartup;
				encontrada_meta = true;
				Debug.Log ("Ruta calculada en: " + (final - inicio));

				parrilla.borrarCasillas ();	
				trayectoria = script_algoritmo.getTrayectoria ();

				if (path_smoothing_activado) {
					path_smoothing = new PathSmoothing (mapa, trayectoria);

					if (ps_bezier) {
						trayectoria = path_smoothing.getTrayectoriaSuavizadaCurvasBezier ();	
					} else if (ps_descenso_gradiente) {
						trayectoria = path_smoothing.getTrayectoriaDescensoGradiente ();	
					} else {
						trayectoria = path_smoothing.eliminarZigZag(trayectoria);
					}
						
					contador_vector = trayectoria.Length - 1;
					DibujarTrayectoria (trayectoria, trayectoria.Length);
				} else {
					contador_vector = trayectoria.Length - 1;

					DibujarTrayectoria (trayectoria, trayectoria.Length);
					
				}
			} 
		}
	}

	// Se ejecuta una vez cada frame
	void Update (){
		if (encontrada_meta == true) {
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
