using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string InteractionPrompt { get; }//상호작용 가능 오브젝트들은 프롬프트를 가진다
    public void Interact(Interactor interactor);
}
