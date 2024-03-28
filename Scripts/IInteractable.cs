public interface IInteractable
{
    public enum InteractableTypes { Default, Storrage, Resource }
    public InteractableTypes InteractableType { get; }
    public void Interact(Interactor interactor, out bool interactionSuccesful);
    public void EndInteraction();
}