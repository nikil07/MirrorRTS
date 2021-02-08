using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField] Targetter targeter;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpwanPoint;
    [SerializeField] float fireRange = 5f;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float rotationSpeed = 20f;

    private float lastFireTime;

    [ServerCallback]
    private void Update()
    {
        if (targeter.getTarget() == null)
            return;

        if (!canFireAtTarget())
            return;

        Quaternion targetRotation = Quaternion.LookRotation(targeter.getTarget().transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Time.time > (1 / fireRate) + lastFireTime) {
            lastFireTime = Time.time;

            Quaternion projectileRotation = Quaternion.LookRotation(targeter.getTarget().getAimAtPoint().transform.position - projectileSpwanPoint.position);
            GameObject projectileInstance = Instantiate(projectilePrefab, projectileSpwanPoint.position, projectileRotation);

            NetworkServer.Spawn(projectileInstance, connectionToClient);

        }
    }

    [Server]
    private bool canFireAtTarget() {


        return (targeter.getTarget().transform.position - transform.position).sqrMagnitude <= fireRange * fireRange;
    }
}
