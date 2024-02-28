using System.Linq;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Interactor : Area2D
{
    private Node2D _activeInteractableNode;
    private IInteractable _activeInteractable => (IInteractable)_activeInteractableNode;
    private IInteractable _interactingWith;

    public void OnBodyEntered(Node2D body){
        if(body is IInteractable){
            if(_activeInteractableNode == null){
                _activeInteractableNode = body;
                return;
            }

            if(Position.DistanceTo(body.Position) < Position.DistanceTo(_activeInteractableNode.Position)){
                _activeInteractableNode = body;
            }
        }
    }

    public void OnBodyExited(Node2D body){
        if(body is not IInteractable){
            return;
        }

        if(_activeInteractableNode == body){
            _activeInteractableNode = null;
        }
        if(_interactingWith != null && _interactingWith == (IInteractable)body){
            _interactingWith.EndInteraction();
            _interactingWith = null;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("Interact") && _activeInteractable != null){
            bool isSuccesfull;
            _activeInteractable.Interact(this, out isSuccesfull);

            if(isSuccesfull){
                _interactingWith = _activeInteractable;
            }
        }
    }
}