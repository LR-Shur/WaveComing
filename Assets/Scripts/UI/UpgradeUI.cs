using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Button btnHealth;
    [SerializeField] private Button btnDamage;
    [SerializeField] private Button btnSpeed;
    
    [SerializeField] private Button btnClipSize;
    [SerializeField] private Button btnFireRate;
    [SerializeField] private Button btnCritChance;
    [SerializeField] private Button btnReloadTime;

    public Player player;
    
    [Header("立绘显示")]
    [SerializeField] private Image portraitRenderer;
    [SerializeField] private Sprite warriorPortrait;
    [SerializeField] private Sprite archerPortrait;
    [SerializeField] private Sprite magePortrait;

    [Header("属性数值显示")]
    [SerializeField] private TextMeshProUGUI txtHealthValue;
    [SerializeField] private TextMeshProUGUI txtDamageValue;
    [SerializeField] private TextMeshProUGUI txtSpeedValue;
    
    [SerializeField] private TextMeshProUGUI txtClipSizeValue;
    [SerializeField] private TextMeshProUGUI txtFireRateValue;
    [SerializeField] private TextMeshProUGUI txtCritChanceValue;
    [SerializeField] private TextMeshProUGUI txtReloadTimeValue;



    private void Awake()
    {
       

        btnHealth.onClick.AddListener(() =>
        {
            player.IncreaseMaxHealth(20);   // 每次加20生命，可按需修改
            CloseUI();
        });
        btnDamage.onClick.AddListener(() =>
        {
            player.IncreaseDamage(5);       // 每次加5伤害
            CloseUI();
        });
        btnSpeed.onClick.AddListener(() =>
        {
            player.IncreaseMoveSpeed(1f); // 每次加0.5移速
            CloseUI();
        });
        // 新增四项
        btnClipSize.onClick.AddListener(() =>
        {
            player.IncreaseClipSize(1);
            CloseUI();
        });
        btnFireRate.onClick.AddListener(() =>
        {
            player.IncreaseFireRate(1);
            CloseUI();
        });
        btnCritChance.onClick.AddListener(() =>
        {
            player.IncreaseCriticalChance(5);
            CloseUI();
        });
        btnReloadTime.onClick.AddListener(() =>
        {
            // 这里用负值让换弹更快：-0.2秒
            player.IncreaseReloadTime(-0.2f);
            CloseUI();
        });
    }

    public void CloseUI()
    {
        player.OnUpgradeUIClosed();
        this.gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        // 先更新立绘和属性数值
        UpdatePortrait();
        UpdateStats();
        
        this.gameObject.SetActive(true);
        
    }
    /// <summary>
    /// 根据选中的角色设置立绘
    /// </summary>
    private void UpdatePortrait()
    {
        switch (GameManager.Instance.SelectedCharacter)
        {
        case CharacterType.Warrior:
            portraitRenderer.sprite = warriorPortrait;
            break;
        case CharacterType.Archer:
            portraitRenderer.sprite = archerPortrait;
            break;
        case CharacterType.Mage:
            portraitRenderer.sprite = magePortrait;
            break;
        }
    }

    /// <summary>
    /// 从 Player 读取当前属性并显示
    /// </summary>
    private void UpdateStats()
    {
        txtHealthValue.text = player.GetMaxHealth().ToString();
        txtDamageValue.text = player.GetDamage().ToString();
        txtSpeedValue.text = player.GetMoveSpeed().ToString("F1");
        // 新增四项
        txtClipSizeValue.text   = player.GetClipSize().ToString();
        txtFireRateValue.text   = player.GetFireRate().ToString();
        txtCritChanceValue.text = player.GetCriticalChance().ToString() + "%";
        txtReloadTimeValue.text = player.GetReloadTime().ToString("F2") + "s";
    }



}
