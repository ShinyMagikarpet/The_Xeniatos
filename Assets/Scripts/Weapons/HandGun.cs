

public class HandGun : Weapon
{

    public HandGun() {

        mName = "HandGun";
        mMaxAmmo = 80;
        mAmmoHeld = mMaxAmmo;
        mMaxAmmoLoaded = 20;
        mAmmoLoaded = mMaxAmmoLoaded;
        mDamage = 12.5f;
        mRange = 200.0f;
        mReloadSpeed = 1.7f;
        mROF = 0.15f;
        mIsProjectile = false;
        mFire_Type = Fire_Type.single;

    }

    
    
}
