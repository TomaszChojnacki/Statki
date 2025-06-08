using UnityEngine;
using UnityEngine.UI;

public class KontrolaGlosnosci : MonoBehaviour
{
	public Slider suwakGlosnosci;       // Slider z UI
	public AudioSource audioSource;     // AudioSource z muzyk¹

	void Start()
	{
		// Ustaw domyœln¹ wartoœæ slidera (0–1)
		suwakGlosnosci.value = audioSource.volume;

		// Dodaj nas³uch na zmianê wartoœci slidera
		suwakGlosnosci.onValueChanged.AddListener(UstawGlosnosc);
	}

	void UstawGlosnosc(float wartosc)
	{
		audioSource.volume = wartosc;
	}
}
