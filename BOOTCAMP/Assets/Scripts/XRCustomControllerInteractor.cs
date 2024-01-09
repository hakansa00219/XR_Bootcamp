using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

public class XRCustomControllerInteractor : MonoBehaviour
{

    private XRBaseControllerInteractor _controller;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller.GetComponent<XRBaseControllerInteractor>();
        Assert.IsNotNull(_controller, "There is no XRBaseControllerInteractor assigned to this hand");
        
        _controller.selectEntered.AddListener(ParentInteractable);
        _controller.selectExited.AddListener(UnParent);
    }

    private void UnParent(SelectExitEventArgs arg0)
    {
        arg0.interactableObject.transform.parent = null;
    }

    private void ParentInteractable(SelectEnterEventArgs arg0)
    {
        arg0.interactableObject.transform.parent = transform;
    }
    
}
