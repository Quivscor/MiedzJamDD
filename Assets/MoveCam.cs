using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public Vector3 maxMovement;
    private Vector3 startPos;

    public float moveSpeed;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (!this.gameObject.activeInHierarchy)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = transform.position;

        movement += transform.up * vertical * moveSpeed * Time.deltaTime;
        movement += transform.right * horizontal * moveSpeed * Time.deltaTime;

        movement = new Vector3(Mathf.Clamp(movement.x, startPos.x - maxMovement.x, startPos.x + maxMovement.x),
            Mathf.Clamp(movement.y, startPos.y - maxMovement.y, startPos.y + maxMovement.y),
            Mathf.Clamp(movement.z, startPos.z - maxMovement.z, startPos.z + maxMovement.z));

        transform.position = movement;
    }
}
