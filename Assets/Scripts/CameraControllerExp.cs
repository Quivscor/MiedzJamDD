using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerExp : MonoBehaviour
{
    public static CameraControllerExp Instance = null;

    private void Awake()
    {
        if (CameraControllerExp.Instance == null)
            CameraControllerExp.Instance = this;
        else
            Destroy(this);
    }

    private Animator animator = null;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Shake()
    {
        animator?.SetTrigger("shakeTrigger");
    }
}
