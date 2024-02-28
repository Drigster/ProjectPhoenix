using System.Linq;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Interactor : Area2D
{
    private Node2D _activeInteractableNode;
    private IInteractable _activeInteractable => (IInteractable)_activeInteractableNode;
    private IInteractable _interactingWith;
    private Shader _outlineShader = GD.Load<Shader>("res://Shaders/Outline.gdshader");

    public void OnBodyEntered(Node2D body){
        if(body is not IInteractable){
            return;
        }

        body.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Material = new ShaderMaterial {
            Shader = _outlineShader
        };

        if(_activeInteractableNode == null){
            _activeInteractableNode = body;
            return;
        }

        if(Position.DistanceTo(body.Position) < Position.DistanceTo(_activeInteractableNode.Position)){
            _activeInteractableNode = body;
        }
    }

    // TODO: Fix bug where the player can't interact with the object after interacting with the new one but never leaving range of the old one
    // TODO: Fix shader cliping outsode of the sprite
    public void OnBodyExited(Node2D body){
        if(body is not IInteractable){
            return;
        }

        body.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Material = null;

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
            if(_interactingWith != null){
                _interactingWith.EndInteraction();
                _interactingWith = null;

                if(_interactingWith != _activeInteractable){
                    _activeInteractable.Interact(this, out isSuccesfull);

                    if(isSuccesfull){
                        _interactingWith = _activeInteractable;
                    }
                }
            }
            else{
                _activeInteractable.Interact(this, out isSuccesfull);

                if(isSuccesfull){
                    _interactingWith = _activeInteractable;
                }
            }
        }
    }
}