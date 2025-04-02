using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : Gun
{
    public float overheatCooldown = 1f;  // The cooldown duration after overheating
    public float shotsBeforeOverheat = 5; // Number of shots before overheating
    private int shotsFired = 0;           // Counter to track the number of shots fired
    private bool isOverheating = false;   // Whether the gun is overheating or not

    public override void Shoot()
    {
        // If the gun is overheating, don't shoot
        if (isOverheating)
        {
            Debug.Log("Gun is overheating, can't shoot.");
            return;
        }

        // Call the base Shoot method only if the cooldown is complete
        if (timeSinceLastShot >= cooldownTime)
        {
            base.Shoot();  // Shoot normally

            shotsFired++;

            // If we've fired enough shots to overheat
            if (shotsFired >= shotsBeforeOverheat)
            {
                shotsFired = 0;           // Reset shots fired count
                isOverheating = true;     // Set the gun to overheating state
                StartCoroutine(CooldownOverheat());   // Start the cooldown coroutine
            }
        }
    }

    private IEnumerator CooldownOverheat()
    {
        Debug.Log("Overheating started, waiting for cooldown.");

        // Simulate the overheating cooldown
        yield return new WaitForSeconds(overheatCooldown);

        // Reset overheating and cooldown logic
        isOverheating = false;  // Reset the overheating state
        timeSinceLastShot = 0f; // Reset the shot cooldown to allow firing again
        Debug.Log("Overheating finished, can shoot again.");
    }
}