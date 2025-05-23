using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkryptStatku : MonoBehaviour
{
    public float przesuniecieX = 0;
    public float przesuniecieZ = 0;
    private float nastepnaRotacjaY = 90f;
    private GameObject kliknietePole;
    int licznikTrafien = 0;
    public int rozmiarStatku;

    private Material[] wszystkieMaterialy;

    List<GameObject> dotknietePola = new List<GameObject>();
    List<Color> wszystkieKolory = new List<Color>();

    private void Start()
    {
        wszystkieMaterialy = GetComponent<Renderer>().materials;
        for (int i = 0; i < wszystkieMaterialy.Length; i++)
            wszystkieKolory.Add(wszystkieMaterialy[i].color);
    }

    private void OnCollisionEnter(Collision kolizja)
    {
        if (kolizja.gameObject.CompareTag("Pole"))
        {
            dotknietePola.Add(kolizja.gameObject);
        }
    }


    public void WyczyscListePol()
    {
        dotknietePola.Clear();
    }

    public Vector3 PobierzPrzesuniecie(Vector3 pozycjaPola)
    {
        return new Vector3(pozycjaPola.x + przesuniecieX, 42, pozycjaPola.z + przesuniecieZ);
    }



    public void ObrocStatek()
    {
        if (kliknietePole == null) return;
        dotknietePola.Clear();
        transform.localEulerAngles += new Vector3(0, nastepnaRotacjaY, 0);
        nastepnaRotacjaY *= -1;
        float tymczasowe = przesuniecieX;
        przesuniecieX = przesuniecieZ;
        przesuniecieZ = tymczasowe;
        UstawPozycje(kliknietePole.transform.position);
    }

    public void UstawPozycje(Vector3 nowaPozycja)
    {
        WyczyscListePol();
        transform.localPosition = new Vector3(nowaPozycja.x + przesuniecieX, 42, nowaPozycja.z + przesuniecieZ);


    }




    public void UstawKliknietePole(GameObject pole)
    {
        kliknietePole = pole;
    }

    public bool NaPlanszy()
    {
        return dotknietePola.Count == rozmiarStatku;
    }

    public bool SprawdzCzyZatopiony()
    {
        licznikTrafien++;
        return rozmiarStatku <= licznikTrafien;
    }

    public void MrugajKolorem(Color tymczasowyKolor)
    {
        foreach (Material mat in wszystkieMaterialy)
        {
            mat.color = tymczasowyKolor;
        }
        Invoke("ResetujKolor", 0.5f);
    }

    private void ResetujKolor()
    {
        int i = 0;
        foreach (Material mat in wszystkieMaterialy)
        {
            mat.color = wszystkieKolory[i++];
        }
    }
}
