# 実装計画

## フェーズ1: プロトタイプ（1周目〜2周目開始）

### 必要なシステム

1. **ゲーム状態管理（GameState）**
   - 現在の周回数
   - プレイヤーの行動履歴
   - 世界の状態

2. **シーン管理**
   - 家の中（Bedroom）
   - 砂漠（Desert）
   - 月の女王パート（MoonQueen）

3. **ダイアログシステム**
   - テキスト表示
   - 文字送り
   - セリフの管理

4. **インタラクションシステム**
   - オブジェクトの調査
   - アイテムの取得
   - アイテムの使用

5. **時間経過システム**
   - 世界の終わりへのカウントダウン
   - 太陽の膨張演出

---

## ファイル構成

```
moonlike/
├── Scenes/
│   ├── Main.tscn              # メインシーン（シーン管理）
│   ├── Bedroom.tscn           # 家の中
│   ├── Desert.tscn            # 砂漠
│   ├── MoonQueen.tscn         # 月の女王パート
│   └── UI/
│       └── DialogBox.tscn     # ダイアログボックス
├── Scripts/
│   ├── Autoload/
│   │   └── GameManager.cs     # ゲーム全体の状態管理
│   ├── Core/
│   │   ├── DialogSystem.cs    # ダイアログ表示
│   │   └── SceneTransition.cs # シーン遷移
│   ├── Player/
│   │   └── PlayerController.cs # プレイヤー操作
│   ├── Interactables/
│   │   ├── Interactable.cs    # インタラクト可能オブジェクトの基底
│   │   ├── ItemPickup.cs      # アイテム取得
│   │   └── PlantSpot.cs       # 種を植える場所
│   └── UI/
│       └── DialogBox.cs       # ダイアログUI
└── Docs/
    ├── GameDesign.md          # ゲームデザイン
    └── Implementation.md      # この文書
```

---

## 実装順序

### Step 1: 基盤システム
- [x] ドキュメント作成
- [x] GameManager（オートロード）
- [x] シーン遷移システム

### Step 2: UIシステム
- [x] ダイアログボックス
- [x] テキスト表示・文字送り

### Step 3: 1周目のシーン
- [x] Bedroom シーン
- [x] Desert シーン
- [x] プレイヤー移動

### Step 4: インタラクション
- [x] アイテム取得（水、種）
- [x] 種を植える機能

### Step 5: 世界の終わり
- [x] 時間経過システム
- [x] 太陽膨張演出
- [x] ホワイトアウト

### Step 6: 月の女王パート
- [x] 月の女王シーン
- [x] 行動に応じたセリフ

### Step 7: 2周目への遷移
- [x] 周回状態の保存
- [x] 世界の変化（季節の誕生）

---

## 技術的な決定事項

### 視点
- 2Dトップダウン または サイドビュー
- プロトタイプではシンプルな2D

### 入力
- 移動: 矢印キー or WASD
- 決定/調べる: Z or Space
- キャンセル: X or Escape

### 画面解像度
- 640x360（16:9、ピクセルアート向け）
- 整数倍でスケーリング

---

## 進捗ログ

### 2026-02-03
- プロジェクト構造の確認
- ゲームデザインドキュメント作成
- 実装計画作成
- スクリプト実装完了
  - GameManager.cs（周回管理、行動記録、インベントリ）
  - DialogSystem.cs（ダイアログ表示、文字送り）
  - SceneTransition.cs（フェード、ホワイトアウト）
  - PlayerController.cs（移動、インタラクション）
  - Interactable.cs / ItemPickup.cs / PlantSpot.cs
  - BedroomScene.cs / DesertScene.cs / MoonQueenScene.cs
- シーン作成完了
  - Bedroom.tscn（家の中、目覚め）
  - Desert.tscn（砂漠、種植え）
  - MoonQueen.tscn（月の女王パート）
  - DialogSystem.tscn / SceneTransition.tscn

---

## 次のステップ（Godotエディタで設定が必要）

### コリジョンの設定
各シーンのCollisionShape2Dにシェイプを設定する必要があります：

1. **Player.tscn**
   - CollisionShape2D: CircleShape2D (半径 8-10px)
   - InteractionArea/CollisionShape2D: CircleShape2D (半径 20-24px)

2. **Bedroom.tscn**
   - 壁のコリジョン: RectangleShape2D
   - WaterItem: CircleShape2D
   - Door: RectangleShape2D

3. **Desert.tscn**
   - HouseDoor: RectangleShape2D
   - SeedItem: CircleShape2D
   - PlantSpot: CircleShape2D

### スプライトの設定
仮のグラフィックまたはプレースホルダーを設定：
- プレイヤーのAnimatedSprite2D
- 太陽、月、恐竜の骨などのSprite2D

### テスト手順
1. Godotでプロジェクトを開く
2. 各シーンのCollisionShapeを設定
3. F5でゲームを実行
4. 動作確認：
   - 目覚めのダイアログが表示される
   - 外に出ると砂漠シーン
   - アイテム取得、種植えができる
   - 時間経過で世界が終わる
   - 月の女王のダイアログ
   - 2周目で季節メッセージが表示される
