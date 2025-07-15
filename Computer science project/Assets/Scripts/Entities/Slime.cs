using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public int splitAmount = 3;
    public GameObject SplitSlime;
    public float splitRadius = 1.0f;
    public int maxSplitGeneration = 2;
    public int currentSplitGeneration = 0;
    public bool hasSplit = false;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // Scale the animation speed based on movement speed
        if (Time.time > lastDamageTime + damageCooldown)
        {
            // Enemy chases player after the damage cooldown ends
            ChasePlayer();
        }
        else
        {
            // Enemy retreats immediately after damaging player and continues until damageCooldown has passed
            RetreatFromPlayer();
        }
    }

    public bool IsPositionClear(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, splitRadius);
        /*foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Wall"))
            {
                return false;
            }
        }*/
        return true;
    }

    public void Split()
    {
        if (SplitSlime == null) { return; } // Ensure SplitSlime is assigned
        if (currentSplitGeneration >= maxSplitGeneration || hasSplit) { return; } // Prevent further splitting
        if (splitAmount <= 0) { return; } // Ensure splitAmount is valid

        for (int i = 0; i < splitAmount; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * splitRadius;
            spawnPosition.z = 0; // Ensure z position is 0 for 2D
            GameObject newslime = Instantiate(SplitSlime, spawnPosition, Quaternion.identity);
                
            Slime newslimeScript = newslime.GetComponent<Slime>();
            newslimeScript.spriteRenderer.color = new Color(1, 1, 1, 1);
            newslimeScript.currentSplitGeneration = currentSplitGeneration + 1;
            newslimeScript.maxSplitGeneration = maxSplitGeneration;

        }
        hasSplit = true;
    }

    public override void Die()
    {
        Split();
        base.Die();
    }
}
