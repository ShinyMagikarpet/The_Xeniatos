using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper_Projectile : Projectile{
    public Sniper_Projectile(){
        mName = "Bubble";
        mSpeed = 1000.0f;
        mTTL = 1.7f;
        mDamage = 50.0f;
    }
}
