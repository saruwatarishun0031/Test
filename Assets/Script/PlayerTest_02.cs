﻿using DG.Tweening;
using System;
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
        //_HpSlider.= (float)CurrentHp / (float)maxHp;

        // DOTween.To() を使って連続的に変化させる
        DOTween.To(() => _HpSlider.value, // 連続的に変化させる対象の値
            x => _HpSlider.value = x, // 変化させた値 x をどう処理するかを書く
            (float)CurrentHp / (float)maxHp, // x をどの値まで変化させるか指示する
            3f);   // 何秒かけて変化させるか指示する

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
    [SerializeField] CapsuleCollider eee;
    [SerializeField] CapsuleCollider ee;
    [SerializeField] int XYspeed;
    [SerializeField] GameObject MPSword;
    [SerializeField] GameObject Sword;
    [SerializeField] GameObject Go;
    private Rigidbody _rb;
    private Animator animator;

    public void GetItem(Item item)
    {
        _itemList.Add(item);
    }

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

    List<Item> _itemList = new List<Item>();


    void Start()
    {
        animator = GetComponent<Animator>();   //アニメーションを取得する
        _rb = GetComponent<Rigidbody>();
        CurrentHp = maxHp;
        CurrentMP = maxMP;
        AddHP(0);
        AddMP(0);
    }

    void Update()
    {
        //PlayerInput();
         
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

        float mousex = Input.GetAxis("Mouse X");
        //float mausu y = Input.GetAxis("Mouse Y");
        transform.RotateAround(transform.position, transform.up, mousex);


        // アイテムを使う
        if (Input.GetButtonDown("Fire1"))
        {
            if (_itemList.Count > 0)
            {
                // リストの先頭にあるアイテムを使って、破棄する
                Item item = _itemList[0];
                _itemList.RemoveAt(0);
                item.Activate();
                Destroy(item.gameObject);
            }
        }
    }

    //void PlayerInput()
    //{
    //    //x, z 平面での移動
    //    x = Input.GetAxisRaw("Horizontal");
    //    z = Input.GetAxisRaw("Vertical");
    //}

    private void FixedUpdate()
    {


        // 上下左右キーを入力して得た値にspeedをかけた値をAddForceに設定して移動させる
        //（ForceMode.Impulseは瞬間的に力を加えるということ）

        //direction *= speed * 5;
        //rb.AddForce(x * speed, 0, z * speed, ForceMode.Impulse);

        //rb.AddForce(direction, ForceMode.Impulse);
        Move();
    }
    
    void Move()
    {
        Vector3 target_dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        /*_rb.velocity = new Vector3(x, _rb.velocity.y, z) * Speed;         //歩く速度
        //dir.y = 0;

        // Y 軸方向の速度を保ちながら、速度ベクトルを求めてセットする
        Vector3 velocity = target_dir.normalized * _moveSpeed;
        velocity.y = _rb.velocity.y;
        _rb.velocity = velocity;*/

        // 左右のキーの入力を取得
        float x = Input.GetAxis("Horizontal");
        // 上下のキーの入力を取得
        float z = Input.GetAxis("Vertical");
        Debug.DrawRay(rb.position, rb.transform.forward * 10, Color.red);
        Vector3 direction = rb.transform.forward * z  + gameObject.transform.right * x;
        //direction.Normalize();
        direction *= XYspeed * Time.deltaTime;
        rb.velocity = direction;


        return;//外して
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


        //if(Input.GetButton("W") && Input.GetButton("D"))s
        //{
        //    Debug.Log("ttt");
        //    animator.SetBool("Ran",true);
        //}
        //if (Input.GetButton("W") && Input.GetButton("A"))
        //{
        //    Debug.Log("tat");
        //    animator.SetBool("Ran", true);
        //}
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
            //_MPSlider.value = (float)CurrentMP / (float)maxHp;
            // DOTween.To() を使って連続的に変化させる
            DOTween.To(() => _MPSlider.value, // 連続的に変化させる対象の値
                x => _MPSlider.value = x, // 変化させた値 x をどう処理するかを書く
                (float)CurrentMP / (float)maxHp, // x をどの値まで変化させるか指示する
                3f);   // 何秒かけて変化させるか指示する
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && CurrentMP >= 50)
        {
            Debug.Log("ooo");
            animator.SetTrigger("MPSword");
            CurrentMP = CurrentMP - 50;
            //_MPSlider.value = (float)CurrentMP / (float)maxHp;
            // DOTween.To() を使って連続的に変化させる
            DOTween.To(() => _MPSlider.value, // 連続的に変化させる対象の値
                x => _MPSlider.value = x, // 変化させた値 x をどう処理するかを書く
                (float)CurrentMP / (float)maxHp, // x をどの値まで変化させるか指示する
                3f);   // 何秒かけて変化させるか指示する
        }
        if (CurrentHp <= 0)
        {
            animator.SetBool("Daeth", true);
            Go.SetActive(true);
        }
    }

    private void Guard()
    {
        guard.enabled = true;
        //ee.enabled = false;
        //eee.enabled = false;
        MPSword.SetActive(false);
        Sword.SetActive(true);
        Debug.Log("da");
    }
    private void GuardGuard()
    {
        isGuard = false;
        guard.enabled = false;
        //ee.enabled = true;
        //eee.enabled = true;
        MPSword.SetActive(false);
        Sword.SetActive(true);
        Debug.Log("bi");
    }

    private void Attack()
    {
        attack.enabled = true;
        guard.enabled = false;
        MPSword.SetActive(false);
        Sword.SetActive(true);
        Debug.Log("ta");
    }
    private void AttackAttack() 
    {
        attack.enabled = false;
        guard.enabled = false;
        MPSword.SetActive(false);
        Sword.SetActive(true);
        Debug.Log("zi");
    }

    private void Fireball()
    {
        GameObject Spawnobject =  Instantiate(projectilePrefab, transform.position, transform.rotation);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        guard.enabled = false;
        MPSword.SetActive(false);
        Sword.SetActive(true);
        Destroy(Spawnobject, 2f);
    }

    private void Hissatuwaza()
    {
        MPSword.SetActive(true);
        Sword.SetActive(false);
        guard.enabled = false;
    }
    private void Hissatuwazaout()
    {
        MPSword.SetActive(false);
        Sword.SetActive(true);
        guard.enabled = false;
    }

    internal void AddHP(int HP)
    {
        CurrentHp += HP;
    }

    internal void AddMP(float MP)
    {
        CurrentMP += MP;
    }
}
