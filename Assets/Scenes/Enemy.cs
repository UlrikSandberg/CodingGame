using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ShatterableEntity
{
    public Transform targetTransform;
    public float speed = 10;

    [HideInInspector]
    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        targetTransform = FindObjectOfType<Player>().transform;
        transform.eulerAngles = Vector3.up * 180;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isActive && targetTransform != null)
        {
            if (collision.transform.tag == "Player")
            {
                targetTransform.GetComponent<ShatterableEntity>().Hit(transform.position, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive && targetTransform != null)
        {
            Vector3 dirToTarget = Vector3.Scale(targetTransform.position - transform.position, new Vector3(1, 0, 1)).normalized;
            transform.eulerAngles = Vector3.up * Mathf.Atan2(dirToTarget.x, dirToTarget.z) * Mathf.Rad2Deg;
            transform.position += dirToTarget * speed * Time.deltaTime;
        }
    }
}
