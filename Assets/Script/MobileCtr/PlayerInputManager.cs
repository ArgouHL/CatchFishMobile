using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-2)]
public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public static InputType current { get; private set; }
    public static InputManager inputs;

    public delegate void InputEvent();
    public static InputEvent onTypeChange;
    public static InputEvent mobileUIOn;
    public static InputEvent mobileUIOff;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);         

        }
           
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            inputs = new InputManager();
        }

        

        inputs.UI.Enable();
    }

    /// <summary>
    /// Change input type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public InputType ChangeType(InputType type,bool andriodUIswitch=true)
    {
        // Disable all actions and re-enable them according to type
        inputs.Shop.Disable();
        inputs.Lobby.Disable(); 
        inputs.Option.Disable();
        //inputs.UI.Disable();
        inputs.GamePlay.Disable();
        if (andriodUIswitch)
            mobileUIOff?.Invoke();


        switch (type)
        {
            case InputType.Shop:
                inputs.Shop.Enable();
                break;

            case InputType.Lobby:
                inputs.Lobby.Enable();

                break;

            case InputType.Option:
                inputs.Option.Enable();
                break;

            case InputType.GamePlay:
                inputs.GamePlay.Enable();
                break;

            //case InputType.UI:
            //    inputs.UI.Enable();
            //    break;          
     
            default:
                break;
        }

        current = type;
        onTypeChange?.Invoke();

        Debug.Log("Input type changed, current type: " + type.ToString());

        return current;
    }

    public InputType ChangeType(int type)
    {
        return ChangeType((InputType)type);
    }
}

public enum InputType
{
    None,
    Shop,
    Lobby,
    Option,
    UI,
    GamePlay
}