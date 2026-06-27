using UnityEngine;

public enum CharacterType
{
    Warrior,
    Archer,
    Mage
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // 存储当前选中的角色
    public CharacterType SelectedCharacter { get; private set; } = CharacterType.Warrior;
    // 新增：存储要加载的地图场景名
    public string SelectedMapScene { get; private set; } = "Main"; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 供外部调用，设置选中的角色
    public void SetCharacter(CharacterType type)
    {
        SelectedCharacter = type;
    }
    
    // 新增接口：设置要加载的场景名
    public void SetMap(string sceneName)
    {
        SelectedMapScene = sceneName;
    }
}
