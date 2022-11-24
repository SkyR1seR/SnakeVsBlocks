using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private bool isPicked = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPicked)
        {
            isPicked = true;
            Generator.generator.FinishLevel();
        }
    }
}
