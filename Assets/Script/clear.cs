using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clear : MonoBehaviour
{
    private Animator animator;
    [SerializeField] GameObject Go;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Playeruepon")
        {
            animator.SetBool("aku",true);
            Go.SetActive(true);
        }
    }
}
