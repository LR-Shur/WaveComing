using UnityEngine;
public class Player : LivingEntity
{
    private PlayerData playerData;


    // 1）挂在角色身上的 SpriteRenderer 和 Animator
    [Header("角色视觉")]
    [SerializeField] private SpriteRenderer selfSpriteRenderer;
    [SerializeField] private Animator selfAnimator;

    // 2）三种角色的立绘 Sprite 和 控制器
    [Header("各角色立绘和动画控制器")]
    [SerializeField] private Sprite warriorSprite;
    [SerializeField] private RuntimeAnimatorController warriorController;
    [SerializeField] private Sprite archerSprite;
    [SerializeField] private RuntimeAnimatorController archerController;
    [SerializeField] private Sprite mageSprite;
    [SerializeField] private RuntimeAnimatorController mageController;

    // 3）挂在角色身上的“枪口” Transform（在不同角色上 localPosition 会不同）
    [Header("枪口挂点")]
    [SerializeField] private Transform muzzlePoint;
    
    // --- 新增经验/等级系统字段 ---
    private int currentXP = 0;
    private int currentLevel = 1;
    private int xpToNextLevel = 50;
    public UpgradeUI levelUpUI;
    
    
    
    protected override void Start()
    {
        // playerData = new PlayerData();
        // startHealth = playerData.Health;
        
        
        // 根据选择的角色类型来初始化数据
        switch (GameManager.Instance.SelectedCharacter)
        {
        case CharacterType.Warrior:
            
            playerData = new PlayerData( health: 60, moveSpeed: 4f, damage: 5 );
            break;
        case CharacterType.Archer:
            playerData = new PlayerData( health: 40, moveSpeed: 5.5f, damage: 10 );
            break;
        case CharacterType.Mage:
            playerData = new PlayerData( health: 30, moveSpeed: 1.5f, damage: 20 );
            break;
        default:
            playerData = new PlayerData(); // 兜底
            break;
        }

        startHealth = playerData.Health;
        

        base.Start();

        print(GameManager.Instance.SelectedCharacter);
        
        // ——(C) 视觉层面：替换立绘、动画控制器、以及设置枪口位置
        switch (GameManager.Instance.SelectedCharacter)
        {
        case CharacterType.Warrior:
            selfSpriteRenderer.sprite = warriorSprite;
            selfAnimator.runtimeAnimatorController = warriorController;
            muzzlePoint.localPosition = Vector3.zero; 
            break;
        case CharacterType.Archer:
            selfSpriteRenderer.sprite = archerSprite;
            print("贴图替换为2");
            selfAnimator.runtimeAnimatorController = archerController;
            // 相对坐标 (-8, -8)，保留原来的 z
            muzzlePoint.localPosition = new Vector3(-0.08f, 0, muzzlePoint.localPosition.z);
            break;
        case CharacterType.Mage:
            selfSpriteRenderer.sprite = mageSprite;
            print("贴图替换为3");
            selfAnimator.runtimeAnimatorController = mageController;
            muzzlePoint.localPosition = new Vector3(-0.08f, 0, muzzlePoint.localPosition.z);
            break;
        }

        // Init player control...
        GetComponent<PlayerController>().Init(playerData.MoveSpeed);



        // Init weapon system...
        GetComponentInChildren<PlayerGun>().Init(playerData);


        GameEvents.OnPlayerHealthChanger.Invoke(Health, startHealth);
        
        // 3. 订阅“敌人死亡”事件
        GameEvents.OnEnemyKilled += OnGainXP;
        
    }
    
    public override void TakeDamage(float damage)
    {
        if (damage >= Health && !isDead)
        {
            GameEvents.GameOver?.Invoke();
        }
        base.TakeDamage(damage);


        GameEvents.OnPlayerHealthChanger.Invoke(Health, startHealth);
    }
    
    /// <summary>
    /// 当获得经验时调用
    /// </summary>
    private void OnGainXP(int xp)
    {
        currentXP += xp;
        Debug.Log($"获得经验：{xp}，当前经验：{currentXP}/{xpToNextLevel}");
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// 升级处理
    /// </summary>
    private void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f);  // 下一级经验需求递增

        // 暂停游戏并弹出升级界面
        Time.timeScale = 0f;
        ShowUpgradeUI();
    }
    
    /// <summary>
    /// 实例化升级界面
    /// </summary>
    private void ShowUpgradeUI()
    {
        // 假设 upgradeUIPrefab 是一个 Canvas，挂有 UpgradeUI 脚本
        levelUpUI.ShowUI();
        
    }

    
    private void OnDestroy()
    {
        // 清理订阅，防止内存泄漏
        GameEvents.OnEnemyKilled -= OnGainXP;
    }



    [ContextMenu("Suicide")]
    public void Suicide()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Please use the suicide function while the application is playing!!");
            return;
        }

        TakeDamage(999);
    }
    
    
    // --- 以下为升级界面按钮会调用的接口 ---

    /// <summary>
    /// 提升最大生命
    /// </summary>
    /// <summary>
    /// 提升最大生命
    /// </summary>
    public void IncreaseMaxHealth(int amount)
    {
        // 1) 增加最大生命值
        startHealth += amount;

        // 2) 给玩家加上这部分生命（调用带负数的 TakeDamage 实现加血）
        base.TakeDamage(-amount);

        // 3) 立刻广播一个“生命改变”事件，让 MainForm 刷新血条
        GameEvents.OnPlayerHealthChanger.Invoke(Health, startHealth);

        Debug.Log($"升级：生命 +{amount}，当前生命 {Health}/{startHealth}");
    }

    /// <summary>
    /// 提升伤害
    /// </summary>
    public void IncreaseDamage(int amount)
    {
        playerData.Damage += amount;
        Debug.Log($"升级：伤害 +{amount}");
    }

    /// <summary>
    /// 提升移动速度
    /// </summary>
    public void IncreaseMoveSpeed(float delta)
    {
        // 如果你用的是 customMoveSpeed 构造方式
        // 需要在 PlayerData 类里暴露方法来额外加速
        // 这里演示直接累加
        playerData = new PlayerData(
            health: (int)playerData.Health,
            moveSpeed: playerData.MoveSpeed + delta,
            damage: playerData.Damage
        );
        Debug.Log($"升级：移速 +{delta}");
    }
    /// <summary>提升弹匣容量</summary>
    public void IncreaseClipSize(int amount)
    {
        playerData.ClipSize += amount;
        Debug.Log($"升级：弹匣容量 +{amount} => {playerData.ClipSize}");
    }

    /// <summary>提升射速（每秒可射击次数）</summary>
    public void IncreaseFireRate(int amount)
    {
        playerData.FireRate += amount;
        Debug.Log($"升级：射速 +{amount} => {playerData.FireRate}");
    }

    /// <summary>提升暴击率（百分比）</summary>
    public void IncreaseCriticalChance(int amount)
    {
        playerData.CriticalChance += amount;
        Debug.Log($"升级：暴击率 +{amount}% => {playerData.CriticalChance}%");
    }

    /// <summary>改变换弹时间（秒），负值代表更快</summary>
    public void IncreaseReloadTime(float delta)
    {
        playerData.ReloadTime += delta;
        Debug.Log($"升级：换弹时间 {(delta>=0?"+":"")}{delta:F2}s => {playerData.ReloadTime:F2}s");
    }

    /// <summary>
    /// 升级界面关闭后调用，恢复游戏
    /// </summary>
    public void OnUpgradeUIClosed()
    {
        Time.timeScale = 1f;
    }
    public float GetMaxHealth() => startHealth;
    public int   GetDamage()    => playerData.Damage;
    public float GetMoveSpeed() => playerData.MoveSpeed;
    public int   GetClipSize()       => playerData.ClipSize;
    public int   GetFireRate()       => playerData.FireRate;
    public int   GetCriticalChance() => playerData.CriticalChance;
    public float GetReloadTime()     => playerData.ReloadTime;

}
