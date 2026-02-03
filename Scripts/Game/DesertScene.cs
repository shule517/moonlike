using Godot;
using System;

public partial class DesertScene : Node2D
{
    [Export]
    public float WorldEndTime = 60.0f; // 世界が終わるまでの秒数

    private DialogSystem _dialogSystem;
    private PlayerController _player;
    private ColorRect? _sun;
    private Vector2 _sunOriginalSize;
    private float _timeElapsed = 0;
    private bool _worldEnding = false;
    private ColorRect? _whiteOverlay;

    public override void _Ready()
    {
        _dialogSystem = GetNode<DialogSystem>("DialogSystem");
        _player = GetNode<PlayerController>("Player");

        if (HasNode("Sun"))
        {
            _sun = GetNode<ColorRect>("Sun");
            _sunOriginalSize = _sun.Size;
        }

        if (HasNode("OverlayLayer/WhiteOverlay"))
        {
            _whiteOverlay = GetNode<ColorRect>("OverlayLayer/WhiteOverlay");
            _whiteOverlay.Color = new Color(1, 1, 1, 0);
        }

        // 2周目以降：季節が生まれた世界
        if (GameManager.Instance.HasSeasons)
        {
            ApplySpringWorld();
            ShowSeasonMessage();
        }
    }

    /// <summary>
    /// 春の世界を適用
    /// </summary>
    private void ApplySpringWorld()
    {
        // 背景を緑に変更
        if (HasNode("Background"))
        {
            var background = GetNode<ColorRect>("Background");
            background.Color = new Color(0.4f, 0.7f, 0.3f, 1); // 緑色
        }

        // 草を追加（PlantSpotの見た目を変更）
        if (HasNode("PlantSpot/Visual"))
        {
            var plantVisual = GetNode<ColorRect>("PlantSpot/Visual");
            plantVisual.Color = new Color(0.2f, 0.8f, 0.2f, 1); // 鮮やかな緑
            plantVisual.Size = new Vector2(40, 24); // 大きく育った
            plantVisual.Position = new Vector2(-20, -12);
        }

        // 地面に草を点在させる（視覚的な演出）
        AddGrassPatches();
    }

    /// <summary>
    /// 草のパッチを追加
    /// </summary>
    private void AddGrassPatches()
    {
        var random = new Random();
        for (int i = 0; i < 15; i++)
        {
            var grass = new ColorRect();
            grass.Color = new Color(0.3f, 0.6f, 0.2f, 1);
            grass.Size = new Vector2(random.Next(10, 30), random.Next(5, 15));
            grass.Position = new Vector2(random.Next(50, 590), random.Next(200, 340));
            AddChild(grass);
            MoveChild(grass, 1); // 背景の上、他のオブジェクトの下に配置
        }
    }

    public override void _Process(double delta)
    {
        if (_worldEnding) return;
        if (_dialogSystem != null && _dialogSystem.IsActive()) return;

        _timeElapsed += (float)delta;

        // 太陽の膨張
        float progress = _timeElapsed / WorldEndTime;
        if (_sun != null)
        {
            float scale = 1.0f + progress * 3.0f; // 最大4倍まで膨張
            _sun.Size = _sunOriginalSize * scale;
            // 中心を維持するためにpositionを調整
            _sun.Position = new Vector2(520 - (_sunOriginalSize.X * scale - _sunOriginalSize.X) / 2,
                                        30 - (_sunOriginalSize.Y * scale - _sunOriginalSize.Y) / 2);
        }

        // 画面が徐々に白くなる
        if (_whiteOverlay != null && progress > 0.5f)
        {
            float alpha = (progress - 0.5f) * 2.0f; // 後半で白くなる
            _whiteOverlay.Color = new Color(1, 1, 1, alpha * 0.5f);
        }

        // 世界の終わり
        if (_timeElapsed >= WorldEndTime)
        {
            StartWorldEnd();
        }
    }

    private void ShowSeasonMessage()
    {
        GetTree().CreateTimer(1.0).Timeout += () =>
        {
            _dialogSystem.StartDialog(new[]
            {
                "…世界が変わっている。",
                "",
                "あの種から、緑が生まれた。",
                "",
                "風が優しい。",
                "春が来たんだ。",
                "",
                "小さな行動が、",
                "世界を変えたのかもしれない。"
            });
        };
    }

    private void StartWorldEnd()
    {
        _worldEnding = true;
        _player.SetCanMove(false);

        // ホワイトアウト演出
        var tween = CreateTween();
        tween.TweenProperty(_whiteOverlay, "color", new Color(1, 1, 1, 1), 3.0f);
        tween.TweenCallback(Callable.From(() =>
        {
            // 世界の終わりを処理
            GameManager.Instance.EndWorld();

            // 月の女王シーンへ
            GetTree().ChangeSceneToFile("res://Scenes/MoonQueen.tscn");
        }));
    }

    /// <summary>
    /// 家に戻る
    /// </summary>
    public void OnHouseDoorInteracted(Node2D body)
    {
        if (body is PlayerController)
        {
            GetTree().ChangeSceneToFile("res://Scenes/Bedroom.tscn");
        }
    }
}
