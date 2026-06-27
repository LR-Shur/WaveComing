# WaveComing

一款基于 Unity 的俯视角肉鸽射击 Demo。玩家选择角色和地图后进入战斗，在持续刷新的敌人波次中移动、射击、换弹、击杀敌人，并通过经验升级获得属性成长。

## 项目简介

- 游戏类型：俯视角 2D 肉鸽射击 / 尸潮生存
- 开发引擎：Unity 2022.3.62f3
- 开发语言：C#
- UI 技术：UGUI、TextMeshPro
- 项目状态：Demo 原型

## 主要功能

- 角色选择：支持战士、弓箭手、法师三种角色配置。
- 地图选择：支持多个战斗场景切换。
- 玩家控制：实现俯视角移动、朝向控制、射击状态、受击击退和死亡处理。
- 射击系统：支持单发 / 连射模式、弹匣数量、射速控制、换弹时间、射线命中、枪口火光、弹道特效和命中反馈。
- 战斗系统：包含伤害计算、暴击判定、敌人受击、死亡事件和伤害飘字。
- 波次系统：根据配置生成敌人，控制敌人数量、出生点、生成间隔和难度递增。
- 成长系统：击杀敌人获得经验，升级时暂停游戏并弹出属性升级界面。
- UI 界面：包含血条、子弹数量、波次提示、角色选择、地图选择、升级面板和游戏结束界面。

## 目录结构

```text
Assets/
  Animations/       动画与 Animator 控制器
  Prefabs/          玩家、敌人、特效、UI 预制体
  Resources/        音频、弹窗文字等运行时资源
  Scenes/           游戏启动、菜单和战斗场景
  Scripts/          核心玩法脚本
    Player/         玩家控制、射击和角色数据
    Enemy/          敌人和可破坏物逻辑
    Wave/           波次刷怪逻辑
    UI/             菜单、战斗 HUD、升级和结算界面
  Sprites/          角色、敌人、UI 等图片资源
Packages/           Unity 包依赖配置
ProjectSettings/    Unity 项目设置
```

## 如何运行

1. 使用 Unity Hub 打开项目根目录。
2. 推荐 Unity 版本：2022.3.62f3。
3. 等待 Unity 导入资源和包依赖。
4. 打开 `Assets/Scenes/GameStart.unity` 或直接从 Build Settings 中的启动场景运行。
5. 点击 Play 进入游戏。

## 核心脚本

- `Assets/Scripts/GameManager.cs`：保存当前选择的角色和地图。
- `Assets/Scripts/Player/PlayerController.cs`：处理玩家移动、朝向和击退。
- `Assets/Scripts/Player/PlayerGun.cs`：处理射击、换弹、命中检测和射击特效。
- `Assets/Scripts/Player/Player.cs`：处理玩家生命、经验、升级和属性成长。
- `Assets/Scripts/Wave/WaveManager.cs`：处理波次生成、敌人数量和难度递增。
- `Assets/Scripts/UI/UpgradeUI.cs`：处理升级选项和属性面板。

## 说明

仓库已排除 Unity 本地缓存目录，例如 `Library/`、`Temp/`、`obj/`、`Logs/` 和 `UserSettings/`。首次打开项目时，Unity 会自动重新生成这些目录。
