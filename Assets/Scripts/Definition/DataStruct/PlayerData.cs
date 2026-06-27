using UnityEngine;
using System.Collections;

public class PlayerData
{
    public int Vitality {get; set; }// 生命值
    public int Agility {get; set; }// 敏捷
    public int Accuracy {get; set; }// 精准度
    public WeaponData WeaponData { get; set; }

    
    // 用于外部指定的自定义速度；如果 <= 0，则走原公式
    private float _customMoveSpeed = -1f;

    
    
    
    // Player...
    public float AimOffset => 1f / Accuracy * 8.0f; // -0.16 ~ 0.16  Accuracy值越大，瞄准偏移值越小
    public float MoveSpeed => _customMoveSpeed > 0f 
        ? _customMoveSpeed 
        : 2f + Agility / 30.0f;
    public float Health => Vitality;

    // Weapon
    public int Damage 
    {
        get => WeaponData.Damage;
        set => WeaponData.Damage = value;
    }

    public int ClipSize
    {
        get => WeaponData.ClipSize;
        set => WeaponData.ClipSize = value;
    }

    public int FireRate
    {
        get => WeaponData.FireRate;
        set => WeaponData.FireRate = value;
    }

    public int CriticalChance
    {
        get => WeaponData.CriticalChance;
        set => WeaponData.CriticalChance = value;
    }

    public float ReloadTime
    {
        get => WeaponData.ReloadTime;
        set => WeaponData.ReloadTime = value;
    }

    public PlayerData()
    {
        Vitality = 40;
        Agility = 80;
        Accuracy = 50;

        // load config
        WeaponData = new WeaponData(5, 20, 5, 7, 2.0f);
    }

    // >>> 新增的带参构造函数 <<<
    // 调用无参构造先把默认值（包含 WeaponData）设好
    public PlayerData(int health, float moveSpeed, int damage) : this()
    {
        Vitality = health;                // 用外部传入的生命
        Agility = 80;
        Accuracy = 50;
        _customMoveSpeed = moveSpeed;     // 用外部传入的速度
        WeaponData = new WeaponData(damage, 20, 5, 7, 2.0f);
        
    }
    
    
    public (int, bool) CalculateDamage()
    {
        float dmg = Damage + Damage * Random.Range(-0.1f, 0.1f);
        if (Random.Range(0, 101) > CriticalChance)
            return ((int)dmg, false);

        dmg *= Random.Range(2.0f, 3.0f);
        return ((int)dmg, true);
    }
}
