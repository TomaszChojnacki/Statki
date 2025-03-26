using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkryptStatku : MonoBehaviour
{
    public float przesuniecieX = 0;
    public float przesuniecieZ = 0;
    private float nastepnaRotacjaY = 90f;
    private GameObject kliknietePole;
    public int rozmiarStatku;

    private Material[] wszystkieMaterialy;

    List<GameObject> dotknietePola = new List<GameObject>();
    List<Color> wszystkieKolory = new List<Color>();

    public void WyczyscListePol()
    {
        dotknietePola.Clear();
    }

    public Vector3 PobierzPrzesuniecie(Vector3 pozycjaPola)
    {
        return new Vector3(pozycjaPola.x + przesuniecieX, 44, pozycjaPola.z + przesuniecieZ);
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
    }

    public void UstawPozycje(Vector3 nowaPozycja)
    {
        transform.localPosition = new Vector3(nowaPozycja.x + przesuniecieX, 44, nowaPozycja.z + przesuniecieZ);
    }

    public void UstawKliknietePole(GameObject pole)
    {
        kliknietePole = pole;
    }
}
