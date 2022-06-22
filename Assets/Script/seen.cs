using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class seen : MonoBehaviour
{

    [SerializeField] GameObject a;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Roou()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void setumei()
    {
        a.SetActive(true);
    }

    public void toziru()
    {
        a.SetActive(false);
    }
}
