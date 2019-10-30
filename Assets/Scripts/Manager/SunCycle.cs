using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCycle : MonoBehaviour
{
    private GameObject sun;
    public Gradient sunColor;
    public float sunDirection;
    public float totalSunRotation;
    void Start(){

        sun = this.gameObject;
        sun.transform.rotation = Quaternion.Euler(sunDirection, 0, 0);
    }

    // Update is called once per frame
    void Update(){

        if(GameManager.Instance.Get_Timer() > 0.0f) {


                float gameTime = GameManager.Instance.Get_Match_Time();

                sun.transform.Rotate((totalSunRotation / gameTime) * Time.deltaTime, 0, 0);

        }
        else {
            this.enabled = false;
        }


    }
}
