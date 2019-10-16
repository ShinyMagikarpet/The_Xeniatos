using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public abstract class ResourceNode : MonoBehaviourPunCallbacks
{
    public enum ResourceSize {
        small,
        medium,
        large,
        huge
    }

    protected string mName;
    [SerializeField]
    protected ResourceSize mSize;
    [SerializeField]
    protected int mResourceCount;
    [SerializeField]
    protected PhotonView mPV;

    private void Start() {
        mPV = GetComponent<PhotonView>();
        Set_Resources();
    }

    private void Update() {

        if(mResourceCount <= 0) {
            this.enabled = false;
        }
    }

    public string Get_Name(){
        return mName;
    }

    public int Get_Resource_Count() {
        return mResourceCount;
    }

    public int Give_Resource() {

        int amount;
        if (mResourceCount >= 3) {
            amount = Random.Range(1, 4);
        }
        else if(mResourceCount == 2) {
            amount = Random.Range(1, 3);
        } 
        else if(mResourceCount == 1) {
            amount = 1;
        } 
        else {
            amount = 0;
        }

        mPV.RPC("Remove_Resource_Count", RpcTarget.All, amount);
        return amount;
    }

    [PunRPC]
    protected void Remove_Resource_Count(int amount) {
        mResourceCount -= amount;
    }

    private void Set_Resources() {

        switch (mSize) {

            case ResourceSize.small:
                mResourceCount = 100;
                break;
            case ResourceSize.medium:
                mResourceCount = 150;
                transform.localScale = new Vector3(transform.localScale.x + 0.5f, transform.localScale.y + 0.5f, transform.localScale.z + 0.5f);
                break;
            case ResourceSize.large:
                mResourceCount = 200;
                transform.localScale = new Vector3(transform.localScale.x + 1f, transform.localScale.y + 1f, transform.localScale.z + 1f);
                break;
            case ResourceSize.huge:
                mResourceCount = 300;
                transform.localScale = new Vector3(transform.localScale.x + 1.5f, transform.localScale.y + 1.5f, transform.localScale.z + 1.5f);
                break;
            default:
                mResourceCount = 150;
                transform.localScale = new Vector3(transform.localScale.x + 0.5f, transform.localScale.y + 0.5f, transform.localScale.z + 0.5f);
                break;
        }
            

    }
}
