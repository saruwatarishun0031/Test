using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour, IInterface//インターフェースを継承する
{
    [Tooltip("プレイヤーのポジション"), SerializeField] Transform _playerTransform;
    [Tooltip("追いかける範囲"), SerializeField] float _chaseDistance;
    Rigidbody _rb;
    [Tooltip("エネミーのスピード"), SerializeField] float _speed = 5;
    [SerializeField]
    Animator animator = null;
    [SerializeField, Min(0)]
    int maxHp = 0;
    [SerializeField]
    float attackDistance = 2;
    [SerializeField]
    Transform target = null;
    [SerializeField]
    int stopingDistansc = 0;
    [SerializeField] private int CurrentHp;
    bool isAttacking = false;
    Transform player;
    Transform thisTransform;
    Transform defaultTarget;

    public void ReceiveDamage(int damage)//インターフェースで使えるメソッドを定義
    {
        CurrentHp -= damage;
        CurrentHp = CurrentHp - damage;
        // _HpSlider.value = (float)CurrentHp / (float)maxHp;
        animator.SetTrigger("Hit");
    }
    // Start is called before the first frame update
    void Start()
    {
        thisTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        SetUp();
    }
    void SetUp()
    {
        ///Rigidbodyをつける
        if (!TryGetComponent(out _rb))
        {
            _rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
