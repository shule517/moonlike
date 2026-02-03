using Godot;
using System;

public partial class SceneTransition : CanvasLayer
{
    [Signal]
    public delegate void TransitionFinishedEventHandler();

    private ColorRect _fadeRect;
    private AnimationPlayer _animPlayer;
    private string _nextScene = "";

    public static SceneTransition Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        _fadeRect = GetNode<ColorRect>("FadeRect");
        _animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        _animPlayer.AnimationFinished += OnAnimationFinished;

        // 初期状態は透明
        _fadeRect.Color = new Color(0, 0, 0, 0);
    }

    /// <summary>
    /// フェードアウトしてシーン遷移
    /// </summary>
    public void ChangeScene(string scenePath)
    {
        _nextScene = scenePath;
        _animPlayer.Play("fade_out");
    }

    /// <summary>
    /// ホワイトアウト（世界の終わり用）
    /// </summary>
    public void WhiteOut(string scenePath)
    {
        _nextScene = scenePath;
        _animPlayer.Play("white_out");
    }

    /// <summary>
    /// フェードイン（シーン開始時）
    /// </summary>
    public void FadeIn()
    {
        _animPlayer.Play("fade_in");
    }

    private void OnAnimationFinished(StringName animName)
    {
        if (animName == "fade_out" || animName == "white_out")
        {
            // シーンを読み込む
            if (!string.IsNullOrEmpty(_nextScene))
            {
                GetTree().ChangeSceneToFile(_nextScene);
            }
            EmitSignal(SignalName.TransitionFinished);
        }
    }
}
