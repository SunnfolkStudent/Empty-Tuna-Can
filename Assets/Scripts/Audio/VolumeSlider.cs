using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum volumeType
    {
        MASTER,
        MUSIC,
        SFX
    }

    [Header("Type")] 
    [SerializeField] private volumeType _volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch (_volumeType)
        {
            case volumeType.MASTER:
                volumeSlider.value = AudioManager.Instance.masterVolume;
                break;
            case volumeType.SFX:
                volumeSlider.value = AudioManager.Instance.sfxVolume;
                break;
            case volumeType.MUSIC:
                volumeSlider.value = AudioManager.Instance.musicVolume;
                break;
        }
    }

    public void OnSliderValueChanged()
    {
        switch (_volumeType)
        {
            case volumeType.MASTER:
                AudioManager.Instance.masterVolume = volumeSlider.value;
                break;
            case volumeType.SFX:
                AudioManager.Instance.sfxVolume = volumeSlider.value;
                break;
            case volumeType.MUSIC:
                AudioManager.Instance.musicVolume = volumeSlider.value;
                break;
        }
    }
}
