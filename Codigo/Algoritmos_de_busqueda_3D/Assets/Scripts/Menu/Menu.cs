using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public void cargarEscena (string nombre_escena) {
		SceneManager.LoadScene (nombre_escena);
	}
}
