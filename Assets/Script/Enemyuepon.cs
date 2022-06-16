using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyuepon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
