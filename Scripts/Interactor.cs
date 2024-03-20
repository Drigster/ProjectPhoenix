using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Interactor : Area2D
{
    List<Node2D> _interactablesInRadius = new List<Node2D>();
    private Node2D _closestInteractableNode;
    private IInteractable _activeInteractable => (IInteractable)_closestInteractableNode;
    private IInteractable _interactingWith;
    private VisualShader _outlineShader = GD.Load<VisualShader>("res://Shaders/Outline.tres");

    public void OnBodyEntered(Node2D body){
        if(body is not IInteractable){
            return;
        }

        _interactablesInRadius.Add(body);
        RecalculateClosestInteractableAndApplyShader();
    }

    public void OnBodyExited(Node2D body){
        if(body is not IInteractable){
            return;
        }
        
        _interactablesInRadius.Remove(body);
        RecalculateClosestInteractableAndApplyShader();

        if(_interactingWith == body){
            _interactingWith.EndInteraction();
            _interactingWith = null;
        }
    }

    private void RecalculateClosestInteractableAndApplyShader(){
        _interactablesInRadius = _interactablesInRadius.OrderBy(x => Position.DistanceTo(x.Position)).ToList();

        if(_closestInteractableNode != _interactablesInRadius.FirstOrDefault()){
            if(_closestInteractableNode != null){
                _closestInteractableNode.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Material = null;
            }

            if(_interactablesInRadius.Count == 0){
                _closestInteractableNode = null;
                return;
            }

            _closestInteractableNode = _interactablesInRadius.FirstOrDefault();
            ShaderMaterial material = new ShaderMaterial(){
                Shader = _outlineShader
            };
            if(((IInteractable)_closestInteractableNode).Type == IInteractable.InteractableType.Resource)
                material.SetShaderParameter("OutlineColor", new Color(1, 0, 0));
            else if(((IInteractable)_closestInteractableNode).Type == IInteractable.InteractableType.Storrage)
                material.SetShaderParameter("OutlineColor", new Color(0, 1, 0));
            _closestInteractableNode.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Material = material;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("interact") && _activeInteractable != null){
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