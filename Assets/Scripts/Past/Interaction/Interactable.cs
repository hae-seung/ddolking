using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
   [SerializeField] private string prompt;
   public string InteractionPrompt => prompt;

   public virtual void Interact(Interactor interactor){ }
}
