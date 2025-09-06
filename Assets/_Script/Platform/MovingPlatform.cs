using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private int startIndex;
    [SerializeField] private Transform[] points;
    private int targetIndex;



    void Start()
    {
        targetIndex = startIndex;
        transform.position = points[startIndex].position;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[targetIndex].position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, points[targetIndex].position) < 0.05f)
        {
            targetIndex++;
            if (targetIndex == points.Length)
            {
                targetIndex = 0;
            }
        }        
    }
}
