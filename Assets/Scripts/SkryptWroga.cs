using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        int[] numeryPól = Enumerable.Range(1, 100).ToArray();
        bool zajête = true;

        foreach (int[] statek in statkiWroga)
        {
            zajête = true;
            while (zajête)
            {
                zajête = false;
                int dzióbStatku = UnityEngine.Random.Range(0, 99);
                int losowyObrót = UnityEngine.Random.Range(0, 2);
                int krok = losowyObrót == 0 ? 10 : 1;

                for (int i = 0; i < statek.Length; i++)
                {
                    if ((dzióbStatku - (krok * i)) < 0 || numeryPól[dzióbStatku - i * krok] < 0)
                    {
                        zajête = true;
                        break;
                    }
                    else if (krok == 1 && dzióbStatku / 10 != ((dzióbStatku - i * krok) - 1) / 10)
                    {
                        zajête = true;
                        break;
                    }
                }

                if (!zajête)
                {
                    for (int j = 0; j < statek.Length; j++)
                    {
                        statek[j] = numeryPól[dzióbStatku - j * krok];
                        numeryPól[dzióbStatku - j * krok] = -1;
                    }
                }
            }
        }

        foreach (var x in statkiWroga)
        {
            Debug.Log("x: " + x[0]);
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
            int mo¿liwaPropozycja = indeksyTrafien[0] + sasiedniePola[index];
            bool naPlanszy = mo¿liwaPropozycja > -1 && mo¿liwaPropozycja < 100;

            while ((!naPlanszy || siatkaZgadniec[mo¿liwaPropozycja] != 'o') && sasiedniePola.Count > 0)
            {
                sasiedniePola.RemoveAt(index);
                if (sasiedniePola.Count == 0) break;
                index = Random.Range(0, sasiedniePola.Count);
                mo¿liwaPropozycja = indeksyTrafien[0] + sasiedniePola[index];
                naPlanszy = mo¿liwaPropozycja > -1 && mo¿liwaPropozycja < 100;
            }

            zgadniecie = mo¿liwaPropozycja;
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

    public void WstrzymajIKoniec(int pud³o)
    {
        if (obecneTrafienia.Count > 0 && obecneTrafienia[0] > pud³o)
        {
            foreach (int potencjal in potencjalneTrafienia.ToList())
            {
                if (obecneTrafienia[0] > pud³o && potencjal < pud³o)
                {
                    potencjalneTrafienia.Remove(potencjal);
                }
                else if (obecneTrafienia[0] <= pud³o && potencjal > pud³o)
                {
                    potencjalneTrafienia.Remove(potencjal);
                }
            }
        }

        Invoke("KoniecTury", 1.0f);
    }
}
