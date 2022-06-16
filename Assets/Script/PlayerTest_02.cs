using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTest_02 : MonoBehaviour,IInterface//インターフェースを継承する
{
    //interfaceで定義したメソッドをすべて定義する必要がある
    public void ReceiveDamage(int damage)//インターフェースで使えるメソッドを定義
    {
        if(isGuard)
        {
            damage = 0;
        }
        CurrentHp -= damage;
        CurrentHp = CurrentHp - damage;
        _HpSlider.value = (float)CurrentHp / (float)maxHp;
        animator.SetTrigger("Hit");
    }

    [SerializeField] private Slider _HpSlider;
    private int MaxHp;
    [SerializeField]private int CurrentHp;
    private float x;
    private float z;
    private float y;
    public float Speed = 1.0f;
    public float _moveSpeed = 1.0f;
    float smooth = 10f;
    private Rigidbody _rb;
    private Animator animator;
    private Rigidbody dir;
    [SerializeField]
    int maxHp;
    [SerializeField] BoxCollider guard;
    [SerializeField] BoxCollider attack;
    private bool isGuard;




    void Start()
    {
        animator = GetComponent<Animator>();   //アニメーションを取得する
        _rb = GetComponent<Rigidbody>();
        CurrentHp = maxHp;
    }

    void Update()
    {
        PlayerInput();
        Move();
        Anim();
    }

    void PlayerInput()
    {
        //x, z 平面での移動
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        Vector3 target_dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _rb.velocity = new Vector3(x, _rb.velocity.y, z) * Speed;         //歩く速度
        //dir.y = 0;

        // Y 軸方向の速度を保ちながら、速度ベクトルを求めてセットする
        Vector3 velocity = target_dir.normalized * _moveSpeed;
        velocity.y = _rb.velocity.y;
        _rb.velocity = velocity;

        if (target_dir.magnitude > 0.1)
        {
            //キーを押し方向転換
            Quaternion rotation = Quaternion.LookRotation(target_dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smooth);
        }
    }

    void Anim()
    {
        animator.SetFloat("Walk", _rb.velocity.magnitude);   //歩くアニメーションに切り替える


        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");    //マウスクリックで攻撃モーション
        }
        if(Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("Guard");
            isGuard = true;
        }
        if(CurrentHp <= 0)
        {
            animator.SetBool("Daeth", true);
        }
    }

    private void Guard()
    {
        guard.enabled = true;
        Debug.Log("da");
    }
    private void GuardGuard()
    {
        isGuard = false;
        guard.enabled = false;
        Debug.Log("bi");
    }

    private void Attack()
    {
        attack.enabled = true;
        Debug.Log("ta");
    }
    private void AttackAttack()
    {
        attack.enabled = false;
        Debug.Log("zi");
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Enemy")
    //    {
    //        IInterface iif = other.gameObject.GetComponent<IInterface>();
    //        if (iif != null)
    //        {

    //            Debug.Log("haitta");
    //            iif.ReceiveDamage(3);
    //        }
    //    }
    //}
}
