using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkryptPocisku : MonoBehaviour
{
	// Odniesienie do glownego menadzera gry zeby mogl sprawdzic trafienie
	private MenadzerGry menadzerGry;

	void Start()
	{
		// Szukamy obiektu o nazwie "MenadzerGry" i bierzemy z niego skrypt
		menadzerGry = GameObject.Find("MenadzerGry").GetComponent<MenadzerGry>();
	}

	// Ta funkcja odpala sie kiedy pocisk w cos trafi
	private void OnCollisionEnter(Collision kolizja)
	{
		// Przekazujemy obiekt ktory zostal trafiony, do menadzera gry
		menadzerGry.SprawdzTrafienie(kolizja.gameObject);

		// Po trafieniu niszczymy pocisk zeby nie zostawal w scenie
		Destroy(gameObject);
	}
}
