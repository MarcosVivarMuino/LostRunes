using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        UIManager.Instance.StartGame();
    }

    public void OnMenuClicked()
    {
        UIManager.Instance.GoToMenuScene();
    }
}
