using UnityEngine;
public class WoodCrate : LivingEntity
{
    [Header("击破奖励经验")]
    public int xpReward = 30;
    
    public override void TakeDamage(float damage)
    {
        if (damage >= Health && !isDead)
        {
            OnDeath += () => {Debug.Log("Wood crate die!!!");};
        }

        // 死亡特效
        // 死亡音效
        base.TakeDamage(damage);
    }
    
    protected override void Start()
    {
        base.Start();
        // 订阅死亡回调，死亡时广播经验
        OnDeath += () => GameEvents.EnemyKilled(xpReward);
    }

}
