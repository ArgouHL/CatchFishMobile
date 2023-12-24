using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCountShow : MonoBehaviour
{
    [SerializeField] private TMP_Text countText;

    internal void ShowCount(int count)
    {
        countText.text = count.ToString();
    }
}
