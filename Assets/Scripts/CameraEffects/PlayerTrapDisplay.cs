using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class PlayerTrapDisplay : MonoBehaviour
{
    [SerializeField] private Camera mEffectCamera;
    [SerializeField] private Shader[] mTrapShaders;

    private void OnEnable() {
        //foreach (Shader shader in mTrapShaders) {
        //    mEffectCamera.SetReplacementShader(shader, "RenderType");
        //}
        mEffectCamera.SetReplacementShader(mTrapShaders[0], "WeebTrap");
    }
}
