using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
   private float _moveSpeed;

   private Vector2 _moveDirection;
   private Rigidbody2D _rigidbody2D;

   private Camera _mainCamera;
   private PlayerGun _playerGun;
   

   [SerializeField] private Transform playerBody;
   [SerializeField] private Animator animator;

   private void Awake()
   {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _mainCamera = Camera.main;
        _playerGun = GetComponentInChildren<PlayerGun>();
        if (_mainCamera == null)
        {
            Debug.LogError("Can't find main camera, please check it!!!");
        }
   }


   public void Init(float moveSpeed)
   {
        _moveSpeed = moveSpeed;
   }

   private void Update()
   {
        // 移动
        _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _moveDirection *= _moveSpeed;

        animator.SetFloat(AnimatorHash.MoveSpeed, Mathf.Abs(_moveDirection.x) + Mathf.Abs(_moveDirection.y));

        // 转向 瞄准
        Vector3 directon = Input.mousePosition- _mainCamera.WorldToScreenPoint(playerBody.position);
        float angle = Mathf.Atan2(directon.x, directon.y) * Mathf.Rad2Deg;
        playerBody.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);

        // 输入
        if (Input.GetMouseButton(0))
        {
            // 扣动扳机
            
            _playerGun.OnTriggerHold();
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            // 松开扳机
            _playerGun.OnTriggerRelease();

        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            // 重新装弹
            _playerGun.Reload();
        }

   }


    private void FixedUpdate()
    {
        _rigidbody2D.AddForce(_moveDirection, ForceMode2D.Impulse);
    }


    public void KnockBack()
    {
        _rigidbody2D.AddRelativeForce(-playerBody.up * 180, ForceMode2D.Force);
    }


    public void SetShootingState(bool IsShooting)
    {
        animator.SetBool(AnimatorHash.IsShooting, IsShooting);
    }
}
