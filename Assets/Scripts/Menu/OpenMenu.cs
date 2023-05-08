using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject Menu;

    private void Start()
    {
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
}
