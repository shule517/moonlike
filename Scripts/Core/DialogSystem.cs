using Godot;
using System;

public partial class DialogSystem : CanvasLayer
{
    [Signal]
    public delegate void DialogFinishedEventHandler();

    private Panel _dialogBox;
    private Label _textLabel;
    private Label _nameLabel;
    private string[] _dialogueLines;
    private int _currentLine = 0;
    private bool _isTyping = false;
    private bool _isActive = false;
    private float _typeSpeed = 0.05f;
    private string _currentText = "";
    private int _charIndex = 0;
    private double _typeTimer = 0;

    public override void _Ready()
    {
        // UIコンポーネントを取得
        _dialogBox = GetNode<Panel>("DialogBox");
        _textLabel = GetNode<Label>("DialogBox/TextLabel");
        _nameLabel = GetNode<Label>("DialogBox/NameLabel");

        // 初期状態では非表示
        _dialogBox.Visible = false;
    }

    public override void _Process(double delta)
    {
        if (!_isActive) return;

        // 文字送り処理
        if (_isTyping)
        {
            _typeTimer += delta;
            if (_typeTimer >= _typeSpeed)
            {
                _typeTimer = 0;
                if (_charIndex < _currentText.Length)
                {
                    _charIndex++;
                    _textLabel.Text = _currentText.Substring(0, _charIndex);
                }
                else
                {
                    _isTyping = false;
                }
            }
        }

        // 入力処理
        if (Input.IsActionJustPressed("ui_accept"))
        {
            if (_isTyping)
            {
                // タイピング中なら全文表示
                _charIndex = _currentText.Length;
                _textLabel.Text = _currentText;
                _isTyping = false;
            }
            else
            {
                // 次の行へ
                NextLine();
            }
        }
    }

    /// <summary>
    /// ダイアログを開始
    /// </summary>
    public void StartDialog(string[] lines, string speakerName = "")
    {
        _dialogueLines = lines;
        _currentLine = 0;
        _isActive = true;
        _dialogBox.Visible = true;

        if (!string.IsNullOrEmpty(speakerName))
        {
            _nameLabel.Text = speakerName;
            _nameLabel.Visible = true;
        }
        else
        {
            _nameLabel.Visible = false;
        }

        ShowLine(_dialogueLines[0]);
    }

    /// <summary>
    /// 1行表示
    /// </summary>
    private void ShowLine(string text)
    {
        _currentText = text;
        _charIndex = 0;
        _textLabel.Text = "";
        _isTyping = true;
        _typeTimer = 0;
    }

    /// <summary>
    /// 次の行へ
    /// </summary>
    private void NextLine()
    {
        _currentLine++;

        if (_currentLine >= _dialogueLines.Length)
        {
            // ダイアログ終了
            EndDialog();
        }
        else
        {
            ShowLine(_dialogueLines[_currentLine]);
        }
    }

    /// <summary>
    /// ダイアログ終了
    /// </summary>
    private void EndDialog()
    {
        _isActive = false;
        _dialogBox.Visible = false;
        EmitSignal(SignalName.DialogFinished);
    }

    /// <summary>
    /// ダイアログがアクティブかどうか
    /// </summary>
    public bool IsActive()
    {
        return _isActive;
    }
}
