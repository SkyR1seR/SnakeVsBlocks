using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Material _material;
    private Material _materialBuf;

    [SerializeField] private GameObject _cube;

    public int maxHP;
    public int HP;
    private int _hp 
    {
        get
        {
            return HP;
        }
        set 
        {
            HP = value;
            hpText.text = HP.ToString();
            _materialBuf.color = new Color(HP / 51f, 1f - HP / 51f, 0);
            if (HP==0)
            {
                gameObject.SetActive(false);
            }

        }
    }

    public void ResetCube()
    {
        _hp = maxHP;
    }

    private void Start()
    {
        _materialBuf = new Material(_material);
        _cube.GetComponent<MeshRenderer>().material = _materialBuf;
        _materialBuf.color = new Color(HP / 51f, 1f-HP/ 51f, 0);
        hpText.text = HP.ToString();
        maxHP = HP;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponentInParent<Score>().RemoveSize(1))
            {
                _hp--;
                SoundScript.soundScript.PlayBreak();
            }
			else
			{
				maxHP = Random.Range(1,maxHP);
			}
        }
    }
}
