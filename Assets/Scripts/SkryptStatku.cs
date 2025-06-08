using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkryptStatku : MonoBehaviour
{
	// Przesuniecia statku wzgledem kliknietego pola ustawienie pozycji
	public float przesuniecieX = 0;
	public float przesuniecieZ = 0;

	// Kierunek kolejnego obrotu przod tyl
	private float nastepnaRotacjaY = 90f;

	// Referencja do klikniêtego pola na ktorym gracz chce postawic statek
	private GameObject kliknietePole;

	// Licznik trafien w ten statek
	int licznikTrafien = 0;

	// Ile "pól" zajmuje statek
	public int rozmiarStatku;

	// Wszystkie materialy dla mrugania kolorami
	private Material[] wszystkieMaterialy;

	// Lista pol na ktorych statek aktualnie stoi wykrytych przez kolizje
	List<GameObject> dotknietePola = new List<GameObject>();

	// Lista oryginalnych kolorow materialow do przywracania po mruganiu
	List<Color> wszystkieKolory = new List<Color>();

	private void Start()
	{
		// Pobieramy wszystkie materialy statku i zapisujemy ich kolory
		wszystkieMaterialy = GetComponent<Renderer>().materials;
		for (int i = 0; i < wszystkieMaterialy.Length; i++)
			wszystkieKolory.Add(wszystkieMaterialy[i].color);
	}

	private void OnCollisionEnter(Collision kolizja)
	{
		// Jesli statek dotknie pola dodajemy je do listy aktualnych pol
		if (kolizja.gameObject.CompareTag("Pole"))
		{
			dotknietePola.Add(kolizja.gameObject);
		}
	}

	// Czysci liste dotknietych pol przy obrocie lub zmianie pozycji
	public void WyczyscListePol()
	{
		dotknietePola.Clear();
	}

	// Zwraca nowz pozycje na podstawie przesuniecia wzgledem kliknietego pola
	public Vector3 PobierzPrzesuniecie(Vector3 pozycjaPola)
	{
		return new Vector3(pozycjaPola.x + przesuniecieX, 42, pozycjaPola.z + przesuniecieZ);
	}

	// Obraca statek zmienia rotacjê i aktualizuje przesuniecia osi
	public void ObrocStatek()
	{
		if (kliknietePole == null) return; // nie obracamy jesli nic nie kliknieto

		dotknietePola.Clear(); // resetujemy kolizje
		transform.localEulerAngles += new Vector3(0, nastepnaRotacjaY, 0); // obrot
		nastepnaRotacjaY *= -1; // zmieniamy kierunek na nastepny raz

		// Zamieniamy przesuniecia osi bo po obrocie sa odwrotne
		float tymczasowe = przesuniecieX;
		przesuniecieX = przesuniecieZ;
		przesuniecieZ = tymczasowe;

		// Przestawiamy statek na nowa pozycjê z nowym przesunieciem
		UstawPozycje(kliknietePole.transform.position);
	}

	// Ustawia statek na konkretnej pozycji
	public void UstawPozycje(Vector3 nowaPozycja)
	{
		WyczyscListePol(); // resetujemy kolizje
		transform.localPosition = new Vector3(nowaPozycja.x + przesuniecieX, 42, nowaPozycja.z + przesuniecieZ);
	}

	// Ustawia pole na ktore gracz kliknak jako punkt odniesienia
	public void UstawKliknietePole(GameObject pole)
	{
		kliknietePole = pole;
	}

	// Sprawdza czy statek jest w pelni na planszy dotyka tylu pol ile powinien
	public bool NaPlanszy()
	{
		return dotknietePola.Count == rozmiarStatku;
	}

	// Zwieksza licznik trafien i sprawdza czy caly statek zostal zatopiony
	public bool SprawdzCzyZatopiony()
	{
		licznikTrafien++;
		return rozmiarStatku <= licznikTrafien;
	}

	// Sprawia ze statek mruga danym kolorem przez chwile sygnal bledu
	public void MrugajKolorem(Color tymczasowyKolor)
	{
		foreach (Material mat in wszystkieMaterialy)
		{
			mat.color = tymczasowyKolor;
		}
		Invoke("ResetujKolor", 0.5f); // po pol sekundy przywracamy kolor
	}

	// Przywraca oryginalne kolory materialow
	private void ResetujKolor()
	{
		int i = 0;
		foreach (Material mat in wszystkieMaterialy)
		{
			mat.color = wszystkieKolory[i++];
		}
	}
}
