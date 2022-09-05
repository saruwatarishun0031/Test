using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class EnemyBoss : MonoBehaviour, IInterface//インターフェースを継承する
{
    [Tooltip("プレイヤーのポジション"), SerializeField] Transform _playerTransform;
    [Tooltip("追いかける範囲"), SerializeField] float _chaseDistance;
    Rigidbody _rb;
    [Tooltip("エネミーのスピード"), SerializeField] float _speed = 5;
    [SerializeField]
    Animator animator = null;
    [SerializeField, Min(0)]int maxHp = 0;
    [SerializeField]float attackDistance = 2;
    [SerializeField]Transform target = null;
    [SerializeField]int stopingDistansc = 0;
    [SerializeField] private int CurrentHp;
    [SerializeField] BoxCollider attack;
    [SerializeField] float timeOut;
    [SerializeField] Quaternion _quaternion;
    [SerializeField] private Slider _HpSlider;
    [SerializeField] BoxCollider attack2;
    [SerializeField] GameObject Go;
    [SerializeField] GameObject Goo;
    bool isAttacking = false;
    Transform player;
    Transform thisTransform;
    Transform defaultTarget;
    public float speed = 10.0f;
    public float xRange = 10;
    public GameObject projectilePrefab;
    int i = 0;
    protected Vector3 forward;
    protected Rigidbody rb;
    protected GameObject characterObject;
    private GameObject attPrefab;

    public void ReceiveDamage(int damage)//インターフェースで使えるメソッドを定義
    {
        CurrentHp -= damage;
        CurrentHp = CurrentHp - damage;
        _HpSlider.value = (float)CurrentHp / (float)maxHp;
        animator.SetTrigger("Hit");
    }

    void Start()
    {
        thisTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        SetUp();
        StartCoroutine(Randam());
    }

    private IEnumerator Randam()
    {     
        while (true)
        {
            i = Random.Range(0, 10);
            yield return new WaitForSeconds(timeOut);

        }
        
    }

    /// <summary>
    /// セットアップ
    /// </summary>
    void SetUp()
    {
        ///Rigidbodyをつける
        if (!TryGetComponent(out _rb))
        {
            _rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    void Update()
    {
        Anim();
        EnemyChase();
    }

    /// <summary>
    /// エネミーが追いかける範囲内にいるかどうか
    /// </summary>
    /// <returns></returns>
    bool IsEnemyChase()
    {
        float dis = Vector3.Distance(transform.position, _playerTransform.position);
        // Debug.Log(dis);

        if (dis < _chaseDistance && dis > stopingDistansc)
        {
            return true;
        }
        else if (dis < stopingDistansc)
        {
            return false;
        }

        return false;
    }

    /// <summary>
    /// 追従処理
    /// </summary>
    void EnemyChase()
    {
        if (IsEnemyChase())
        {
            Vector3 vec = new Vector3(_playerTransform.position.x, transform.position.y, _playerTransform.position.z);
            transform.LookAt(vec);

            _rb.velocity = transform.forward * _speed;
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }

    }

    void InitEnemy()
    {
        //Hp = maxHp;
    }

   void Anim()
    {
        //Debug.Log("walk");
        animator.SetBool("walk", true);
        float diff = (player.position - thisTransform.position).sqrMagnitude;
        // Debug.Log(diff);
        // 距離を比較。比較対象も二乗するのを忘れずに
        if (diff < attackDistance * attackDistance)
        {
            
            if (!isAttacking)
            {
                
                Debug.Log(i);
                if (i == 1)
                {
                    animator.SetTrigger("Attack");
                }
                else if (i == 2 || i == 8)
                {
                    animator.SetTrigger("touteki");
                    //Debug.Log("2");
                }
                else if (i == 0 || i == 3 || i == 9)
                {
                    animator.SetTrigger("kyoukougeki");
                    //Debug.Log("sss");
                }
                else if (i == 4 || i == 5 || i == 6 || i == 7)
                {
                    animator.SetTrigger("kaitenn");
                }
            }
        }
        else if (diff < _chaseDistance * _chaseDistance)
        {
            target = player;
            //animator.SetBool("Attack", false);
        }
        else
        {
            target = defaultTarget;
        }
        if (CurrentHp <= 0)
        {
            animator.SetBool("Daeth", true);
            Destroy(this.gameObject, 1.7f);
            Destroy(Go, 1.7f);
            Destroy(Goo, 1.7f);
        }
        return;
    }

    private void touteki()
    {
        GameObject Spawnobject = Instantiate(projectilePrefab, transform.position, transform.rotation);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        Destroy(Spawnobject, 7f);
    }

    private void Attack()
    {
        StartCoroutine("Randam");
        attack.enabled = true;
        Debug.Log("ta");
    }
    private void AttackAttack()
    {
        attack.enabled = false;
        Debug.Log("zi");
    }

    private void kaiten()
    {
        attack.enabled = true;
        attack2.enabled = true;
    }
    private void kaitenn()
    {
        attack.enabled = false;
        attack2.enabled = false;
    }
    static void Main(string[] args)
    {
        string[] stringArray = { "tuiseki", "kougeki", "touteki", "dennzyarasu",  };
        stringArray.Where(i => i.Length > 11).ToList().ForEach(i => Console.WriteLine(i));
    }
}
