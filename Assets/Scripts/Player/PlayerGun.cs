using UnityEngine;
using System.Collections;
using GameStartStudio;
using TMPro;


public class PlayerGun : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform shootTrail;
    [SerializeField] private Transform muzzleFlash;
    [SerializeField] private Transform hitParticle;
    [SerializeField] private TMP_Text remainingBulletText;

    private string remainingBulletCount => (playerData.ClipSize - shotsNumber).ToString();
    private PlayerData playerData;


    //武器系统
    [Header("武器属性")]
    public LayerMask whatToHit;
    public FireMode fireMode = FireMode.Burst;
    private bool isReloading;
    private float nextShotTime;
    private int shotsNumber;

    private bool isTriggerReleased;

    // public void EquipGun(Weapon weapon)
    // {
    //     // Init weapon...
    // }
    

    public void Init(PlayerData pd)
    {
        playerData = pd;
        remainingBulletText.SetText(remainingBulletCount);
        if (fireMode == FireMode.Single)
        {
            isTriggerReleased = true;
        }
    }
    

    public void OnTriggerHold()
    {
        if(isReloading)return;

        if(shotsNumber >= playerData.ClipSize)
        {
            StartCoroutine(ReloadCoroutine());  

            return;
        }


        switch(fireMode)
        {
            case FireMode.Single:
            {
                playerController.SetShootingState(false);
                if(!isTriggerReleased) return;
                Shoot();
                isTriggerReleased = false;
                
                break;
            }
            case FireMode.Burst:
            {
                if(Time.time < nextShotTime)
                {
                    playerController.SetShootingState(false);
                    return;
                }
                nextShotTime = Time.time + 1/(float)playerData.FireRate;
                Shoot();
                break;
            }
        }
 
    }
    public void OnTriggerRelease()
    {
        
        isTriggerReleased = true;
        playerController.SetShootingState(false);
        
      
    }

    #region 重新装弹
    public void Reload()
    {
        StartCoroutine(ReloadCoroutine());   
    }


    

    private IEnumerator ReloadCoroutine()
    {
        // TODO  Animation...
        Debug.Log("重新装弹");
        isReloading = true;
        playerController.SetShootingState(false);
        SoundManager.Instance.PlaySound("Assault Reload");

        yield return new WaitForSeconds(playerData.ReloadTime);

        // After reloading
        shotsNumber = 0;
        isReloading = false;
        remainingBulletText.SetText(remainingBulletCount);
    }

    #endregion

    #region 射击与射击效果

    private void Shoot()
    {
        

        shotsNumber++;
        playerController.SetShootingState(true);
        SoundManager.Instance.PlaySound("Assault Shoot");
        remainingBulletText.SetText(remainingBulletCount);

        Vector2 shootDirection = GetShootDirection();
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, shootDirection, 100, whatToHit);
        if (hit.collider != null)
        {
            ShootEffect(hit);
        }
        else
        {
            ShootEffect(shootDirection);
        }

        playerController.KnockBack();
        
    }


    private void ShootEffect(Vector2 shootDirection)
    {
        float trailAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        Quaternion trailRotation = Quaternion.AngleAxis(trailAngle, Vector3.forward);
        Transform trail = Instantiate(shootTrail, firePoint.position, trailRotation);
        Destroy(trail.gameObject, 0.05f);
        //Debug.DrawLine(firePointPosition, shootDirection * 100, Color.red);

        MuzzleFlash();

    }
    private void ShootEffect(RaycastHit2D hit)
    {
        Vector3 firePointPosition = firePoint.position;
        Transform trail = Instantiate(shootTrail, firePointPosition, Quaternion.identity);
        LineRenderer lineRenderer = trail.GetComponent<LineRenderer>();
        Vector3 endPosition = new Vector3(hit.point.x, hit.point.y, firePointPosition.z);
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(0, firePointPosition);
        lineRenderer.SetPosition(1, endPosition);
        Destroy(trail.gameObject, 0.05f);

        // 打击效果
        Transform sparks = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(sparks.gameObject, 0.2f);

        //制造伤害
        IDamageable damageable = hit.transform.GetComponent<IDamageable>();
        if (damageable != null)
        {
            (int damage, bool isCriticalHit) = playerData.CalculateDamage();
            damageable.TakeDamage(damage);
            PopupText.Create(hit.point, damage, isCriticalHit);
            
        }
        damageable?.TakeDamage(10);
       
        MuzzleFlash();


    }
    private Vector2 GetShootDirection()
    {
        Vector2 shotDir = transform.up;

        shotDir.x += Random.Range(-playerData.AimOffset, playerData.AimOffset);
        shotDir.y += Random.Range(-playerData.AimOffset, playerData.AimOffset);

        return shotDir.normalized;
            
    }


    //枪口火光
    private void MuzzleFlash()
    {
        Transform flash = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
        flash.SetParent(firePoint);
        float randomSize = Random.Range(0.6f, 0.9f);
        flash.localScale = new Vector3(randomSize, randomSize, randomSize);
        Destroy(flash.gameObject, 0.05f);
    }

    #endregion


    

}
