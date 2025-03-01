using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : Interactable
{
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private InteractionBehaviour interactionBehaviour;

    private void Awake()
    {
        SetInteractState(false);
    }

    public override void Interact(Interactor interactor, InputAction.CallbackContext context)
    {
        if (context.control.name.Equals("f"))
        {
            // F키 눌림 의미 todo: 상호작용 : NPC 대화, 장소이동, CraftTab 열기 등등
           interactionBehaviour.Operate(interactor);
        }
    }

    
    
    public override void SetInteractState(bool state)
    {
        interactionUI.SetActive(state);
    }
}
