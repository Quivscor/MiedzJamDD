using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveSceneAndQuit : MonoBehaviour
{
    public void ChangeScene(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
