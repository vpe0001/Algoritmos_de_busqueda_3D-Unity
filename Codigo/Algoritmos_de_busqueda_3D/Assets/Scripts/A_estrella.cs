using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class A_estrella : ControladorCoche {
	
	private Cerrados cerrados = new Cerrados ();
	private List <Nodo> sucesores = new List <Nodo> ();
	private Abiertos abiertos = new Abiertos ();
	private Vector3[] v_trayectoria;
	private Vector3 inicio;
	private Vector3 meta;
	private ObtenerMapa mapa;
	private Parrilla parrilla;
	private Nodo nodo_final;
	private Nodo n_actual;
	private Nodo n_inicio;
	private bool meta_encontrada;

	public override void MoverCoche (WheelCollider[] m_WheelColliders, GameObject[] m_WheelMeshes) {
		float thrustTorque = 5000000f; 
		m_WheelColliders [0].motorTorque = thrustTorque;
		m_WheelColliders [1].motorTorque = thrustTorque;
		m_WheelColliders [2].motorTorque = thrustTorque;
		m_WheelColliders [3].motorTorque = thrustTorque;

		m_WheelColliders [0].steerAngle = 0f;
		m_WheelColliders [1].steerAngle = 0f;
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

	public override void MoverCoche (GameObject coche, Vector3 posicion){
		coche.transform.position = posicion;
	}

	public override void iniciarPasoAestrella(Vector3 v_inicio, Vector3 v_meta, ObtenerMapa v_mapa, Parrilla v_parrilla) {
		inicio = v_inicio;
		meta = v_meta;
		mapa = v_mapa;
		parrilla = v_parrilla;

		meta_encontrada = false;

		nodo_final = null;
		n_actual = null;
		n_inicio = new Nodo ();
		n_inicio.vector = inicio;
		n_inicio.padre = null;
		n_inicio.coste = 0;

		abiertos.add (n_inicio);
	}

	public override bool pasoAestrella () {
		//Buscar cola de prioridad

		if (abiertos.count() > 0 && !meta_encontrada) {
			n_actual = abiertos.getFirst ();
			cerrados.add (n_actual);
			parrilla.crearCasilla (n_actual.vector, 1);

			if (esMeta (n_actual, meta)) {
				meta_encontrada = true;
				nodo_final = n_actual;

			} else {
				sucesores = CalcularSucesores (n_actual, meta, mapa);

				foreach (Nodo siguiente in sucesores) {
					if (!abiertos.comprobar (siguiente) && !cerrados.comprobar (siguiente)) {
						abiertos.add (siguiente);
						parrilla.crearCasilla (siguiente.vector, 0);

					} else {
						Nodo anterior;
						Nodo mover_padre;

						if (abiertos.comprobar (siguiente)) {
							
							anterior = abiertos.find (siguiente);	

							if (anterior.coste > siguiente.coste) {
								
								anterior.padre = n_actual;
								anterior.coste = siguiente.coste;
								anterior = abiertos.find (siguiente);

							}
						} else {
							if (cerrados.comprobar (siguiente)) {
								
								anterior = cerrados.find (siguiente);	

								if (anterior.coste > siguiente.coste) {
									anterior.padre = n_actual;
									anterior.coste = siguiente.coste;

									anterior = cerrados.find (siguiente);
									Debug.Log ("siguiente3 coste = " + siguiente.coste);
									Debug.Log ("Anterior3 coste = " + anterior.coste);

									mover_padre = cerrados.find (siguiente.padre);
									cerrados.delete (mover_padre);
									abiertos.add (mover_padre);
									parrilla.crearCasilla (mover_padre.vector, 0);
								}
							}
						}
					}


				}


			}
		}


		v_trayectoria = vectoresCamino (nodo_final);

		return meta_encontrada;
	}

	public override Vector3[] getTrayectoria (){
		return v_trayectoria;
	}

	// A*
	public override Vector3[] CalcularRuta (Vector3 inicio, Vector3 meta, ObtenerMapa mapa, Parrilla parrilla) {
		
		iniciarPasoAestrella(inicio, meta, mapa, parrilla);

		while (!pasoAestrella ()) {
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
		}


		sucesores = SucesoresValidos (sucesor, mapa);

		foreach (Nodo sucesor_valido in sucesores) {
			sucesor_valido.padre = n_actual;

			sucesor_valido.costeG = funcionG (sucesor_valido) + sucesor_valido.padre.costeG;
			sucesor_valido.costeH = funcionH (sucesor_valido, meta);
			sucesor_valido.coste = sucesor_valido.costeG + sucesor_valido.costeH;
		}

		return sucesores;
	}

	// comprueba que los posibles sucesores esten dentro del rango posible
	private List <Nodo> SucesoresValidos (Nodo[] sucesor, ObtenerMapa mapa){
		List <Nodo> sucesores = new List<Nodo> ();

		foreach (Nodo sucesor_valido in sucesor) {
			
			if ( mapa.esRecorrible(sucesor_valido.vector) ){
				sucesores.Add (sucesor_valido);
			}
		}

		return sucesores;
	}


	private bool esMeta(Nodo nodo, Vector3 meta) {
		bool es_meta = false;

		/*
		if (nodo.vector == meta){
			es_meta = true;
		}
		*/
		// No tenemos en cuenta la altura
		if (nodo.vector.x == meta.x && nodo.vector.z == meta.z){
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

	private class Nodo {
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
		private List <Nodo> abiertos;
		private IComparer <Nodo> comparador;

		public Abiertos (){
			abiertos = new List <Nodo> ();
			comparador = new ComparadorNodos ();
		}

		public bool add (Nodo nodo) {
			bool existe = false;

			existe = comprobar (nodo);

			if (!existe) {
				abiertos.Add (nodo);
				abiertos.Sort (comparador);
			}

			return existe;
		}

		public bool comprobar (Nodo nodo) {
			bool existe = false;

			foreach (Nodo busqueda in abiertos ) {
				if (busqueda.vector == nodo.vector) {
					existe = true;
					break;
				}
			}

			return existe;
		}

		public bool delete (Nodo nodo) {
			bool existe = false;

			foreach (Nodo busqueda in abiertos ) {
				if (busqueda.vector == nodo.vector) {
					abiertos.Remove (busqueda);
					abiertos.Sort (comparador);
					existe = true;
					break;
				}
			}

			return existe;
		}

		public Nodo find (Nodo nodo) {
			Nodo encontrado = null;

			foreach (Nodo busqueda in abiertos ) {
				if (busqueda.vector == nodo.vector) {
					encontrado = busqueda;

					break;
				}
			}

			return encontrado;
		}

		public Nodo getFirst (){
			Nodo primero = null;

			if (count() > 0) {
				primero = abiertos [0];
				abiertos.RemoveAt (0);
			}

			return primero;
		}

		public Nodo getIndex (int index){
			Nodo primero = null;

			if (index < count() ){
				primero = abiertos [index];
			}

			return primero;
		}

		public List<Nodo>.Enumerator GetEnumerator(){
			return abiertos.GetEnumerator();
		}

		public int count (){
			return abiertos.Count;
		}
			
	}

	private class Cerrados {
		private List <Nodo> cerrados;


		public Cerrados (){
			cerrados = new List <Nodo> ();
		}

		public bool add (Nodo nodo) {
			bool existe = false;

			existe = comprobar (nodo);

			if (!existe) {
				cerrados.Add (nodo);
			}

			return existe;
		}

		public bool comprobar (Nodo nodo) {
			bool existe = false;

			foreach (Nodo busqueda in cerrados ) {
				if (busqueda.vector == nodo.vector) {
					existe = true;
					break;
				}
			}

			return existe;
		}

		public bool delete (Nodo nodo) {
			bool existe = false;

			foreach (Nodo busqueda in cerrados ) {
				if (busqueda.vector == nodo.vector) {
					cerrados.Remove (busqueda);

					existe = true;
					break;
				}
			}

			return existe;
		}

		public Nodo find (Nodo nodo) {
			Nodo encontrado = null;

			foreach (Nodo busqueda in cerrados ) {
				if (busqueda.vector == nodo.vector) {
					encontrado = busqueda;

					break;
				}
			}

			return encontrado;
		}
			
		public Nodo getIndex (int index){
			Nodo primero = null;

			if (index < count() ){
				primero = cerrados [index];
			}

			return primero;
		}

		public List<Nodo>.Enumerator GetEnumerator(){
			return cerrados.GetEnumerator();
		}

		public int count (){
			return cerrados.Count;
		}

		public Vector3[] toArray(){
			int size = 0;
			int i = 0;
			Nodo meta = cerrados [count () - 1];

			while (meta != null){
				size++;
				meta = meta.padre;
			}

			Vector3 [] camino = new Vector3[size];
			meta = cerrados [size - 1];
			while (meta != null){
				//Debug.Log ("MEta: " + meta.vector);
				camino [i] = meta.vector;
				i++;
				meta = meta.padre;
			}
				
			return camino;
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

}
