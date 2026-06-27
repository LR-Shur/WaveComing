using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using GameStartStudio;

public class MenuForm : MonoBehaviour
{
    [SerializeField] private ToggleGroup toggleGroup;
    
    
    [Header("角色选择")]
    [SerializeField] private Button warriorButton;
    [SerializeField] private Button archerButton;
    [SerializeField] private Button mageButton;
    [SerializeField] private Image portraitImage;
    [SerializeField] private Sprite warriorSprite;
    [SerializeField] private Sprite archerSprite;
    [SerializeField] private Sprite mageSprite;
    [Header("开始游戏")]
    [SerializeField] private Button startGameButton;

    public TextMeshProUGUI playerName;
    
    
    [Header("地图选择")]
    [SerializeField] private Button map1Button;
    [SerializeField] private Button map2Button;
    [SerializeField] private Button map3Button;
    [SerializeField] private TextMeshProUGUI map1Label;
    [SerializeField] private TextMeshProUGUI map2Label;
    [SerializeField] private TextMeshProUGUI map3Label;

    private Toggle currentSelection => toggleGroup.GetFirstActiveToggle();
    private Toggle onToggle;

    private void Start()
    {
        var toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener(_ => OnToggleValueChanged(toggle));
        }

        currentSelection.onValueChanged?.Invoke(true);
        
        
        // 绑定按钮回调
        warriorButton.onClick.AddListener(() => SelectCharacter(CharacterType.Warrior));
        archerButton.onClick  .AddListener(() => SelectCharacter(CharacterType.Archer));
        mageButton.onClick    .AddListener(() => SelectCharacter(CharacterType.Mage));

        // startGameButton.onClick.AddListener(() =>
        // {
        //     // 确保已经选了角色
        //     SceneManager.LoadScene("Main");
        // });

        // 默认选中第一个
        SelectCharacter(CharacterType.Warrior);
        
        
        // —— 地图选择绑定 —— 
        map1Button.onClick.AddListener(() => SelectMap("Main", map1Label));
        map2Button.onClick.AddListener(() => SelectMap("Main1", map2Label));
        map3Button.onClick.AddListener(() => SelectMap("Main2", map3Label));

        // 默认选中第一张地图
        SelectMap("Main", map1Label);

        
        
    }


    private void OnToggleValueChanged(Toggle toggle)
    {
        if (currentSelection == onToggle)
        {
            SoundManager.Instance.PlaySound("Finger Snap");

            switch (toggle.name)
            {
                case "GameStart":
                    SceneManager.LoadScene(GameManager.Instance.SelectedMapScene);
                    break;
                case "Settings":
                Debug.LogWarning("TODO: Open settings form...");
                    break;
                case "Sponsor":
                Debug.LogWarning("TODO: Open sponsor form...");
                    break;
                case "Quit":
                {
                Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
                }
                    break;
                default:
                    throw new UnityException("Toggle name is invalid.");
            }
            
            return;
        }
        if (toggle.isOn)
        {

            onToggle = toggle;
            onToggle.transform.Find("Label").GetComponent<TMP_Text>().color = Color.yellow;

            SoundManager.Instance.PlaySound("Select");
        }
        else
        {
            onToggle.transform.Find("Label").GetComponent<TMP_Text>().color = Color.white;
        }

    }

    /// <summary>
    /// 地图按钮回调：记录地图、更新 UI 高亮
    /// </summary>
    private void SelectMap(string sceneName, TextMeshProUGUI selectedLabel)
    {
        // 1. 存储选择
        GameManager.Instance.SetMap(sceneName);

        // 2. UI 高亮（简单示例：三张 Label 颜色重置，再把选中 one 置黄）
        map1Label.color = Color.white;
        map2Label.color = Color.white;
        map3Label.color = Color.white;
        selectedLabel.color = Color.yellow;
    }


    private void SelectCharacter(CharacterType type)
    {
        // 更新全局存储
        GameManager.Instance.SetCharacter(type);

        // 更新立绘
        switch (type)
        {
        case CharacterType.Warrior:
            portraitImage.sprite = warriorSprite;
            playerName.text = "Assault";
            break;
        case CharacterType.Archer:
            portraitImage.sprite = archerSprite;
            playerName.text = "Tank";
            break;
        case CharacterType.Mage:
            portraitImage.sprite = mageSprite;
            playerName.text = "sniper";
            break;
        }

        
    }


}
