using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text _sizeText;
    public static Score score;

    public int size = 0;
    public Tail Tail;

    private void Start()
    {
        if (score == null)
        {
            score = this;
        }
        else
        {
            score = null;
        }
    }

    public void RemoveTail()
    {
        for (int i = 0; i < size; i++)
        {
            Tail.RemoveBody();
        }
        size = 0;
        _sizeText.text = $"{size}";
    }

    public void AddSize(int count = 1)
    {
        size += count;
        for (int i = 0; i < count; i++)
        {
            Tail.AddBody();
        }
        _sizeText.text = $"{size}";
    }

    public bool RemoveSize(int count = 1)
    {
        if (size == 0)
        {
            Generator.generator.GameOver();
            return false;
        }
        size -= count;
        for (int i = 0; i < count; i++)
        {
            Tail.RemoveBody();
        }
        _sizeText.text = $"{size}";
        return true;
    }
}
