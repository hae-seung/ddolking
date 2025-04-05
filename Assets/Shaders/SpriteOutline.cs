using UnityEngine;

public class SpriteOutline : MonoBehaviour
{
    
    private Color color = Color.red;
    private int outlineSize = 1;
    private SpriteRenderer spriteRenderer;

  
    
    void UpdateOutline(bool outline) {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}