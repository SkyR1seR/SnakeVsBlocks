using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;

    [SerializeField] private AudioClip _eatSound;
    [SerializeField] private AudioClip _gameoverSound;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _breakSound;

    public static SoundScript soundScript;

    // Start is called before the first frame update
    void Start()
    {
        if (soundScript == null)
        {
            soundScript = this;
        }
        else
        {
            soundScript = null;
        }
    }

    public void PlayBreak()
    {
        _audio.clip = _breakSound;
        _audio.Play();
    }

    public void PlayEat()
    {
        _audio.clip = _eatSound;
        _audio.Play();
    }

    public void PlayWin()
    {
        _audio.clip = _winSound;
        _audio.Play();
    }

    public void PlayLose()
    {
        _audio.clip = _gameoverSound;
        _audio.Play();
    }
}
