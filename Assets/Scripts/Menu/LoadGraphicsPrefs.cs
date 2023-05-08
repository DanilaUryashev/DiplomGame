using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LoadGraphicsPrefs : MonoBehaviour
{
    [Header("Resolution Setting"), SerializeField]
    private Dropdown resolutionDropdown;

    [Header("General Setting"), SerializeField]
    private bool canUse;

    [SerializeField]
    private GraphicSetting graphicSetting;

    [Header("Brightness"), SerializeField]
    private Slider brightnessSlider;

    [SerializeField]
    private TMP_Text brightnessValue;

    [Header("Quality level"), SerializeField]
    private Dropdown quailityDropdown;

    [Header("Shadow Resolution Setting"), SerializeField]
    private Dropdown quailityShadowDropdown;

    [Header("FullScreen setting"), SerializeField]
    private Toggle fullScreenToogle;

    [Header("V-Sync setting"), SerializeField]
    private Toggle vSyncToggle;

    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int @int = PlayerPrefs.GetInt("masterQuality");
                quailityDropdown.value = @int;
                QualitySettings.SetQualityLevel(@int);
            }
            if (PlayerPrefs.HasKey("masterVSync"))
            {
                if (PlayerPrefs.GetInt("masterFullscreen") == 1)
                {
                    QualitySettings.vSyncCount = 1;
                    fullScreenToogle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    fullScreenToogle.isOn = false;
                }
            }
            if (PlayerPrefs.HasKey("masterFullscreen"))
            {
                if (PlayerPrefs.GetInt("masterFullscreen") == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToogle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    fullScreenToogle.isOn = false;
                }
            }
            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float @float = PlayerPrefs.GetFloat("masterBrightness");
                brightnessValue.text = @float.ToString("0.0");
                brightnessSlider.value = @float;
            }
            if (PlayerPrefs.HasKey("masterShadowRes"))
            {
                PlayerPrefs.GetString("masterShadowRes");
                int int2 = PlayerPrefs.GetInt("masterShadowResValue");
                QualitySettings.shadowResolution = (ShadowResolution)Enum.Parse(typeof(ShadowResolution), PlayerPrefs.GetString("masterShadowRes", QualitySettings.shadowResolution.ToString()));
                quailityShadowDropdown.value = int2;
            }
            if (PlayerPrefs.HasKey("masterResolutionIndex") && PlayerPrefs.HasKey("masterResolutionW") && PlayerPrefs.HasKey("masterResolutionH"))
            {
                int int3 = PlayerPrefs.GetInt("masterResolutionH");
                int arg_1AA_0 = PlayerPrefs.GetInt("masterResolutionW");
                int int4 = PlayerPrefs.GetInt("masterResolutionIndex");
                resolutionDropdown.value = int4;
                Screen.SetResolution(arg_1AA_0, int3, Screen.fullScreen);
            }
        }
    }
}
