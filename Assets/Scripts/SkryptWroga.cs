using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class SkryptWroga : MonoBehaviour
{
    char[] siatkaZgadniec;
    List<int> potencjalneTrafienia;
    List<int> obecneTrafienia;
    private int zgadniecie;
    public GameObject prefabPociskuWroga;
    public MenadzerGry menadzerGry;

    private void Start()
    {
        potencjalneTrafienia = new List<int>();
        obecneTrafienia = new List<int>();
        siatkaZgadniec = Enumerable.Repeat('o', 100).ToArray();
    }

    public List<int[]> UmiescStatkiWroga()
    {
        List<int[]> statkiWroga = new List<int[]>
        {
            new int[]{-1, -1, -1, -1, -1},
            new int[]{-1, -1, -1, -1},
            new int[]{-1, -1, -1},
            new int[]{-1, -1, -1},
            new int[]{-1, -1}
        };

        int[] numeryPol = Enumerable.Range(1, 100).ToArray();
        bool zajete = true;

        foreach (int[] statek in statkiWroga)
        {
            zajete = true;
            while (zajete)
            {
                zajete = false;
                int dziobStatku = UnityEngine.Random.Range(0, 99);
                int losowyObrot = UnityEngine.Random.Range(0, 2);
                int krok = losowyObrot == 0 ? 10 : 1;

                for (int i = 0; i < statek.Length; i++)
                {
                    if ((dziobStatku - (krok * i)) < 0 || numeryPol[dziobStatku - i * krok] < 0)
                    {
                        zajete = true;
                        break;
                    }
                    else if (krok == 1 && dziobStatku / 10 != ((dziobStatku - i * krok) - 1) / 10)
                    {
                        zajete = true;
                        break;
                    }
                }

                if (!zajete)
                {
                    for (int j = 0; j < statek.Length; j++)
                    {
                        statek[j] = numeryPol[dziobStatku - j * krok];
                        numeryPol[dziobStatku - j * krok] = -1;
                    }
                }
            }
        }

        foreach (int[] pole in statkiWroga)
        {
            string temp = "";
            foreach (int numer in pole)
            {
                int indeks = numer - 1; // od 0
                int wiersz = indeks / 10;       // 0–9  A–J
                int kolumna = indeks % 10 + 1;  // 1–10

                char litera = (char)('A' + wiersz);
                temp += litera.ToString() + kolumna + " ";
            }
            Debug.Log("Statek wroga: " + temp);
        }


        return statkiWroga;
    }

    public void TuraNPC()
    {
        List<int> indeksyTrafien = new List<int>();
        for (int i = 0; i < siatkaZgadniec.Length; i++)
        {
            if (siatkaZgadniec[i] == 'h') indeksyTrafien.Add(i);
        }

        if (indeksyTrafien.Count > 1)
        {
            int roznica = indeksyTrafien[1] - indeksyTrafien[0];
            int znak = Random.Range(0, 2) * 2 - 1;
            int kolejnyIndeks = indeksyTrafien[0] + roznica;

            while (siatkaZgadniec[kolejnyIndeks] != 'o')
            {
                if (siatkaZgadniec[kolejnyIndeks] == 'm' || kolejnyIndeks > 100 || kolejnyIndeks < 0)
                {
                    roznica *= -1;
                }
                kolejnyIndeks += roznica;
            }

            zgadniecie = kolejnyIndeks;
        }
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
        else
        {
            int kolejnyIndeks = Random.Range(0, 100);
            while (siatkaZgadniec[kolejnyIndeks] != 'o') kolejnyIndeks = Random.Range(0, 100);
            kolejnyIndeks = SprawdzJeszczeRaz(kolejnyIndeks);
            Debug.Log(" --- ");
            kolejnyIndeks = SprawdzJeszczeRaz(kolejnyIndeks);
            Debug.Log(" -########-- ");
            zgadniecie = kolejnyIndeks;
        }

        GameObject pole = GameObject.Find("Pole (" + (zgadniecie + 1) + ")");
        siatkaZgadniec[zgadniecie] = 'm';
        Vector3 pozycja = pole.transform.position;
        pozycja.y += 15;
        GameObject pocisk = Instantiate(prefabPociskuWroga, pozycja, prefabPociskuWroga.transform.rotation);
        pocisk.GetComponent<SkryptPociskuWroga>().UstawCel(zgadniecie);
        pocisk.GetComponent<SkryptPociskuWroga>().pozycjaCelu = pole.transform.position;
    }

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

        if (przypadekBrzegowy || wPoblizu) noweZgadniecie = Random.Range(0, 100);
        while (siatkaZgadniec[noweZgadniecie] != 'o') noweZgadniecie = Random.Range(0, 100);

        Debug.Log(str + " noweZgadniecie: " + noweZgadniecie + " brzeg:" + przypadekBrzegowy + " blisko:" + wPoblizu);
        return noweZgadniecie;
    }

    public void TrafionyPocisk(int trafienie)
    {
        siatkaZgadniec[zgadniecie] = 'h';
        Invoke("KoniecTury", 1.0f);
    }

    public void ZatopionoGracza()
    {
        for (int i = 0; i < siatkaZgadniec.Length; i++)
        {
            if (siatkaZgadniec[i] == 'h') siatkaZgadniec[i] = 'x';
        }
    }

    private void KoniecTury()
    {
        menadzerGry.GetComponent<MenadzerGry>().KoniecTuryWroga();
    }

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
