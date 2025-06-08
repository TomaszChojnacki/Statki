using UnityEngine;

public class pole : MonoBehaviour
{
	void Start()
	{
		// Szukamy wszystkich obiektow na scenie ktore maja tag "Pole"
		GameObject[] pola = GameObject.FindGameObjectsWithTag("Pole");

		// Wypisujemy do konsoli info ze zaraz pokazemy liste wszystkich pol
		Debug.Log(" Lista wszystkich pol i ich pozycji:");

		// Przechodzimy przez wszystkie znalezione pola
		foreach (GameObject pole in pola)
		{
			// Wypisujemy nazwe i pozycjê danego pola
			Debug.Log(pole.name + " | Pozycja: " + pole.transform.position);
		}
	}
}
