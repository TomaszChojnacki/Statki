using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkryptPociskuWroga : MonoBehaviour
{
	// Odwolanie do Menadzera Gry zeby np. poinformowac o trafieniu
	MenadzerGry menadzerGry;

	// Skrypt wroga do zakonczenia tury jesli pudlo
	SkryptWroga skryptWroga;

	// Pozycja w ktora leci pocisk czyli cel
	public Vector3 pozycjaCelu;

	// Numer pola w ktore celuje wrog
	private int celnePole = -1;

	void Start()
	{
		// Znajdujemy menadzera gry po nazwie obiektu
		menadzerGry = GameObject.Find("MenadzerGry").GetComponent<MenadzerGry>();

		// Pobieramy skrypt wroga z obiektu o nazwie "Wrog"
		skryptWroga = GameObject.Find("Wrog").GetComponent<SkryptWroga>();
	}

	// Ta metoda uruchamia sie gdy pocisk w cos uderzy
	private void OnCollisionEnter(Collision kolizja)
	{
		// Sprawdzamy czy trafilismy w statek gracza
		if (kolizja.gameObject.CompareTag("Statek"))
		{
			// Jezeli trafiony obiekt to konkretny "Statek" delikatnie podnosimy pozycje ognia
			if (kolizja.gameObject.name == "Statek")
				pozycjaCelu.y += 0.3f;

			// Informujemy menadzera gry o trafieniu
			menadzerGry.WrogTrafilGracza(pozycjaCelu, celnePole, kolizja.gameObject);
		}
		else
		{
			// Jesli nie trafiono w statek konczymy ture wroga (pudlo)
			skryptWroga.WstrzymajIKoniec(celnePole);
		}

		// Niszczymy pocisk po kolizji zeby nie zostal w scenie
		Destroy(gameObject);
	}

	// Ustawiamy numer pola w ktore wrog ma celowac
	public void UstawCel(int cel)
	{
		celnePole = cel;
	}
}
