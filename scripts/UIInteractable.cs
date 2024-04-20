using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

[GlobalClass]
public partial class UIInteractable : Node
{
    [Signal] public delegate void UIHoverEventHandler(Vector3 where);
    [Signal] public delegate void UIMouseDownEventHandler(Vector3 where);
    [Signal] public delegate void UIMouseUpEventHandler(Vector3 where);

    public override void _EnterTree()
    {
        Node parent = GetParent();
        if (parent == null)
        {
            return;            
        }

        parent.SetMeta("UIInteractable", this);
    }

    public override void _ExitTree()
    {
        Node parent = GetParent();
        if (parent == null)
        {
            return;
        }

        parent.RemoveMeta("UIInteractable");
    }

    public void EmitHover(Vector3 where)
    {
        EmitSignal(SignalName.UIHover, where);
    }
    public void EmitMouseDown(Vector3 where)
    {
        EmitSignal(SignalName.UIMouseDown, where);
    }
    public void EmitMouseUp(Vector3 where)
    {
        EmitSignal(SignalName.UIMouseUp, where);
    }
}
