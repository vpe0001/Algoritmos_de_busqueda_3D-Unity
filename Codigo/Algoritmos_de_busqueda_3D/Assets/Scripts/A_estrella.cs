using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class A_estrella : ControladorCoche {
	// Mapa de prueba: Escala 10x10 Que equivale a 100X100 metros
	// Cada unidad de coordenadas es un metro
	// El centro del plano/mapa es la coordenada 0,0,0
	// En este caso solo usaremos la x y la z debido a que no tenemos alturas
	// En el inicio el coche este orientado hacia las z positivas, es decir, hacia adelante
	// es moverse en el eje z en sentido positivo, y hacia atras en el sentido negativo
	// El coche en el inicio esta colocado en la coordenada 0,0,0, aunque hay que tener en 
	// cuenta que eso es el centro del vehiculo. El coche mide 2,43x4,47
	// Si tenemos precision de 1 metro, el morro del vehiculo esta en la coordenada 0,0,3
	// Esto es porque si 0 es el centro del vehuculo, la mitad 2,23 esta por delante de la
	// 0, redondeando queda 3
	// Para el ejemplo primero vale esta aproximacion
	// Se ha colocado un obstaculo en la posicion 0,0,20 Es decir delante del vehiculo
	// Mide 3 de ancho asi que se choca con el en 0,0,18.5 o 0,0,18 en la aproximacion
	// Mide 10 de largo asi que va desde la -5,0,20 hasta la 5,0,20
	// Al final se ha dibujado el mapa para mas claridad
	private int [,] mapa = new int[11,11];

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

	// A*
	public override Vector3[] CalcularRuta (Vector3 inicio, Vector3 meta, float x_pos, float x_neg, float z_pos, float z_neg) {
		Cerrados cerrados = new Cerrados ();
		List <Nodo> sucesores = new List <Nodo> ();
		bool meta_encontrada = false;
		Abiertos abiertos = new Abiertos ();

		rellenarMapaPrueba ();

		Nodo nodo_final = null;
		Nodo n_actual = null;
		Nodo n_inicio = new Nodo ();
		n_inicio.vector = inicio;
		n_inicio.padre = null;
		n_inicio.coste = 0;

		abiertos.add (n_inicio);

		while (abiertos.count() > 0 && !meta_encontrada) {
			n_actual = abiertos.getFirst ();
			cerrados.add (n_actual);

			if (esMeta (n_actual, meta)) {
				meta_encontrada = true;
				nodo_final = n_actual;
				Debug.Log ("Es meta" + n_actual.vector + "Padre:" + n_actual.padre.vector);
			} else {
				sucesores = CalcularSucesores (n_actual, meta, x_pos, x_neg, z_pos, z_neg);
				Debug.Log ("Sucesores: " + sucesores.Count);

				foreach (Nodo siguiente in sucesores) {
					if (!abiertos.comprobar (siguiente) && !cerrados.comprobar (siguiente)) {
						abiertos.add (siguiente);
						Debug.Log ("No es ni en abiertos ni en cerrados");
					} else {
						Nodo anterior;
						Nodo mover_padre;

						if (abiertos.comprobar (siguiente)) {
							Debug.Log ("Esta en abiertos");
							anterior = abiertos.find (siguiente);	

							if (anterior.coste > siguiente.coste) {
								Debug.Log ("Anterior1 coste = " + anterior.coste);
								anterior.padre = n_actual;
								anterior.coste = siguiente.coste;
								anterior = abiertos.find (siguiente);
								Debug.Log ("siguiente coste = " + siguiente.coste);
								Debug.Log ("Anterior2 coste = " + anterior.coste);
							}
						} else {
							if (cerrados.comprobar (siguiente)) {
								Debug.Log ("Esta en cerrados");
								anterior = cerrados.find (siguiente);	

								if (anterior.coste > siguiente.coste) {
									anterior.padre = n_actual;
									anterior.coste = siguiente.coste;


									mover_padre = cerrados.find (siguiente.padre);
									cerrados.delete (mover_padre);
									abiertos.add (mover_padre);
								}
							}
						}
					}


				}


			}
		}


		Vector3 [] v_trayectoria = vectoresCamino (nodo_final);

		return v_trayectoria;
	}

	private List <Nodo> CalcularSucesores (Nodo n_actual, Vector3 meta, float x_pos, float x_neg, float z_pos, float z_neg) {
		List <Nodo> sucesores = new List <Nodo> ();
		int num_sucesores = 8; //las 8 direcciones posibles

		//cada nodo 8 posibles sucesores
		Nodo[] sucesor = new Nodo[num_sucesores];

		for (int i=0; i < num_sucesores; i++){
			sucesor [i] = new Nodo ();
		}

		sucesor[0].vector.x = n_actual.vector.x - 1;
		sucesor[0].vector.y = n_actual.vector.y;
		sucesor[0].vector.z = n_actual.vector.z - 1;

		sucesor[1].vector.x = n_actual.vector.x - 1;
		sucesor[1].vector.y = n_actual.vector.y;
		sucesor[1].vector.z = n_actual.vector.z;

		sucesor[2].vector.x = n_actual.vector.x - 1;
		sucesor[2].vector.y = n_actual.vector.y;
		sucesor[2].vector.z = n_actual.vector.z + 1;

		sucesor[3].vector.x = n_actual.vector.x;
		sucesor[3].vector.y = n_actual.vector.y;
		sucesor[3].vector.z = n_actual.vector.z - 1;

		sucesor[4].vector.x = n_actual.vector.x;
		sucesor[4].vector.y = n_actual.vector.y;
		sucesor[4].vector.z = n_actual.vector.z + 1;

		sucesor[5].vector.x = n_actual.vector.x + 1;
		sucesor[5].vector.y = n_actual.vector.y;
		sucesor[5].vector.z = n_actual.vector.z - 1;

		sucesor[6].vector.x = n_actual.vector.x + 1;
		sucesor[6].vector.y = n_actual.vector.y;
		sucesor[6].vector.z = n_actual.vector.z;

		sucesor[7].vector.x = n_actual.vector.x + 1;
		sucesor[7].vector.y = n_actual.vector.y;
		sucesor[7].vector.z = n_actual.vector.z + 1;

		sucesores = SucesoresValidos (sucesor, x_pos, x_neg, z_pos, z_neg);

		foreach (Nodo sucesor_valido in sucesores) {
			float coste = 0.0f;
			sucesor_valido.padre = n_actual;

			coste = funcionG (sucesor_valido) + funcionH (sucesor_valido, meta);
			sucesor_valido.coste = coste + sucesor_valido.padre.coste;
		}

		return sucesores;
	}

	// comprueba que los posibles sucesores esten dentro del rango posible
	private List <Nodo> SucesoresValidos (Nodo[] sucesor, float x_pos, float x_neg, float z_pos, float z_neg){
		List <Nodo> sucesores = new List<Nodo> ();

		foreach (Nodo sucesor_valido in sucesor) {
			
			if (sucesor_valido.vector.x < x_neg || sucesor_valido.vector.x > x_pos || sucesor_valido.vector.z < z_neg || sucesor_valido.vector.z > z_pos){
				//sucesor fuera de los limites

				//Debug.Log ("Fuera de los limites");
				//Debug.Log (sucesor_valido.vector.x + "<" + x_neg);
				//Debug.Log (sucesor_valido.vector.x + ">" + x_pos);
				//Debug.Log (sucesor_valido.vector.z + "<" + z_neg);
				//Debug.Log (sucesor_valido.vector.z + ">" + z_pos);
			}else{
				//Debug.Log ("¿Obstaculo?");
				if (!esObstaculo(sucesor_valido)){
					//Debug.Log ("Sucesor valido" + sucesor_valido.vector);
					sucesores.Add (sucesor_valido);
				}

				//Tambien se puede comprobar que la distancia del vector a un obstaculo sea al menos
				//la mitad del ancho del coche
			}
		}

		return sucesores;
	}

	// Comprueba que no sea una posicion valida y no un obstaculo
	private bool esObstaculo(Nodo sucesor) {
		int[] posicion_mapa = new int[2];
		bool obstaculo = false;

		// El mapa esta en coordenadas de eje x y eje z, ademas al ser un vector las posiciones no son negativas
		// y las coordenadas de los vectores pueden serlo, asi que hacemos una conversion para poder compararlo
		posicion_mapa [0] = (int) (sucesor.vector.z - 5) * (-1);
		posicion_mapa [1] = (int) (sucesor.vector.x + 5);

		//Debug.Log ("Sucesor: " + sucesor.vector);
		//Debug.Log ("Posicion mapa:" + posicion_mapa [0] + " | " +posicion_mapa [1]);

		if (mapa [posicion_mapa [0], posicion_mapa [1]] == 3) {
			obstaculo = true;
			Debug.Log ("OBSTACULO");
		}

		return obstaculo;
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
			Debug.Log ("Meta: " + meta.vector);
			camino [i] = meta.vector;
			i++;
			meta = meta.padre;
		}

		return camino;
	}

	// Rellena el mapa de prueba
	private void rellenarMapaPrueba () {
		// 0 = vacio
		// 1 = salida
		// 2 = meta
		// 3 = Obstaculo
		for (int i=0; i<11; i++){
			for (int j=0; j<11; j++){
				mapa [i, j] = 0;
			}	
		}

		mapa [1 , 5] = 2;
		mapa [5 , 5] = 1;
		mapa [3 , 4] = 3;
		mapa [3 , 5] = 3;
		mapa [3 , 6] = 3;
	}

	private class Nodo {
		public Vector3 vector;
		public Nodo padre;
		public float coste;

		public Nodo (){}

		public Nodo (Vector3 _vector, Nodo _padre, int _coste){
			vector = _vector;
			padre = _padre;
			coste = _coste;
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
				Debug.Log ("MEta: " + meta.vector);
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

/*
    -5 -4 -3 -2 -1  0  1  2  3  4  5 X                                                                  
  5                                                                     
  4                 X
  3                                                                    
  2              O  O  O                                                    
  1                                                                       
  0                 C                                     
 -1                                                                      
 -2                                                                     
 -3                                                                    
 -4                                                                      
 -5
  Z
*/

/*
Nodo nodo_prueba_padre = new Nodo ();
Nodo nodo_prueba = new Nodo ();
Nodo nodo_prueba2 = new Nodo ();
Nodo nodo_prueba3 = new Nodo ();

nodo_prueba_padre.coste = 0;
nodo_prueba_padre.padre = null;
nodo_prueba_padre.vector = new Vector3 (0.0f, 1.0f, 2.0f);

nodo_prueba.coste = 10;
nodo_prueba.padre = nodo_prueba_padre;
nodo_prueba.vector = new Vector3 (1.0f, 2.0f, 3.0f);

nodo_prueba2.coste = 12;
nodo_prueba2.padre = nodo_prueba;
nodo_prueba2.vector = new Vector3 (2.0f, 3.0f, 4.0f);

nodo_prueba3.coste = 3;
nodo_prueba3.padre = null;
nodo_prueba3.vector = new Vector3 (2.0f, 3.0f, 4.0f);

abiertos.add (nodo_prueba);
abiertos.add (nodo_prueba_padre);
abiertos.add (nodo_prueba2);

foreach (Nodo _nodo in abiertos) {
	Debug.Log (_nodo.vector);
}

if (abiertos.comprobar (nodo_prueba2)) {
	Debug.Log ("Comprobar1 true");
} else {
	Debug.Log ("Comprobar1 false");
}

if (abiertos.comprobar (nodo_prueba3)) {
	Debug.Log ("Comprobar2 true");
} else {
	Debug.Log ("Comprobar2 false");
}

Debug.Log ("Count: " + abiertos.count());

if (abiertos.delete (nodo_prueba3)) {
	Debug.Log ("Delete true");
} else {
	Debug.Log ("Delete false");
}

Debug.Log ("Count: " + abiertos.count());

Nodo primero;
primero = abiertos.getFirst ();
Debug.Log (primero.vector);
Debug.Log ("Count: " + abiertos.count());
primero = abiertos.getFirst ();
Debug.Log (primero.vector);
Debug.Log ("Count: " + abiertos.count());
*/