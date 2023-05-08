using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindingKey : MonoBehaviour
{
    private const string RebindsKey = "rebinds";

    private GameObject player;

    [SerializeField]
    private InputActionReference Action;

    [SerializeField]
    private PlayerControl playerController;

    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private TMP_Text bindingDisplayNameText;

    [SerializeField]
    private GameObject startRebindObject;

    [SerializeField]
    private GameObject waitingforInputObject;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Update()
    {
        if (rebindingOperation != null && rebindingOperation.completed)
        {
            Action.action.Enable();
        }
    }

    private void Start()
    {
        LoadRebindings();
    }

    public void LoadRebindings()
    {
        string @string = PlayerPrefs.GetString("rebinds");
        if (string.IsNullOrEmpty(@string))
        {
            return;
        }
        playerInput.actions.LoadBindingOverridesFromJson(@string, true);
        Action.action.GetBindingIndexForControl(Action.action.controls[0]);
        bindingDisplayNameText.text = InputControlPath.ToHumanReadableString(Action.action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice, null);
    }

    public void Save()
    {
        string value = playerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", value);
        Debug.Log("Save");
    }

    public void StartRebinding()
    {
        startRebindObject.SetActive(false);
        waitingforInputObject.SetActive(true);
        Action.action.Disable();
        rebindingOperation = Action.action.PerformInteractiveRebinding(-1).WithControlsExcluding("Mouse").WithCancelingThrough("<Keyboard>/escape").OnMatchWaitForAnother(0.1f).OnComplete(delegate (InputActionRebindingExtensions.RebindingOperation operation)
        {
            RebindingComplite();
        }).Start();
    }

    private void RebindingComplite()
    {
        Action.action.GetBindingIndexForControl(Action.action.controls[0]);
        bindingDisplayNameText.text = InputControlPath.ToHumanReadableString(Action.action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice, null);
        rebindingOperation.Dispose();
        startRebindObject.SetActive(true);
        waitingforInputObject.SetActive(false);
        Save();
    }
}
