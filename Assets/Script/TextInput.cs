using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInput : MonoBehaviour
{
   public static void EnableTextInput()
    {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, true);
    }
}
