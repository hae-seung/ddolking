
using System;
using UnityEngine;

public class DrawOutline : MonoBehaviour
{
    [Header("외곽선 설정")]
    private Color color = Color.red;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private int outlineSize = 1;

    private bool isPointerHovering = false;
    private bool isPlayerNear = false;

    private float alpha = 0.2f;
    private float normalAlpha = 1.0f;

    public bool CanInteract => isPointerHovering && isPlayerNear; 

    private void OnMouseEnter()
    {
        isPointerHovering = true;
        UpdateOutline(isPlayerNear); 
    }

    private void OnMouseExit()
    {
        isPointerHovering = false;
        UpdateOutline(false);
    }


    public void SetPlayerNear(bool state)
    {
        isPlayerNear = state;
        
        if (isPlayerNear)
        {
            if(isPointerHovering)
                UpdateOutline(true);
        }
        else
        {
            UpdateOutline(false);
        }
    }

    private void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}