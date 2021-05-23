using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance = null;

    private void Awake()
    {
        if (CameraController.Instance == null)
            CameraController.Instance = this;
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
