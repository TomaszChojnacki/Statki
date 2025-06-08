using UnityEngine;
using UnityEngine.UI;

public class KontrolaGlosnosci : MonoBehaviour
{
	public Slider suwakGlosnosci;       // Slider z UI
	public AudioSource audioSource;     // AudioSource z muzyk�

	void Start()
	{
		// Ustaw domy�ln� warto�� slidera (0�1)
		suwakGlosnosci.value = audioSource.volume;

		// Dodaj nas�uch na zmian� warto�ci slidera
		suwakGlosnosci.onValueChanged.AddListener(UstawGlosnosc);
	}

	void UstawGlosnosc(float wartosc)
	{
		audioSource.volume = wartosc;
	}
}
