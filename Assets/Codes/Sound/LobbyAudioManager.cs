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
        // Slider ���� ����� �� ȣ��� �޼��� ����
        bgmSlider.onValueChanged.AddListener(AudioManager.instance.UpdateBgmVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.instance.UpdateSfxVolume);
        AudioManager.instance.PlayBGMLobby();
    }

    // ��� ��ġ �����ϱ�
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
