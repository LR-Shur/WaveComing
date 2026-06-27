using UnityEngine;
using System;
using GameStartStudio;
public class LivingEntity :MonoBehaviour, IDamageable
{

    public float startHealth;
    protected float Health { get; private set; }
    protected bool isDead;

    public ParticleSystem deathParticle;
    public string deathSound;

    public event Action OnDeath;

    protected virtual void Start()
    {
        Health = startHealth;
    }
    public virtual void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health > 0 || isDead)
        {
            return;
        }

        Health = 0;

        isDead = true;
        Destroy(gameObject);
        OnDeath?.Invoke();

        Destroy(gameObject);

        // 死亡特效
        if (deathParticle == null)
        {
            Debug.LogError($"Can't find deaath particle on {gameObject.name}");
            return;
        }
        ParticleSystem ps = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(ps.gameObject, ps.main.duration);

        // 死亡音效
        if (!string.IsNullOrEmpty(deathSound))
        {
            SoundManager.Instance.PlaySound(deathSound);
        }
        

    }

}
