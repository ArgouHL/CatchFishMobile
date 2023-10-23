using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
   public static void ShowAndClickable (CanvasGroup canvas,bool enable)
    {
        canvas.alpha = enable?1:0;
        canvas.interactable = enable;
        canvas.blocksRaycasts = enable;
    }

}
