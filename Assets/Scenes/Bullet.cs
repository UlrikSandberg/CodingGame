using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50;
    public LayerMask targetMask;

    // Update is called once per frame
    void Update()
    {
        Vector3 trajectoryVel = transform.position + transform.forward * speed * Time.deltaTime;

        RaycastHit hit;
        if (Physics.SphereCast(transform.position - transform.forward * Time.deltaTime * speed * .1f, .3f, (trajectoryVel - transform.position).normalized, out hit, speed * Time.deltaTime + 0.5f, targetMask))
        {
            // Debug.Log(hit.collider.name);
            if (hit.collider.gameObject.GetComponent<ShatterableEntity>())
            {
                hit.collider.gameObject.GetComponent<ShatterableEntity>().Hit(transform.position, speed);
            }
            Destroy(gameObject);
        }

        transform.position = trajectoryVel;
    }
}
