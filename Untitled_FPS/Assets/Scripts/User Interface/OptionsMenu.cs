using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using JetBrains.Annotations;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsmenu;
    public GameObject SettingsMenu;
    public GameObject KeyBindingsMenu;
    [Header("Keybindings")]
    public KeyBindings bindings;
    public TMP_Dropdown _ddForward;
    public TMP_Dropdown _ddBackward;
    public TMP_Dropdown _ddLeft;
    public TMP_Dropdown _ddRight;
    public TMP_Dropdown _ddJump;
    [Header("Settings")]
    public PlayerConfig settings;
    public Slider volumeLevel;
    public Slider mouseXSens;
    public Slider mouseYSens;
    public AudioMixer mixer;
    public void Awake()
    {
        mixer.SetFloat("Master Volume", Mathf.Log10(settings.volumeLevel) * 20);
        mouseXSens.value = settings.mouseXSensitivity;
        mouseYSens.value = settings.mouseYSensitivity;
        volumeLevel.value = settings.volumeLevel;
    }
    public void OnSettings()
    {
        SettingsMenu.SetActive(true);
        KeyBindingsMenu.SetActive(false);
        optionsmenu.SetActive(false);
    }
    public void OnKeyBindings()
    {
        SettingsMenu.SetActive(false);
        KeyBindingsMenu.SetActive(true);
        optionsmenu.SetActive(false);
    }
    public void OnBack()
    {
        SettingsMenu.SetActive(false);
        KeyBindingsMenu.SetActive(false);
        optionsmenu.SetActive(true);
    }
    public void OnLogout()
    {

    }
    public void OnQuit()
    {
        Application.Quit();
    }
    public void OnSaveSettings()
    {
        settings.mouseXSensitivity = mouseXSens.value;
        settings.mouseYSensitivity = mouseYSens.value;
        settings.volumeLevel = volumeLevel.value;
        mixer.SetFloat("Master Volume", Mathf.Log10(settings.volumeLevel) * 20);
        OnBack();
    }
    public void OnSaveKeyBindings()
    {
        bindings.Forward = getCode(_ddForward.value);
        bindings.Backward = getCode(_ddBackward.value);
        bindings.Left = getCode(_ddLeft.value);
        bindings.Right = getCode(_ddRight.value);
        bindings.Jump = getCode(_ddJump.value);
        OnBack();
    }

    private KeyCode getCode(int dropdownValue) // We can update this later by returning the ascii value of the char (KeyCode.xxxx = # => ascii.# = xxxx)
    {
        switch (dropdownValue)
        {
            case 0:
                return KeyCode.A;
            case 1:
                return KeyCode.B;
            case 2:
                return KeyCode.C;
            case 3:
                return KeyCode.D;
            case 4:
                return KeyCode.E;
            case 5:
                return KeyCode.F;
            case 6:
                return KeyCode.G;
            case 7:
                return KeyCode.H;
            case 8:
                return KeyCode.I;
            case 9:
                return KeyCode.J;
            case 10:
                return KeyCode.K;
            case 11:
                return KeyCode.L;
            case 12:
                return KeyCode.M;
            case 13:
                return KeyCode.N;
            case 14:
                return KeyCode.O;
            case 15:
                return KeyCode.P;
            case 16:
                return KeyCode.Q;
            case 17:
                return KeyCode.R;
            case 18:
                return KeyCode.S;
            case 19:
                return KeyCode.T;
            case 20:
                return KeyCode.U;
            case 21:
                return KeyCode.V;
            case 22:
                return KeyCode.W;
            case 23:
                return KeyCode.X;
            case 24:
                return KeyCode.Y;
            case 25:
                return KeyCode.Z;
            case 26:
                return KeyCode.Space;
        }
        return KeyCode.None;
    }
}
