using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackMenu : MonoBehaviour
{
	// Referencja do przycisku (mo¿esz te¿ przypisaæ przez Unity Inspector)
	public Button przyciskPowrotu;
	void Start()
	{
		// Dodanie nas³uchu klikniêcia
		przyciskPowrotu.onClick.AddListener(WrocDoSceny0);
	}

	// Funkcja wywo³ywana po klikniêciu
	void WrocDoSceny0()
	{
		SceneManager.LoadScene(0); // Za³aduj scenê o indeksie 0
	}

}
