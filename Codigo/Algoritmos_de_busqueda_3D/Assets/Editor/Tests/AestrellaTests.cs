using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class AestrellaTests {

	//Obtenemos una ruta valida
	[Test]
	public void aEstrellaHayRutaTest() {
		A_estrella astar = new A_estrella ();
		GameObject coche = GameObject.FindGameObjectWithTag ("Coche");

		Vector3 inicio = new Vector3 (44.0f, 0.0f, 0.0f);
		Vector3 meta = new Vector3 (44.0f, 0.0f, 10.0f);
		float angulo = coche.transform.rotation.eulerAngles.y;
		ObtenerMapa mapa = new ObtenerMapa ();
		Parrilla parrilla = new Parrilla (new GameObject (), new GameObject ());

		Vector3[] trayectoria;
		bool error = false;

		astar.iniciarCalcularRuta (inicio, meta, angulo, mapa, parrilla, 1.0f, 100*100, 100, 100, false);

		while ( !astar.pasoCalcularRuta (out error) && !error) {
		}

		Assert.IsFalse (error, "1) No ha encontrado una ruta");

		trayectoria = astar.getTrayectoria ();


		for (int i = 0; i < trayectoria.Length; i++) {
			Vector3 comprobar = new Vector3 (44.0f, 0.0f, 0.0f + i);
			Assert.IsTrue (trayectoria [i] == comprobar, "2) No es correcto. Es " + trayectoria [i] + " y debia ser" + comprobar);
		}


		Assert.IsTrue ( trayectoria [trayectoria.Length - 1] == meta, "3) No ha llegado a la meta. Es " + trayectoria [trayectoria.Length - 1] + " y debia ser " + meta);
	}


	//Obtenemos una ruta valida
	[Test]
	public void aEstrellaNoRutaTest() {
		A_estrella astar = new A_estrella ();
		GameObject coche = GameObject.FindGameObjectWithTag ("Coche");

		Vector3 inicio = new Vector3 (20.0f, 0.0f, 0.0f);
		Vector3 meta = new Vector3 (0.0f, 0.0f, 0.0f);
		float angulo = coche.transform.rotation.eulerAngles.y;
		ObtenerMapa mapa = new ObtenerMapa ();
		Parrilla parrilla = new Parrilla (new GameObject (), new GameObject ());

		bool error = false;

		astar.iniciarCalcularRuta (inicio, meta, angulo, mapa, parrilla, 1.0f, 100*100, 100, 100, false);

		while ( !astar.pasoCalcularRuta (out error) && !error) {
		}

		Assert.IsTrue (error, "4) No ha devuelto error cuando no hay ruta");
	}
		
}
