using UnityEngine;


public class Interactor : MonoBehaviour
{
    private IInteractable interactableObject;
    [SerializeField]private Transform interactionPoint;
    [SerializeField]private float interactionPointRadius = 0.5f;
    [SerializeField]private LayerMask interactableMask;
    [SerializeField] private InteractionPromptUI interactionPromptUI;
    
    private readonly Collider2D[] colliders = new Collider2D[3];
    public int numFound;

    private void OnEnable()
    {
        GameEventsManager.Instance.inputEvents.onInteractPressed += InteractPressed;
    }
    

    private void Update()
    {
        numFound = Physics2D.OverlapCircleNonAlloc(interactionPoint.position, interactionPointRadius, 
            colliders, interactableMask);

        if (numFound > 0)
        {
            interactableObject = colliders[0].GetComponent<IInteractable>();
            if (interactableObject != null)
            {
                if(!interactionPromptUI.isDisplayed) 
                    interactionPromptUI.SetUp(interactableObject.InteractionPrompt);
            }
        }
        else
        {
            if (interactableObject != null)
                interactableObject = null;
            if(interactionPromptUI.isDisplayed)
                interactionPromptUI.Close();
        }
    }
    
    private void InteractPressed()
    {
        if (numFound > 0)
        {
            if (interactableObject != null)
                interactableObject.Interact(this);
        }
    }
    
}
