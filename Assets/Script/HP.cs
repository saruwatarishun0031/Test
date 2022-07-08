using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP :Item
{
    [SerializeField] int _recoverHP;
    public override void Activate()
    {
        FindObjectOfType<PlayerTest_02>().AddHP(_recoverHP);
    }
}
