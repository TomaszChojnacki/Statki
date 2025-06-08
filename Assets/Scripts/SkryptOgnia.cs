using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkryptOgnia : MonoBehaviour
{
	// Obiekty ognia ktore bedziemy kolorowac
	public GameObject czerwonyOgien;
	public GameObject zoltyOgien;
	public GameObject pomaranczowyOgien;

	private int licznik = 0; // licznik do odmierzania czasu
	private List<Color> koloryBazowe; // lista z trzema kolorami ognia

	void Start()
	{
		// Ustawiamy liste z podstawowymi kolorami ognia
		koloryBazowe = new List<Color> {
			Color.red,                         // czerwony
            Color.yellow,                      // zolty
            new Color(1.0f, 0.64f, 0f)         // pomaranczowy
        };
	}

	void FixedUpdate()
	{
		// Odliczamy czas co klatke
		licznik++;

		// Co 30 klatek zmieniamy kolory ognia
		if (licznik > 30)
		{
			// Robimy kopie oryginalnej listy kolorow
			List<Color> losoweKolory = new List<Color>(koloryBazowe);

			// Tasujemy kolejnosc kolorow
			Shuffle(losoweKolory);

			// Ustawiamy nowe kolory dla ognia
			czerwonyOgien.GetComponent<Renderer>().material.color = losoweKolory[0];
			zoltyOgien.GetComponent<Renderer>().material.color = losoweKolory[1];
			pomaranczowyOgien.GetComponent<Renderer>().material.color = losoweKolory[2];

			// Resetujemy licznik zeby zaczac od nowa
			licznik = 0;
		}
	}

	// algorytm listy shuffle
	void Shuffle(List<Color> lista)
	{
		for (int i = lista.Count - 1; i > 0; i--)
		{
			int j = Random.Range(0, i + 1); // losowy indeks
			Color temp = lista[i];          // zamieniamy miejscami i-j
			lista[i] = lista[j];
			lista[j] = temp;
		}
	}
}
