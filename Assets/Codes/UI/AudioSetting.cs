using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [Header("[ BGM ]")]
    public Slider BGMSlider;
    public GameObject BGMOn;
    public GameObject BGMOff;
    public Image BGMHandle;

    [Header("[ SFX ]")]
    public Slider SFXSlider;
    public GameObject SFXOn;
    public GameObject SFXOff;
    public Image SFXHandle;

    [Header("[ Handle Sprite ]")]
    public Sprite HandleOnSprite;
    public Sprite HandleOffSprite;


    void Awake()
    {
        if (PlayerPrefs.GetFloat("BgmVolume", -1) == -1) PlayerPrefs.SetFloat("BgmVolume", 0.5f);
        if (PlayerPrefs.GetFloat("SfxVolume", -1) == -1) PlayerPrefs.SetFloat("SfxVolume", 0.5f);
    }

    void Start()
    {
        float bgm = PlayerPrefs.GetFloat("BgmVolume");
        float sfx = PlayerPrefs.GetFloat("SfxVolume");

        BGMSlider.value = bgm;
        SFXSlider.value = sfx;

        SetBgm(bgm);
        SetSfx(sfx);
    }

    public void SetBgm(float bgmV)
    {
        if (PlayerPrefs.GetFloat("BgmVolume") != bgmV)
            PlayerPrefs.SetFloat("BgmVolume", bgmV);

        if (bgmV == 0)
        {
            BGMOn.SetActive(false);
            BGMOff.SetActive(true);

            BGMHandle.sprite = HandleOffSprite;
        }
        else
        {
            BGMOn.SetActive(true);
            BGMOff.SetActive(false);

            BGMHandle.sprite = HandleOnSprite;
        }
    }

    public void SetSfx(float sfxV)
    {
        if (PlayerPrefs.GetFloat("SfxVolume") != sfxV)
            PlayerPrefs.SetFloat("SfxVolume", sfxV);

        if (sfxV == 0)
        {
            SFXOn.SetActive(false);
            SFXOff.SetActive(true);

            SFXHandle.sprite = HandleOffSprite;
        }
        else
        {
            SFXOn.SetActive(true);
            SFXOff.SetActive(false);

            SFXHandle.sprite = HandleOnSprite;
        }
    }
}
