# 開発環境

## 概要

本プロジェクトは **Godot 4.5 Mono (C#)** を使用して開発しています。

## 必要な環境

| 項目 | バージョン |
|------|-----------|
| Godot Engine | 4.5 (Mono / .NET版) |
| .NET SDK | 8.0 以上 |
| C# | 12.0 |

## セットアップ

1. [Godot 4.5 .NET版](https://godotengine.org/download)をダウンロード・インストール
2. [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)をインストール
3. プロジェクトをクローン
   ```bash
   git clone <repository-url>
   cd moonlike
   ```
4. Godotエディタでプロジェクトを開く

## プロジェクト構成

```
moonlike/
├── .godot/          # Godot内部ファイル（自動生成）
├── icon.svg         # プロジェクトアイコン
├── project.godot    # プロジェクト設定
└── DEVELOPMENT.md   # 本ファイル
```

## レンダラー

- **Forward Plus** (Vulkan)

## 推奨エディタ

- Visual Studio Code + C# Dev Kit拡張
- Visual Studio 2022
- JetBrains Rider
