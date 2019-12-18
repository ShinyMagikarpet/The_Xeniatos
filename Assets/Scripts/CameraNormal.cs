using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraNormal : MonoBehaviour{

    public Shader shader;
    float rotation = 0f;

    private void OnEnable() {
        GetComponent<Camera>().SetReplacementShader(shader, "Xray");
    }

    private void OnDisable() {
        GetComponent<Camera>().ResetReplacementShader();
    }
}
