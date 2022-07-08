using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    [SerializeField] ActivateTiming _whenActivated = ActivateTiming.Get;
    public abstract void Activate();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            //if (_sound)
            //{
            //    AudioSource.PlayClipAtPoint(_sound, Camera.main.transform.position);
            //}

            // アイテム発動タイミングによって処理を分ける
            if (_whenActivated == ActivateTiming.Get)
            {
                Activate();
                Destroy(this.gameObject);
            }
            else if (_whenActivated == ActivateTiming.Use)
            {
                // 見えない所に移動する
                this.transform.position = Camera.main.transform.position;
                // コライダーを無効にする
                GetComponent<Collider>().enabled = false;
                // プレイヤーにアイテムを渡す
                collision.gameObject.GetComponent<PlayerTest_02>().GetItem(this);
            }
        }
    }

    enum ActivateTiming
    {
        /// <summary>取った時にすぐ使う</summary>
        Get,
        /// <summary>「使う」コマンドで使う</summary>
        Use,
    }
}
