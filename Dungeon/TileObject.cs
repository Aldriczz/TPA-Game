using UnityEngine;

public class TileObject : MonoBehaviour
{
    private Renderer tileRenderer;
    private Color originalColor;

    void Start()
    {
        tileRenderer = GetComponent<Renderer>();
        originalColor = tileRenderer.material.color;
    }

    void OnMouseEnter()
    {
        HoverManager.Instance.OnTileHovered(this);
    }

    void OnMouseExit()
    {
        HoverManager.Instance.OnTileUnhovered(this);
    }

    public void Highlight()
    {
        tileRenderer.material.color = Color.green;
    }

    public void ResetHighlight()
    {
        tileRenderer.material.color = originalColor;
    }
}