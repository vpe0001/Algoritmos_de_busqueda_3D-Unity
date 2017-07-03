using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class PathSmoothingTests {
	/*
	 * Con los valores de muestra
	 *"[0.00000, 0.00000] -> [0.00000000, 0.00000000]\n",
      "[0.00000, 1.00000] -> [0.02941174, 0.97058830]\n",
      "[0.00000, 2.00000] -> [0.17647060, 1.82352900]\n",
      "[1.00000, 2.00000] -> [1.02941200, 1.97058800]\n",
      "[2.00000, 2.00000] -> [2.00000000, 2.00000000]\n",
      "[3.00000, 2.00000] -> [2.97058800, 2.02941200]\n",
      "[4.00000, 2.00000] -> [3.82352900, 2.17647100]\n",
      "[4.00000, 3.00000] -> [3.97058800, 3.02941200]\n",
      "[4.00000, 4.00000] -> [4.00000000, 4.00000000]\n"
	 */
	[Test]
	public void descensoGradienteTest() {
		Vector3[] trayectoria_prueba = new Vector3[9];
		Vector3[] correcto = new Vector3[9];
		Vector3[] resultado_prueba = new Vector3[9];
		//ObtenerMapa mapa = new ObtenerMapa ();

		trayectoria_prueba [0] = new Vector3 (0.0f, 0.0f, 0.0f);
		trayectoria_prueba [1] = new Vector3 (0.0f, 0.0f, 1.0f);
		trayectoria_prueba [2] = new Vector3 (0.0f, 0.0f, 2.0f);
		trayectoria_prueba [3] = new Vector3 (1.0f, 0.0f, 2.0f);
		trayectoria_prueba [4] = new Vector3 (2.0f, 0.0f, 2.0f);
		trayectoria_prueba [5] = new Vector3 (3.0f, 0.0f, 2.0f);
		trayectoria_prueba [6] = new Vector3 (4.0f, 0.0f, 2.0f);
		trayectoria_prueba [7] = new Vector3 (4.0f, 0.0f, 3.0f);
		trayectoria_prueba [8] = new Vector3 (4.0f, 0.0f, 4.0f);

		correcto [0] = new Vector3 (0.0f,     0.0f, 0.0f);
		correcto [1] = new Vector3 (0.02941174f, 0.0f, 0.9705883f);
		correcto [2] = new Vector3 (0.1764706f, 0.0f, 1.823529f);
		correcto [3] = new Vector3 (1.029412f, 0.0f, 1.970588f);
		correcto [4] = new Vector3 (2.0f,     0.0f, 2.0f);
		correcto [5] = new Vector3 (2.970588f, 0.0f, 2.029412f);
		correcto [6] = new Vector3 (3.823529f, 0.0f, 2.176471f);
		correcto [7] = new Vector3 (3.970588f, 0.0f, 3.029412f);
		correcto [8] = new Vector3 (4.0f,     0.0f, 4.0f);

		PathSmoothing pruebaPS = new PathSmoothing (null, trayectoria_prueba, 0.5f, 0.1f);
		pruebaPS.setPesoTrayectoria (0.5f);
		pruebaPS.setPesoSuavizado (0.1f);
		pruebaPS.setTolerancia (0.000001f);

		resultado_prueba = pruebaPS.descensoGradiente (trayectoria_prueba);

		for (int i=0; i < resultado_prueba.Length; i++) {
			Assert.IsTrue(resultado_prueba[i] == correcto[i], "NO son iguales: es " + resultado_prueba[i] + " | debia ser: " + correcto[i]);
		}
	}

	//Comprobamos que elimina los puntos donde hay visibilidad
	[Test]
	public void eliminarZigZagTest () {
		Vector3[] trayectoria_prueba = new Vector3[7];
		Vector3[] resultado_prueba;
		ObtenerMapa mapa = new ObtenerMapa ();


		trayectoria_prueba [0] = new Vector3 (45.0f, 0.0f, 0.0f);
		trayectoria_prueba [1] = new Vector3 (45.0f, 0.0f, 1.0f);
		trayectoria_prueba [2] = new Vector3 (45.0f, 0.0f, 2.0f);
		trayectoria_prueba [3] = new Vector3 (45.0f, 0.0f, 3.0f);
		trayectoria_prueba [4] = new Vector3 (45.0f, 0.0f, 4.0f);
		trayectoria_prueba [5] = new Vector3 (45.0f, 0.0f, 5.0f);
		trayectoria_prueba [6] = new Vector3 (45.0f, 0.0f, 6.0f);

		PathSmoothing pruebaPS = new PathSmoothing (mapa, trayectoria_prueba, 0.5f, 0.1f);

		resultado_prueba = pruebaPS.eliminarZigZag (trayectoria_prueba);

		Assert.IsTrue ( resultado_prueba.Length == 2, "NO son iguales: es " + resultado_prueba.Length + " | debia ser: " + 2 );
	}

	//Comprobamos que devuelve el numero de puntos correcto
	[Test]
	public void curvaBezierTest1() {
		Vector3[] trayectoria_prueba = new Vector3[7];
		Vector3[] resultado_prueba;
		ObtenerMapa mapa = new ObtenerMapa ();
		int comprobar = Mathf.RoundToInt ( (3 * Constantes.ps_num_puntos_bezier) + 4 );


		trayectoria_prueba [0] = new Vector3 (45.0f, 0.0f, 0.0f);
		trayectoria_prueba [1] = new Vector3 (45.0f, 0.0f, 1.0f);
		trayectoria_prueba [2] = new Vector3 (45.0f, 0.0f, 2.0f);
		trayectoria_prueba [3] = new Vector3 (45.0f, 0.0f, 3.0f);
		trayectoria_prueba [4] = new Vector3 (45.0f, 0.0f, 4.0f);
		trayectoria_prueba [5] = new Vector3 (45.0f, 0.0f, 5.0f);
		trayectoria_prueba [6] = new Vector3 (45.0f, 0.0f, 6.0f);

		PathSmoothing pruebaPS = new PathSmoothing (mapa, trayectoria_prueba, 0.5f, 0.1f);

		resultado_prueba = pruebaPS.curvaBezier (trayectoria_prueba);

		Assert.IsTrue ( resultado_prueba.Length == comprobar, "NO son iguales: es " + resultado_prueba.Length + " | debia ser: " + comprobar );
		
	}
		
	// Comprobamos que calcula bien el primer punto bezier
	[Test]
	public void curvaBezierTest2 () {
		Vector3[] trayectoria_prueba = new Vector3[7];
		Vector3[] resultado_prueba;
		ObtenerMapa mapa = new ObtenerMapa ();
		Vector3 comprobar = new Vector3 (44.8f, 0.0f, 0.2f);


		trayectoria_prueba [0] = new Vector3 (45.0f, 0.0f, 0.0f);
		trayectoria_prueba [1] = new Vector3 (44.0f, 0.0f, 1.0f);
		trayectoria_prueba [2] = new Vector3 (43.0f, 0.0f, 2.0f);


		PathSmoothing pruebaPS = new PathSmoothing (mapa, trayectoria_prueba, 0.5f, 0.1f);

		resultado_prueba = pruebaPS.curvaBezier (trayectoria_prueba);

		Assert.IsTrue ( resultado_prueba[1] == comprobar, "NO son iguales: es " + resultado_prueba[1] + " | debia ser: " + comprobar );

	}
}
