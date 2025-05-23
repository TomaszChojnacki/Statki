using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenadzerGry : MonoBehaviour
{
    [Header("Statki")]
    public GameObject[] statki;
    public SkryptWroga skryptWroga;
    private SkryptStatku skryptStatku;
    private List<int[]> statkiWroga;
    private int indeksStatku = 0;
    public List<SkryptPola> wszystkieSkryptyPol;

    [Header("HUD")]
    public Button przyciskDalej;
    public Button przyciskObrotu;
    public Button przyciskRestartu;
    public TextMeshProUGUI tekstNaglowka;
    public TextMeshProUGUI tekstStatkowGracza;
    public TextMeshProUGUI tekstStatkowWroga;

    [Header("Obiekty")]
    public GameObject pociskPrefab;
    public GameObject pociskWrogaPrefab;
    public GameObject ogienPrefab;
    public GameObject dokDrewniany;

    private bool konfiguracjaZakonczona = false;
    private bool turaGracza = true;

    private List<GameObject> ognieGracza = new List<GameObject>();
    private List<GameObject> ognieWroga = new List<GameObject>();

    private int liczbaStatkowWroga = 5;
    private int liczbaStatkowGracza = 5;

    void Start()
    {
        skryptStatku = statki[indeksStatku].GetComponent<SkryptStatku>();
        przyciskDalej.onClick.AddListener(() => KliknietoDalej());
        przyciskObrotu.onClick.AddListener(() => KliknietoObrot());
        przyciskRestartu.onClick.AddListener(() => KliknietoRestart());
        statkiWroga = skryptWroga.UmiescStatkiWroga();
    }

    private void KliknietoDalej()
    {
        if (!skryptStatku.NaPlanszy())
        {
            skryptStatku.MrugajKolorem(Color.red);
        }
        else
        {
            if (indeksStatku <= statki.Length - 2)
            {
                indeksStatku++;
                skryptStatku = statki[indeksStatku].GetComponent<SkryptStatku>();
                skryptStatku.MrugajKolorem(Color.yellow);
            }
            else
            {
                przyciskObrotu.gameObject.SetActive(false);
                przyciskDalej.gameObject.SetActive(false);
                dokDrewniany.SetActive(false);
                tekstNaglowka.text = "Zgadnij pole wroga.";
                konfiguracjaZakonczona = true;
                for (int i = 0; i < statki.Length; i++) statki[i].SetActive(false);
            }
        }
    }

    public void KliknietoPole(GameObject pole)
    {
        if (konfiguracjaZakonczona && turaGracza)
        {
            Vector3 pozycjaPola = pole.transform.position;
            pozycjaPola.y += 15;
            turaGracza = false;
            Instantiate(pociskPrefab, pozycjaPola, pociskPrefab.transform.rotation);
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

        Debug.Log("Pozycja ustawianego statku: " + nowaPozycja);
        statki[indeksStatku].transform.localPosition = nowaPozycja;
    }

    void KliknietoObrot()
    {
        skryptStatku.ObrocStatek();
    }

    public void SprawdzTrafienie(GameObject pole)
    {
        int numerPola = Int32.Parse(Regex.Match(pole.name, @"\d+").Value);
        int licznikTrafien = 0;

        foreach (int[] statekWroga in statkiWroga)
        {
            if (statekWroga.Contains(numerPola))
            {
                for (int i = 0; i < statekWroga.Length; i++)
                {
                    if (statekWroga[i] == numerPola)
                    {
                        statekWroga[i] = -5;
                        licznikTrafien++;
                    }
                    else if (statekWroga[i] == -5)
                    {
                        licznikTrafien++;
                    }
                }

                if (licznikTrafien == statekWroga.Length)
                {
                    liczbaStatkowWroga--;
                    tekstNaglowka.text = "Zatopiony";
                    ognieWroga.Add(Instantiate(ogienPrefab, pole.transform.position, Quaternion.identity));
                    pole.GetComponent<SkryptPola>().UstawKolorPola(1, new Color32(68, 0, 0, 255));
                    pole.GetComponent<SkryptPola>().ZmienKolory(1);
                }
                else
                {
                    tekstNaglowka.text = "Trafiony";
                    pole.GetComponent<SkryptPola>().UstawKolorPola(1, new Color32(255, 0, 0, 255));
                    pole.GetComponent<SkryptPola>().ZmienKolory(1);
                }
                break;
            }
        }

        if (licznikTrafien == 0)
        {
            pole.GetComponent<SkryptPola>().UstawKolorPola(1, new Color32(38, 57, 76, 255));
            pole.GetComponent<SkryptPola>().ZmienKolory(1);
            tekstNaglowka.text = "Pudlo";
        }

        Invoke("KoniecTuryGracza", 1.0f);
    }

    public void WrogTrafilGracza(Vector3 pole, int numerPola, GameObject trafionyObiekt)
    {
        skryptWroga.TrafionyPocisk(numerPola);
        Vector3 pozycjaOgnia = pole + new Vector3(0, 8.0f, 0);
        ognieGracza.Add(Instantiate(ogienPrefab, pozycjaOgnia, Quaternion.identity));


        if (trafionyObiekt.GetComponent<SkryptStatku>().SprawdzCzyZatopiony())
        {
            liczbaStatkowGracza--;
            tekstStatkowGracza.text = liczbaStatkowGracza.ToString();
            skryptWroga.ZatopionoGracza();
        }

        Invoke("KoniecTuryWroga", 2.0f);
    }

    private void KoniecTuryGracza()
    {
        foreach (GameObject statek in statki) statek.SetActive(true);
        foreach (GameObject ogien in ognieGracza) ogien.SetActive(true);
        foreach (GameObject ogien in ognieWroga) ogien.SetActive(false);
        tekstStatkowWroga.text = liczbaStatkowWroga.ToString();
        tekstNaglowka.text = "Tura wroga";
        skryptWroga.TuraNPC();
        PokolorujWszystkiePola(0);
        if (liczbaStatkowGracza < 1) KoniecGry("Wrog wygrywa");
    }

    public void KoniecTuryWroga()
    {
        foreach (GameObject statek in statki) statek.SetActive(false);
        foreach (GameObject ogien in ognieGracza) ogien.SetActive(false);
        foreach (GameObject ogien in ognieWroga) ogien.SetActive(true);
        tekstStatkowGracza.text = liczbaStatkowGracza.ToString();
        tekstNaglowka.text = "Wybierz pole";
        turaGracza = true;
        PokolorujWszystkiePola(1);
        if (liczbaStatkowWroga < 1) KoniecGry("Wygrales");
    }

    private void PokolorujWszystkiePola(int indeksKoloru)
    {
        foreach (SkryptPola skryptPola in wszystkieSkryptyPol)
        {
            skryptPola.ZmienKolory(indeksKoloru);
        }
    }

    void KoniecGry(string zwyciezca)
    {
        tekstNaglowka.text = "Koniec gry: " + zwyciezca;
        przyciskRestartu.gameObject.SetActive(true);
        turaGracza = false;
    }

    void KliknietoRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
