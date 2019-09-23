
public class Weapon
{
    public string mName;
    public float mDamage;
    public float mROF;
    public float mRange;
    public float mReloadSpeed;
    public int mAmmoCount;
    public int mMaxAmmo;
    public bool mIsProjectile;

    public Weapon() {
        mAmmoCount = mMaxAmmo;
    }
}
