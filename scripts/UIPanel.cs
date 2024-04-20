using Godot;
using System;

[GlobalClass]
public partial class UIPanel : MeshInstance3D
{
	[Export] public SubViewport ControlViewport;
	[Export] public StaticBody3D StaticBody;
    [Export] public PackedScene UIScene;
	[Export(PropertyHint.Layers3DPhysics)] public uint UICollisionLayer;
    [Export] public Vector2 PanelSize;
    [Export] public int ResolutionScale;


	private Control _sceneInstance;


    public override void _Ready()
	{
		//configure viewport and UI scene. a filled viewport is presumed to be pre-configured.
		if (ControlViewport == null)
		{
			ControlViewport = new SubViewport();
			AddChild(ControlViewport);
            ControlViewport.Size = (Vector2I)(ResolutionScale * PanelSize);
			ControlViewport.Disable3D = true; //3D rendering isn't necessary for UI
        }

        _sceneInstance = (Control)UIScene.Instantiate();
		ControlViewport.AddChild(_sceneInstance);


		//configure mesh. a filled mesh is presumed to be pre-configured.
		if (Mesh == null)
		{
			QuadMesh newMesh = new QuadMesh();
			newMesh.Size = PanelSize;

			StandardMaterial3D newMaterial = new StandardMaterial3D();
			newMaterial.AlbedoTexture = ControlViewport.GetTexture();

			newMesh.Material = newMaterial;
			Mesh = newMesh;
		}


		//configure collision object. a filled object is presumed to be pre-configured.
		if (StaticBody == null)
		{
			CreateTrimeshCollision();
			StaticBody = (StaticBody3D)GetChild(GetChildCount() - 1);
			StaticBody.CollisionMask = 0;
			StaticBody.CollisionLayer = UICollisionLayer;

			UIInteractable interactable = new UIInteractable();
			interactable.UIHover += MouseHover;
			interactable.UIMouseDown += MouseDown;
			interactable.UIMouseUp += MouseUp;

            StaticBody.AddChild(interactable);
		}
	}

	private Vector2I InverseTransformToViewport(Vector3 v)
	{
		//local space
		Vector3 localSpace = (GlobalTransform.Inverse() * v);
		//screen space, but in units
		Vector2 screenSpace = new Vector2(
			localSpace.X + (PanelSize.X / 2), 
			(PanelSize.Y / 2) - localSpace.Y
		);
		//screen space, in pixels
		Vector2I pixels = (Vector2I)(screenSpace * (float)ResolutionScale);

        return pixels;
	}

	public void MouseHover(Vector3 where)
	{
		InputEventMouseMotion @event = new InputEventMouseMotion();

		@event.Device = -1;
		@event.Position = InverseTransformToViewport(where);

		ControlViewport.PushInput(@event);
	}

	public void MouseDown(Vector3 where)
    {
        InputEventMouseButton @event = new InputEventMouseButton();

        @event.Device = -1;
        @event.Position = InverseTransformToViewport(where);
        @event.ButtonIndex = MouseButton.Left;
        @event.Pressed = true;

        ControlViewport.PushInput(@event);
    }

	public void MouseUp(Vector3 where)
	{
        InputEventMouseButton @event = new InputEventMouseButton();

        @event.Device = -1;
        @event.Position = InverseTransformToViewport(where);
        @event.ButtonIndex = MouseButton.Left;
        @event.Pressed = false;

        ControlViewport.PushInput(@event);
    }
}
