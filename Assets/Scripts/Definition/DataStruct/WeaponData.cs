
public class WeaponData
{
    public int Damage { get; set; }
    public int ClipSize { get; set; }
    public int FireRate { get; set; }
    public int CriticalChance { get; set; }
    public float ReloadTime { get; set; }

    public WeaponData(int damage, int clipSize, int fireRate, int criticalChance, float reloadTime)
    {
        Damage = damage;
        ClipSize = clipSize;
        FireRate = fireRate;
        CriticalChance = criticalChance;
        ReloadTime = reloadTime;
    }
    
}
