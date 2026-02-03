using Godot;
using System;

public partial class MoonQueenScene : Node2D
{
    private DialogSystem _dialogSystem;
    private Sprite2D _moonQueen;
    private bool _dialogStarted = false;

    public override void _Ready()
    {
        _dialogSystem = GetNode<DialogSystem>("DialogSystem");

        if (HasNode("MoonQueen"))
        {
            _moonQueen = GetNode<Sprite2D>("MoonQueen");
        }

        _dialogSystem.DialogFinished += OnDialogFinished;

        // 少し待ってからダイアログ開始
        GetTree().CreateTimer(1.5).Timeout += StartDialog;
    }

    private void StartDialog()
    {
        if (_dialogStarted) return;
        _dialogStarted = true;

        var dialogue = GameManager.Instance.GetMoonQueenDialogue();
        _dialogSystem.StartDialog(dialogue, "月の女王");
    }

    private void OnDialogFinished()
    {
        // 次の周回を開始
        GameManager.Instance.StartNewCycle();

        // フェードアウトして次の周回へ
        var overlay = GetNode<ColorRect>("OverlayLayer/BlackOverlay");
        var tween = CreateTween();

        tween.TweenProperty(overlay, "color", new Color(0, 0, 0, 1), 2.0f);
        tween.TweenInterval(1.0);
        tween.TweenCallback(Callable.From(() =>
        {
            GetTree().ChangeSceneToFile("res://Scenes/Bedroom.tscn");
        }));
    }
}
