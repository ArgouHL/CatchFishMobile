using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Rendering;

public class GameStart : MonoBehaviour
{
    [SerializeField] private CanvasGroup StartUI;
    [SerializeField] private CanvasGroup NameInput;
    [SerializeField] private CanvasGroup NamePop;
    [SerializeField] private CanvasGroup GameLoading;


    private string InputedName;

    private void Start()
    {
        DebugManager.instance.enableRuntimeUI = false;
    }
    public void PressGameStart()
    {
        StartUI.alpha = 0;
        StartUI.interactable = false;
        if (PlayerDataControl.instance.LoadPlayer())
        {
            GameLoading.alpha = 1;
            GameLoading.interactable = true;
            GameLoading.blocksRaycasts = true;
            StartCoroutine(LoadLobby());
        }
        else
        {
            NameInput.alpha = 1;
            NameInput.interactable = true;
            NameInput.blocksRaycasts = true;
        }
    }

    public void ShowNameConfirm()
    {
        NameInput.interactable = false;
        NamePop.alpha = 1;
        NamePop.interactable = true;
        NamePop.blocksRaycasts = true;
        InputedName = GetComponentInChildren<TMP_InputField>().text;
        NamePop.transform.GetComponentInChildren<TMP_Text>().text = InputedName;
    }

    public void HideNameConfirm()
    {
        NameInput.interactable = true;
        NamePop.alpha = 0;
        NamePop.interactable = false;
        NamePop.blocksRaycasts = false;
    }

    public void NameConfirmYes()
    {
        NameInput.alpha = 0;
        NameInput.interactable = false;
        NameInput.blocksRaycasts = false;
        NamePop.alpha = 0;
        NamePop.interactable = false;
        NamePop.blocksRaycasts = false;
        GameLoading.alpha = 1;
        PlayerDataControl.instance.NewSave(InputedName);
        StartCoroutine(LoadLobby());

    }

    private IEnumerator LoadLobby()
    {
        var asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;
        yield return new WaitForSeconds(2);
        asyncLoad.allowSceneActivation = true;
    }

    public void ClearData()
    {
        PlayerDataControl.instance.DeleteSave();
    }
}
