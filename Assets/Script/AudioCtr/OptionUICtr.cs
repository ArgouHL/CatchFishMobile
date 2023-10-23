using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUICtr : MonoBehaviour
{
    public static OptionUICtr instance;
    private CanvasGroup optionUI;
    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        optionUI = GetComponent<CanvasGroup>();
    }

    public void ShowOptionUI()
    {

        UIHelper.ShowAndClickable(optionUI, true);
        PlayerInputManager.instance.ChangeType(InputType.Option);

    }
    public void HideOptionUI()
    {
        Debug.Log("back");
        UIHelper.ShowAndClickable(optionUI, false);
        PlayerInputManager.instance.ChangeType(InputType.Lobby);

    }
}
