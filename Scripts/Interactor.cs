using System;
using Godot;
using Godot.Collections;

public partial class Interactor : Node
{
    [Export] private Transform _interactionPoint;
    [Export] private LayerMask _interactionLayer;
    [Export] private float _interactionRadius = 1f;
    private List<IInteractable> _interactablesInRadius;
    private GameObject _closestInteractable;
    private IInteractable _currentInteractable;
    private Player _player;

    [Export] private Material _outlineMaterial;
    private Material _originalMaterial;

    private PlayerInput _playerInput;

    public bool IsInteracting { get; private set; }
    public Player Player => _player;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _playerInput = new PlayerInput();
        _interactablesInRadius = new List<IInteractable>();
    }

    private void OnEnable()
    {
        _playerInput.Player.Interact.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Player.Interact.Disable();
    }

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_interactionPoint.position, _interactionRadius);

        _interactablesInRadius.Clear();
        GameObject oldClosestInteractable = _closestInteractable;
        _closestInteractable = null;

        if (colliders.Length > 0)
        {
            float closestDistance = _interactionRadius + 1;

            for (int i = 0; i < colliders.Length; i++)
            {
                var interactable = colliders[i].GetComponent<IInteractable>();

                if (interactable != null)
                {
                    _interactablesInRadius.Add(interactable);

                    float currentDistance = Vector2.Distance(_interactionPoint.position, colliders[i].gameObject.transform.position);
                    if (currentDistance < closestDistance)
                    {
                        closestDistance = currentDistance;
                        _closestInteractable = colliders[i].gameObject;
                    }
                }
            }

            if (_playerInput.Player.Interact.WasPerformedThisFrame())
            {
                if (IsInteracting)
                {
                    EndInteraction(_currentInteractable);
                }
                if (_closestInteractable != null)
                {
                    StartInteraction(_closestInteractable.GetComponent<IInteractable>());
                }
            }
        }
        if (_closestInteractable != null)
        {
            if (oldClosestInteractable != null)
            {
                oldClosestInteractable.GetComponent<SpriteRenderer>().material = _originalMaterial;
                _originalMaterial = _closestInteractable.GetComponent<SpriteRenderer>().material;
                if(_closestInteractable.GetComponent<IInteractable>().Type == IInteractable.InteractableType.Resource)
                {
                    _outlineMaterial.SetColor("_OutlineColor", Color.cyan);
                }
                else
                {
                    _outlineMaterial.SetColor("_OutlineColor", Color.green);
                }
                _closestInteractable.GetComponent<SpriteRenderer>().material = _outlineMaterial;
            }
            else
            {
                _originalMaterial = _closestInteractable.GetComponent<SpriteRenderer>().material;
                if (_closestInteractable.GetComponent<IInteractable>().Type == IInteractable.InteractableType.Resource)
                {
                    _outlineMaterial.SetColor("_OutlineColor", Color.cyan);
                }
                else
                {
                    _outlineMaterial.SetColor("_OutlineColor", Color.green);
                }
                _closestInteractable.GetComponent<SpriteRenderer>().material = _outlineMaterial;
            }
        }
        else if(oldClosestInteractable != null)
        {
            oldClosestInteractable.GetComponent<SpriteRenderer>().material = _originalMaterial;
            _originalMaterial = null;
        }

        if (!_interactablesInRadius.Contains(_currentInteractable) && _currentInteractable != null)
        {
            EndInteraction(_currentInteractable);
        }
    }

    void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccesful);
        IsInteracting = interactSuccesful;
        _currentInteractable = interactable;
    }

    void EndInteraction(IInteractable interactable)
    {
        interactable.EndInteraction();
        IsInteracting = false;
        _currentInteractable = null;
    }
}