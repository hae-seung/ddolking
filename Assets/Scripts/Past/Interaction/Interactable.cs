using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
   [SerializeField] private string prompt;
   public string InteractionPrompt => prompt;//인터페이스에 있던 내용인데 prompt 출력하는 람다식으로 함수 정의
   
   public virtual void Interact(Interactor interactor){}
}
