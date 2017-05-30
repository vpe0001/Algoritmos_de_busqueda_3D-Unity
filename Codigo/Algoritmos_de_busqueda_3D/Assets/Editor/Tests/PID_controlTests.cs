using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class PID_controlTests {

	[Test]
	public void calcularAnguloEjeZTest() {
		GameObject mock_coche = new GameObject ();
		Vector3[] mock_trayectoria = new Vector3[1];
		Vector3 prueba;
		float respuesta;

		PID_control pid = new PID_control (mock_coche, mock_trayectoria);

		/* ---------------------------------------------------------------------------------------------------------------------- */

		prueba = new Vector3 (0.0f, 0.0f, 1.0f);
		respuesta = pid.calcularAnguloEjeZ (prueba);
		Assert.IsTrue (Mathf.Round (respuesta) ==  0, "1 ERROR: es " + respuesta + " y debia ser 0.0f");

		/* ---------------------------------------------------------------------------------------------------------------------- */

		prueba = new Vector3 (1.0f, 0.0f, 0.0f);
		respuesta = pid.calcularAnguloEjeZ (prueba);
		Assert.IsTrue (Mathf.Round (respuesta) ==  90, "2 ERROR: es " + respuesta + " y debia ser 90.0f");

		/* ---------------------------------------------------------------------------------------------------------------------- */

		prueba = new Vector3 (1.0f, 0.0f, 1.0f);
		respuesta = pid.calcularAnguloEjeZ (prueba);
		Assert.IsTrue (Mathf.Round (respuesta) ==  45, "3 ERROR: es " + respuesta + " y debia ser 45.0f");

		/* ---------------------------------------------------------------------------------------------------------------------- */

		prueba = new Vector3 (1.0f, 0.0f, -1.0f);
		respuesta = pid.calcularAnguloEjeZ (prueba);
		Assert.IsTrue (Mathf.Round (respuesta) ==  135, "4 ERROR: es " + respuesta + " y debia ser 135.0f");

		/* ---------------------------------------------------------------------------------------------------------------------- */

		prueba = new Vector3 (-1.0f, 0.0f, -1.0f);
		respuesta = pid.calcularAnguloEjeZ (prueba);
		Assert.IsTrue (Mathf.Round (respuesta) ==  225, "5 ERROR: es " + respuesta + " y debia ser 225.0f");

		/* ---------------------------------------------------------------------------------------------------------------------- */

		prueba = new Vector3 (-1.0f, 0.0f, 0.0f);
		respuesta = pid.calcularAnguloEjeZ (prueba);
		Assert.IsTrue (Mathf.Round (respuesta) ==  270, "6 ERROR: es " + respuesta + " y debia ser 270.0f");

		/* ---------------------------------------------------------------------------------------------------------------------- */

		prueba = new Vector3 (-1.0f, 0.0f, 1.0f);
		respuesta = pid.calcularAnguloEjeZ (prueba);
		Assert.IsTrue (Mathf.Round (respuesta) ==  315, "7 ERROR: es " + respuesta + " y debia ser 315.0f");

		/* ---------------------------------------------------------------------------------------------------------------------- */

		prueba = new Vector3 (-5.0f, 0.0f, 5.0f);
		respuesta = pid.calcularAnguloEjeZ (prueba);
		Assert.IsTrue (Mathf.Round (respuesta) ==  315, "8 ERROR: es " + respuesta + " y debia ser 315.0f");
	}

	[Test]
	public void anguloRespectoCocheTest() {
		GameObject mock_coche = new GameObject ();
		Vector3[] mock_trayectoria = new Vector3[1];
		float angulo_coche = 0.0f;
		float angulo_trayectoria = 0.0f;
		float angulo_respuesta = 0.0f;

		/* ---------------------------------------------------------------------------------------------------------------------- */

		angulo_coche = 0.0f;
		mock_coche.transform.localRotation = Quaternion.Euler(0.0f, angulo_coche, 0.0f);
		PID_control pid = new PID_control (mock_coche, mock_trayectoria);

		// TEST: coche 0 trayectoria 0
		angulo_trayectoria = 0.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 0.0f), "1 ERROR: es " + angulo_respuesta + " y debia ser 0.0f");
		// TEST: coche 0 trayectoria 45
		angulo_trayectoria = 45.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 45.0f), "2 ERROR: es " + angulo_respuesta + " y debia ser 45.0f");
		// TEST: coche 0 trayectoria 90
		angulo_trayectoria = 90.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 90.0f), "3 ERROR: es " + angulo_respuesta + " y debia ser 90.0f");
		// TEST: coche 0 trayectoria 135
		angulo_trayectoria = 135.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 135.0f), "4 ERROR: es " + angulo_respuesta + " y debia ser 135.0f");
		// TEST: coche 0 trayectoria 180
		angulo_trayectoria = 180.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 180.0f), "5 ERROR: es " + angulo_respuesta + " y debia ser 180.0f");
		// TEST: coche 0 trayectoria 225
		angulo_trayectoria = 225.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -135.0f), "6 ERROR: es " + angulo_respuesta + " y debia ser -135.0f");
		// TEST: coche 0 trayectoria 270
		angulo_trayectoria = 270.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -90.0f), "7 ERROR: es " + angulo_respuesta + " y debia ser -90.0f");
		// TEST: coche 0 trayectoria 315
		angulo_trayectoria = 315.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -45.0f), "8 ERROR: es " + angulo_respuesta + " y debia ser -45.0f");
		// TEST: coche 0 trayectoria 360
		angulo_trayectoria = 360.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 0.0f), "9 ERROR: es " + angulo_respuesta + " y debia ser 0.0f");

		/* ---------------------------------------------------------------------------------------------------------------------- */

		angulo_coche = 45.0f;
		mock_coche.transform.rotation *= Quaternion.Euler(0.0f, angulo_coche, 0.0f);
		pid = new PID_control (mock_coche, mock_trayectoria);

		// TEST: coche 45 trayectoria 0
		angulo_trayectoria = 0.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -45.0f), "10 ERROR: es " + angulo_respuesta + " y debia ser -45.0f");
		// TEST: coche 45 trayectoria 45
		angulo_trayectoria = 45.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 0.0f), "11 ERROR: es " + angulo_respuesta + " y debia ser 0.0f");
		// TEST: coche 45 trayectoria 90
		angulo_trayectoria = 90.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 45.0f), "12 ERROR: es " + angulo_respuesta + " y debia ser 45.0f");
		// TEST: coche 45 trayectoria 135
		angulo_trayectoria = 135.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 90.0f), "13 ERROR: es " + angulo_respuesta + " y debia ser 90.0f");
		// TEST: coche 45 trayectoria 180
		angulo_trayectoria = 180.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 135.0f), "14 ERROR: es " + angulo_respuesta + " y debia ser 135.0f");
		// TEST: coche 45 trayectoria 225
		angulo_trayectoria = 225.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 180.0f), "15 ERROR: es " + angulo_respuesta + " y debia ser 180.0f");
		// TEST: coche 45 trayectoria 270
		angulo_trayectoria = 270.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -135.0f), "16 ERROR: es " + angulo_respuesta + " y debia ser -135.0f");
		// TEST: coche 45 trayectoria 315
		angulo_trayectoria = 315.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -90.0f), "17 ERROR: es " + angulo_respuesta + " y debia ser -90.0f");
		// TEST: coche 45 trayectoria 360
		angulo_trayectoria = 360.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -45.0f), "18 ERROR: es " + angulo_respuesta + " y debia ser -45.0f");

		/* ---------------------------------------------------------------------------------------------------------------------- */

		angulo_coche = 45.0f;
		mock_coche.transform.rotation *= Quaternion.Euler(0.0f, angulo_coche, 0.0f);
		pid = new PID_control (mock_coche, mock_trayectoria);

		// TEST: coche 90 trayectoria 0
		angulo_trayectoria = 0.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -90.0f), "19 ERROR: es " + angulo_respuesta + " y debia ser -90.0f");
		// TEST: coche 90 trayectoria 45
		angulo_trayectoria = 45.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -45.0f), "20 ERROR: es " + angulo_respuesta + " y debia ser -45.0f");
		// TEST: coche 90 trayectoria 90
		angulo_trayectoria = 90.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 0.0f), "21 ERROR: es " + angulo_respuesta + " y debia ser 0.0f");
		// TEST: coche 90 trayectoria 135
		angulo_trayectoria = 135.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 45.0f), "22 ERROR: es " + angulo_respuesta + " y debia ser 45.0f");
		// TEST: coche 90 trayectoria 180
		angulo_trayectoria = 180.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 90.0f), "23 ERROR: es " + angulo_respuesta + " y debia ser 90.0f");
		// TEST: coche 90 trayectoria 225
		angulo_trayectoria = 225.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 135.0f), "24 ERROR: es " + angulo_respuesta + " y debia ser 135.0f");
		// TEST: coche 90 trayectoria 270
		angulo_trayectoria = 270.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 180.0f), "25 ERROR: es " + angulo_respuesta + " y debia ser 180.0f");
		// TEST: coche 90 trayectoria 315
		angulo_trayectoria = 315.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -135.0f), "26 ERROR: es " + angulo_respuesta + " y debia ser -135.0f");
		// TEST: coche 90 trayectoria 360
		angulo_trayectoria = 360.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -90.0f), "27 ERROR: es " + angulo_respuesta + " y debia ser -90.0f");


		/* ---------------------------------------------------------------------------------------------------------------------- */

		angulo_coche = 45.0f;
		mock_coche.transform.rotation *= Quaternion.Euler(0.0f, angulo_coche, 0.0f);
		pid = new PID_control (mock_coche, mock_trayectoria);

		// TEST: coche 135 trayectoria 0
		angulo_trayectoria = 0.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -135.0f), "28 ERROR: es " + angulo_respuesta + " y debia ser -135.0f");
		// TEST: coche 135 trayectoria 45
		angulo_trayectoria = 45.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -90.0f), "29 ERROR: es " + angulo_respuesta + " y debia ser -90.0f");
		// TEST: coche 135 trayectoria 90
		angulo_trayectoria = 90.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -45.0f), "30 ERROR: es " + angulo_respuesta + " y debia ser -45.0f");
		// TEST: coche 135 trayectoria 135
		angulo_trayectoria = 135.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 0.0f), "31 ERROR: es " + angulo_respuesta + " y debia ser 0.0f");
		// TEST: coche 135 trayectoria 180
		angulo_trayectoria = 180.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 45.0f), "32 ERROR: es " + angulo_respuesta + " y debia ser 45.0f");
		// TEST: coche 135 trayectoria 225
		angulo_trayectoria = 225.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 90.0f), "33 ERROR: es " + angulo_respuesta + " y debia ser 90.0f");
		// TEST: coche 135 trayectoria 270
		angulo_trayectoria = 270.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 135.0f), "34 ERROR: es " + angulo_respuesta + " y debia ser 135.0f");
		// TEST: coche 135 trayectoria 315
		angulo_trayectoria = 315.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 180.0f), "35 ERROR: es " + angulo_respuesta + " y debia ser 180.0f");
		// TEST: coche 135 trayectoria 360
		angulo_trayectoria = 360.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -135.0f), "36 ERROR: es " + angulo_respuesta + " y debia ser -135.0f");

		/* ---------------------------------------------------------------------------------------------------------------------- */

		angulo_coche = 45.0f;
		mock_coche.transform.rotation *= Quaternion.Euler(0.0f, angulo_coche, 0.0f);
		pid = new PID_control (mock_coche, mock_trayectoria);

		// TEST: coche 180 trayectoria 0
		angulo_trayectoria = 0.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 180.0f), "37 ERROR: es " + angulo_respuesta + " y debia ser -180.0f");
		// TEST: coche 180 trayectoria 45
		angulo_trayectoria = 45.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -135.0f), "38 ERROR: es " + angulo_respuesta + " y debia ser -135.0f");
		// TEST: coche 180 trayectoria 90
		angulo_trayectoria = 90.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -90.0f), "39 ERROR: es " + angulo_respuesta + " y debia ser -90.0f");
		// TEST: coche 180 trayectoria 135
		angulo_trayectoria = 135.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, -45.0f), "40 ERROR: es " + angulo_respuesta + " y debia ser -45.0f");
		// TEST: coche 180 trayectoria 180
		angulo_trayectoria = 180.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 0.0f), "41 ERROR: es " + angulo_respuesta + " y debia ser 0.0f");
		// TEST: coche 180 trayectoria 225
		angulo_trayectoria = 225.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 45.0f), "42 ERROR: es " + angulo_respuesta + " y debia ser 45.0f");
		// TEST: coche 180 trayectoria 270
		angulo_trayectoria = 270.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 90.0f), "43 ERROR: es " + angulo_respuesta + " y debia ser 90.0f");
		// TEST: coche 180 trayectoria 315
		angulo_trayectoria = 315.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 135.0f), "44 ERROR: es " + angulo_respuesta + " y debia ser 135.0f");
		// TEST: coche 180 trayectoria 360
		angulo_trayectoria = 360.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Approximately(angulo_respuesta, 180.0f), "45 ERROR: es " + angulo_respuesta + " y debia ser 180.0f");


		/* ---------------------------------------------------------------------------------------------------------------------- */

		angulo_coche = 45.0f;
		mock_coche.transform.rotation *= Quaternion.Euler(0.0f, angulo_coche, 0.0f);
		pid = new PID_control (mock_coche, mock_trayectoria);

		// TEST: coche 225 trayectoria 0
		angulo_trayectoria = 0.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  135, "46 ERROR: es " + angulo_respuesta + " y debia ser 135.0f");
		// TEST: coche 225 trayectoria 45
		angulo_trayectoria = 45.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  180, "47 ERROR: es " + angulo_respuesta + " y debia ser -180.0f");
		// TEST: coche 225 trayectoria 90
		angulo_trayectoria = 90.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  -135, "48 ERROR: es " + angulo_respuesta + " y debia ser -135.0f");
		// TEST: coche 225 trayectoria 135
		angulo_trayectoria = 135.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  -90, "49 ERROR: es " + angulo_respuesta + " y debia ser -90.0f");
		// TEST: coche 225 trayectoria 180
		angulo_trayectoria = 180.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) == -45, "50 ERROR: es " + angulo_respuesta + " y debia ser -45.0f");
		// TEST: coche 225 trayectoria 225
		angulo_trayectoria = 225.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  0, "51 ERROR: es " + angulo_respuesta + " y debia ser 0.0f");
		// TEST: coche 225 trayectoria 270
		angulo_trayectoria = 270.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  45, "52 ERROR: es " + angulo_respuesta + " y debia ser 45.0f");
		// TEST: coche 225 trayectoria 315
		angulo_trayectoria = 315.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  90, "53 ERROR: es " + angulo_respuesta + " y debia ser 90.0f");
		// TEST: coche 225 trayectoria 360
		angulo_trayectoria = 360.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  135, "54 ERROR: es " + angulo_respuesta + " y debia ser 135.0f");


		/* ---------------------------------------------------------------------------------------------------------------------- */

		angulo_coche = 45.0f;
		mock_coche.transform.rotation *= Quaternion.Euler(0.0f, angulo_coche, 0.0f);
		pid = new PID_control (mock_coche, mock_trayectoria);

		// TEST: coche 270 trayectoria 0
		angulo_trayectoria = 0.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  90, "55 ERROR: es " + angulo_respuesta + " y debia ser 90.0f");
		// TEST: coche 270 trayectoria 45
		angulo_trayectoria = 45.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  135, "56 ERROR: es " + angulo_respuesta + " y debia ser 135.0f");
		// TEST: coche 270 trayectoria 90
		angulo_trayectoria = 90.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  180, "57 ERROR: es " + angulo_respuesta + " y debia ser 180.0f");
		// TEST: coche 270 trayectoria 135
		angulo_trayectoria = 135.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  -135, "58 ERROR: es " + angulo_respuesta + " y debia ser -135.0f");
		// TEST: coche 270 trayectoria 180
		angulo_trayectoria = 180.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) == -90, "59 ERROR: es " + angulo_respuesta + " y debia ser -90.0f");
		// TEST: coche 270 trayectoria 225
		angulo_trayectoria = 225.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  -45, "60 ERROR: es " + angulo_respuesta + " y debia ser -45.0f");
		// TEST: coche 270 trayectoria 270
		angulo_trayectoria = 270.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  0, "61 ERROR: es " + angulo_respuesta + " y debia ser 0.0f");
		// TEST: coche 270 trayectoria 315
		angulo_trayectoria = 315.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  45, "62 ERROR: es " + angulo_respuesta + " y debia ser 45.0f");
		// TEST: coche 270 trayectoria 360
		angulo_trayectoria = 360.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  90, "63 ERROR: es " + angulo_respuesta + " y debia ser 90.0f");


		/* ---------------------------------------------------------------------------------------------------------------------- */

		angulo_coche = 45.0f;
		mock_coche.transform.rotation *= Quaternion.Euler(0.0f, angulo_coche, 0.0f);
		pid = new PID_control (mock_coche, mock_trayectoria);

		// TEST: coche 315 trayectoria 0
		angulo_trayectoria = 0.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  45, "64 ERROR: es " + angulo_respuesta + " y debia ser 45.0f");
		// TEST: coche 315 trayectoria 45
		angulo_trayectoria = 45.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  90, "65 ERROR: es " + angulo_respuesta + " y debia ser 90.0f");
		// TEST: coche 315 trayectoria 90
		angulo_trayectoria = 90.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  135, "66 ERROR: es " + angulo_respuesta + " y debia ser 135.0f");
		// TEST: coche 315 trayectoria 135
		angulo_trayectoria = 135.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  180, "67 ERROR: es " + angulo_respuesta + " y debia ser 180.0f");
		// TEST: coche 315 trayectoria 180
		angulo_trayectoria = 180.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) == -135, "68 ERROR: es " + angulo_respuesta + " y debia ser -135.0f");
		// TEST: coche 315 trayectoria 225
		angulo_trayectoria = 225.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  -90, "69 ERROR: es " + angulo_respuesta + " y debia ser -90.0f");
		// TEST: coche 315 trayectoria 270
		angulo_trayectoria = 270.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  -45, "70 ERROR: es " + angulo_respuesta + " y debia ser -45.0f");
		// TEST: coche 315 trayectoria 315
		angulo_trayectoria = 315.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  0, "71 ERROR: es " + angulo_respuesta + " y debia ser 0.0f");
		// TEST: coche 315 trayectoria 360
		angulo_trayectoria = 360.0f;
		angulo_respuesta = pid.anguloRespectoCoche (angulo_trayectoria);
		Assert.IsTrue (Mathf.Round (angulo_respuesta) ==  45, "72 ERROR: es " + angulo_respuesta + " y debia ser 45.0f");
	}
}
