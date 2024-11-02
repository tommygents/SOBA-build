using UnityEngine;

public class SpriteLineMaterial : MonoBehaviour
{
    public Sprite spriteTexture;
    private LineRenderer lineRenderer;
    
    void Start()
    {
        // Get the Line Renderer component
        lineRenderer = GetComponent<LineRenderer>();
        
        if (spriteTexture == null || lineRenderer == null)
        {
            Debug.LogError("Please assign a sprite and ensure there's a LineRenderer component!");
            return;
        }
        
        // Create a new material using the default sprite shader
        Material material = new Material(Shader.Find("Sprites/Default"));
        
        // Set the sprite texture as the main texture of the material
        material.mainTexture = spriteTexture.texture;
        
        // Configure the material properties
        material.SetColor("_Color", Color.white);
        material.EnableKeyword("_ALPHABLEND_ON");
        material.renderQueue = 3000; // Transparent queue
        
        // Assign the material to the line renderer
        lineRenderer.material = material;
        
        // Optional: Configure basic line renderer properties
        lineRenderer.textureMode = LineTextureMode.Tile; // or LineTextureMode.Tile
        lineRenderer.alignment = LineAlignment.View; // Makes the line face the camera
    }
}