

using UnityEngine.XR.Interaction.Toolkit;

public class XRTagLimitedSocketInteractor : XRSocketInteractor
{
    public string interactableTag;

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.CompareTag(interactableTag);
    }
    
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.CompareTag(interactableTag);
    }
}
