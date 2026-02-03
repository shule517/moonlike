using Godot;
using System.Collections.Generic;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }

    // 周回数
    public int CycleCount { get; private set; } = 1;

    // プレイヤーの行動履歴（全周回を通じて蓄積）
    public List<string> ActionHistory { get; private set; } = new();

    // 現在の周回での行動
    public List<string> CurrentCycleActions { get; private set; } = new();

    // インベントリ
    public List<string> Inventory { get; private set; } = new();

    // 世界の状態
    public bool HasSeasons { get; private set; } = false;
    public bool PlantedSeed { get; private set; } = false;

    public override void _Ready()
    {
        Instance = this;
        GD.Print($"GameManager initialized. Cycle: {CycleCount}");
    }

    /// <summary>
    /// 行動を記録する
    /// </summary>
    public void RecordAction(string action)
    {
        CurrentCycleActions.Add(action);
        GD.Print($"Action recorded: {action}");
    }

    /// <summary>
    /// アイテムを取得する
    /// </summary>
    public void AddItem(string item)
    {
        if (!Inventory.Contains(item))
        {
            Inventory.Add(item);
            GD.Print($"Item acquired: {item}");
        }
    }

    /// <summary>
    /// アイテムを持っているか確認
    /// </summary>
    public bool HasItem(string item)
    {
        return Inventory.Contains(item);
    }

    /// <summary>
    /// アイテムを使用（削除）
    /// </summary>
    public void UseItem(string item)
    {
        Inventory.Remove(item);
        GD.Print($"Item used: {item}");
    }

    /// <summary>
    /// 種を植えたことを記録
    /// </summary>
    public void PlantSeed()
    {
        PlantedSeed = true;
        RecordAction("planted_seed");
    }

    /// <summary>
    /// 世界の終わり - 次の周回へ
    /// </summary>
    public void EndWorld()
    {
        // 現在の行動を履歴に追加
        ActionHistory.AddRange(CurrentCycleActions);

        // 世界の状態を更新
        if (PlantedSeed)
        {
            HasSeasons = true;
        }

        GD.Print($"World ended. Actions this cycle: {CurrentCycleActions.Count}");
    }

    /// <summary>
    /// 次の周回を開始
    /// </summary>
    public void StartNewCycle()
    {
        CycleCount++;
        CurrentCycleActions.Clear();
        Inventory.Clear();
        PlantedSeed = false;

        GD.Print($"New cycle started: {CycleCount}");
        GD.Print($"World state - HasSeasons: {HasSeasons}");
    }

    /// <summary>
    /// 月の女王のセリフを取得
    /// </summary>
    public string[] GetMoonQueenDialogue()
    {
        var dialogue = new List<string>
        {
            "…時が来たようです。",
            "この世界はただいま終わります。",
            "",
            "けれど、あなたの行いは",
            "消えてしまうわけではありません。"
        };

        // 行動に応じたセリフを追加
        if (PlantedSeed)
        {
            dialogue.Add("");
            dialogue.Add("あなたは地に緑を植えました。");
            dialogue.Add("");
            dialogue.Add("あなたがまいた種により");
            dialogue.Add("季節が生まれました。");
        }
        else if (CurrentCycleActions.Count == 0)
        {
            dialogue.Add("");
            dialogue.Add("あなたは静かに時を過ごしました。");
            dialogue.Add("その静けさもまた、");
            dialogue.Add("世界の一部となるのです。");
        }

        dialogue.AddRange(new[]
        {
            "",
            "それはとても小さなことかもしれません。",
            "",
            "ですが、そのひとつひとつが",
            "次の世界へと静かに息づいていくのです。",
            "",
            "あなたの人生は",
            "たしかに短かったけれど、",
            "意味のないことなんて何ひとつありません。",
            "",
            "わたしは見ていました。",
            "ずっと見守っていましたよ。"
        });

        return dialogue.ToArray();
    }
}
