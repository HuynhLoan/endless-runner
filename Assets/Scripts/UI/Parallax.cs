using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cameraTransform;  // camera follow player
    public float parallaxFactor;       // 0-1, càng nhỏ → layer càng xa, di chuyển chậm

    private Vector3 lastCameraPos;

    void Start()
    {
        lastCameraPos = cameraTransform.position;
    }

    void Update()
    {
        Vector3 delta = cameraTransform.position - lastCameraPos;
        transform.position += new Vector3(delta.x * parallaxFactor, 0, 0);
        lastCameraPos = cameraTransform.position;
    }
}
