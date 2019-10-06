using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ResourceNode : MonoBehaviour
{
    protected string mName;
    protected int mResourceCount;

    public string Get_Name(){
        return mName;
    }

}
