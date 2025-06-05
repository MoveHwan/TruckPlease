using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyAudioManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // Slider 값이 변경될 때 호출될 메서드 연결
        bgmSlider.onValueChanged.AddListener(AudioManager.instance.UpdateBgmVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.instance.UpdateSfxVolume);
        AudioManager.instance.PlayBGMLobby();
    }

    // 브금 수치 조정하기
    public void SetBgmVolume()
    {
        AudioManager.instance.bgmVolume = bgmSlider.value;
        PlayerPrefs.SetFloat("BgmVol", AudioManager.instance.bgmVolume);
        PlayerPrefs.SetInt("isEditBgmVolume", 1);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume()
    {
        AudioManager.instance.sfxVolume = sfxSlider.value;
        PlayerPrefs.SetFloat("SfxVol", AudioManager.instance.sfxVolume);
        PlayerPrefs.SetInt("isEditSfxVolume", 1);
        PlayerPrefs.Save();
    }
}
