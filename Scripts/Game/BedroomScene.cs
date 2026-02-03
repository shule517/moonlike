using Godot;
using System;

public partial class BedroomScene : Node2D
{
    private DialogSystem _dialogSystem;
    private PlayerController _player;
    private ColorRect _curtainOverlay;
    private bool _introFinished = false;

    public override void _Ready()
    {
        _dialogSystem = GetNode<DialogSystem>("DialogSystem");
        _player = GetNode<PlayerController>("Player");
        _curtainOverlay = GetNode<ColorRect>("CurtainOverlay");

        // 最初は移動不可
        _player.SetCanMove(false);

        // ダイアログ終了時の処理
        _dialogSystem.DialogFinished += OnIntroDialogFinished;

        // フェードインしてからイントロ開始
        CallDeferred(nameof(StartIntro));
    }

    private void StartIntro()
    {
        // お母さんのセリフ
        var introLines = new[]
        {
            "・・うた！ ・・・しょうたってば！",
            "まだ寝てるの？もう何時だと思ってるの！",
            "お日様はとっくに起きてるのに、あなたときたら…",
            "ほら、カーテン開けるからね！"
        };

        // 少し待ってからダイアログ開始
        GetTree().CreateTimer(0.5).Timeout += () =>
        {
            _dialogSystem.StartDialog(introLines, "？？？");
        };
    }

    private void OnIntroDialogFinished()
    {
        if (!_introFinished)
        {
            _introFinished = true;
            _dialogSystem.DialogFinished -= OnIntroDialogFinished;
            OpenCurtains();
        }
    }

    private void OpenCurtains()
    {
        // カーテンを開ける演出（画面を明るくする）
        var tween = CreateTween();
        tween.TweenProperty(_curtainOverlay, "color", new Color(0, 0, 0, 0), 1.0f);
        tween.TweenCallback(Callable.From(() =>
        {
            _player.SetCanMove(true);
        }));
    }

    /// <summary>
    /// ドアのインタラクションから呼ばれる
    /// </summary>
    public void OnDoorInteracted(Node2D body)
    {
        if (body is PlayerController)
        {
            // 砂漠シーンへ遷移
            GetTree().ChangeSceneToFile("res://Scenes/Desert.tscn");
        }
    }
}
