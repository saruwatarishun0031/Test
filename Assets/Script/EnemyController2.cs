using UnityEngine;

/// <summary>
/// エネミーのコンポーネント
/// </summary>
public class EnemyController2 : MonoBehaviour, IInterface//インターフェースを継承する
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

    void Start()
    {
        thisTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        SetUp();
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
                animator.SetBool("Attack", true);
               // Debug.Log("sss");
            }
        }
        else if (diff < _chaseDistance * _chaseDistance)
        {
            target = player;
            animator.SetBool("Attack", false);
        }
        else
        {
            target = defaultTarget;
        }
        if (CurrentHp <= 0)
        {
            animator.SetBool("Daeth", true);
            Destroy(this.gameObject, 1.7f);
        }

    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            IInterface iif = other.gameObject.GetComponent<IInterface>();
            if (iif != null)
            {

               // Debug.Log("haitta");
                iif.ReceiveDamage(3);
            }
        }
    }
}