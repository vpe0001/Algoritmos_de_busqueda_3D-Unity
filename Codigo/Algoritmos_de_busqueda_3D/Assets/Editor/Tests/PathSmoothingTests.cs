using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class PathSmoothingTests {
	/*
	 *"[0.00000, 0.00000] -> [0.00000, 0.00000]\n",
      "[0.00000, 1.00000] -> [0.02128, 0.97872]\n",
      "[0.00000, 2.00000] -> [0.14894, 1.85106]\n",
      "[1.00000, 2.00000] -> [1.02128, 1.97872]\n",
      "[2.00000, 2.00000] -> [2.00000, 2.00000]\n",
      "[3.00000, 2.00000] -> [2.97872, 2.02128]\n",
      "[4.00000, 2.00000] -> [3.85106, 2.14894]\n",
      "[4.00000, 3.00000] -> [3.97872, 3.02128]\n",
      "[4.00000, 4.00000] -> [4.00000, 4.00000]\n"
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
}
