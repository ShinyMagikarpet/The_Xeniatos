using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCycle : MonoBehaviour
{
    private GameObject sun;
    public Gradient sunColor;
    public float sunDirection;
    public float totalSunRotation;
    public bool isGameScene;
    public float totalTime;
    public float xDir;
    public float yDir;
    public float zDir;
    void Awake(){

        sun = this.gameObject;
        sun.transform.rotation = Quaternion.Euler(sunDirection, 0, 0);
    }

    // Update is called once per frame
    void Update(){

        if (isGameScene) {

            if (GameManager.Instance.Get_Timer() > 0.0f) {


                float gameTime = GameManager.Instance.Get_Match_Time();

                sun.transform.Rotate((totalSunRotation / gameTime) * Time.deltaTime, 0, 0);

            }
            else {
                this.enabled = false;
            }
        }
        else {
            

            sun.transform.Rotate((totalSunRotation / totalTime) * Time.deltaTime * xDir, (totalSunRotation / totalTime) * Time.deltaTime * yDir, (totalSunRotation / totalTime) * Time.deltaTime * zDir);

        }


    }
}
