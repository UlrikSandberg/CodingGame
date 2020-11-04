using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterableEntity : MonoBehaviour
{
    public FragmentSpawner fragments;

    // Start is called before the first frame update
    public void Hit(Vector3 hitPoint, float speed)
    {
        var f = Instantiate(fragments, transform.position, Quaternion.Euler(Vector3.up * 45 * Random.Range(0, 4)));
        f.transform.localScale = transform.localScale / 2;
        f.Spawn(Vector3.zero, hitPoint, speed);
        f.col = GetComponent<MeshRenderer>().material.color;
        Destroy(gameObject);
    }
}
