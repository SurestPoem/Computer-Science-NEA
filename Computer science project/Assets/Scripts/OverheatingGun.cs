using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheatingGun : Gun
{
    public float overheatCooldown = 1f;  // The cooldown duration after overheating
    public float shotsBeforeOverheat = 5; // Number of shots before overheating
    private int shotsFired = 0;           // Counter to track the number of shots fired
    private bool isOverheating = false;   // Whether the gun is overheating or not
    public Color overheatColor;
    public AudioClip overheatSound; // Sound to play when overheating

    public override void Shoot()
    {
        // If the gun is overheating, don't shoot
        if (isOverheating)
        {
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
                shotsFired = 0;          
                isOverheating = true;     
                StartCoroutine(CooldownOverheat());  
            }
        }
    }

    private IEnumerator CooldownOverheat()
    {
        Debug.Log("Overheating started, waiting for cooldown.");
        AudioManager.instance.PlaySound(overheatSound, Random.Range(0.5f, 1.5f)); // Play the overheat sound
        gunSpriteRenderer.color = overheatColor; // Change the gun color to indicate overheating

        // Simulate the overheating cooldown
        yield return new WaitForSeconds(overheatCooldown);

        // Reset overheating and cooldown logic
        gunSpriteRenderer.color = Color.white; // Reset color
        isOverheating = false;  // Reset the overheating state
        timeSinceLastShot = 0f; // Reset the shot cooldown to allow firing again
        Debug.Log("Overheating finished, can shoot again.");
    }
}