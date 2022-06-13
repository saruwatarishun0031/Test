using System.Collections;
using UnityEngine;
using UnityEngine.AI;  // ★変更1
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    Animator animator = null;
    [SerializeField]
    NavMeshAgent navmeshAgent = null;   // ★変更1
    [SerializeField]
    Transform target = null;   // ★変更1
    [SerializeField]
    CapsuleCollider capsuleCollider = null;
    [SerializeField, Min(0)]
    int maxHp = 0;
    [SerializeField]
    float deadWaitTime = 3;
    // ★変更２
    [SerializeField]
    float chaseDistance = 5;
    [SerializeField]
    Collider attackCollider = null;
    [SerializeField]
    int attackPower = 10;
    [SerializeField]
    float attackTime = 0.5f;
    [SerializeField]
    float attackInterval = 2;
    [SerializeField]
    float attackDistance = 2;
    // アニメーターのパラメーターのIDを取得（高速化のため）
    readonly int SpeedHash = Animator.StringToHash("Speed");
    readonly int AttackHash = Animator.StringToHash("Attack");
    readonly int DeadHash = Animator.StringToHash("Dead");
    bool isDead = false;
    int hp = 0;
    Transform thisTransform;
    // ★変更２
    bool isAttacking = false;
    Transform player;
    Transform defaultTarget;
    WaitForSeconds attackWait;
    WaitForSeconds attackIntervalWait;

    // オブジェクトの移動速度を格納する変数
    public float moveSpeed;
    // オブジェクトが停止するターゲットオブジェクトとの距離を格納する変数
    public float stopDistance;
    // オブジェクトがターゲットに向かって移動を開始する距離を格納する変数
    public float moveDistance;
    public int Hp
    {
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);
        }
        get
        {
            return hp;
        }
    }
    void Start()
    {
        thisTransform = transform;  // transformをキャッシュ（高速化）
                                    // ★変更2
        defaultTarget = target;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // ★変更2。WaitForSecondsをキャッシュして高速化
        attackWait = new WaitForSeconds(attackTime);
        attackIntervalWait = new WaitForSeconds(attackInterval);
        InitEnemy();
    }
    void Update()
    {
        
        if (isDead)
        {
            return;
        }
        CheckDistance();  // ★変更2
        Move();  // ★変更1
        UpdateAnimator();

        /* 変数 targetPos を作成してターゲットオブジェクトの座標を格納
        Vector3 targetPos = target.position;
        // 自分自身のY座標を変数 target のY座標に格納
        //（ターゲットオブジェクトのX、Z座標のみ参照）
        targetPos.y = transform.position.y;
        // オブジェクトを変数 targetPos の座標方向に向かせる
        transform.LookAt(targetPos);

        // 変数 distance を作成してオブジェクトの位置とターゲットオブジェクトの距離を格納
        float distance = Vector3.Distance(transform.position, target.position);
        // オブジェクトとターゲットオブジェクトの距離判定
        // 変数 distance（ターゲットオブジェクトとオブジェクトの距離）が変数 moveDistance の値より小さければ
        // さらに変数 distance が変数 stopDistance の値よりも大きい場合
        if (distance < moveDistance && distance > stopDistance)
        {
            Debug.Log(distance);
            // 変数 moveSpeed を乗算した速度でオブジェクトを前方向に移動する
            transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
        }*/
    }
    void InitEnemy()
    {
        Hp = maxHp;
    }
    // 被ダメージ処理
    public void Damage(int value)
    {
        if (value <= 0)
        {
            return;
        }
        Hp -= value;
        if (Hp <= 0)
        {
            Dead();
        }
    }
    // 死亡時の処理
    void Dead()
    {
        isDead = true;
        capsuleCollider.enabled = false;
        animator.SetBool(DeadHash, true);
        // ★変更2
        StopAttack();
        navmeshAgent.isStopped = true;
        StartCoroutine(nameof(DeadTimer));
    }
    // 死亡してから数秒間待つ処理
    IEnumerator DeadTimer()
    {
        yield return new WaitForSeconds(deadWaitTime);
        Destroy(gameObject);
    }
    // ★変更1
    void Move()
    {
        navmeshAgent.SetDestination(target.position);
    }
    // アニメーターのアップデート処理
    void UpdateAnimator()
    {
        // ★変更1
        animator.SetFloat(SpeedHash, navmeshAgent.desiredVelocity.magnitude);
    }
    // ★変更2 以下を追加

    void CheckDistance()
    {
        // プレイヤーまでの距離（二乗された値）を取得
        // sqrMagnitudeは平方根の計算を行わないので高速。距離を比較するだけならそちらを使った方が良い
        float diff = (player.position - thisTransform.position).sqrMagnitude;
       // Debug.Log(diff);
        // 距離を比較。比較対象も二乗するのを忘れずに
        if (diff < attackDistance * attackDistance)
        {
            
            if (!isAttacking)
            {
                StartCoroutine(nameof(Attack));
              
            }
        }
        else if (diff < chaseDistance * chaseDistance)
        {
            target = player;
        }
        else
        {
            target = defaultTarget;
        }
    }
    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger(AttackHash);
        attackCollider.enabled = true;
        yield return attackWait;
        attackCollider.enabled = false;
        yield return attackIntervalWait;
        isAttacking = false;
    }
    void StopAttack()
    {
        StopCoroutine(nameof(Attack));
        attackCollider.enabled = false;
        isAttacking = false;
    }
    /* private void OnTriggerEnter(Collider other)
     {
         if (other.gameObject.CompareTag("Player"))
         {
             FpsGunControler gun = other.gameObject.GetComponent<FpsGunControler>();
             if (gun != null)
             {
                 gun.CurrentAmmo -= attackPower;
             }
         }
     }*/
    //ダメージを与えるクラス内のメソッド
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            IInterface iif = other.gameObject.GetComponent<IInterface>();
            if (iif != null)
            {

                Debug.Log("haitta");
                iif.ReceiveDamage(3);
            }
        }
    }

}