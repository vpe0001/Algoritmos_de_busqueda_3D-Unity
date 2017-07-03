using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class HybridAEstrellaTests {

	//Obtenemos una ruta valida
	[Test]
	public void HybridAEstrellaTestsHayRutaTest() {
		AlgoritmoRuta hybrid = new Hybrid_a_estrella ();
		GameObject coche = GameObject.FindGameObjectWithTag ("Coche");

		Vector3 inicio = new Vector3 (44.0f, 0.0f, 0.0f);
		Vector3 meta = new Vector3 (44.0f, 0.0f, 10.0f);
		float angulo = coche.transform.rotation.eulerAngles.y;
		ObtenerMapa mapa = new ObtenerMapa ();
		Parrilla parrilla = new Parrilla (new GameObject (), new GameObject ());

		Nodo[] trayectoria;
		bool error = false;

		hybrid.iniciarCalcularRuta (inicio, meta, angulo, mapa, parrilla, 0.0f, 100*100, 100, 100, false);

		while ( !hybrid.pasoCalcularRuta (out error) && !error) {
		}

		Assert.IsFalse (error, "1) No ha encontrado una ruta");

		trayectoria = hybrid.getTrayectoriaNodos ();
		float distancia = Vector3.Distance (trayectoria [trayectoria.Length - 1].vector_hybrid, meta);

		Assert.IsTrue ( distancia < 2, "2) No ha llegado a la meta. Es " + trayectoria [trayectoria.Length - 1].vector_hybrid + " y debia ser " + meta);
	}


	//Obtenemos una ruta valida
	[Test]
	public void HybridAEstrellaTestsNoRutaTest() {
		AlgoritmoRuta hybrid = new Hybrid_a_estrella ();
		GameObject coche = GameObject.FindGameObjectWithTag ("Coche");

		Vector3 inicio = new Vector3 (20.0f, 0.0f, 0.0f);
		Vector3 meta = new Vector3 (0.0f, 0.0f, 0.0f);
		float angulo = coche.transform.rotation.eulerAngles.y;
		ObtenerMapa mapa = new ObtenerMapa ();
		Parrilla parrilla = new Parrilla (new GameObject (), new GameObject ());

		bool error = false;

		hybrid.iniciarCalcularRuta (inicio, meta, angulo, mapa, parrilla, 0.0f, 100*100, 100, 100, false);

		while ( !hybrid.pasoCalcularRuta (out error) && !error) {
		}

		Assert.IsTrue (error, "3) No ha devuelto error cuando no hay ruta");
	}


	//Comprobamos mapa de obstaculos
	[Test]
	public void HybridAEstrellaMapaObstaculosTest() {
		AlgoritmoRuta hybrid = new Hybrid_a_estrella ();
		GameObject coche = GameObject.FindGameObjectWithTag ("Coche");

		Vector3 inicio = new Vector3 (20.0f, 0.0f, 0.0f);
		Vector3 meta = new Vector3 (0.0f, 0.0f, 0.0f);
		float angulo = coche.transform.rotation.eulerAngles.y;
		ObtenerMapa mapa = new ObtenerMapa ();
		Parrilla parrilla = new Parrilla (new GameObject (), new GameObject ());

		hybrid.iniciarCalcularRuta (inicio, meta, angulo, mapa, parrilla, 0.0f, 100*100, 100, 100, false);

		int[,,] m_obstaculos = hybrid.getMapaObstaculos ();

		/*
		 * Hay obstaculos en:
		 * 0, 0 ,0 
		 * -31, -0, 25
		 * 27, 0, 30
		 * 
		 * No hay en: 
		 * 0,0, 45
		 * 0,0, -45
		 * 
		 * hay que pasarlos a indices sumandoles la mitad del ancho (50)
		*/

		Assert.IsTrue (m_obstaculos [50, 50, 0] == Constantes._OBSTACULO, "4) No hay obstaculo pero debia haberlo");
		Assert.IsTrue (m_obstaculos[19,75,0] == Constantes._OBSTACULO, "5) No hay obstaculo pero debia haberlo");
		Assert.IsTrue (m_obstaculos[57,80,0] == Constantes._OBSTACULO, "6) No hay obstaculo pero debia haberlo");

		Assert.IsTrue (m_obstaculos[50,95,0] == Constantes._LIBRE, "7) No es espacio libre pero debia serlo");
		Assert.IsTrue (m_obstaculos[50,5,0] == Constantes._LIBRE, "8) No es espacio libre pero debia serlo");

	}


	//Comprobamos mapa de obstaculos
	[Test]
	public void HybridAEstrellaMapaDistancias() {
		AlgoritmoRuta hybrid = new Hybrid_a_estrella ();
		GameObject coche = GameObject.FindGameObjectWithTag ("Coche");

		Vector3 inicio = new Vector3 (20.0f, 0.0f, 0.0f);
		Vector3 meta = new Vector3 (0.0f, 0.0f, 0.0f);
		float angulo = coche.transform.rotation.eulerAngles.y;
		ObtenerMapa mapa = new ObtenerMapa ();
		Parrilla parrilla = new Parrilla (new GameObject (), new GameObject ());

		hybrid.iniciarCalcularRuta (inicio, meta, angulo, mapa, parrilla, 0.0f, 100*100, 100, 100, false);

		int[,,] m_obstaculos = hybrid.getMapaDistancias ();

		/*
		 * Hay obstaculos en:
		 * 0, 0 ,0 
		 * -31, -0, 25
		 * 27, 0, 30
		 * 
		 * No hay en: 
		 * 0,0, 45
		 * 0,0, -45
		 * 
		 * hay que pasarlos a indices sumandoles la mitad del ancho (50)
		*/

		Assert.IsTrue (m_obstaculos [50, 50, 0] == 0, "9) Es obstaculos la distancia debia ser 0, y es " + m_obstaculos [50, 50, 0]);
		Assert.IsTrue (m_obstaculos[19,75,0] == 0, "10) Es obstaculos la distancia debia ser 0, y es " + m_obstaculos [19, 75, 0]);
		Assert.IsTrue (m_obstaculos[57,80,0] == 0, "11) Es obstaculos la distancia debia ser 0, y es " + m_obstaculos [57, 80, 0]);

		Assert.IsTrue (m_obstaculos[50,95,0] == 30, "12) La distancia debia ser 30, y es " + m_obstaculos [50, 95, 0]);
		Assert.IsTrue (m_obstaculos[50,5,0] == 10, "13) La distancia debia ser 10, y es " + m_obstaculos [50, 5, 0]);

	}
		
}
