using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hybrid_a_estrella  : A_estrella {
	protected int ancho;
	protected int largo;
	protected List<NodoObstaculo> mapa_obstaculos;
	protected int[, , ] array_mapa_obstaculos;



	private float tiempo_inicio;
	private float tiempo_final;

	public override void iniciarCalcularRuta(Vector3 v_inicio, Vector3 v_meta, float v_angulo_coche, ObtenerMapa v_mapa, Parrilla v_parrilla, float p_peso, int tam_parrilla, int v_ancho, int v_largo) {
		Vector3[] array_vertices;

		cerrados = new Cerrados ();
		sucesores = new List <Nodo> ();
		abiertos = new Abiertos (tam_parrilla);

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
		ancho = v_ancho;
		largo = v_largo;


		mapa.setRadio (0.5f); //

		//mapa_obstaculos = new SortedDictionary<Vector3, int>(new ComparadorVectores());
		//mapa_obstaculos = new SortedList<Vector3, int>(new ComparadorVectores ());
		mapa_obstaculos = new List<NodoObstaculo>();
		array_mapa_obstaculos = new int[ancho+1, largo+1, 1];

		tiempo_inicio = Time.realtimeSinceStartup;
		crearMapaObstaculos ();
		tiempo_final = Time.realtimeSinceStartup;

		Debug.Log ("Mapa de obstaculos creado en: " + (tiempo_final - tiempo_inicio) + " segs.");

		/*
		foreach (NodoObstaculo n in mapa_obstaculos) {
			parrilla.crearCasilla (n.vector, n.obstaculo);
		}
		*/


		//int inicio_ancho = (ancho / 2) * (-1);
		//int inicio_largo = (largo / 2) * (-1);

		/*
		for (int i = inicio_ancho; i <= (ancho / 2); i++) {
			for (int j = inicio_largo; j <= (largo / 2); j++) {
				Vector3 n = new Vector3 (i, 0.0f, j);
				parrilla.crearCasilla (n , array_mapa_obstaculos[i+(ancho/2), j+(largo/2), 0]);

				
				//if (array_mapa_obstaculos [i + (ancho / 2), j + (largo / 2), 0] == Constantes._ABIERTOS) {
				//	parrilla.crearCasilla (n, Constantes._CERRADOS);
				//} else {
				//	parrilla.crearCasilla (n, Constantes._ABIERTOS);
				//}

			}
		}
		*/


		tiempo_inicio = Time.realtimeSinceStartup;
		crearMapaVoronoi ();
		tiempo_final = Time.realtimeSinceStartup;

		Debug.Log ("Mapa de Voronoi creado en: " + (tiempo_final - tiempo_inicio) + " segs.");

		//Prueba de generar un nuevo nodo del Hybrid
		//La casilla sera la misma que con el A*, la diferencia es que dentro del nodo de esa casilla guardamos la ruta
		// continua. El vector discreto sirve para identificar la casilla
		//Nodo n_padre = new Nodo();
		//n_padre.vector = new Vector3 (1.0f, 0.0f, 1.0f);
		//n_padre.vector_hybrid = n_padre.vector;
		//n_padre.padre = null;
		//n_padre.sentido = 1;
		//n_padre.angulo_hybrid = 0.0f;

		//sucesores = CalcularSucesores (n_padre, vector_meta, mapa);
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
					//parrilla.crearCasilla (vector, 1);
				} else {
					ocupado = Constantes._OBSTACULO;
					//parrilla.crearCasilla (vector, 0);
				}

				array_mapa_obstaculos [i+(ancho/2), j+(largo/2), 0] = ocupado;
				mapa_obstaculos.Add ( new NodoObstaculo(vector, ocupado));
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

		float angulo_coche = n_actual.angulo_hybrid;
		float angulo_coche_rad = (Mathf.Deg2Rad * angulo_coche);

		float x = 0.0f;
		float z = 0.0f;
		float y = 0.0f;

		int indice_sucesor = 0;

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
					angulo_calculos = (Constantes.radio_giro_rad * direccion_giro) + angulo_coche_rad;

					sucesor.sentido = Constantes.hacia_adelante;
					
				} else { //hacia atras
					angulo_calculos = (Constantes.radio_giro_rad * direccion_giro) + angulo_coche_rad + Constantes.angulo_atras_rad;

					sucesor.sentido = Constantes.hacia_atras;
				}

				//Nuestro eje de referencia es Z
				z = z + Constantes.vdt * Mathf.Cos (angulo_calculos);
				x = x + Constantes.vdt * Mathf.Sin (angulo_calculos);
				//Y sera la misma, si cambiase la altura seria y + altura de la posicion nueva, del suelo en el nuevo z,x

				sucesor.vector_hybrid = new Vector3 (x, y, z);
				sucesor.angulo_hybrid = (Constantes.radio_giro * direccion_giro) + n_actual.angulo_hybrid;

				if (sucesor.angulo_hybrid < 0.0f) {
					sucesor.angulo_hybrid += 360.0f;
				}

				sucesores_array[indice_sucesor] = sucesor;
				indice_sucesor++;

				direccion_giro++;
			}
		}
			
		//Calcular casilla

		foreach (Nodo sucesor in sucesores_array){
			/*
			float x_casilla = Mathf.Round (sucesor.vector_hybrid.x);
			float y_casilla = Mathf.Round (sucesor.vector_hybrid.y);
			float z_casilla = Mathf.Round (sucesor.vector_hybrid.z);

			Vector3 nuevo_casilla = new Vector3 (x_casilla, y_casilla, z_casilla);
			*/
			//sucesor.vector = nuevo_casilla;
			sucesor.vector = sucesor.vector_hybrid;
		}




		//Sucesores validos
		sucesores = SucesoresValidos(sucesores_array, mapa);

		//Calcular coste
		foreach (Nodo sucesor_valido in sucesores) {
			sucesor_valido.costeG = funcionG (sucesor_valido) + sucesor_valido.padre.costeG;
			sucesor_valido.costeH = funcionH (sucesor_valido, meta);
			sucesor_valido.coste = (peso * sucesor_valido.costeG) + sucesor_valido.costeH;
		}

		return sucesores;
	}

	protected override float funcionG (Nodo nodo){
		float coste = 0.0f;
		Vector3 distancia;

		if (nodo.padre != null) { 
			distancia = nodo.vector_hybrid - nodo.padre.vector_hybrid;
			coste += distancia.magnitude;

			// Si va en linea recta es menos costoso que girar
			if (Mathf.Approximately (nodo.padre.angulo_hybrid, nodo.angulo_hybrid)) {
				coste *= 1.0f;
			} else {
				coste *= 1.4f;
			}
		}else {
			distancia = nodo.vector_hybrid - nodo_inicio.vector;

			coste += distancia.magnitude;
		}

		//IS hacia atras es mas costoso que hacia adelante
		if (nodo.sentido == Constantes.hacia_atras) {
			coste *= 1.8f;
		} else {
			coste *= 1.0f;
		}

		return coste;
	}

	protected override  float funcionH (Nodo nodo, Vector3 meta){
		float coste = 0;

		Vector3 distancia;
		distancia = nodo.vector - meta;
		coste = distancia.magnitude;

		return coste;
	}


	protected override bool esMeta (Nodo nodo, Vector3 meta) {
		bool es_meta = false;
		float distancia = Vector3.Distance (nodo.vector, meta);

		if (distancia < 1.0f){
			es_meta = true;
		}

		return es_meta;
	}



	private void crearMapaVoronoi (){
		
	}
}

