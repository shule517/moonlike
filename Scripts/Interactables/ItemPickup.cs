using Godot;
using System;

public partial class ItemPickup : Interactable
{
    [Signal]
    public delegate void ItemPickedUpEventHandler(string itemName);

    [Export]
    public string ItemName = "item";

    [Export]
    public string PickupMessage = "を手に入れた！";

    private bool _pickedUp = false;

    protected override void OnReady()
    {
        InteractionPrompt = "拾う";
    }

    public override void Interact()
    {
        if (_pickedUp) return;

        _pickedUp = true;

        // GameManagerにアイテムを追加
        GameManager.Instance.AddItem(ItemName);
        GameManager.Instance.RecordAction($"picked_up_{ItemName}");

        EmitSignal(SignalName.ItemPickedUp, ItemName);
        EmitSignal(SignalName.Interacted);

        GD.Print($"Picked up: {ItemName}");

        // アイテムを非表示にする
        Visible = false;
        SetDeferred("monitoring", false);
    }

    /// <summary>
    /// 取得メッセージを取得
    /// </summary>
    public string GetPickupMessage()
    {
        return $"{ItemName}{PickupMessage}";
    }
}
