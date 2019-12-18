using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFrozen : MonoBehaviour
{
    public Shader shader;

    private void OnEnable() {
        GetComponent<Camera>().SetReplacementShader(shader, "IceTrap");
    }

    private void OnDisable() {
        GetComponent<Camera>().ResetReplacementShader();
    }
}
