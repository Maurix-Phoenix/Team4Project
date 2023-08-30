using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButtonUI : MonoBehaviour
{
    public List<GameObject> ToggleList = new List<GameObject>();

    public void ToggleElements()
    {
        foreach (GameObject go in ToggleList)
        {
            go.SetActive(!go.activeSelf);
        }
    }
}
