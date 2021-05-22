using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public Vector2 maxMovement;
    private Vector3 startPos;

    public float moveSpeed;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = transform.position;

        movement.z = Mathf.Clamp(transform.position.z + -1 * vertical * moveSpeed * Time.deltaTime, startPos.z - maxMovement.y, startPos.z + maxMovement.y);
        movement.x = Mathf.Clamp(transform.position.x + -1 * horizontal * moveSpeed * Time.deltaTime, startPos.x - maxMovement.x, startPos.x + maxMovement.x);

        transform.position = movement;
    }
}
