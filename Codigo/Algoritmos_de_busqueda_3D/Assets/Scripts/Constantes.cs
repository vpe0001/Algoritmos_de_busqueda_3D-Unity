using UnityEngine;
using System.Collections;

public static class Constantes {
	public const float coche_largo = 4.47f;
	public const float coche_ancho = 2.43f;
	public const int _OBSTACULO = 1;
	public const int _LIBRE = 0;
	public const int _OCUPADO = 1;
	public const int _CERRADOS = 1;
	public const int _ABIERTOS = 0;
	public const float coche_max_angulo = 40.0f;
	public const float coche_max_torque = 1000.0f;
	public const float distancia = 1.8f; //la distancia que se mueve el coche en cada nodo
	//constanstes para no tener que calcularlo en cada iteracion
	//El coche siempre avanza lo mismo (para el hybrid *) y siempre gira con las ruedas al maximo
	//public const float radio_giro_rad = 0.3355705f;
	//public const float radio_giro_rad = ((Constantes.distancia / Constantes.coche_largo) * Mathf.Tan (Mathf.Deg2Rad * Constantes.coche_max_angulo));
	//public const float radio_giro = Mathf.Rad2Deg * radio_giro_rad;
	public const float vdt = Constantes.distancia; //Para seguir la nomenclatura de las formulas
	public const int numero_angulos = 3; //izquierda recto derecha
	public const int numero_marchas = 2; //adelante y atras
	public const int hacia_adelante = 1; //marcha adelante
	public const int hacia_atras = -1;   //marcha atras
	public const int parado = 0;
	public const float angulo_atras = 180.0f;
	public const float angulo_atras_rad = (Mathf.Deg2Rad * Constantes.angulo_atras);
	public const float ps_tolerancia = 0.000001f;
	public const float ps_num_puntos_bezier = 10.0f;
}
