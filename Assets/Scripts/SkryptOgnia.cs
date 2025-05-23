using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkryptOgnia : MonoBehaviour
{
    public GameObject czerwonyOgien;
    public GameObject zoltyOgien;
    public GameObject pomaranczowyOgien;
    int licznik;

    List<Color> koloryOgnia = new List<Color> { Color.red, Color.yellow, new Color(1.0f, 0.64f, 0) };

    void FixedUpdate()
    {
        if (licznik > 30)
        {
            koloryOgnia.Add(Color.red);
            int losowy = Random.Range(0, koloryOgnia.Count);
            czerwonyOgien.GetComponent<Renderer>().material.SetColor("_Color", koloryOgnia[losowy]);
            koloryOgnia.RemoveAt(losowy);

            losowy = Random.Range(0, koloryOgnia.Count);
            pomaranczowyOgien.GetComponent<Renderer>().material.SetColor("_Color", koloryOgnia[losowy]);
            koloryOgnia.RemoveAt(losowy);

            zoltyOgien.GetComponent<Renderer>().material.SetColor("_Color", koloryOgnia[0]);

            koloryOgnia.Clear();
            koloryOgnia = new List<Color> { Color.red, Color.yellow, new Color(1.0f, 0.64f, 0) };
            licznik = 0;
        }
        licznik++;
    }
}