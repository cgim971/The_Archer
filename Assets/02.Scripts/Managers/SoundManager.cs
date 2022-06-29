using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public AudioSource _bgmAudioSource;
    public AudioClip _bgmClip;
    public Slider _bgmSlider;

    private void Start()
    {
        _bgmAudioSource.clip = _bgmClip;
        _bgmSlider.value = GameManager.Instance._PLAYERSAVE._bgmVolume;
    }
    private void Update()
    {
        _bgmAudioSource.volume = _bgmSlider.value;
        GameManager.Instance._PLAYERSAVE._bgmVolume = _bgmSlider.value;
    }


}
