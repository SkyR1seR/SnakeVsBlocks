using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private TMP_Text _countText;
    public int foodCount = 1;

    void Start()
    {
        _countText.text = foodCount.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<Score>().AddSize(foodCount);
            SoundScript.soundScript.PlayEat();
            gameObject.SetActive(false);
        }
        
    }
}
