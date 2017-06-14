using UnityEngine;
using System.Collections;


public class MoverCoche : MonoBehaviour {
	//Compos de la clase
	private ObtenerMapa mapa;
	private PathSmoothing path_smoothing;
	private GameObject coche;
	private Rigidbody rb_coche;
	private int contador_vector;
	private Parrilla parrilla;
	private bool encontrada_meta;
	private float inicio;
	private float final;
	private bool error;
	private ControladorCoche script_algoritmo;
	private Vector3[] trayectoria;
	private Nodo[] trayectoria_nodos;
	private Vector3 salida_coche;
	private Vector3 meta;
	private PID_control pid;
	private PID_control_hybrid pid_hybrid;
	private float angulo_coche;

	private float input_fuerza;
	private float input_angulo;

	//Campor para el editor
	[SerializeField] private bool a_A_estrella = false;
	[SerializeField] private bool a_A_estrella_vertices = true;
	[SerializeField] private bool a_theta_estrella = false;
	[SerializeField] private bool a_hybrid_a_estrella = false;

	[SerializeField] private bool control_pid = true;
	[SerializeField] private float control_pid_param_p = 0.5f;
	[SerializeField] private float control_pid_param_i = 0.1f;
	[SerializeField] private float control_pid_param_d = 0.1f;
	[SerializeField] private bool control_manual = false;
	[SerializeField] private float coche_max_torque = 1000.0f;
	[SerializeField] private float coche_max_angulo = 45.0f;

	[SerializeField] private bool ps_path_smoothing_activado = true;
	[SerializeField] private bool ps_bezier = true;
	[SerializeField] private bool ps_descenso_gradiente = false;

	[SerializeField] private float a_param_velocidad = 10.0f;
	[SerializeField] private float a_param_peso_heuristica = 0.5f;
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
		int tam_parrilla;
		int ancho;
		int largo;

		error = false;
		encontrada_meta = false;

		parrilla = new Parrilla (casilla_abiertos, casilla_cerrados);

		mapa = new ObtenerMapa ();

		// Elegir algoritmo
		if (a_hybrid_a_estrella) {
			Debug.Log ("Usando Hybrid A Estrella");
			script_algoritmo = GetComponent<Hybrid_a_estrella> ();
		} else if (a_A_estrella) {
			Debug.Log ("Usando A Estrella");
			script_algoritmo = GetComponent<A_estrella> ();
		} else if (a_A_estrella_vertices) {
			Debug.Log ("Usando A Estrella con vertices");
			script_algoritmo = GetComponent<A_estrella_vertices> ();
		}else if (a_theta_estrella) {
			Debug.Log ("Usando Theta Estrella");
			script_algoritmo = GetComponent<Theta_estrella> ();
		}
			
		coche = GameObject.FindGameObjectWithTag ("Coche");
		rb_coche = coche.GetComponent<Rigidbody> ();
		salida_coche = coche.transform.position;
		meta = GameObject.FindGameObjectWithTag ("Meta").transform.position;
		meta.y = meta.y - 0.01f; // Esto es porque esta posicionada elevada por motivos esteticos
		largo = Mathf.FloorToInt((GameObject.FindGameObjectWithTag ("Suelo").transform.localScale.z * 10.0f));
		ancho = Mathf.FloorToInt((GameObject.FindGameObjectWithTag ("Suelo").transform.localScale.x * 10.0f));
		tam_parrilla = ancho * largo;

		angulo_coche = coche.transform.rotation.eulerAngles.y;

		parrilla.Ancho = ancho;
		parrilla.Largo = largo;

		Debug.Log ("Ancho: " + ancho + " | largo: " + largo);
		//Debug.Log ("Creando parrilla");
		//float inicio_parrilla = Time.realtimeSinceStartup;
		//parrilla.crearTodasCasilla ();
		//float final_parrilla = Time.realtimeSinceStartup;
		//Debug.Log ("Parrilla creada en: " + (final_parrilla - inicio_parrilla));


		//trayectoria = script_algoritmo.CalcularRuta (salida_coche, meta, mapa);
		encontrada_meta = false;

		inicio = Time.realtimeSinceStartup;
		script_algoritmo.iniciarCalcularRuta(salida_coche, meta, angulo_coche, mapa, parrilla, a_param_peso_heuristica, tam_parrilla, ancho, largo);
	}
	
	// Se ejecuta cada vez que se calculan las fisicas. Suele ser mas de una vez por frame
	void FixedUpdate () {
		if (control_manual) {

			moverCocheFisicas (input_fuerza * coche_max_torque, input_angulo * coche_max_angulo);

		}else if (encontrada_meta == true) {
			if (control_pid) {
				if (a_hybrid_a_estrella) {
					float[] resultado_pid;

					resultado_pid = pid_hybrid.pasoPID (control_pid_param_p, 0.0f, control_pid_param_d);

					if ( Mathf.Approximately(resultado_pid[0], 0.0f) && Mathf.Approximately(resultado_pid[1], 360.0f) ) {
						freno ();
					} else {
						moverCocheFisicas (resultado_pid[0], resultado_pid[1]);
					}
				}else{

					float[] resultado_pid;

					resultado_pid = pid.pasoPID (control_pid_param_p, 0.0f, control_pid_param_d);

					if ( Mathf.Approximately(resultado_pid[0], 0.0f) && Mathf.Approximately(resultado_pid[1], 360.0f) ) {
						freno ();
					} else {
						moverCocheFisicas (resultado_pid[0], resultado_pid[1]);
					}
				}
			
			} else if (contador_vector < trayectoria.Length) {
				bool llegado = false;

				llegado = moverCocheSinFisicas (trayectoria [contador_vector]);
				//script_algoritmo.MoverCoche (m_WheelColliders, m_WheelMeshes);
				if (llegado){
					contador_vector += 1;			
				}
			}
		}
	}


	// Se ejecuta una vez cada frame
	void Update (){
		bool fin;

		input_fuerza = Input.GetAxis ("Vertical");
		input_angulo = Input.GetAxis ("Horizontal");

		//input_fuerza *= Time.deltaTime;
		//input_angulo *= Time.deltaTime;

		if (error) {

			Debug.Log ("Error: no se ha encontrado un camino");

			parrilla.borrarTodasCasillas ();

		} else if (!encontrada_meta) {
			//script_algoritmo.MoverCoche (m_WheelColliders, m_WheelMeshes);
			fin = script_algoritmo.pasoCalcularRuta (out error);

			if (fin) {
				final = Time.realtimeSinceStartup;
				encontrada_meta = true;
				Debug.Log ("Ruta calculada en: " + (final - inicio));

				parrilla.borrarTodasCasillas ();	
				trayectoria = script_algoritmo.getTrayectoria ();
				trayectoria_nodos = script_algoritmo.getTrayectoriaNodos ();

				if (ps_path_smoothing_activado) {
					Vector3[] trayectoria_ps = trayectoria;

					if (a_hybrid_a_estrella) {
						trayectoria_ps = trayectoriaHybrid (trayectoria_nodos, trayectoria_nodos.Length);
					}

					path_smoothing = new PathSmoothing (mapa, trayectoria_ps);

					if (ps_bezier) {
						trayectoria = path_smoothing.getTrayectoriaSuavizadaCurvasBezier ();	
					} else if (ps_descenso_gradiente) {
						trayectoria = path_smoothing.getTrayectoriaDescensoGradiente ();	
					} else {
						trayectoria = path_smoothing.eliminarZigZag (trayectoria);
					}

					contador_vector = 0;
					DibujarTrayectoria (trayectoria, trayectoria.Length);
				} else {
					contador_vector = 0;

					if (a_hybrid_a_estrella) {
						DibujarTrayectoria (trayectoria_nodos, trayectoria_nodos.Length);
					} else {
						DibujarTrayectoria (trayectoria, trayectoria.Length);
					}

				}

				pid = new PID_control (coche, trayectoria);
				pid_hybrid = new PID_control_hybrid (coche, trayectoria_nodos);
				pid.setParrilla (parrilla);
				pid_hybrid.setParrilla (parrilla);
			} 
		}

	}

	// Dibujamos la trayectoria
	void DibujarTrayectoria (Vector3[] vertices, int num_vertices) {
		
		//m_LineRenderer.
		m_LineRenderer.SetVertexCount(num_vertices);
		m_LineRenderer.SetPositions (vertices);
	}

	// Dibujamos la trayectoria
	void DibujarTrayectoria (Nodo[] nodos, int num_vertices) {
		Vector3[] vertices = new Vector3[num_vertices];
		int indice = 0;

		foreach (Nodo n in nodos) {
			vertices [indice] = n.vector_hybrid;
			indice++;
		}

		//m_LineRenderer.
		m_LineRenderer.SetVertexCount(num_vertices);
		m_LineRenderer.SetPositions (vertices);
	}

	Vector3[] trayectoriaHybrid (Nodo[] nodos, int num_vertices) {
		Vector3[] vertices = new Vector3[num_vertices];
		int indice = 0;

		foreach (Nodo n in trayectoria_nodos) {
			vertices [indice] = n.vector_hybrid;
			indice++;
		}

		return vertices;
	}

	void ControlManual () {
		
	}

	void moverCocheFisicas (float p_fuerza_motor, float p_angulo_giro) {
		float angulo_giro = 0.0f;
		float fuerza_motor = 0.0f;

		if (p_angulo_giro > coche_max_angulo) {
			angulo_giro = coche_max_angulo;
		}else if (p_angulo_giro < (-1)*coche_max_angulo){
			angulo_giro = (-1)*coche_max_angulo;
		}else{
			angulo_giro = p_angulo_giro;	
		}

		if (p_fuerza_motor > coche_max_torque) {
			fuerza_motor = coche_max_torque;
		}else if (p_fuerza_motor < (-1)*coche_max_torque){
			fuerza_motor = (-1)*coche_max_torque;
		}else{
			fuerza_motor = p_fuerza_motor;	
		}
			
		// Si le damos fuerza a las 4 ruedas: 4x4
		// Si le damos fuerza a 0 y 1: tracion delantera
		// Si le damos fuerza a 2 y 3; traccion trasera
		m_WheelColliders [0].motorTorque = fuerza_motor;
		m_WheelColliders [1].motorTorque = fuerza_motor;
		m_WheelColliders [2].motorTorque = fuerza_motor;
		m_WheelColliders [3].motorTorque = fuerza_motor;

		//Solo giramos las ruedas delanteras
		m_WheelColliders [0].steerAngle = angulo_giro;
		m_WheelColliders [1].steerAngle = angulo_giro;
		m_WheelColliders [2].steerAngle = 0f;
		m_WheelColliders [3].steerAngle = 0f;

		// Para actualizar el giro de los modelos de las ruedas
		for (int i = 0; i < 4; i++) {
			Quaternion quat;
			Vector3 position;
			m_WheelColliders [i].GetWorldPose (out position, out quat);
			m_WheelMeshes [i].transform.position = position;
			m_WheelMeshes [i].transform.rotation = quat;
		}


		//Debug.Log ("Velocidad: " + Mathf.Round (rb_coche.velocity.magnitude * 3.6f) + "km/h");
	}

	bool moverCocheSinFisicas (Vector3 posicion){
		bool llegado = false;
		Vector3 movimiento;

		//Desactivamos las fisicas del coche porque no vamos a usarlas para moverlo en este metodo
		coche.GetComponent<Rigidbody> ().isKinematic = true;
		coche.GetComponent<Rigidbody> ().detectCollisions = false; 

		if (coche.transform.position == posicion) {
			llegado = true;
		} else {
			movimiento = Vector3.MoveTowards (coche.transform.position, posicion, a_param_velocidad * Time.deltaTime);
			coche.transform.LookAt (posicion, Vector3.up);
			coche.GetComponent<Rigidbody> ().MovePosition (movimiento);

		}

		//coche.transform.position = posicion;

		return llegado;
	}

	void freno (){
		m_WheelColliders [0].brakeTorque = 1000f;
		m_WheelColliders [1].brakeTorque = 1000f;
		m_WheelColliders [2].brakeTorque = 1000f;
		m_WheelColliders [3].brakeTorque = 1000f;
	}
}
