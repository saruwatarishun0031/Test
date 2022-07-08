using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP : Item
{
    [SerializeField] int _recoverMP;
    public override void Activate()
    {
        FindObjectOfType<PlayerTest_02>().AddMP(_recoverMP);
    }
}
