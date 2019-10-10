using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiGun : Weapon
{

    public ConfettiGun() {
        mName = "Confetti Gun";
        mDamage = 0.1f;
        mIsParticle = true;
        mMaxAmmo = 300;
        mAmmoHeld = 300;
        mAmmoLoaded = 150;
        mMaxAmmoLoaded = 150;
        mROF = 2f;
        mFire_Type = Fire_Type.Beam;
        mReloadSpeed = 2.0f;
    }

}
