using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCan : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator animator;
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("Open", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Open", false);
    }
}
