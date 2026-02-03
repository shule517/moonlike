using Godot;
using System;

public partial class Interactable : Area2D
{
    [Signal]
    public delegate void InteractedEventHandler();

    [Export]
    public string InteractionPrompt = "調べる";

    protected Label _promptLabel;

    public override void _Ready()
    {
        // プロンプトラベルがあれば取得
        if (HasNode("PromptLabel"))
        {
            _promptLabel = GetNode<Label>("PromptLabel");
            _promptLabel.Visible = false;
        }

        OnReady();
    }

    /// <summary>
    /// 子クラスでオーバーライド可能な初期化
    /// </summary>
    protected virtual void OnReady() { }

    /// <summary>
    /// インタラクション実行
    /// </summary>
    public virtual void Interact()
    {
        EmitSignal(SignalName.Interacted);
        GD.Print($"Interacted with: {Name}");
    }

    /// <summary>
    /// プロンプトを表示
    /// </summary>
    public virtual void ShowPrompt()
    {
        if (_promptLabel != null)
        {
            _promptLabel.Text = InteractionPrompt;
            _promptLabel.Visible = true;
        }
    }

    /// <summary>
    /// プロンプトを非表示
    /// </summary>
    public virtual void HidePrompt()
    {
        if (_promptLabel != null)
        {
            _promptLabel.Visible = false;
        }
    }
}
