using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        // hủy khi ra khỏi màn hình bên trái
        float camLeftX = Camera.main.ViewportToWorldPoint(
            new Vector3(0f, 0f, 0f)
        ).x;

        if (transform.position.x < camLeftX - 2f)
        {
            Destroy(gameObject);
        }
    }
}
