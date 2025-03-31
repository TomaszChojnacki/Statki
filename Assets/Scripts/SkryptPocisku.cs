using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkryptPocisku : MonoBehaviour
{
    private MenadzerGry menadzerGry;

    void Start()
    {
        menadzerGry = GameObject.Find("MenadzerGry").GetComponent<MenadzerGry>();
    }

    private void OnCollisionEnter(Collision kolizja)
    {
        menadzerGry.SprawdzTrafienie(kolizja.gameObject);
        Destroy(gameObject);
    }
}
