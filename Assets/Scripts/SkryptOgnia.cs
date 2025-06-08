using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkryptOgnia : MonoBehaviour
{
	public GameObject czerwonyOgien;
	public GameObject zoltyOgien;
	public GameObject pomaranczowyOgien;

	private int licznik = 0;
	private List<Color> koloryBazowe;

	void Start()
	{
		// Bazowe kolory (pocz¹tkowe)
		koloryBazowe = new List<Color> {
			Color.red,
			Color.yellow,
			new Color(1.0f, 0.64f, 0f) // pomarañczowy
        };
	}

	void FixedUpdate()
	{
		licznik++;
		if (licznik > 30)
		{
			// Tworzymy losow¹ kopiê listy
			List<Color> losoweKolory = new List<Color>(koloryBazowe);
			Shuffle(losoweKolory);

			czerwonyOgien.GetComponent<Renderer>().material.color = losoweKolory[0];
			zoltyOgien.GetComponent<Renderer>().material.color = losoweKolory[1];
			pomaranczowyOgien.GetComponent<Renderer>().material.color = losoweKolory[2];

			licznik = 0;
		}
	}

	// Prosta metoda do losowania kolejnoœci listy
	void Shuffle(List<Color> lista)
	{
		for (int i = lista.Count - 1; i > 0; i--)
		{
			int j = Random.Range(0, i + 1);
			Color temp = lista[i];
			lista[i] = lista[j];
			lista[j] = temp;
		}
	}
}