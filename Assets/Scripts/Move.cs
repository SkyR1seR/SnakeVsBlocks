using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 2f;

    [SerializeField] private Camera _cam;
    [SerializeField] public CharacterController CC;

    private Vector3 startPos;
    bool go = true;

    private void FixedUpdate()
    {
        if (go)
        {
            CC.Move(new Vector3(Input.GetAxis("Horizontal") * speed * 2 * Time.deltaTime, 0, speed * Time.deltaTime));

            _cam.transform.position = new Vector3(_cam.transform.position.x, _cam.transform.position.y, CC.transform.position.z - 2f);
        }
        
    }

    public void GoToStart()
    {
        go = false;
        CC.enabled = false;
        CC.transform.position = startPos;
        CC.enabled = true;
        go = true;
    }

    public void SetStartPos()
    {
        startPos = CC.transform.position;
    }
}
