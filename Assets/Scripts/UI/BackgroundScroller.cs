using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    private Renderer quadRenderer;

    void Start()
    {
        quadRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Calculate the new offset based on time and speed
        Vector2 textureOffset = new Vector2(Time.time * scrollSpeed, 0); 

        // Apply the offset to the material's main texture
        quadRenderer.material.mainTextureOffset = textureOffset;
    }
}
