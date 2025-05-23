using UnityEngine;

public class pole : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] pola = GameObject.FindGameObjectsWithTag("Pole");

        Debug.Log(" Lista wszystkich pól i ich pozycji:");
        foreach (GameObject pole in pola)
        {
            Debug.Log(pole.name + " | Pozycja: " + pole.transform.position);
        }
    }


}
