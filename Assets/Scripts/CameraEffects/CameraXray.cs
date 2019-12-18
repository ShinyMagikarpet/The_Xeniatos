using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraXray : MonoBehaviour{

    public Shader shader;

    private void OnEnable() {
        GetComponent<Camera>().SetReplacementShader(shader, "Xray");
    }

    private void OnDisable() {
        GetComponent<Camera>().ResetReplacementShader();
    }
}
