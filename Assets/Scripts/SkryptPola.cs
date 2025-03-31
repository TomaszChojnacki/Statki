using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkryptPola : MonoBehaviour
{
    MenadzerGry menadzerGry;
    Ray promien;
    RaycastHit trafienie;

    private bool trafionyPocisk = false;
    Color32[] koloryTrafienia = new Color32[2];

    void Start()
    {
        menadzerGry = GameObject.Find("MenadzerGry").GetComponent<MenadzerGry>();
        koloryTrafienia[0] = gameObject.GetComponent<MeshRenderer>().material.color;
        koloryTrafienia[1] = gameObject.GetComponent<MeshRenderer>().material.color;

    }

    void Update()
    {
        promien = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(promien, out trafienie))
        {
            if (Input.GetMouseButtonDown(0) && trafienie.collider.gameObject.name == gameObject.name)
            {
                if (trafionyPocisk == false)
                {
                    menadzerGry.KliknietoPole(trafienie.collider.gameObject);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision kolizja)
    {
        if (kolizja.gameObject.CompareTag("Pocisk"))
        {
            trafionyPocisk = true;
        }
        else if (kolizja.gameObject.CompareTag("PociskWroga"))
        {
            koloryTrafienia[0] = new Color32(38, 20, 76, 255);
            GetComponent<Renderer>().material.color = koloryTrafienia[0];
        }
    }

    public void UstawKolorPola(int index, Color32 kolor)
    {
        koloryTrafienia[index] = kolor;
    }

    public void ZmienKolory(int indeksKoloru)
    {
        GetComponent<Renderer>().material.color = koloryTrafienia[indeksKoloru];
    }
}
