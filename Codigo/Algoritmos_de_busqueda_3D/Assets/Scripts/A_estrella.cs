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
		LinkedList <Nodo> abiertos = new LinkedList <Nodo> ();
		LinkedList <Nodo> cerrados = new LinkedList <Nodo> ();
		LinkedList <Nodo> sucesores = new LinkedList <Nodo> ();
		bool meta_encontrada = false;
		//Queue <Nodo> otra = new Queue<Nodo> ();


		rellenarMapaPrueba ();

		Nodo n_actual;
		Nodo n_inicio = new Nodo ();
		n_inicio.vector = inicio;
		n_inicio.padre = null;
		n_inicio.coste_padre = 0;
		n_inicio.coste = 0;

		abiertos.AddLast (n_inicio);

		while (abiertos.Count > 0 && !meta_encontrada) {
			n_actual = abiertos.First.Value;
			abiertos.RemoveFirst ();
			cerrados.AddLast(n_actual);

			if (n_actual.vector.x == meta.x && n_actual.vector.x == meta.x && n_actual.vector.x == meta.x) {
				meta_encontrada = true;
			} else {
				sucesores = CalcularSucesores (n_actual, x_pos, x_neg, z_pos, z_neg);

				foreach (Nodo siguiente in sucesores) {
					if (!abiertos.Contains (siguiente) && !cerrados.Contains (siguiente)) {
						abiertos.AddLast (siguiente);
					} else {
						Nodo anterior;
						Nodo mover_padre;

						if (abiertos.Contains (siguiente)) {
							anterior = abiertos.Find (siguiente).Value;	

							if (anterior.coste > siguiente.coste) {
								anterior.padre = n_actual;
								anterior.coste = siguiente.coste;
								anterior.coste_padre = siguiente.coste_padre;
							}
						} else {
							if (cerrados.Contains (siguiente)) {
								anterior = cerrados.Find (siguiente).Value;	

								if (anterior.coste > siguiente.coste) {
									anterior.padre = n_actual;
									anterior.coste = siguiente.coste;
									anterior.coste_padre = siguiente.coste_padre;

									mover_padre = cerrados.Find (siguiente.padre);
									cerrados.Remove (mover_padre);
									abiertos.AddLast (mover_padre);
								}
							}
						}
					}


				}

				//Reordenar abiertos
			}
		}



		//int tamanio = trayectoria.Count();
		int tamanio = 1;
		Vector3 [] v_trayectoria = new Vector3[tamanio];

		return v_trayectoria;
	}

	private LinkedList <Nodo> CalcularSucesores (Nodo n_actual, float x_pos, float x_neg, float z_pos, float z_neg) {
		LinkedList <Nodo> sucesores = new LinkedList<Nodo> ();

		//cada nodo 8 posibles sucesores
		Nodo[] sucesor = new Nodo[8];
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
			sucesor_valido.padre = n_actual;
			sucesor_valido.coste_padre = n_actual.coste;
			// Falta h y g
			sucesor_valido.coste = sucesor_valido.coste_padre + 1;
		}

		return sucesores;
	}

	// comprueba que los posibles sucesores esten dentro del rango posible
	private LinkedList <Nodo> SucesoresValidos (Nodo[] sucesor, float x_pos, float x_neg, float z_pos, float z_neg){
		LinkedList <Nodo> sucesores = new LinkedList<Nodo> ();

		foreach (Nodo sucesor_valido in sucesor) {
			
			if (sucesor_valido.vector.x < x_neg || sucesor_valido.vector.x > x_pos || sucesor_valido.vector.z < z_neg || sucesor_valido.vector.z > z_neg){
				//sucesor fuera de los limites	
			}else{
				if (!esObstaculo(sucesor_valido)){
					sucesores.AddLast (sucesor_valido);
				}
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

		if (mapa [posicion_mapa [0], posicion_mapa [1]] == 3) {
			obstaculo = true;			
		}

		return obstaculo;
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
		public int coste_padre;
		public int coste;
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
