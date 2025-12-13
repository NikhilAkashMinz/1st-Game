using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject virtualCamera;
    public Color gizmoColor = new Color(0f, 1f, 0f, 0.35f);

    private PolygonCollider2D poly;

    void Awake()
    {
        poly = GetComponent<PolygonCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
            virtualCamera.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
            virtualCamera.SetActive(false);
    }

    void OnDrawGizmos()
    {
        if (poly == null)
            poly = GetComponent<PolygonCollider2D>();

        if (poly == null)
            return;

        Gizmos.color = gizmoColor;

        for (int i = 0; i < poly.pathCount; i++)
        {
            Vector2[] points = poly.GetPath(i);

            for (int j = 0; j < points.Length; j++)
            {
                Vector3 p1 = transform.TransformPoint(points[j]);
                Vector3 p2 = transform.TransformPoint(points[(j + 1) % points.Length]);
                Gizmos.DrawLine(p1, p2);
            }
        }
    }
}
