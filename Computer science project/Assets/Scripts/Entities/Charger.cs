using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    public float chargeSpeed = 4f;
    public float chargeDistance = 3f;
    public float chargeCooldown = 2f;
    public float retreatTime = 4f;

    private bool isCharging = false;
    private float lastChargeTime = -Mathf.Infinity;

    protected override void Update()
    {
        base.Update();
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        // If currently charging, skip decision logic
        if (isCharging) return;

        // Can we charge?
        if (Time.time >= lastChargeTime + chargeCooldown && distanceToPlayer <= chargeDistance)
        {
            StartCoroutine(Charge());
        }
        else
        {
            // Too far to charge, just chase
            ChasePlayer();
        }

    }

    private IEnumerator Charge()
    {
        isCharging = true;
        lastChargeTime = Time.time;

        moveSpeed /= 2f;
        // Fixed direction at start
        Vector3 chargeDirection = (playerTransform.position - transform.position).normalized;

        float chargeDuration = chargeDistance / chargeSpeed;
        float chargeTimer = 0f;

        while (chargeTimer < chargeDuration)
        {
            transform.position += chargeDirection * chargeSpeed * Time.deltaTime;
            chargeTimer += Time.deltaTime;

            // (Optional) You can check for collisions here
            // CheckCollisionWithPlayer();

            yield return null;
        }

        // Optional retreat after the charge
        float retreatStartTime = Time.time;
        while (Time.time < retreatStartTime + retreatTime)
        {
            RetreatFromPlayer();
            yield return null;
        }

        isCharging = false;
    }

    /*float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        // If currently charging, skip decision logic
        if (isCharging) return;

        // Can we charge?
        if (Time.time >= lastChargeTime + chargeCooldown && distanceToPlayer <= chargeDistance)
        {
            StartCoroutine(Charge());
        }
        else
        {
            // Too far to charge, just chase
            ChasePlayer();
        }*/
}