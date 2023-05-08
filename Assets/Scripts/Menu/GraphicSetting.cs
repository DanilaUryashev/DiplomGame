using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicSetting : MonoBehaviour
{
    [Header("Wainting image completion"), SerializeField]
    private GameObject ConformationPromt;

    [Header("Graphics settings"), SerializeField]
    private Dropdown qualityDropdown;

    [Header("Brightness"), SerializeField]
    private Slider brightnessSlider;

    [SerializeField]
    private TMP_Text brightnessValue;

    [SerializeField]
    private float defaultBrightness = 0.5f;

    [Header("V-Sync"), SerializeField]
    private Toggle vSyncToggle;

    [Header("Resolution Dropdown")]
    public Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    [Header("Shadow Resolution Dropdown")]
    public Dropdown shadowResDropdown;

    private string[] ShadowRes;

    private int _shadowResIndex;

    private int _qualityLevel;

    private bool _isFullscreen = true;

    private bool _isVSync;

    private float _brightnessLevel;

    private int _resolutionH;

    private int _resolutionW;

    private int currentResolutionIndex;

    private void Start()
    {
        this.resolutions = Screen.resolutions;
        this.resolutionDropdown.ClearOptions();
        List<string> list = new List<string>();
        int value = 0;
        for (int i = 0; i < this.resolutions.Length; i++)
        {
            string item = this.resolutions[i].width.ToString() + " x " + this.resolutions[i].height.ToString();
            list.Add(item);
            if (this.resolutions[i].width == Screen.width && this.resolutions[i].height == Screen.height)
            {
                value = i;
            }
        }
        this.resolutionDropdown.AddOptions(list);
        this.resolutionDropdown.value = value;
        this.resolutionDropdown.RefreshShownValue();
        this.ShadowRes = Enum.GetNames(typeof(ShadowResolution));
        this.shadowResDropdown.ClearOptions();
        this.shadowResDropdown.options = new List<Dropdown.OptionData>();
        new List<string>();
        for (int j = 0; j < this.ShadowRes.Length; j++)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = this.ShadowResolutionNames(this.ShadowRes[j]);
            this.shadowResDropdown.options.Add(optionData);
            if (this.ShadowRes[j].CompareTo(QualitySettings.shadowResolution.ToString()) == 0)
            {
                this.shadowResDropdown.value = j;
            }
        }
        this.shadowResDropdown.value = this.shadowResDropdown.value;
        this.shadowResDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = this.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        this.currentResolutionIndex = resolutionIndex;
        this._resolutionH = resolution.height;
        this._resolutionW = resolution.width;
    }

    public void SetShadowResolution(int ShadowResolutionIndex)
    {
        QualitySettings.shadowResolution = (ShadowResolution)Enum.Parse(typeof(ShadowResolution), this.ShadowRes[ShadowResolutionIndex]);
        this._shadowResIndex = ShadowResolutionIndex;
    }

    public void SetBrightness(float brightness)
    {
        this._brightnessLevel = brightness;
        this.brightnessValue.text = brightness.ToString("0.00");
    }

    public void SetV_Sync(bool is_VSync)
    {
        this._isVSync = is_VSync;
    }

    public void SetFullScreen(bool isFullscreen)
    {
        this._isFullscreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        this._qualityLevel = qualityIndex;
    }

    private string ShadowResolutionNames(string value)
    {
        if (value.CompareTo("High") == 0)
        {
            return "Высокое";
        }
        if (value.CompareTo("Low") == 0)
        {
            return "Низкое";
        }
        if (value.CompareTo("Medium") == 0)
        {
            return "Среднее";
        }
        if (value.CompareTo("VeryHigh") == 0)
        {
            return "Максимальное";
        }
        return "Error";
    }

    public void GraphicRevert()
    {
        QualitySettings.vSyncCount = 0;
        QualitySettings.antiAliasing = 0;
        QualitySettings.masterTextureLimit = 3;
        QualitySettings.shadows = ShadowQuality.Disable;
        QualitySettings.shadowResolution = ShadowResolution.Low;
    }

    public void GraphicApply()
    {
        PlayerPrefs.SetInt("masterResolutionIndex", this.currentResolutionIndex);
        PlayerPrefs.SetInt("masterResolutionW", this._resolutionW);
        PlayerPrefs.SetInt("masterResolutionH", this._resolutionH);
        PlayerPrefs.SetFloat("masterBrightness", this._brightnessLevel);
        PlayerPrefs.SetInt("masterQuality", this._qualityLevel);
        QualitySettings.SetQualityLevel(this._qualityLevel);
        PlayerPrefs.SetInt("masterFullscreen", this._isFullscreen ? 1 : 0);
        Screen.fullScreen = this._isFullscreen;
        PlayerPrefs.SetInt("masterVSync", this._isVSync ? 1 : 0);
        QualitySettings.vSyncCount = (this.vSyncToggle.isOn ? 1 : 0);
        PlayerPrefs.SetString("masterShadowRes", QualitySettings.shadowResolution.ToString());
        PlayerPrefs.SetInt("masterShadowResValue", this._shadowResIndex);
        QualitySettings.shadowResolution = (ShadowResolution)Enum.Parse(typeof(ShadowResolution), this.ShadowRes[this._shadowResIndex]);
        base.StartCoroutine(this.ConfirmationBox());
    }

    
    public IEnumerator ConfirmationBox()
    {
        int num=0;
        while (num == 0)
        {
            this.ConformationPromt.SetActive(true);
            yield return new WaitForSeconds(2f);
        }
        if (num != 1)
        {
            yield break;
        }
        this.ConformationPromt.SetActive(false);
        yield break;
    }
}
