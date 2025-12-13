using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float XparallaxValue;
    [SerializeField] private float YparallaxValue;

    private float spriteLength;
    private UnityEngine.Camera cam;
    private Vector3 lastCameraPosition;

    void Start()
    {
        cam = UnityEngine.Camera.main;
        lastCameraPosition = cam.transform.position;
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cam.transform.position - lastCameraPosition;
        transform.position += new Vector3(
            deltaMovement.x * XparallaxValue,
            deltaMovement.y * YparallaxValue
        );
        lastCameraPosition = cam.transform.position;
    }
}
