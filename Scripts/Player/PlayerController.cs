using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    [Export]
    public float Speed = 100.0f;

    private Area2D _interactionArea;
    private Interactable? _nearestInteractable;
    private bool _canMove = true;

    public override void _Ready()
    {
        _interactionArea = GetNode<Area2D>("InteractionArea");

        _interactionArea.BodyEntered += OnInteractionAreaEntered;
        _interactionArea.BodyExited += OnInteractionAreaExited;
        _interactionArea.AreaEntered += OnInteractionAreaAreaEntered;
        _interactionArea.AreaExited += OnInteractionAreaAreaExited;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_canMove) return;

        Vector2 velocity = Vector2.Zero;

        if (Input.IsActionPressed("ui_right"))
            velocity.X += 1;
        if (Input.IsActionPressed("ui_left"))
            velocity.X -= 1;
        if (Input.IsActionPressed("ui_down"))
            velocity.Y += 1;
        if (Input.IsActionPressed("ui_up"))
            velocity.Y -= 1;

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        if (!_canMove) return;

        if (@event.IsActionPressed("ui_accept") && _nearestInteractable != null)
        {
            _nearestInteractable.Interact();
        }
    }

    /// <summary>
    /// 移動を有効/無効にする
    /// </summary>
    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
        if (!canMove)
        {
            Velocity = Vector2.Zero;
        }
    }

    private void OnInteractionAreaEntered(Node2D body)
    {
        if (body is Interactable interactable)
        {
            _nearestInteractable = interactable;
            interactable.ShowPrompt();
        }
    }

    private void OnInteractionAreaExited(Node2D body)
    {
        if (body is Interactable interactable && _nearestInteractable == interactable)
        {
            _nearestInteractable = null;
            interactable.HidePrompt();
        }
    }

    private void OnInteractionAreaAreaEntered(Area2D area)
    {
        if (area is Interactable interactable)
        {
            _nearestInteractable = interactable;
            interactable.ShowPrompt();
        }
    }

    private void OnInteractionAreaAreaExited(Area2D area)
    {
        if (area is Interactable interactable && _nearestInteractable == interactable)
        {
            _nearestInteractable = null;
            interactable.HidePrompt();
        }
    }
}
