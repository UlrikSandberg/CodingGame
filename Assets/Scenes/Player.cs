using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : ShatterableEntity
{
    public float speed = 10;
    private Rigidbody rb;

    [HideInInspector]
    public Vector3 direction;

    private Action startLineCallback;
    private Action goalCallback;

    public void SetStartLineCallback(Action callback)
    {
        startLineCallback = callback;
    }

    public void SetGoalCallback(Action callback)
    {
        goalCallback = callback;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "StartLine")
        {
            startLineCallback?.Invoke();
        }

        if(other.tag == "Goal")
        {
            goalCallback?.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidBody component of respective player GameObject
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle inputs and move objects around the scene :D

        // This can be done using either GetAxis --> Accelerating input over a couple of frames
        // Or GetAxisRaw which will just return the full direction.
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        direction = input.normalized;

        // If magnitude is different from 0 we have key pressed.
        if(input.magnitude != 0)
        {
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * targetAngle;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * speed * direction.magnitude; 
    }
}
