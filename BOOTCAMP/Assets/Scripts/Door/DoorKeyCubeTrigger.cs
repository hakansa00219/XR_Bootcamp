using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorKeyCubeTrigger : DoorTrigger
{
    [SerializeField] private int keyCubeLevel = 1;
    [SerializeField] private XRSocketInteractor _socket;
    [SerializeField] private Light _light;
    [SerializeField] private Color _defaultColor = Color.yellow;
    [SerializeField] private Color _errorColor = Color.red;
    [SerializeField] private Color _successColor = Color.green;
    
    private bool _isOpen = false;

    private void Start()
    {
        SetLightColor(_defaultColor);
        
        _socket.selectEntered.AddListener(KeyCubeInserted);
        _socket.selectExited.AddListener(KeyCubeRemoved);
    }

    private void KeyCubeRemoved(SelectExitEventArgs arg0)
    {
        SetLightColor(_defaultColor);
        _isOpen = false;
        CloseDoor();
    }

    private void KeyCubeInserted(SelectEnterEventArgs arg0)
    {
        if (!arg0.interactableObject.transform.TryGetComponent(out KeyCube keycube))
        {
            Debug.LogWarning("No keycard");
            SetLightColor((_errorColor));
            return;
        }
    
        if (keycube.keyCubeLevel >= keyCubeLevel)
        {
            SetLightColor(_successColor);
            _isOpen = true;
            OpenDoor();
        }
        else
        {
            SetLightColor(_errorColor);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (_isOpen) return;
        
        base.OnTriggerExit(other);
    }

    private void SetLightColor(Color color)
    {
        _light.color = color;
    }
}
