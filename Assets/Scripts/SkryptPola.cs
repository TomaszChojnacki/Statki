using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkryptPola : MonoBehaviour
{
	// Odwolanie do Menadzera Gry
	MenadzerGry menadzerGry;

	// Zmienna do rysowania promienia z kamery
	Ray promien;

	// Info o tym w co trafilismy
	RaycastHit trafienie;

	// Flaga – czy juz byl trafiony
	private bool trafionyPocisk = false;

	// Dwa kolory pola: domyslny i po trafieniu
	Color32[] koloryTrafienia = new Color32[2];

	void Start()
	{
		// Szukamy menadzera gry po nazwie obiektu
		menadzerGry = GameObject.Find("MenadzerGry").GetComponent<MenadzerGry>();

		// Zapamietujemy poczatkowy kolor pola dwa razy dla trafienia i domyslnie
		koloryTrafienia[0] = gameObject.GetComponent<MeshRenderer>().material.color;
		koloryTrafienia[1] = gameObject.GetComponent<MeshRenderer>().material.color;
	}

	void Update()
	{
		// Tworzymy promien z kamery do pozycji myszy
		promien = Camera.main.ScreenPointToRay(Input.mousePosition);

		// Jesli trafiamy jakis collider
		if (Physics.Raycast(promien, out trafienie))
		{
			// Klikniecie mysza na to konkretne pole nazwy git
			if (Input.GetMouseButtonDown(0) && trafienie.collider.gameObject.name == gameObject.name)
			{
				// Jesli jeszcze nie trafiono tego pola
				if (!trafionyPocisk)
				{
					// Mowimy menadzerowi gry ze kliknieto pole to i to
					menadzerGry.KliknietoPole(trafienie.collider.gameObject);
				}
			}
		}
	}

	// Gdy cos wpadnie na to pole
	private void OnCollisionEnter(Collision kolizja)
	{
		// Jezeli to pocisk gracza to oznacz pole jako trafione
		if (kolizja.gameObject.CompareTag("Pocisk"))
		{
			trafionyPocisk = true;
		}
		// Jezeli pocisk wroga – zmien kolor dla efektu trafienia
		else if (kolizja.gameObject.CompareTag("PociskWroga"))
		{
			koloryTrafienia[0] = new Color32(38, 20, 76, 255); // kolor po trafieniu
			GetComponent<Renderer>().material.color = koloryTrafienia[0];
		}
	}

	// Ustawiamy kolor danego indeksu 0 – domyslny, 1 – po trafieniu
	public void UstawKolorPola(int index, Color32 kolor)
	{
		koloryTrafienia[index] = kolor;
	}

	// Faktyczna zmiana koloru pola po trafieniu
	public void ZmienKolory(int indeksKoloru)
	{
		GetComponent<Renderer>().material.color = koloryTrafienia[indeksKoloru];
	}
}
