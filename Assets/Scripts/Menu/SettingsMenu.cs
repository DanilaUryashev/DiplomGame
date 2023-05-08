using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text GameText;

    [SerializeField]
    private TMP_Text GraphicText;

    [SerializeField]
    private TMP_Text ControlText;

    [SerializeField]
    private GameObject GameSetting;

    [SerializeField]
    private GameObject GraphSetting;

    [SerializeField]
    private GameObject ControlSetting;

    [SerializeField]
    private GameObject Menu;

    private void Start()
    {
        GameText.color = Color.red;
        GraphicText.color = Color.white;
        ControlText.color = Color.white;
        GameSetting.SetActive(true);
        GraphSetting.SetActive(false);
        ControlSetting.SetActive(false);
    }

    public void openMenu()
    {
        if (!Menu.activeInHierarchy)
        {
            Menu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Menu.SetActive(false);
            Time.timeScale = 1f;
        }
        Start();
    }

    public void SelectGameSetting()
    {
        GameText.color = Color.red;
        GraphicText.color = Color.white;
        ControlText.color = Color.white;
        GameSetting.SetActive(true);
        GraphSetting.SetActive(false);
        ControlSetting.SetActive(false);
    }

    public void SelectGraphicSetting()
    {
        GameText.color = Color.white;
        GraphicText.color = Color.red;
        ControlText.color = Color.white;
        GameSetting.SetActive(false);
        GraphSetting.SetActive(true);
        ControlSetting.SetActive(false);
    }

    public void SelectControlSetting()
    {
        GameText.color = Color.white;
        GraphicText.color = Color.white;
        ControlText.color = Color.red;
        GameSetting.SetActive(false);
        GraphSetting.SetActive(false);
        ControlSetting.SetActive(true);
    }
}
