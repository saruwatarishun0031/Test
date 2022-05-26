using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SkeltonTest003 : MonoBehaviour
{
    private Animator animator;              //アニメーターを使う
    private NavMeshAgent agent;             //NavMeshAgentを使う
    public Transform target;                //ターゲットに設定できるようにする
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    //アニメーターを情報取得
        agent = GetComponent<NavMeshAgent>();   //NavMeshAgentを情報取得
        agent.destination = target.position;    //敵(agent)がプレーヤー(target)に向かう
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;    //プレーヤのほうに向かっていく
    }
}
