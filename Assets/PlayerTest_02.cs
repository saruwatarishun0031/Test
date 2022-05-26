using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest_02 : MonoBehaviour
{
    private float x;
    private float z;
    public float Speed = 1.0f;
    float smooth = 10f;
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();   //アニメーションを取得する
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //x, z 平面での移動
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        Vector3 target_dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.velocity = new Vector3(x, 0, z) * Speed;         //歩く速度
        animator.SetFloat("Walk", rb.velocity.magnitude);   //歩くアニメーションに切り替える


        // Y 軸方向の速度を保ちながら、速度ベクトルを求めてセットする
        Vector3 velocity = target_dir.normalized * _moveSpeed;

        if (target_dir.magnitude > 0.1)
        {
            //キーを押し方向転換
            Quaternion rotation = Quaternion.LookRotation(target_dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smooth);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");    //マウスクリックで攻撃モーション
        }
    }
}