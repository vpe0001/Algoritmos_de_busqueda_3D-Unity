using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

public class A_estrella : ControladorCoche {
	
	protected Cerrados cerrados;
	protected List <Nodo> sucesores;
	protected Abiertos abiertos;
	protected Vector3[] v_trayectoria;
	protected Vector3 vector_inicio;
	protected Vector3 vector_meta;
	protected ObtenerMapa mapa;
	protected Parrilla parrilla;
	protected Nodo nodo_final;
	protected Nodo nodo_actual;
	protected Nodo nodo_inicio;
	protected bool meta_encontrada;
	protected float peso;
	protected HashSet <Vector3> vertices;

	public override void iniciarCalcularRuta(Vector3 v_inicio, Vector3 v_meta, ObtenerMapa v_mapa, Parrilla v_parrilla, float p_peso, int tam_parrilla) {
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
	public override Vector3[] CalcularRuta (Vector3 inicio, Vector3 meta, ObtenerMapa mapa, Parrilla parrilla, float p_peso, int tam_parrilla) {
		bool error;
		peso = p_peso;
		
		iniciarCalcularRuta(inicio, meta, mapa, parrilla, peso, tam_parrilla);

		while (!pasoCalcularRuta (out error)) {
		}
			
		return v_trayectoria;
	}

	protected virtual List <Nodo> CalcularSucesores (Nodo n_actual, Vector3 meta, ObtenerMapa mapa) {
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
			
		Nodo[] sucesor = rellenarSucesores(num_sucesores, n_actual, movimientos);

		sucesores = SucesoresValidos (sucesor, mapa);

		foreach (Nodo sucesor_valido in sucesores) {
			sucesor_valido.costeG = funcionG (sucesor_valido) + sucesor_valido.padre.costeG;
			sucesor_valido.costeH = funcionH (sucesor_valido, meta);
			sucesor_valido.coste = (peso * sucesor_valido.costeG) + sucesor_valido.costeH;
		}
			
		return sucesores;
	}

	protected virtual Nodo [] rellenarSucesores (int num_sucesores, Nodo n_actual, Vector3[] movimientos){
		Nodo[] sucesor = new Nodo[num_sucesores];

		for (int i=0; i < num_sucesores; i++){
			sucesor [i] = new Nodo ();
			sucesor [i].vector = n_actual.vector + movimientos[i];
			sucesor [i].padre = n_actual;
		}

		return sucesor;
	}

	// comprueba que los posibles sucesores esten dentro del rango posible
	protected List <Nodo> SucesoresValidos (Nodo[] sucesor, ObtenerMapa mapa){
		List <Nodo> sucesores = new List<Nodo> ();

		foreach (Nodo sucesor_valido in sucesor) {
			//Comprobamos las dos direcciones porque hemos encontrado que no siempre, aunque deberia, da el mismo resultado
			//Asi evitamos bugs debido a que sea visible/alcanzable desde una direccion pero no desde la otra
			if ( mapa.lineaVision (sucesor_valido.padre.vector, sucesor_valido.vector) && mapa.lineaVision (sucesor_valido.vector, sucesor_valido.padre.vector)){
			//if ( mapa.lineaVision (sucesor_valido.padre.vector, sucesor_valido.vector)){
				sucesores.Add (sucesor_valido);
			}
		}
			
		return sucesores;
	}


	protected bool esMeta(Nodo nodo, Vector3 meta) {
		bool es_meta = false;

		if (nodo.vector == meta){
			es_meta = true;
		}
			
		return es_meta;
	}

	protected float funcionG(Nodo nodo){
		float coste = 0;
		Vector3 distancia;

		if (nodo.padre != null) { 
			distancia = nodo.vector - nodo.padre.vector;
		}else {
			distancia = nodo.vector - nodo_inicio.vector;
		}

		coste = distancia.magnitude;

		return coste;
	}

	protected float funcionH(Nodo nodo, Vector3 meta){
		float coste = 0;

		Vector3 distancia;
		distancia = nodo.vector - meta;
		coste = distancia.magnitude;

		return coste;
	}

	protected Vector3[] vectoresCamino (Nodo nodo_final){
		int size = 0;
		int i = 0;
		int j = 0;
		Nodo meta = nodo_final;


		while (meta != null){
			size++;
			meta = meta.padre;
		}

		Vector3 [] camino = new Vector3[size];
		Vector3 [] camino_orden = new Vector3[size];
		meta = nodo_final;
		while (meta != null){
			camino [i] = meta.vector;
			i++;
			meta = meta.padre;
		}

		// Ahora la trayectoria va de la meta al inicaio, la vamos a dar la vuelta
		//  para que nos facilite el trabajo a posteriori
		i = size -1;
		j = 0;
		while (i>=0) {
			camino_orden [i] = camino [j];
			i--;
			j++;
		}
			
		return camino_orden;
	}

}
