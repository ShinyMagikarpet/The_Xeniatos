

public class HandGun : Weapon
{

    public HandGun() {

        mName = "HandGun";
        mMaxAmmo = 20;
        mAmmoCurrent = mMaxAmmo;
        mDamage = 12.5f;
        mRange = 200.0f;
        mReloadSpeed = 2.5f;
        mROF = 0.7f;
        mIsProjectile = false;

    }

    
    
}
