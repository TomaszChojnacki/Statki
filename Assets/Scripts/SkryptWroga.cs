using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class SkryptWroga : MonoBehaviour
{
	/// Siatka reprezentujaca plansze 10x10 100 pol; 'o' - niezgadniete, 'm' - pudlo, 'h' - trafienie, 'x' - zatopiony
	char[] siatkaZgadniec;

	/// Lista mozliwych pol do trafienia po analizie trafien
	List<int> potencjalneTrafienia;

	/// Lista obecnych trafien trwa proba zatopienia statku
	List<int> obecneTrafienia;

	/// Numer pola ktore wrog zgaduje w tej turze
	private int zgadniecie;

	/// Prefabrykowany pocisk wroga
	public GameObject prefabPociskuWroga;

	/// Referencja do menadzera gry
	public MenadzerGry menadzerGry;

	private void Start()
	{
		/// Inicjalizacja list trafien i ustawienie wszystkich pol jako 'o' otwarte
		potencjalneTrafienia = new List<int>();
		obecneTrafienia = new List<int>();
		siatkaZgadniec = Enumerable.Repeat('o', 100).ToArray();
	}

	/// Metoda umieszczajaca statki wroga losowo na planszy
	public List<int[]> UmiescStatkiWroga()
	{
		/// Lista statkow o roznych dlugosciach
		List<int[]> statkiWroga = new List<int[]>
		{
			new int[]{-1, -1, -1, -1, -1}, /// 5-miejscowy
            new int[]{-1, -1, -1, -1},     /// 4-miejscowy
            new int[]{-1, -1, -1},         /// 3-miejscowy
            new int[]{-1, -1, -1},         /// 3-miejscowy
            new int[]{-1, -1}              /// 2-miejscowy
        };

		/// Lista dostepnych pol od 1 do 100
		int[] numeryPol = Enumerable.Range(1, 100).ToArray();
		bool zajete = true;

		/// Iteracja po wszystkich statkach
		foreach (int[] statek in statkiWroga)
		{
			zajete = true;
			while (zajete)
			{
				zajete = false;
				int dziobStatku = UnityEngine.Random.Range(0, 99); /// losowe pole startowe
				int losowyObrot = UnityEngine.Random.Range(0, 2);  /// losowy kierunek pion poziom
				int krok = losowyObrot == 0 ? 10 : 1; /// 10 = pion 1 = poziom

				/// Sprawdzenie czy da sie ustawic statek
				for (int i = 0; i < statek.Length; i++)
				{
					if ((dziobStatku - (krok * i)) < 0 || numeryPol[dziobStatku - i * krok] < 0)
					{
						zajete = true;
						break;
					}
					else if (krok == 1 && dziobStatku / 10 != ((dziobStatku - i * krok) - 1) / 10)
					{
						/// Sprawdzenie czy nie wychodzi poza rzad tylko dla poziomych statkow
						zajete = true;
						break;
					}
				}

				/// Jesli miejsce jest wolne ustawiamy statek
				if (!zajete)
				{
					for (int j = 0; j < statek.Length; j++)
					{
						statek[j] = numeryPol[dziobStatku - j * krok];
						numeryPol[dziobStatku - j * krok] = -1; /// zaznacz jako zajete
					}
				}
			}
		}

		/// Debugowe wypisanie pozycji statkow A5 B6
		foreach (int[] pole in statkiWroga)
		{
			string temp = "";
			foreach (int numer in pole)
			{
				int indeks = numer - 1;
				int wiersz = indeks / 10;
				int kolumna = indeks % 10 + 1;
				char litera = (char)('A' + wiersz);
				temp += litera.ToString() + kolumna + " ";
			}
			Debug.Log("Statek wroga: " + temp);
		}

		return statkiWroga;
	}

	/// Golwna logika tury NPC
	public void TuraNPC()
	{
		/// Zbieramy indeksy trafien
		List<int> indeksyTrafien = new List<int>();
		for (int i = 0; i < siatkaZgadniec.Length; i++)
		{
			if (siatkaZgadniec[i] == 'h') indeksyTrafien.Add(i);
		}

		/// Jesli sa dwa trafienia obok siebie probujemy kontynuowac linie
		if (indeksyTrafien.Count > 1)
		{
			int roznica = indeksyTrafien[1] - indeksyTrafien[0];
			int znak = Random.Range(0, 2) * 2 - 1;
			int kolejnyIndeks = indeksyTrafien[0] + roznica;

			while (siatkaZgadniec[kolejnyIndeks] != 'o')
			{
				if (siatkaZgadniec[kolejnyIndeks] == 'm' || kolejnyIndeks > 100 || kolejnyIndeks < 0)
					roznica *= -1;

				kolejnyIndeks += roznica;
			}

			zgadniecie = kolejnyIndeks;
		}
		/// Jesli jedno trafienie – zgaduj sasiadujace pole
		else if (indeksyTrafien.Count == 1)
		{
			List<int> sasiedniePola = new List<int> { 1, -1, 10, -10 };
			int index = Random.Range(0, sasiedniePola.Count);
			int mozliwaPropozycja = indeksyTrafien[0] + sasiedniePola[index];
			bool naPlanszy = mozliwaPropozycja > -1 && mozliwaPropozycja < 100;

			while ((!naPlanszy || siatkaZgadniec[mozliwaPropozycja] != 'o') && sasiedniePola.Count > 0)
			{
				sasiedniePola.RemoveAt(index);
				if (sasiedniePola.Count == 0) break;
				index = Random.Range(0, sasiedniePola.Count);
				mozliwaPropozycja = indeksyTrafien[0] + sasiedniePola[index];
				naPlanszy = mozliwaPropozycja > -1 && mozliwaPropozycja < 100;
			}

			zgadniecie = mozliwaPropozycja;
		}
		/// Jesli brak trafien – strzal losowy
		else
		{
			int kolejnyIndeks = Random.Range(0, 100);
			while (siatkaZgadniec[kolejnyIndeks] != 'o') kolejnyIndeks = Random.Range(0, 100);
			kolejnyIndeks = SprawdzJeszczeRaz(kolejnyIndeks);
			zgadniecie = SprawdzJeszczeRaz(kolejnyIndeks);
		}

		/// Odpalenie pocisku
		GameObject pole = GameObject.Find("Pole (" + (zgadniecie + 1) + ")");
		siatkaZgadniec[zgadniecie] = 'm';
		Vector3 pozycja = pole.transform.position;
		pozycja.y += 15;
		GameObject pocisk = Instantiate(prefabPociskuWroga, pozycja, prefabPociskuWroga.transform.rotation);
		pocisk.GetComponent<SkryptPociskuWroga>().UstawCel(zgadniecie);
		pocisk.GetComponent<SkryptPociskuWroga>().pozycjaCelu = pole.transform.position;
	}

	/// Sprawdzenie czy dany indeks jest sensownym zgadywaniem
	private int SprawdzJeszczeRaz(int indeks)
	{
		string str = "nx: " + indeks;
		int noweZgadniecie = indeks;

		bool przypadekBrzegowy = indeks < 10 || indeks > 89 || indeks % 10 == 0 || indeks % 10 == 9;
		bool wPoblizu = false;

		if (indeks + 1 < 100) wPoblizu = siatkaZgadniec[indeks + 1] != 'o';
		if (!wPoblizu && indeks - 1 > 0) wPoblizu = siatkaZgadniec[indeks - 1] != 'o';
		if (!wPoblizu && indeks + 10 < 100) wPoblizu = siatkaZgadniec[indeks + 10] != 'o';
		if (!wPoblizu && indeks - 10 > 0) wPoblizu = siatkaZgadniec[indeks - 10] != 'o';

		if (przypadekBrzegowy || wPoblizu)
			noweZgadniecie = Random.Range(0, 100);

		while (siatkaZgadniec[noweZgadniecie] != 'o')
			noweZgadniecie = Random.Range(0, 100);

		Debug.Log(str + " noweZgadniecie: " + noweZgadniecie + " brzeg:" + przypadekBrzegowy + " blisko:" + wPoblizu);
		return noweZgadniecie;
	}

	/// Jesli wrog trafi – zapisujemy trafienie
	public void TrafionyPocisk(int trafienie)
	{
		siatkaZgadniec[zgadniecie] = 'h';
		Invoke("KoniecTury", 1.0f);
	}

	/// Jesli wrog zatopil statek – zamieniamy 'h' na 'x'
	public void ZatopionoGracza()
	{
		for (int i = 0; i < siatkaZgadniec.Length; i++)
		{
			if (siatkaZgadniec[i] == 'h') siatkaZgadniec[i] = 'x';
		}
	}

	/// Zakonczenie tury wroga
	private void KoniecTury()
	{
		menadzerGry.GetComponent<MenadzerGry>().KoniecTuryWroga();
	}

	/// Jesli wrog spudlowal – wyczysc potencjalne trafienia i zakoncz ture
	public void WstrzymajIKoniec(int pudlo)
	{
		if (obecneTrafienia.Count > 0 && obecneTrafienia[0] > pudlo)
		{
			foreach (int potencjal in potencjalneTrafienia.ToList())
			{
				if (obecneTrafienia[0] > pudlo && potencjal < pudlo)
				{
					potencjalneTrafienia.Remove(potencjal);
				}
				else if (obecneTrafienia[0] <= pudlo && potencjal > pudlo)
				{
					potencjalneTrafienia.Remove(potencjal);
				}
			}
		}

		Invoke("KoniecTury", 1.0f);
	}
}
