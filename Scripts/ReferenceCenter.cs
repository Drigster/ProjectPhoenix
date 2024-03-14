using Godot;

public partial class ReferenceCenter : Node
{
    [Export] public UITransferSlot UITransferSlot;
    [Export] public Player Player;
    [Export] public ItemsController ItemsController;

    public async override void _EnterTree()
    {
        Player = GetNodeOrNull<Player>("/root/Main/Player");

        CanvasLayer ui = GetNodeOrNull<CanvasLayer>("/root/Main/UI");
        UITransferSlot = GetNodeOrNull<UITransferSlot>("/root/UITransferSlot");
        await ToSignal(GetTree(), "process_frame");
        if (ui != null && UITransferSlot != null)
        {
            GetTree().Root.RemoveChild(UITransferSlot);
            ui.AddChild(UITransferSlot);
        }
    }
}
