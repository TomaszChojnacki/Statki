using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackMenu : MonoBehaviour
{
	// Referencja do przycisku (mo�esz te� przypisa� przez Unity Inspector)
	public Button przyciskPowrotu;
	void Start()
	{
		// Dodanie nas�uchu klikni�cia
		przyciskPowrotu.onClick.AddListener(WrocDoSceny0);
	}

	// Funkcja wywo�ywana po klikni�ciu
	void WrocDoSceny0()
	{
		SceneManager.LoadScene(0); // Za�aduj scen� o indeksie 0
	}

}
