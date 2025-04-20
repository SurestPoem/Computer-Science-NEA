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
        foreach (var hitCollider in hitColliders)
        {
            /*if (hitCollider.CompareTag("Wall"))
            {
                return false;
            }*/
        }
        return true;
    }

    public IEnumerator Split()
    {
        float newScale = transform.localScale.x / 1.8f;
        float newMaxHealth = Mathf.RoundToInt(baseMaxHealth / 1.7f);
        float newMoveSpeed = baseMoveSpeed * 1.5f;
        float newCollideDamage = Mathf.RoundToInt(baseCollideDamage / 2f);
        for (int i = 0; i < splitAmount; i++)
        {
            
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * splitRadius;
            spawnPosition.z = 0; // Ensure z position is 0 for 2D

            // Ensure position is clear before spawning new slime
            if (IsPositionClear(spawnPosition))
            {
                GameObject newslime = Instantiate(SplitSlime, spawnPosition, Quaternion.identity);
                newslime.transform.localScale = new Vector3(newScale, newScale, 0);
                
                Slime newslimeScript = newslime.GetComponent<Slime>();
                newslimeScript.spriteRenderer.color = new Color(1, 1, 1, 1);
                newslimeScript.currentSplitGeneration++;
                newslimeScript.baseMaxHealth = newMaxHealth;
                newslimeScript.baseMoveSpeed = newMoveSpeed;
                newslimeScript.baseCollideDamage = newCollideDamage;

            }
            yield return new WaitForSeconds(0.1f); // Small delay between splits
        }
    }

    public override void Die()
    {
        if (currentSplitGeneration < maxSplitGeneration)
        {
            //StartCoroutine(Split());
        }
        base.Die();
    }
}
