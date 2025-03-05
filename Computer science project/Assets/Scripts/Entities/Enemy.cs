using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public float detectionDistance;
    public float collideDamage;
    protected Transform playerTransform;
    public float damageCooldown = 1f; // Cooldown time
    private float lastDamageTime = -1f;
    public SpriteRenderer EnemySpriteRenderer;
    public List<LootItem> lootTable = new List<LootItem>();

    void Update()
    {
        base.Update();
    }

    public void DealCollideDamage(Player player)
    {
        if (Time.time > lastDamageTime + damageCooldown)
        {
            damageAmount = collideDamage;
            player.takeDamage(damageAmount);
            Debug.Log("Player damaged");
            lastDamageTime = Time.time;
        }
    }

    protected void ChasePlayer()
    {
        if (IsFreeze != true)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized; // Use playerTransform
            transform.position += direction * moveSpeed * Time.deltaTime;

            if (direction.x < 0)
            {
                EnemySpriteRenderer.flipX = true; // Flip the sprite
            }
            else
            {
                EnemySpriteRenderer.flipX = false; // Reset flip
            }
        }
    }

        public void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                Player playerComponent = collider.GetComponent<Player>();
                if (playerComponent != null)
                {
                    DealCollideDamage(playerComponent);
                }
            }
        }


        public void DropItems()
        {
            // Loop to drop 3 items
            for (int i = 0; i < 3; i++)
            {
                // Randomly select a loot item from the loot table
                LootItem lootItem = lootTable[Random.Range(0, lootTable.Count)];

                // Check if the item should drop based on dropChance
                if (Random.Range(0f, 100f) <= lootItem.dropChance)
                {
                    // Instantiate loot with a random offset within 0.5f range
                    Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
                    InstantiateLoot(lootItem.itemPrefab, transform.position + offset);
                }
            }
        }

        void InstantiateLoot(GameObject loot, Vector3 position)
        {
            if (loot)
            {
                // Instantiate the loot at the position with a random offset
                GameObject droppedLoot = Instantiate(loot, position, Quaternion.identity);
            }
        }

    public override void Die()
    {
        DropItems();  // Call DropItems method
        base.Die();   // Call base class's Die() method
    }

    protected override void Initialise()
    {
        base.Initialise();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Update to use playerTransform
    }
}