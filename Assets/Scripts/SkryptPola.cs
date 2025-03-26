using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkryptPola : MonoBehaviour
{
    MenedzerGry menedzerGry;
    Ray promien;
    RaycastHit trafienie;

    private bool trafionyPocisk = false;
    Color32[] kolorTrafienia = new Color32[2];

    void Start()
    {
        menedzerGry = GameObject.Find("MenedzerGry").GetComponent<MenedzerGry>();
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
                    menedzerGry.KliknietoPole(trafienie.collider.gameObject);
                }
            }
        }
    }
}
