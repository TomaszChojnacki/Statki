using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenedzerGry : MonoBehaviour
{
    [Header("Statki")]
    public GameObject[] statki;

    private SkryptStatku skryptStatku;

    private int indeksStatku = 0;

    [Header("HUD")]
    public Button przyciskNastepny;
    public Button przyciskObrotu;

    private bool konfiguracjaZakonczona = false;
    private bool turaGracza = true;

    // Start jest wywo³ywany przed pierwsz¹ klatk¹
    void Start()
    {
        skryptStatku = statki[indeksStatku].GetComponent<SkryptStatku>();
        przyciskNastepny.onClick.AddListener(() => KliknietoNastepnyStatek());
        przyciskObrotu.onClick.AddListener(() => KliknietoObrot());
    }

    private void KliknietoNastepnyStatek()
    {
        if (indeksStatku <= statki.Length - 2)
        {
            indeksStatku++;
            skryptStatku = statki[indeksStatku].GetComponent<SkryptStatku>();
            //skryptStatku.MrugajKolorem(Color.yellow);
        }
    }

    public void KliknietoPole(GameObject pole)
    {
        if (konfiguracjaZakonczona && turaGracza)
        {
            // logika tury gracza
        }
        else if (!konfiguracjaZakonczona)
        {
            UmiescStatek(pole);
            skryptStatku.UstawKliknietePole(pole);
        }
    }

    private void UmiescStatek(GameObject pole)
    {
        skryptStatku = statki[indeksStatku].GetComponent<SkryptStatku>();
        skryptStatku.WyczyscListePol();
        Vector3 nowaPozycja = skryptStatku.PobierzPrzesuniecie(pole.transform.position);
        statki[indeksStatku].transform.localPosition = nowaPozycja;
    }

    void KliknietoObrot()
    {
        skryptStatku.ObrocStatek();
    }
}
