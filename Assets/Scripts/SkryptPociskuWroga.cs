using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkryptPociskuWroga : MonoBehaviour
{
    MenadzerGry menadzerGry;
    SkryptWroga skryptWroga;
    public Vector3 pozycjaCelu;
    private int celnePole = -1;

    void Start()
    {
        menadzerGry = GameObject.Find("MenadzerGry").GetComponent<MenadzerGry>();
        skryptWroga = GameObject.Find("Wrog").GetComponent<SkryptWroga>();
    }

    private void OnCollisionEnter(Collision kolizja)
    {
        if (kolizja.gameObject.CompareTag("Statek"))
        {
            if (kolizja.gameObject.name == "Statek") pozycjaCelu.y += 0.3f;
            menadzerGry.WrogTrafilGracza(pozycjaCelu, celnePole, kolizja.gameObject);
        }
        else
        {
            skryptWroga.WstrzymajIKoniec(celnePole);
        }
        Destroy(gameObject);
    }

    public void UstawCel(int cel)
    {
        celnePole = cel;
    }
}
