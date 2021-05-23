using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBack : MonoBehaviour
{
    void Start()
    {
        GetComponent<MoveSceneAndQuit>().ChangeScene(0);
    }
}
