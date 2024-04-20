using Godot;
using System;

public partial class LaserPointer : RayCast3D
{
    [Export] private bool UseReticle = true;
	[Export] private PackedScene Reticle;
    [Export] private Vector3 ReticleScale = Vector3.One * 0.05f;


	private Node3D _ReticleInstance;
    private UIInteractable _HoverTarget;
    private Vector3 _LastCollisionPoint;


    public override void _Ready()
    {
        _ReticleInstance = (Node3D)Reticle.Instantiate();
        AddChild(_ReticleInstance);
        _ReticleInstance.Visible = false;
        _ReticleInstance.Basis = Basis.Scaled(ReticleScale);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        if (!IsColliding())
        {
            _ReticleInstance.Visible = false;
            _HoverTarget?.EmitMouseUp(_LastCollisionPoint);
            _HoverTarget = null;
            return;
        }
        _LastCollisionPoint = GetCollisionPoint();

        Node collider = (Node3D)GetCollider();
        if (collider.HasMeta("UIInteractable")) 
        {
            _HoverTarget = (UIInteractable)collider.GetMeta("UIInteractable");
            _HoverTarget.EmitHover(GetCollisionPoint());
        }

        if (UseReticle)
        {
            _ReticleInstance.Visible = true;
            _ReticleInstance.GlobalPosition = GetCollisionPoint();
            _ReticleInstance.GlobalBasis = Basis.Scaled(ReticleScale) * Basis.LookingAt(GetCollisionNormal());
        }
	}

    public void OnButtonPressed(string name)
    {
        //guard clause
        if (_HoverTarget == null)
        {
            return;
        }


        if (name == "trigger_click")
        {
            _HoverTarget.EmitMouseDown(GetCollisionPoint());
        }
    }

    public void OnButtonReleased(string name)
    {
        //guard clause
        if (_HoverTarget == null)
        {
            return;
        }


        if (name == "trigger_click")
        {
            _HoverTarget.EmitMouseUp(GetCollisionPoint());
        }
    }
}
