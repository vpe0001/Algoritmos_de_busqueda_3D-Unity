using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class ObtenerMapaTests {

	//Comprobar esRecorrible
	[Test]
	public void esRecorribleTest() {
		ObtenerMapa mapa = new ObtenerMapa ();
		bool comprobar;
		Vector3 posicion;
		
		mapa.setRadio (0.5f);

		posicion = new Vector3 (45.0f, 0.0f, 0.0f);
		comprobar = mapa.esRecorrible (posicion);
		Assert.IsTrue (comprobar, "1) No es recorrible y debia serlo.");

		posicion = new Vector3 (45.0f, 0.0f, 40.0f);
		comprobar = mapa.esRecorrible (posicion);
		Assert.IsTrue (comprobar, "2) No es recorrible y debia serlo.");

		posicion = new Vector3 (-45.0f, 0.0f, -40.0f);
		comprobar = mapa.esRecorrible (posicion);
		Assert.IsTrue (comprobar, "3) No es recorrible y debia serlo.");

		posicion = new Vector3 (0.0f, 0.0f, 0.0f);
		comprobar = mapa.esRecorrible (posicion);
		Assert.IsFalse (comprobar, "4) No es obstaculo y debia serlo.");

		posicion = new Vector3 (-31.0f, 0.0f, -25.0f);
		comprobar = mapa.esRecorrible (posicion);
		Assert.IsFalse (comprobar, "5) No es obstaculo y debia serlo.");

		posicion = new Vector3 (-4.0f, 0.0f, -38.0f);
		comprobar = mapa.esRecorrible (posicion);
		Assert.IsFalse (comprobar, "6) No es obstaculo y debia serlo.");
	}

	//Comprobar lineaVision
	[Test]
	public void lineaVisionTest() {
		ObtenerMapa mapa = new ObtenerMapa ();
		bool comprobar;
		Vector3 posicion1;
		Vector3 posicion2;

		mapa.setRadio (0.5f);

		posicion1 = new Vector3 (45.0f, 0.0f, 0.0f);
		posicion2 = new Vector3 (0.0f, 0.0f, 0.0f);
		comprobar = mapa.lineaVision (posicion1, posicion2);
		Assert.IsFalse (comprobar, "7) No deberia haber linea de vision.");

		posicion1 = new Vector3 (45.0f, 0.0f, 40.0f);
		posicion2 = new Vector3 (-31.0f, 0.0f, -25.0f);
		comprobar = mapa.lineaVision (posicion1, posicion2);
		Assert.IsFalse (comprobar, "8) No deberia haber linea de vision.");

		posicion1 = new Vector3 (-45.0f, 0.0f, -40.0f);
		posicion2 = new Vector3 (-4.0f, 0.0f, -38.0f);
		comprobar = mapa.lineaVision (posicion1, posicion2);
		Assert.IsFalse (comprobar, "9) No deberia haber linea de vision.");

		posicion1 = new Vector3 (-45.0f, 0.0f, 45.0f);
		posicion2 = new Vector3 (45.0f, 0.0f, 45.0f);
		comprobar = mapa.lineaVision (posicion1, posicion2);
		Assert.IsTrue (comprobar, "9) No deberia haber obstaculos en el camino.");

		posicion1 = new Vector3 (45.0f, 0.0f, 0.0f);
		posicion2 = new Vector3 (45.0f, 0.0f, 40.0f);
		comprobar = mapa.lineaVision (posicion1, posicion2);
		Assert.IsTrue (comprobar, "10) No deberia haber obstaculos en el camino.");
	}
}
