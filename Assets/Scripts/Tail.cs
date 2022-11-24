using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public Transform sneakHead;
    public Transform sneakTail;
    public float circleDiametr;

    [SerializeField]private List<Transform> snakeCircles = new List<Transform>();
    [SerializeField] private List<Vector3> positions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        positions.Add(sneakHead.position);

    }
    private void FixedUpdate()
    {
        float distance = (sneakHead.transform.position - positions[0]).magnitude;
        if (distance > circleDiametr)
        {
            Vector3 dir = (sneakHead.transform.position - positions[0]).normalized;
            positions.Insert(0, positions[0] + dir * circleDiametr);
            positions.RemoveAt(positions.Count - 1);
            distance -= circleDiametr;
        }
        for (int i = 0; i < snakeCircles.Count; i++)
        {
            snakeCircles[i].position = Vector3.Lerp(positions[i + 1], positions[i], distance / circleDiametr);
        }
    }

    

    public void AddBody()
    {
        Transform part = Instantiate(sneakTail, positions[positions.Count - 1], Quaternion.identity, transform);
        part.gameObject.tag = "Tail";
        snakeCircles.Add(part);
        positions.Add(part.position);
    }

    public void RemoveBody()
    {
        Destroy(snakeCircles.LastOrDefault().gameObject);
        snakeCircles.RemoveAt(snakeCircles.Count-1);
        positions.RemoveAt(positions.Count-1);
        
    }
}
