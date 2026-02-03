using Godot;
using System;

public partial class PlantSpot : Interactable
{
    [Signal]
    public delegate void SeedPlantedEventHandler();

    [Signal]
    public delegate void WateredEventHandler();

    private enum PlantState
    {
        Empty,      // 何もない
        Planted,    // 種を植えた
        Watered,    // 水をあげた
        Sprouted    // 芽が出ている（2周目以降）
    }

    private PlantState _state = PlantState.Empty;
    private Sprite2D _stateSprite;

    protected override void OnReady()
    {
        if (HasNode("StateSprite"))
        {
            _stateSprite = GetNode<Sprite2D>("StateSprite");
        }

        // 2周目以降で種を植えていた場合、芽が出ている
        if (GameManager.Instance.HasSeasons)
        {
            _state = PlantState.Sprouted;
            UpdatePrompt();
            UpdateVisual();
        }
    }

    public override void Interact()
    {
        switch (_state)
        {
            case PlantState.Empty:
                TryPlantSeed();
                break;

            case PlantState.Planted:
                TryWater();
                break;

            case PlantState.Watered:
                ShowMessage("小さな種が、水を吸って静かに眠っている…");
                break;

            case PlantState.Sprouted:
                ShowMessage("小さな芽が顔を出している。\n季節の始まりを感じる…");
                break;
        }

        base.Interact();
    }

    private void TryPlantSeed()
    {
        if (GameManager.Instance.HasItem("種"))
        {
            GameManager.Instance.UseItem("種");
            _state = PlantState.Planted;

            ShowMessage("土に種を植えた。");
            UpdatePrompt();
            UpdateVisual();
        }
        else
        {
            ShowMessage("乾いた土だ。\n何か植えられそう…");
        }
    }

    private void TryWater()
    {
        if (GameManager.Instance.HasItem("水"))
        {
            GameManager.Instance.UseItem("水");
            GameManager.Instance.PlantSeed(); // 種を植えた行動を記録
            _state = PlantState.Watered;

            ShowMessage("水をあげた。\nきっと、いつか芽を出すだろう…");
            EmitSignal(SignalName.SeedPlanted);
            UpdatePrompt();
            UpdateVisual();
        }
        else
        {
            ShowMessage("種を植えた。\n水があればいいのだけど…");
        }
    }

    private void UpdatePrompt()
    {
        InteractionPrompt = _state switch
        {
            PlantState.Empty => "調べる",
            PlantState.Planted => "水をあげる",
            PlantState.Watered => "調べる",
            PlantState.Sprouted => "調べる",
            _ => "調べる"
        };
    }

    private void UpdateVisual()
    {
        if (_stateSprite == null) return;

        // 状態に応じてスプライトを変更（フレームで管理する場合）
        // _stateSprite.Frame = (int)_state;
    }

    private void ShowMessage(string message)
    {
        // DialogSystemがあれば使用
        var dialog = GetTree().Root.FindChild("DialogSystem", true, false) as DialogSystem;
        if (dialog != null)
        {
            dialog.StartDialog(new[] { message });
        }
        else
        {
            GD.Print(message);
        }
    }
}
