using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class PID_controlTests {

	//Comprobamos que devuelve al angulo correcto
	[Test]
	public void anguloGiroTest() {
		GameObject coche = GameObject.FindGameObjectWithTag ("Coche");
		Vector3[] trayectoria = new Vector3[3];
		PID_control pid;
		float angulo;
		float comprobar;

		trayectoria [0] = new Vector3 (45.0f, 0.0f, 0.0f);
		trayectoria [1] = new Vector3 (45.0f, 0.0f, 1.0f);
		trayectoria [2] = new Vector3 (45.0f, 0.0f, 2.0f);

		coche.transform.position = new Vector3 (1.0f, 0.0f, 0.0f);
		Vector3 prueba = new Vector3 (0.0f, 0.0f, 1.0f);
		comprobar = coche.transform.rotation.eulerAngles.y;

		if (comprobar > 180.0001f) {
			comprobar = comprobar - 360;
		}

		pid = new PID_control (coche, trayectoria);

		angulo = pid.anguloGiro (prueba);

		Assert.IsTrue ( Mathf.Approximately ( angulo, comprobar ), "NO son iguales: es " + angulo + " | debia ser: " + comprobar );
	}


	//Comprobamos que devuelve al angulo correcto para las ruedas y la fuerza del motor
	[Test]
	public void pasoPIDTest() {
		GameObject coche = GameObject.FindGameObjectWithTag ("Coche");
		Vector3[] trayectoria = new Vector3[3];
		PID_control pid;
		float [] angulo;
		float comprobar;

		//Vamos a poner el siguiente punto en frente del coche
		trayectoria [0] = new Vector3 (1.0f, 0.0f, 10.0f);
		trayectoria [1] = new Vector3 (1.0f, 0.0f, 11.0f);
		trayectoria [2] = new Vector3 (1.0f, 0.0f, 12.0f);

		coche.transform.position = new Vector3 (1.0f, 0.0f, 0.0f);

		comprobar = coche.transform.rotation.eulerAngles.y;

		if (comprobar > 180.0001f) {
			comprobar = comprobar - 360;
		}

		pid = new PID_control (coche, trayectoria);

		angulo = pid.pasoPID (1.0f, 0.0f, 0.0f);

		Assert.IsTrue ( Mathf.Approximately ( angulo[1], comprobar ), "NO son iguales: es " + angulo[1] + " | debia ser: " + comprobar );

		comprobar = 130.0f; //Es la fuerza del motor que tiene que devolver si este en frente (180º)

		Assert.IsTrue ( Mathf.Approximately ( angulo[0], comprobar ), "NO son iguales: es " + angulo[0] + " | debia ser: " + comprobar );
	}

	//Comprobamos que devuelve al angulo correcto
	[Test]
	public void anguloGiroHybridTest() {
		GameObject coche = GameObject.FindGameObjectWithTag ("Coche");
		Nodo[] trayectoria = new Nodo[3];
		PID_control_hybrid pid;
		float angulo;
		float comprobar;

		trayectoria [0] = new Nodo();
		trayectoria [1] = new Nodo();
		trayectoria [2] = new Nodo();

		trayectoria [0].vector_hybrid = new Vector3 (45.0f, 0.0f, 0.0f);
		trayectoria [1].vector_hybrid = new Vector3 (45.0f, 0.0f, 1.0f);
		trayectoria [2].vector_hybrid = new Vector3 (45.0f, 0.0f, 2.0f);

		coche.transform.position = new Vector3 (1.0f, 0.0f, 0.0f);
		Vector3 prueba = new Vector3 (0.0f, 0.0f, 1.0f);
		comprobar = coche.transform.rotation.eulerAngles.y;

		if (comprobar > 180.0001f) {
			comprobar = comprobar - 360;
		}

		pid = new PID_control_hybrid (coche, trayectoria);

		angulo = pid.anguloGiro (prueba, Constantes.hacia_adelante);

		Assert.IsTrue ( Mathf.Approximately ( angulo, comprobar ), "NO son iguales: es " + angulo + " | debia ser: " + comprobar );
	}


	//Comprobamos que devuelve al angulo correcto para las ruedas y la fuerza del motor
	[Test]
	public void pasoPIDHybridTest() {
		GameObject coche = GameObject.FindGameObjectWithTag ("Coche");
		Nodo[] trayectoria = new Nodo[3];
		PID_control_hybrid pid;
		float [] angulo;
		float comprobar;

		//Vamos a poner el siguiente punto en frente del coche
		trayectoria [0] = new Nodo();
		trayectoria [1] = new Nodo();
		trayectoria [2] = new Nodo();

		trayectoria [0].vector_hybrid = new Vector3 (1.0f, 0.0f, 10.0f);
		trayectoria [1].vector_hybrid = new Vector3 (1.0f, 0.0f, 11.0f);
		trayectoria [2].vector_hybrid = new Vector3 (1.0f, 0.0f, 12.0f);
		trayectoria [0].angulo_hybrid = 180.0f;
		trayectoria [1].angulo_hybrid = 180.0f;
		trayectoria [2].angulo_hybrid = 180.0f;
		trayectoria [0].sentido = Constantes.hacia_adelante;
		trayectoria [1].sentido = Constantes.hacia_adelante;
		trayectoria [2].sentido = Constantes.hacia_adelante;

		coche.transform.position = new Vector3 (1.0f, 0.0f, 0.0f);

		comprobar = coche.transform.rotation.eulerAngles.y;

		if (comprobar > 180.0001f) {
			comprobar = comprobar - 360;
		}

		pid = new PID_control_hybrid (coche, trayectoria);

		angulo = pid.pasoPID (1.0f, 0.0f, 0.0f);

		Assert.IsTrue ( Mathf.Approximately ( angulo[1], comprobar ), "1) NO son iguales: es " + angulo[1] + " | debia ser: " + comprobar );

		comprobar = 50.0f; //Es la fuerza del motor que tiene que devolver si este en frente (180º)

		Assert.IsTrue ( Mathf.Approximately ( angulo[0], comprobar ), "2) NO son iguales: es " + angulo[0] + " | debia ser: " + comprobar );
	}

}
