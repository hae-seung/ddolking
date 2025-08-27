using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EnterFutureBehaviour : InteractionBehaviour
{
    [SerializeField] private Transform goalTransform;
    [SerializeField] private CinemachineVirtualCamera currentCamera;
    [SerializeField] private CinemachineVirtualCamera willOnCamera;
    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        UIManager.Instance.StartTransition();
        interactor.transform.position = goalTransform.position;
        
        currentCamera.gameObject.SetActive(false);
        
        willOnCamera.gameObject.SetActive(true);
    }
}
