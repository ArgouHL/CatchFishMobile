using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AquaUICTR : MonoBehaviour
{
    public void BackLobby()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadClothing()
    {
        SceneManager.LoadScene(5);
    }

    public void LoadCollection()
    {
        SceneManager.LoadScene(6);
    }
}
