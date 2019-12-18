using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeebTrapDisplay : MonoBehaviour
{
    [SerializeField] private Camera mEffectCamera;
    [SerializeField] private Shader[] mTrapShaders;

    private void OnEnable() {
        foreach(Shader shader in mTrapShaders) {
            mEffectCamera.SetReplacementShader(shader, "WeebTrap");
        }
    }
}
