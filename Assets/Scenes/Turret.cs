using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    //Turret gameObjects for rotation purposes...
    public Transform turretJoint;
    public Transform muzzle;

    public LineRenderer lineRenderer;

    public Bullet bulletPrefab;
    public float reloadSpeed;
    public float smoothRot = 8;
    public LayerMask targetMask;
    
    //[HideInInspector]
    public bool isActive;

    private float lastShotTime;
    private float time;
    private float aimError;
    private bool aimed;

    public void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = .1f;
        lineRenderer.endWidth = .1f;
    }

    public void Reset()
    {
        turretJoint.rotation = Quaternion.identity;
        lastShotTime = time;
        if(lineRenderer != null)
        {
            lineRenderer.SetPosition(0, muzzle.position);
            lineRenderer.SetPosition(1, muzzle.position);
        }
    }

    private void Update()
    {
        if(isActive && aimed) // Shoot
        {
            time += Time.deltaTime;
            if(time > lastShotTime + reloadSpeed && aimError < 2)
            {
                lastShotTime = time;
                Shoot();
            }

            RaycastHit hit;
            if (Physics.Raycast(muzzle.position, muzzle.forward * 100, out hit, targetMask))
            {
                lineRenderer.SetPosition(0,muzzle.position);
                lineRenderer.SetPosition(1, hit.point);

                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
            }
        }

        aimed = false;
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        StartCoroutine(AnimShot());
    }

    private IEnumerator AnimShot()
    {
        muzzle.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        muzzle.gameObject.SetActive(false);
    }

    public void Aim(float x, float y, float z)
    {
        Vector3 aim = new Vector3(x, y, z);
        Quaternion lookRot = Quaternion.LookRotation((aim - turretJoint.position).normalized, Vector3.up);
        turretJoint.rotation = Quaternion.Slerp(turretJoint.rotation, lookRot, Time.deltaTime * smoothRot);
        aimError = Quaternion.Angle(turretJoint.rotation, lookRot);

        aimed = true;
    }

    public void TargetClosest(Enemy[] enemies)
    {
        // Find the target closets to the turret and aim at fire
        Enemy closest = null;
        float closestDst = float.PositiveInfinity;

        for(int i = 0; i < enemies.Length; i++)
        {
            var enemy = enemies[i];

            var dx = turretJoint.transform.position.x - enemy.transform.position.x;
            var dy = turretJoint.transform.position.y - enemy.transform.position.y;
            var dz = turretJoint.transform.position.z - enemy.transform.position.z;

            var sqrtDistance = dx * dx + dy * dy + dz * dz;

            if(sqrtDistance < closestDst)
            {
                closest = enemy;
            }
        }

        if(closest != null)
        {
            Aim(closest.transform.position.x, closest.transform.position.y, closest.transform.position.z);
        }
    }
}
