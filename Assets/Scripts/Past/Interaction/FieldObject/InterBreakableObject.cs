using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InterBreakableObject : BreakableObject
{
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private InteractionBehaviour interactionBehaviour;

    

    protected override void Start()
    {
        base.Start();
        SetInteractState(false);
    }
    
    public override void Interact(Interactor interactor, InputAction.CallbackContext context, Item currentGripItem = null)
    {
        if (context.control.name.Equals("f"))
        {
            // F키 눌림 의미 todo: 상호작용 : NPC 대화, 장소이동, CraftTab 열기 등등
            interactionBehaviour.Operate(interactor, currentGripItem);
        }
        else
        {
            //부수기 동작
            base.Interact(interactor, context, currentGripItem);
        }
    }
    
    
    public override void SetInteractState(bool state)
    {
        base.SetInteractState(state);
        interactionUI.SetActive(state);
    }

    public void SetEstablishItem(EstablishItem item)
    {
        if (item is ReinforceStructureItem reinforceStructureItem && 
            interactionBehaviour is CraftTabBehaviour craftTabBehaviour)
        {
            structureItem = reinforceStructureItem;
            craftTabBehaviour.SetStructureData(structureItem);
        }
    }
}
