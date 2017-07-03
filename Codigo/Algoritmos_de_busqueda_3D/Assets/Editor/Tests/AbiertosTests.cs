using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class AbiertosTests {

	//Comprobar añadir nodo
	[Test]
	public void addNodoTest() {
		Abiertos abiertos = new Abiertos (9, 3, 3);

		Nodo nodo = new Nodo ();
		nodo.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo.coste = 1.0f;

		abiertos.add (nodo);

		Nodo comprobar = abiertos.getFirst ();
		Assert.IsTrue (comprobar.coste == nodo.coste && comprobar.vector == nodo.vector, "1) No es el mismo nodo que introducimos");
		
	}

	//Comprobar comprobar nodo
	[Test]
	public void comprobarTest() {
		Abiertos abiertos = new Abiertos (9, 3, 3);

		Nodo nodo = new Nodo ();
		nodo.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo.coste = 1.0f;

		abiertos.add (nodo);

		bool comprobar = abiertos.comprobar (nodo);
		Assert.IsTrue (comprobar, "2) No esta el nodo que introducimos");

	}

	//Comprobar borrar nodo
	[Test]
	public void deleteTest() {
		Abiertos abiertos = new Abiertos (9, 3, 3);
		bool comprobar;

		Nodo nodo = new Nodo ();
		nodo.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo.coste = 1.0f;

		abiertos.add (nodo);
		comprobar = abiertos.comprobar (nodo);
		Assert.IsTrue (comprobar, "3) No esta el nodo que introducimos");

		comprobar = abiertos.delete (nodo);
		Assert.IsTrue (comprobar, "4) Se ha borrado el nodo");

		comprobar = abiertos.comprobar (nodo);
		Assert.IsFalse (comprobar, "5) No se ha borrado");

	}

	//Comprobar encontrar nodo
	[Test]
	public void findNodoTest() {
		Abiertos abiertos = new Abiertos (9, 3, 3);
		bool comprobar;
		Nodo comprobar_nodo;

		Nodo nodo = new Nodo ();
		nodo.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo.coste = 1.0f;

		abiertos.add (nodo);

		comprobar = abiertos.find (nodo, out comprobar_nodo);
		Assert.IsTrue (comprobar_nodo.coste == nodo.coste && comprobar_nodo.vector == nodo.vector, "6) No es el mismo nodo que introducimos");
		Assert.IsTrue (comprobar, "7) No se ha encontrado el nodo que introducimos");

		abiertos.delete (nodo);

		comprobar = abiertos.find (nodo, out comprobar_nodo);
		Assert.IsNull (comprobar_nodo, "8) No tenia que haber encontrado el nodo");
		Assert.IsFalse (comprobar, "9) No tenia que haber encontrado el nodo");

	}

	//Comprobar obtener primer nodo
	[Test]
	public void getFirstNodoTest() {
		Abiertos abiertos = new Abiertos (9, 3, 3);
		Nodo comprobar_nodo;

		Nodo nodo1 = new Nodo ();
		nodo1.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo1.coste = 10.0f;
		Nodo nodo2 = new Nodo ();
		nodo2.vector = new Vector3 (2.0f, 0.0f, 0.0f);
		nodo2.coste = 1.0f;

		abiertos.add (nodo1);
		abiertos.add (nodo2);

		comprobar_nodo = abiertos.getFirst ();
		Assert.IsTrue (comprobar_nodo.coste == nodo2.coste && comprobar_nodo.vector == nodo2.vector, "10) No es el nodo que debia ser el primero");
	}

	//Comprobar numero nodos
	[Test]
	public void countTest() {
		Abiertos abiertos = new Abiertos (9, 3, 3);

		Assert.IsTrue (abiertos.count() == 0, "11) No es correcto, es " + abiertos.count() + " y debia ser " + 0);

		Nodo nodo1 = new Nodo ();
		nodo1.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo1.coste = 10.0f;
		Nodo nodo2 = new Nodo ();
		nodo2.vector = new Vector3 (2.0f, 0.0f, 0.0f);
		nodo2.coste = 1.0f;

		abiertos.add (nodo1);
		abiertos.add (nodo2);

		Assert.IsTrue (abiertos.count() == 2, "12) No es correcto, es " + abiertos.count() + " y debia ser " + 2);

		abiertos.getFirst ();

		Assert.IsTrue (abiertos.count() == 1, "13) No es correcto, es " + abiertos.count() + " y debia ser " + 1);

		abiertos.delete (nodo1);
		Assert.IsTrue (abiertos.count() == 0, "14) No es correcto, es " + abiertos.count() + " y debia ser " + 0);
	}

	//Comprobar actualizar prioridad
	[Test]
	public void updatePrioridadTest() {
		Abiertos abiertos = new Abiertos (9, 3, 3);
		Nodo comprobar_nodo;


		Nodo nodo1 = new Nodo ();
		nodo1.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo1.coste = 10.0f;
		Nodo nodo2 = new Nodo ();
		nodo2.vector = new Vector3 (2.0f, 0.0f, 0.0f);
		nodo2.coste = 5.0f;

		abiertos.add (nodo1);
		abiertos.add (nodo2);

		nodo1.coste = 2.0f;
		abiertos.updatePrioridad (nodo1, 2.0f);

		comprobar_nodo = abiertos.getFirst ();
		Assert.IsTrue (comprobar_nodo.coste == nodo1.coste && comprobar_nodo.vector == nodo1.vector, "15) No es el nodo que debia ser el primero");

	}


	//Comprobar vaciar
	[Test]
	public void getEmptyTest() {
		Abiertos abiertos = new Abiertos (9, 3, 3);

		Nodo nodo1 = new Nodo ();
		nodo1.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo1.coste = 10.0f;
		Nodo nodo2 = new Nodo ();
		nodo2.vector = new Vector3 (2.0f, 0.0f, 0.0f);
		nodo2.coste = 5.0f;

		abiertos.add (nodo1);
		abiertos.add (nodo2);

		abiertos.getEmpty ();

		Assert.IsTrue (abiertos.count() == 0, "16) No esta vacio");

	}
		
}
