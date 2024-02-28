using Godot;
using Microsoft.VisualBasic;

public partial class ReferenceCenter : Node
{
	public UITransferSlot UITransferSlot;

    public async override void _Ready()
    {
		CanvasLayer ui = GetNodeOrNull<CanvasLayer>("/root/Main/UI");
		UITransferSlot = GetNodeOrNull<UITransferSlot>("/root/UITransferSlot");
		await ToSignal(GetTree(), "process_frame");
		if(ui != null && UITransferSlot != null){
			GetTree().Root.RemoveChild(UITransferSlot);
			ui.AddChild(UITransferSlot);
		}
    }
}
