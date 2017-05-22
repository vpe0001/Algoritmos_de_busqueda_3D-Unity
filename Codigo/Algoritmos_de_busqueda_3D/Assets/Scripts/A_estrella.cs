using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

public class A_estrella : ControladorCoche {
	
	private Cerrados cerrados = new Cerrados ();
	private List <Nodo> sucesores = new List <Nodo> ();
	private Abiertos abiertos = new Abiertos (100000);
	private Vector3[] v_trayectoria;
	private Vector3 vector_inicio;
	private Vector3 vector_meta;
	private ObtenerMapa mapa;
	private Parrilla parrilla;
	private Nodo nodo_final;
	private Nodo nodo_actual;
	private Nodo nodo_inicio;
	private bool meta_encontrada;
	private float peso;

	public override void MoverCoche (WheelCollider[] m_WheelColliders, GameObject[] m_WheelMeshes) {
		float thrustTorque = 5000000f; 
		m_WheelColliders [0].motorTorque = thrustTorque;
		m_WheelColliders [1].motorTorque = thrustTorque;
		m_WheelColliders [2].motorTorque = thrustTorque;
		m_WheelColliders [3].motorTorque = thrustTorque;

		m_WheelColliders [0].steerAngle = 35f;
		m_WheelColliders [1].steerAngle = 35f;
		m_WheelColliders [2].steerAngle = 0f;
		m_WheelColliders [3].steerAngle = 0f;

		for (int i = 0; i < 4; i++) {
			Quaternion quat;
			Vector3 position;
			m_WheelColliders [i].GetWorldPose (out position, out quat);
			m_WheelMeshes [i].transform.position = position;
			m_WheelMeshes [i].transform.rotation = quat;
		}
	}

	public override bool MoverCoche (GameObject coche, Vector3 posicion, float velocidad){
		bool llegado = false;
		Vector3 movimiento;

		coche.GetComponent<Rigidbody> ().isKinematic = true;
		coche.GetComponent<Rigidbody> ().detectCollisions = false; 

		if (coche.transform.position == posicion) {
			llegado = true;
		} else {
			movimiento = Vector3.MoveTowards (coche.transform.position, posicion, velocidad * Time.deltaTime);
			coche.transform.LookAt (posicion, Vector3.up);
			coche.GetComponent<Rigidbody> ().MovePosition (movimiento);

		}


		//coche.transform.position = posicion;

		return llegado;
	}

	public override void iniciarCalcularRuta(Vector3 v_inicio, Vector3 v_meta, ObtenerMapa v_mapa, Parrilla v_parrilla, float p_peso) {
		peso = p_peso;

		abiertos.getEmpty ();
		cerrados.getEmpty ();

		vector_inicio = v_inicio;
		vector_meta = v_meta;
		mapa = v_mapa;
		parrilla = v_parrilla;

		meta_encontrada = false;

		nodo_final = null;
		nodo_actual = null;
		nodo_inicio = new Nodo ();
		nodo_inicio.vector = vector_inicio;
		nodo_inicio.padre = null;
		nodo_inicio.coste = 0;

		abiertos.add (nodo_inicio);

	}

	public override bool pasoCalcularRuta (out bool error) {
		error = false;

		if (abiertos.count () > 0 && !meta_encontrada) {
			nodo_actual = abiertos.getFirst ();

			if (esMeta (nodo_actual, vector_meta)) { 
				meta_encontrada = true;
				nodo_final = nodo_actual;

				v_trayectoria = vectoresCamino (nodo_final);

			} else {
				cerrados.add (nodo_actual);
				parrilla.crearCasilla (nodo_actual.vector, 1);

				sucesores = CalcularSucesores (nodo_actual, vector_meta, mapa);

				foreach (Nodo sucesor in sucesores) {
					Nodo anterior;
					Nodo mover_padre;

					if (abiertos.find (sucesor, out anterior)) {

						if (anterior.coste > sucesor.coste) {

							anterior.padre = nodo_actual;
							anterior.coste = sucesor.coste;
							abiertos.updatePrioridad (anterior, anterior.coste);

						}
					} else {
						if (cerrados.find (sucesor, out anterior)) {

							if (anterior.coste > sucesor.coste) {
								anterior.padre = nodo_actual;
								anterior.coste = sucesor.coste;

								cerrados.find (sucesor.padre, out mover_padre);
								cerrados.delete (mover_padre);
								abiertos.add (mover_padre);
								parrilla.crearCasilla (mover_padre.vector, 0);
							}
						} else { //No esta ni en abiertos ni en cerrados
							abiertos.add (sucesor);
							parrilla.crearCasilla (sucesor.vector, 0);
						}
					}

				}

			}
		} else { //abiertos esta vacio
			if (abiertos.count () == 0) {
				error = true;
			}
		}
			
		return meta_encontrada;
	}

	public override Vector3[] getTrayectoria (){
		return v_trayectoria;
	}

	// A*
	public override Vector3[] CalcularRuta (Vector3 inicio, Vector3 meta, ObtenerMapa mapa, Parrilla parrilla, float p_peso) {
		bool error;
		peso = p_peso;
		
		iniciarCalcularRuta(inicio, meta, mapa, parrilla, peso);

		while (!pasoCalcularRuta (out error)) {
		}
			
		return v_trayectoria;
	}

	private List <Nodo> CalcularSucesores (Nodo n_actual, Vector3 meta, ObtenerMapa mapa) {
		List <Nodo> sucesores = new List <Nodo> ();
		const int num_sucesores = 8; //las 8 direcciones posibles
		Vector3[] movimientos = new Vector3[num_sucesores] {
			new Vector3 (0.0f, 0.0f, 1.0f),   // adelante
			new Vector3 (-1.0f, 0.0f, 1.0f),   // adelante derecha
			new Vector3 (-1.0f, 0.0f, 0.0f),   // derecha
			new Vector3 (-1.0f, 0.0f, -1.0f),  // atras derecha
			new Vector3 (0.0f, 0.0f, -1.0f),  // atras
			new Vector3 (1.0f, 0.0f, -1.0f), // atras izquierda
			new Vector3 (1.0f, 0.0f, 0.0f),  // izquiera
			new Vector3 (1.0f, 0.0f, 1.0f)   // adelante izquierda
		};

		//cada nodo 8 posibles sucesores
		Nodo[] sucesor = new Nodo[num_sucesores];

		for (int i=0; i < num_sucesores; i++){
			sucesor [i] = new Nodo ();
			sucesor [i].vector = n_actual.vector + movimientos[i];
			sucesor [i].padre = n_actual;
		}


		sucesores = SucesoresValidos (sucesor, mapa);

		foreach (Nodo sucesor_valido in sucesores) {
			sucesor_valido.costeG = funcionG (sucesor_valido) + sucesor_valido.padre.costeG;
			sucesor_valido.costeH = funcionH (sucesor_valido, meta);
			sucesor_valido.coste = (peso * sucesor_valido.costeG) + sucesor_valido.costeH;
		}
			
		return sucesores;
	}

	// comprueba que los posibles sucesores esten dentro del rango posible
	private List <Nodo> SucesoresValidos (Nodo[] sucesor, ObtenerMapa mapa){
		List <Nodo> sucesores = new List<Nodo> ();

		foreach (Nodo sucesor_valido in sucesor) {
			
			//if ( mapa.esRecorrible(sucesor_valido.vector) ){
			//Comprobamos las dos direcciones porque hemos encontrado que no siempre, aunque deberia, da el mismo resultado
			//Asi evitamos bugs debido a que sea visible/alcanzable desde una direccion pero no desde la otra
			if ( mapa.lineaVision (sucesor_valido.padre.vector, sucesor_valido.vector) && mapa.lineaVision (sucesor_valido.vector, sucesor_valido.padre.vector)){
				sucesores.Add (sucesor_valido);
			}
		}
			
		return sucesores;
	}


	private bool esMeta(Nodo nodo, Vector3 meta) {
		bool es_meta = false;

		if (nodo.vector == meta){
			es_meta = true;
		}
			
		return es_meta;
	}

	private float funcionG(Nodo nodo){
		float coste = 0;
		Vector3 distancia;

		if (nodo.padre != null) { 
			distancia = nodo.vector - nodo.padre.vector;
		}else {
			distancia = new Vector3(0.0f, 0.0f, 0.0f);
		}

		coste = distancia.magnitude;

		return coste;
	}

	private float funcionH(Nodo nodo, Vector3 meta){
		float coste = 0;

		Vector3 distancia;
		distancia = nodo.vector - meta;
		coste = distancia.magnitude;

		return coste;
	}

	private Vector3[] vectoresCamino (Nodo nodo_final){
		int size = 0;
		int i = 0;
		Nodo meta = nodo_final;


		while (meta != null){
			size++;
			meta = meta.padre;
		}

		Vector3 [] camino = new Vector3[size];
		meta = nodo_final;
		while (meta != null){
			camino [i] = meta.vector;
			i++;
			meta = meta.padre;
		}
			
		return camino;
	}

	private class Nodo : FastPriorityQueueNode {
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

	private class Abiertos {
		//private List <Nodo> abiertos;
		//private IComparer <Nodo> comparador;
		private FastPriorityQueue <Nodo> abiertos;
		//private SortedList <Nodo, Nodo> abiertos;
		//Queue <Nodo> prueba = new Queue<Nodo> ();

		public Abiertos (int max_num_nodos){
			//abiertos = new List <Nodo> ();
			//comparador = new ComparadorNodos ();
			abiertos = new FastPriorityQueue<Nodo>(max_num_nodos);

			//abiertos = new SortedList <Nodo, Nodo> (new ComparadorNodosParaSorted());
		}

		public void add (Nodo nodo) {
			abiertos.Enqueue(nodo, nodo.coste);
		}

		public bool comprobar (Nodo nodo) {
			Nodo encontrado;

			return find (nodo, out encontrado);
			//return abiertos.Contains(nodo);
		}

		public bool delete (Nodo nodo) {
			bool existe = false;

			existe = comprobar (nodo);

			if (existe) {
				abiertos.Remove (nodo);
			}

			return existe;
		}

		public bool find (Nodo nodo, out Nodo encontrado) {
			bool existe = false;
			encontrado = null;

			//existe = comprobar (nodo);

			foreach (Nodo n in abiertos){
				if (n.vector == nodo.vector) {
					encontrado = n;
					existe = true;

					break;
				}
			}

			return existe;
		}

		public Nodo getFirst (){
			/*
			Nodo primero = null;

			if (abiertos.Count > 0) {
				primero = abiertos.Keys [0];
				abiertos.RemoveAt (0);
			}
			*/
				
			return abiertos.Dequeue ();
		}
			

		public IEnumerator GetEnumerator(){
			return abiertos.GetEnumerator ();
		}

		public int count (){
			return abiertos.Count;
		}

		public void updatePrioridad (Nodo nodo, float prioridad){
			abiertos.UpdatePriority (nodo, prioridad);
		}

		public void getEmpty (){
			abiertos.Clear ();
		}
			
	}

	private class Cerrados {
		//private List <Nodo> cerrados;
		private SortedDictionary <Vector3, Nodo> cerrados;

		public Cerrados (){
			//cerrados = new List <Nodo> ();
			cerrados = new SortedDictionary <Vector3, Nodo> (new ComparadorVectores());
			//cerrados = new SortedDictionary <Vector3, Nodo> ();
		}

		public bool add (Nodo nodo) {
			bool existe = false;

			existe = comprobar (nodo);

			if (!existe) {
				cerrados.Add (nodo.vector, nodo);
			}

			return existe;
		}

		public bool comprobar (Nodo nodo) {
			return cerrados.ContainsKey(nodo.vector);
		}

		public bool delete (Nodo nodo) {
			return cerrados.Remove(nodo.vector);
		}

		public bool find (Nodo nodo, out Nodo encontrado) {
			return cerrados.TryGetValue(nodo.vector, out encontrado);
		}

		public SortedDictionary <Vector3, Nodo>.Enumerator GetEnumerator(){
			return cerrados.GetEnumerator();
		}

		public int count (){
			return cerrados.Count;
		}

		public void getEmpty (){
			cerrados.Clear ();
		}
	}

	private class ComparadorNodos : IComparer<Nodo> {
		// 0 si son iguales 
		// -1 si x es menor
		// 1 si x es mayor

		public int Compare(Nodo x, Nodo y) {
			int mayor = 0;

			if (x.coste < y.coste){
				mayor = -1;
			}else if (x.coste > y.coste) {
				mayor = 1;
			}

			return mayor;
		}
	}

	private class ComparadorNodosParaSorted : IComparer<Nodo> {
		// 0 si son iguales 
		// -1 si x es menor
		// 1 si x es mayor

		public int Compare(Nodo x, Nodo y) {
			int mayor = 0;

			if (x.vector == y.vector){
				mayor = 0;
			} else {
				if (x.coste < y.coste){
					mayor = -1;
				}else if (x.coste > y.coste) {
					mayor = 1;
				}
			}

			return mayor;
		}
	}

	private class ComparadorVectores : IComparer<Vector3> {
		// 0 si son iguales 
		// -1 si x es menor
		// 1 si x es mayor

		public int Compare(Vector3 vector_1, Vector3 vector_2) {
			int mayor = 0;

			if (vector_1.x == vector_2.x && vector_1.y == vector_2.y && vector_1.z == vector_2.z){
				mayor = 0;
			}else {
				if (vector_1.magnitude < vector_2.magnitude) {
					mayor = -1;
				} else {
					mayor = 1;
				}
			}

			return mayor;
		}
	}

	private class ComparadorIgualdadVectores : IEqualityComparer<Vector3> {
		
		public bool Equals (Vector3 vector_1, Vector3 vector_2) {
			bool iguales = false;

			if (vector_1.x == vector_2.x && vector_1.y == vector_2.y && vector_1.z == vector_2.z) {
				iguales = true;
			}

			return iguales;
		}

		public int GetHashCode (Vector3 vector) {
			return vector.GetHashCode ();
		}
		
	}

}
