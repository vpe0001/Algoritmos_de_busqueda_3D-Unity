using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class CerradosTests {

	//Comprobar añadir nodo
	[Test]
	public void addNodoTest() {
		Cerrados cerrados = new Cerrados ();
		Nodo nodo_comprobar;
		bool comprobar;

		Nodo nodo = new Nodo ();
		nodo.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo.coste = 1.0f;

		cerrados.add (nodo);

		comprobar = cerrados.find (nodo, out nodo_comprobar);
		Assert.IsTrue (comprobar && nodo_comprobar.coste == nodo.coste && nodo_comprobar.vector == nodo.vector, "1) No es el mismo nodo que introducimos");
		
	}

	//Comprobar comprobar nodo
	[Test]
	public void comprobarTest() {
		Cerrados cerrados = new Cerrados ();

		Nodo nodo = new Nodo ();
		nodo.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo.coste = 1.0f;

		cerrados.add (nodo);

		bool comprobar = cerrados.comprobar (nodo);
		Assert.IsTrue (comprobar, "2) No esta el nodo que introducimos");

	}

	//Comprobar borrar nodo
	[Test]
	public void deleteTest() {
		Cerrados cerrados = new Cerrados ();
		bool comprobar;

		Nodo nodo = new Nodo ();
		nodo.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo.coste = 1.0f;

		cerrados.add (nodo);
		comprobar = cerrados.comprobar (nodo);
		Assert.IsTrue (comprobar, "3) No esta el nodo que introducimos");

		comprobar = cerrados.delete (nodo);
		Assert.IsTrue (comprobar, "4) Se ha borrado el nodo");

		comprobar = cerrados.comprobar (nodo);
		Assert.IsFalse (comprobar, "5) No se ha borrado");

	}

	//Comprobar encontrar nodo
	[Test]
	public void findNodoTest() {
		Cerrados cerrados = new Cerrados ();
		bool comprobar;
		Nodo comprobar_nodo;

		Nodo nodo = new Nodo ();
		nodo.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo.coste = 1.0f;

		cerrados.add (nodo);

		comprobar = cerrados.find (nodo, out comprobar_nodo);
		Assert.IsTrue (comprobar_nodo.coste == nodo.coste && comprobar_nodo.vector == nodo.vector, "6) No es el mismo nodo que introducimos");
		Assert.IsTrue (comprobar, "7) No se ha encontrado el nodo que introducimos");

		cerrados.delete (nodo);

		comprobar = cerrados.find (nodo, out comprobar_nodo);
		Assert.IsNull (comprobar_nodo, "8) No tenia que haber encontrado el nodo");
		Assert.IsFalse (comprobar, "9) No tenia que haber encontrado el nodo");

	}
		

	//Comprobar numero nodos
	[Test]
	public void countTest() {
		Cerrados cerrados = new Cerrados ();

		Assert.IsTrue (cerrados.count() == 0, "11) No es correcto, es " + cerrados.count() + " y debia ser " + 0);

		Nodo nodo1 = new Nodo ();
		nodo1.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo1.coste = 10.0f;
		Nodo nodo2 = new Nodo ();
		nodo2.vector = new Vector3 (2.0f, 0.0f, 0.0f);
		nodo2.coste = 1.0f;

		cerrados.add (nodo1);
		cerrados.add (nodo2);

		Assert.IsTrue (cerrados.count() == 2, "12) No es correcto, es " + cerrados.count() + " y debia ser " + 2);

		cerrados.delete (nodo2);

		Assert.IsTrue (cerrados.count() == 1, "13) No es correcto, es " + cerrados.count() + " y debia ser " + 1);

		cerrados.delete (nodo1);
		Assert.IsTrue (cerrados.count() == 0, "14) No es correcto, es " + cerrados.count() + " y debia ser " + 0);
	}


	//Comprobar vaciar
	[Test]
	public void getEmptyTest() {
		Cerrados cerrados = new Cerrados ();

		Nodo nodo1 = new Nodo ();
		nodo1.vector = new Vector3 (1.0f, 0.0f, 0.0f);
		nodo1.coste = 10.0f;
		Nodo nodo2 = new Nodo ();
		nodo2.vector = new Vector3 (2.0f, 0.0f, 0.0f);
		nodo2.coste = 5.0f;

		cerrados.add (nodo1);
		cerrados.add (nodo2);

		cerrados.getEmpty ();

		Assert.IsTrue (cerrados.count() == 0, "16) No esta vacio");

	}
		
}
