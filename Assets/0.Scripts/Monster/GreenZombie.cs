using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenZombie : Monster
{
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        data.Speed = 1f;
        data.Power = 5;
        data.HP = 20;
        data.AttDelay = 1f;

        base.Init();
    }
}
