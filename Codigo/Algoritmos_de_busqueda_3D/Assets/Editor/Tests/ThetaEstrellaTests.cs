using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class ThetaEstrellaTests {

	//Obtenemos una ruta valida
	[Test]
	public void ThetaEstrellaTestsHayRutaTest() {
		AlgoritmoRuta theta_star = new Theta_estrella ();
		GameObject coche = GameObject.FindGameObjectWithTag ("Coche");

		Vector3 inicio = new Vector3 (44.0f, 0.0f, 0.0f);
		Vector3 meta = new Vector3 (44.0f, 0.0f, 10.0f);
		float angulo = coche.transform.rotation.eulerAngles.y;
		ObtenerMapa mapa = new ObtenerMapa ();
		Parrilla parrilla = new Parrilla (new GameObject (), new GameObject ());

		Vector3[] trayectoria;
		bool error = false;

		theta_star.iniciarCalcularRuta (inicio, meta, angulo, mapa, parrilla, 1.0f, 100*100, 100, 100, false);

		while ( !theta_star.pasoCalcularRuta (out error) && !error) {
		}

		Assert.IsFalse (error, "1) No ha encontrado una ruta");

		trayectoria = theta_star.getTrayectoria ();

		Assert.IsTrue ( trayectoria [trayectoria.Length - 1] == meta, "2) No ha llegado a la meta. Es " + trayectoria [trayectoria.Length - 1] + " y debia ser " + meta);
	}


	//Obtenemos una ruta valida
	[Test]
	public void ThetaEstrellaTestsNoRutaTest() {
		AlgoritmoRuta theta_star = new Theta_estrella ();
		GameObject coche = GameObject.FindGameObjectWithTag ("Coche");

		Vector3 inicio = new Vector3 (20.0f, 0.0f, 0.0f);
		Vector3 meta = new Vector3 (0.0f, 0.0f, 0.0f);
		float angulo = coche.transform.rotation.eulerAngles.y;
		ObtenerMapa mapa = new ObtenerMapa ();
		Parrilla parrilla = new Parrilla (new GameObject (), new GameObject ());

		bool error = false;

		theta_star.iniciarCalcularRuta (inicio, meta, angulo, mapa, parrilla, 1.0f, 100*100, 100, 100, false);

		while ( !theta_star.pasoCalcularRuta (out error) && !error) {
		}

		Assert.IsTrue (error, "3) No ha devuelto error cuando no hay ruta");
	}
		
}
