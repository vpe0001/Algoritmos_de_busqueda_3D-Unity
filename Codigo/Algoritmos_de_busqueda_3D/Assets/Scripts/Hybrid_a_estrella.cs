using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hybrid_a_estrella : A_estrella {
	protected int ancho;
	protected int largo;
	//protected List<NodoObstaculo> mapa_obstaculos;
	protected int [,,] array_mapa_obstaculos;
	protected int [,,] array_mapa_distancias;
	protected int [,,] array_heuristica_obstaculos;


	private float radio_giro_rad;
	private float radio_giro;
	private float tiempo_inicio;
	private float tiempo_final;

	public override void iniciarCalcularRuta(Vector3 v_inicio, Vector3 v_meta, float v_angulo_coche, ObtenerMapa v_mapa, Parrilla v_parrilla, float p_peso, int tam_parrilla, int v_ancho, int v_largo, bool v_dibujar_casillas) {
		Vector3[] array_vertices;

		dibujar_casillas = v_dibujar_casillas;

		ancho = v_ancho;
		largo = v_largo;

		//cerrados = new Cerrados (ancho, largo);
		cerrados = new Cerrados ();
		sucesores = new List <Nodo> ();
		abiertos = new Abiertos (tam_parrilla, ancho, largo);
		//abiertos = new Abiertos (tam_parrilla);

		peso = p_peso;

		abiertos.getEmpty ();
		cerrados.getEmpty ();

		vector_inicio = v_inicio;
		vector_meta = v_meta;
		mapa = v_mapa;
		parrilla = v_parrilla;

		vertices = new HashSet<Vector3> ();
		array_vertices = mapa.getVertices ();

		foreach (Vector3 vertice in array_vertices){
			vertices.Add (vertice);
		}

		vertices.Add (vector_meta);


		meta_encontrada = false;

		nodo_final = null;
		nodo_actual = null;
		nodo_inicio = new Nodo ();
		nodo_inicio.vector = vector_inicio;
		nodo_inicio.vector_hybrid = vector_inicio;
		nodo_inicio.angulo_hybrid = v_angulo_coche;
		nodo_inicio.padre = null;
		nodo_inicio.coste = 0;

		abiertos.add (nodo_inicio);



		mapa.setRadio (0.5f); //

		array_mapa_obstaculos = new int[ancho+1, largo+1, 1];

		tiempo_inicio = Time.realtimeSinceStartup;
		crearMapaObstaculos ();
		tiempo_final = Time.realtimeSinceStartup;

		Debug.Log ("Mapa de obstaculos creado en: " + (tiempo_final - tiempo_inicio) + " segs.");


		tiempo_inicio = Time.realtimeSinceStartup;
		array_mapa_distancias = crearMapaDistancias ();
		tiempo_final = Time.realtimeSinceStartup;

		Debug.Log ("Mapa de Distancias creado en: " + (tiempo_final - tiempo_inicio) + " segs.");


		tiempo_inicio = Time.realtimeSinceStartup;
		array_heuristica_obstaculos = crearMapaHeuristicaObstaculos ();
		tiempo_final = Time.realtimeSinceStartup;

		Debug.Log ("Mapa de heuristica de obstaculos creado en: " + (tiempo_final - tiempo_inicio) + " segs.");

		radio_giro_rad = ((Constantes.distancia / Constantes.coche_largo) * Mathf.Tan (Mathf.Deg2Rad * Constantes.coche_max_angulo));
		radio_giro = Mathf.Rad2Deg * radio_giro_rad;

		Debug.Log ("Radio giro rad: " + radio_giro_rad);
		Debug.Log ("Radio giro: " + radio_giro);
	}

	private void crearMapaObstaculos (){
		int ocupado = Constantes._LIBRE;

		int inicio_ancho = (ancho / 2) * (-1);
		int inicio_largo = (largo / 2) * (-1);

		for (int i = inicio_ancho; i <= (ancho / 2); i++) {
			for (int j = inicio_largo; j <= (largo / 2); j++) {
				Vector3 vector = new Vector3 (i, 0.0f, j);

				if (mapa.esRecorrible (vector)) {
					ocupado = Constantes._LIBRE;
				} else {
					ocupado = Constantes._OBSTACULO;
				}

				array_mapa_obstaculos [i+(ancho/2), j+(largo/2), 0] = ocupado;
			}
		}
	}

	protected override List <Nodo> CalcularSucesores (Nodo n_actual, Vector3 meta, ObtenerMapa mapa) {
		
		//float radio_giro_rad = ((Constantes.distancia / Constantes.coche_largo) * Mathf.Tan (Mathf.Deg2Rad * Constantes.coche_max_angulo));
		//float radio_giro = Mathf.Rad2Deg * radio_giro_rad;

		//Habra 6 sucesores: adelante, retroceder
		//					 adelante giro izquierda, adelante giro derecha
		//					 retroceder giro izquierda, retroceder giro derecha
		Nodo[] sucesores_array = new Nodo[6];
		Nodo[] sucesores_validos;
		List <Nodo> sucesores_discretos = new List<Nodo>();
		List <Nodo> sucesores_seleccionados = new List<Nodo>();
		//List <Nodo> final = new List<Nodo> ();

		float angulo_coche = n_actual.angulo_hybrid;
		float angulo_coche_rad = (Mathf.Deg2Rad * angulo_coche);

		float x = 0.0f;
		float z = 0.0f;
		float y = 0.0f;

		int indice_sucesor = 0;

		//Calculamos la posicion continua
		for (int i = 0; i < Constantes.numero_marchas; i++) {
			int direccion_giro = ((Constantes.numero_angulos - 1) / 2) * (-1);

			for (int j=0; j < Constantes.numero_angulos; j++) {
				Nodo sucesor = new Nodo();
				float angulo_calculos = 0.0f;

				x = n_actual.vector_hybrid.x;
				z = n_actual.vector_hybrid.z;
				y = n_actual.vector_hybrid.y;

				sucesor.padre = n_actual;

				if (i == 0) { //hacia adelante
					angulo_calculos = (radio_giro_rad * direccion_giro) + angulo_coche_rad;

					sucesor.sentido = Constantes.hacia_adelante;
					
				} else { //hacia atras
					angulo_calculos = (radio_giro_rad * direccion_giro) + angulo_coche_rad + Constantes.angulo_atras_rad;

					sucesor.sentido = Constantes.hacia_atras;
				}

				//Nuestro eje de referencia es Z
				z = z + Constantes.vdt * Mathf.Cos (angulo_calculos);
				x = x + Constantes.vdt * Mathf.Sin (angulo_calculos);
				//Y sera la misma, si cambiase la altura seria y + altura de la posicion nueva, del suelo en el nuevo z,x

				sucesor.vector_hybrid = new Vector3 (x, y, z);
				sucesor.angulo_hybrid = (radio_giro * direccion_giro) + n_actual.angulo_hybrid;

				if (sucesor.angulo_hybrid < 0.0f) {
					sucesor.angulo_hybrid += 360.0f;
				}

				sucesores_array[indice_sucesor] = sucesor;
				indice_sucesor++;

				direccion_giro++;
			}
		}

		//Calculamos la posicion discreta
	
		//Calcular casilla
		for (int i =0; i < sucesores_array.Length; i++){
			Nodo actual = sucesores_array [i];


			float x_casilla_i = Mathf.Round (sucesores_array[i].vector_hybrid.x);
			float y_casilla_i = Mathf.Round (sucesores_array[i].vector_hybrid.y);
			float z_casilla_i = Mathf.Round (sucesores_array[i].vector_hybrid.z);

			Vector3 nuevo_casilla = new Vector3 (x_casilla_i, y_casilla_i, z_casilla_i);
			actual.vector = nuevo_casilla;
			sucesores_discretos.Add (actual);


			//actual.vector = actual.vector_hybrid;
			//sucesores_discretos.Add (actual);
		}

		//Calcular coste
		foreach (Nodo sucesor_valido in sucesores_discretos) {
			sucesor_valido.costeG = funcionG (sucesor_valido);
			sucesor_valido.costeH = funcionH (sucesor_valido, meta);
			sucesor_valido.coste = (peso * sucesor_valido.costeG) + sucesor_valido.costeH;
		}
			
		//Descartar peores

		for (int i=0; i<sucesores_discretos.Count; i++) {
			int contador = 0;

			for (int j=1; j<sucesores_discretos.Count; j++) {
				if (sucesores_discretos [i].vector == sucesores_discretos [j].vector) {
					if (sucesores_discretos [i].coste > sucesores_discretos [j].coste) {
						contador++;
					}
				}
			}


			if (contador == 0) {
				sucesores_seleccionados.Add (sucesores_discretos [i]);
			}
		}

		sucesores_validos = new Nodo[sucesores_seleccionados.Count];
		for (int i = 0; i < sucesores_seleccionados.Count; i++) {
			sucesores_validos [i] = sucesores_seleccionados [i];
		}

		//Sucesores validos
		sucesores = SucesoresValidos (sucesores_validos, mapa);
	
		return sucesores;
	}

	// comprueba que los posibles sucesores esten dentro del rango posible
	protected override List <Nodo> SucesoresValidos (Nodo[] sucesor, ObtenerMapa mapa){
		List <Nodo> sucesores = new List<Nodo> ();

		foreach (Nodo sucesor_valido in sucesor) {
			if ( mapa.lineaVision (sucesor_valido.padre.vector_hybrid, sucesor_valido.vector_hybrid) ){
					sucesores.Add (sucesor_valido);	
			}
		}

		return sucesores;
	}

	protected override float funcionG (Nodo nodo){
		float coste = 0.0f;
		Vector3 distancia;
		int pos_x = Mathf.RoundToInt ( nodo.vector.x + (ancho / 2) );
		int pos_z = Mathf.RoundToInt ( nodo.vector.z + (largo / 2) );
		int distancia_obstaculo = array_mapa_distancias [pos_x, pos_z, 0];
		float penalizacion_distancias = 1.0f;

		if (nodo.padre != null) { 
			distancia = nodo.vector_hybrid - nodo.padre.vector_hybrid;
			coste = distancia.magnitude + nodo.padre.costeG;
		}else {
			distancia = nodo.vector_hybrid - nodo_inicio.vector;

			coste = distancia.magnitude;
		}

		// Si va en linea recta es menos costoso que girar
		if (Mathf.Approximately (nodo.padre.angulo_hybrid, nodo.angulo_hybrid)) {
			coste *= 1.0f;
		} else {
			/*
			 * El multiplicador debe ser pequeño para que pueda girar y encuentre una ruta
			 * pero no tan pequeño que resulte en rutas que trazen circulor
			*/

			//coste *= 1.0f;
			coste *= 1.0053f;
			//coste *= 1.0042f;
			//coste *= 2.009f;
		}

		//Si hacia atras es mas costoso que hacia adelante
		if (nodo.sentido == Constantes.hacia_atras) {
			/*
			 * El multiplicador debe ser grande porque si no elegiria ir siempre hacia atras
			 * Ademas si es muy pequeño los nodos de busqueda que tendra en cuenta sera mayor,
			 *  al tener en cuenta los caminos hacia atras mas a menudo
			 * Por contra al ser grande es dificil que haga maniobras a medio camino
			*/
			//coste *= 1.0f;
			coste *= 4.545f;
			//coste *= 5.0f;
		} else {
			coste *= 1.0f;
		}


		/*
		 * En el mapa de distancias se guardan como enteros multiplicados por 10
		 *  para que los decimales pasen a la parte entera
		 * Se hace un porcentaje y se disminuye el coste porque es mejor cuanto mayor sea la distancia
		*/
		penalizacion_distancias = (100-(distancia_obstaculo/10));
		penalizacion_distancias /= 100;
		coste *= penalizacion_distancias;

		return coste;
	}

	protected override float funcionH (Nodo nodo, Vector3 meta){
		float coste = 0.0f;
		Vector3 distancia;
		int distancia_array_heuristica = 0;
		int x = Mathf.RoundToInt (nodo.vector.x + (ancho / 2));
		int z = Mathf.RoundToInt (nodo.vector.z + (largo / 2));

		distancia_array_heuristica = (array_heuristica_obstaculos [x, z, 0] / 10);
		distancia = nodo.vector_hybrid - meta;

		if (distancia_array_heuristica > distancia.magnitude) {
			coste = distancia_array_heuristica;	
		} else {
			coste = distancia.magnitude;
		}

		//coste = distancia.magnitude;

		return coste;
	}

	protected override bool esMeta (Nodo nodo, Vector3 meta) {
		bool es_meta = false;
		float distancia = Vector3.Distance (nodo.vector_hybrid, meta);

		if (distancia < 2.0f){
			es_meta = true;
		}

		return es_meta;
	}
		
	//Reescribimos esta funcion de la libreria porque no redondea correctamente
	// x.5 puede ser 1 o 0 basado en si x es par o impar
	//Nuestro funcion siempre sera 0.5 = 1.0
	private float round (float numero) {
		float redondeo = 0.0f;
		float floor_float =	Mathf.Floor (numero);
		float resta = numero - floor_float;

		if (resta < 0.5f) {
			redondeo = floor_float;
		} else {
			redondeo = Mathf.Ceil (numero);
		}
			
		return redondeo;
	}


	private int[,,] crearMapaDistancias (){
		int [,,] mapa_distancias = new int[ancho+1, largo+1, 1];

		Queue <ItemMapaDistancias> cola = new Queue <ItemMapaDistancias> ();

		mapa.setRadio (0.5f);

		//int inicio_ancho = (ancho / 2) * (-1);
		//int inicio_largo = (largo / 2) * (-1);

		//rellenamos el mapa antes de empezar
		for (int i = 0; i <= ancho; i++) {
			for (int j = 0; j <= largo; j++) {
				mapa_distancias [i, j, 0] = int.MaxValue;

				if (array_mapa_obstaculos [i, j, 0] == Constantes._OBSTACULO) {
					ItemMapaDistancias celda = new ItemMapaDistancias ();

					mapa_distancias [i, j, 0] = 0;

					celda.x = i - (ancho/2);
					celda.y = 0;
					celda.z = j - (largo/2);
					celda.distancia = 0;

					cola.Enqueue (celda);
				}
			}
		}

		brushFire ( cola, mapa_distancias );			

		return mapa_distancias;
	}

	private int[,,] crearMapaHeuristicaObstaculos (){
		int [,,] mapa_heuristica_obstaculos = new int[ancho+1, largo+1, 1];

		Queue <ItemMapaDistancias> cola = new Queue <ItemMapaDistancias> ();

		//mapa.setRadio (0.5f);

		//int inicio_ancho = (ancho / 2) * (-1);
		//int inicio_largo = (largo / 2) * (-1);

		//rellenamos el mapa antes de empezar
		for (int i = 0; i <= ancho; i++) {
			for (int j = 0; j <= largo; j++) {
				mapa_heuristica_obstaculos [i, j, 0] = int.MaxValue;
			}
		}

		int x_meta = Mathf.RoundToInt (vector_meta.x + (ancho/2));
		int z_meta = Mathf.RoundToInt (vector_meta.z + (largo/2));

		mapa_heuristica_obstaculos [x_meta, z_meta, 0] = 0;

		ItemMapaDistancias celda_meta = new ItemMapaDistancias ();
		celda_meta.x = x_meta - (ancho/2);
		celda_meta.y = 0;
		celda_meta.z = z_meta - (largo/2);
		celda_meta.distancia = 0;

		cola.Enqueue (celda_meta);

		brushFire ( cola, mapa_heuristica_obstaculos );

		return mapa_heuristica_obstaculos;
	}

	private int [,,] brushFire ( Queue <ItemMapaDistancias> cola, int [,,] mapa ) {
		while (cola.Count > 0) {
			
			ItemMapaDistancias actual = cola.Dequeue ();
			ItemMapaDistancias[] siguientes = siguienteCeldas (actual);

			foreach (ItemMapaDistancias item_dis in siguientes) {
				int pos_x = item_dis.x + (ancho / 2);
				int pos_z = item_dis.z + (largo / 2);

				//Esta dentro de nuestro array
				if ( pos_x >= 0 && pos_z >=0 && pos_x <= ancho && pos_z <= largo ) {
					
					if ( array_mapa_obstaculos [pos_x, pos_z, 0] == Constantes._LIBRE ) { //Si es obstaculo no hace falta calcular nada
						if ( mapa [pos_x, pos_z, 0] > item_dis.distancia ) { //Si es mayor es que hemos enconstrado un obstaculo mas cercano
							mapa [pos_x, pos_z, 0] = item_dis.distancia;

							cola.Enqueue (item_dis);
						}
					}	
				}

			}
		}

		return mapa;
	}

	private class ItemMapaDistancias {
		public int x;
		public int y;
		public int z;
		public int distancia;

		public ItemMapaDistancias () {
			x = 0;
			y = 0;
			z = 0;

			distancia = 0;
		}

		public ItemMapaDistancias (int _x, int _y, int _z) {
			x = _x;
			y = _y;
			z = _z;

			distancia = 0;
		}

		public ItemMapaDistancias (int _x, int _y, int _z, int _distancia) {
			x = _x;
			y = _y;
			z = _z;

			distancia = _distancia;
		}
	}

	private ItemMapaDistancias[] siguienteCeldas (ItemMapaDistancias actual){
		ItemMapaDistancias [] siguientes = new ItemMapaDistancias[8] {
			new ItemMapaDistancias (),
			new ItemMapaDistancias (),
			new ItemMapaDistancias (),
			new ItemMapaDistancias (),
			new ItemMapaDistancias (),
			new ItemMapaDistancias (),
			new ItemMapaDistancias (),
			new ItemMapaDistancias ()
		};
		ItemMapaDistancias[] movimientos = new ItemMapaDistancias[8] {
			new ItemMapaDistancias (0, 0, 1, 10),   // adelante
			new ItemMapaDistancias (-1, 0, 1, 14),   // adelante izquierda
			new ItemMapaDistancias (-1, 0, 0, 10),   // izquierda
			new ItemMapaDistancias (-1, 0, -1, 14),  // atras izquierda
			new ItemMapaDistancias (0, 0, -1, 10),  // atras
			new ItemMapaDistancias (1, 0, -1, 14), // atras derecha
			new ItemMapaDistancias (1, 0, 0, 10),  // derecha
			new ItemMapaDistancias (1, 0, 1, 14)   // adelante derecha
		};

		int indice = 0;

		foreach (ItemMapaDistancias item_sig in movimientos) {
			siguientes [indice].x = actual.x + item_sig.x;
			siguientes [indice].y = actual.y + item_sig.y;
			siguientes [indice].z = actual.z + item_sig.z;

			siguientes [indice].distancia = actual.distancia + item_sig.distancia;

			indice++;
		}

		return siguientes;
	}

	public override int [,,] getMapaObstaculos () {
		return array_mapa_obstaculos;
	}

	public override int [,,] getMapaDistancias () {
		return array_mapa_distancias;
	}
}


