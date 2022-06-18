using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTest_02 : MonoBehaviour, IInterface//インターフェースを継承する
{
    //interfaceで定義したメソッドをすべて定義する必要がある
    public void ReceiveDamage(int damage)//インターフェースで使えるメソッドを定義
    {
        if (isGuard)
        {
            damage = 0;
        }
        CurrentHp -= damage;
        CurrentHp = CurrentHp - damage;
        _HpSlider.value = (float)CurrentHp / (float)maxHp;
        animator.SetTrigger("Hit");
    }

    [SerializeField] private Slider _HpSlider;
    [SerializeField] private Slider _MPSlider;
    [SerializeField] int maxHp;
    [SerializeField] int maxMP;
    [SerializeField] BoxCollider guard;
    [SerializeField] BoxCollider attack;
    [SerializeField] private int CurrentHp;
    [SerializeField] private float CurrentMP;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Quaternion _quaternion;
    private Rigidbody _rb;
    private Animator animator;
    private Rigidbody dir;
    private float x;
    private float z;
    private float y;
    private bool isGuard;
    public float Speed = 1.0f;
    public float _moveSpeed = 1.0f;
    public float speed = 10.0f;
    float smooth = 10f;
    protected Vector3 forward;
    [SerializeField] Rigidbody rb;
    protected GameObject characterObject;
    protected GameObject attPrefab;


    void Start()
    {
        animator = GetComponent<Animator>();   //アニメーションを取得する
        _rb = GetComponent<Rigidbody>();
        CurrentHp = maxHp;
        CurrentMP = maxMP;
    }

    void Update()
    {
        PlayerInput();
        Move();
        Anim();

        if (CurrentMP > 100)
        {
            _MPSlider.value = CurrentMP / (float)maxMP;
        }
        else
        {
            CurrentMP += Time.deltaTime;
            _MPSlider.value = CurrentMP / (float)maxMP;
        }
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
        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("Guard");
            isGuard = true;
        }
        //if (Input.GetKeyDown("KeyCode.Space"))
        if (Input.GetKeyDown(KeyCode.Space) && CurrentMP >= 20)
        {
            Debug.Log("oo");
            animator.SetTrigger("MP");
            CurrentMP = CurrentMP - 20;
            _MPSlider.value = (float)CurrentMP / (float)maxHp;
        }
        if (CurrentHp <= 0)
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

    private void Fireball()
    {
        GameObject Spawnobject =  Instantiate(projectilePrefab, transform.position, transform.rotation);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        Destroy(Spawnobject, 2f);
    }
}
