using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{

    public AudioSource song;

    private void Start() {
        //song.volume = 0.7f;
    }

    public void Adjust_Volume(Slider slider) {
        song.volume = slider.value;
    }
}
