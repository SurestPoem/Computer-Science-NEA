using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public float chaseDistance;
    public float collideDamage;
    public float baseCollideDamage;
    protected Transform playerTransform;
    public float damageCooldown = 1f; // Cooldown time
    protected float lastDamageTime = -1f;
    public List<LootItem> lootTable = new List<LootItem>();
    public GameObject deadBodyPrefab;
    public float animationSpeed = 1f; // Animation speed multiplier
    [Header("Pathfinding things - ignore")] //Ignore for now A* currently not working
    public Node currentNode;
    public List<Node> path = new List<Node>();
    private Vector3 currentDirection;
    

    protected override void Update()
    {
        base.Update();
        if (animator != null)
        {
            animator.speed = moveSpeed * animationSpeed; // Adjust animation speed based on movement speed
        }
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
            currentDirection = (playerTransform.position - transform.position).normalized;
            transform.position += currentDirection * moveSpeed * Time.deltaTime;

            spriteRenderer.flipX = currentDirection.x < 0;
        }
    }

    protected void RetreatFromPlayer()
    {
        if (IsFreeze != true)
        {
            currentDirection = (transform.position - playerTransform.position).normalized;
            transform.position += currentDirection * moveSpeed * Time.deltaTime;

            spriteRenderer.flipX = currentDirection.x < 0;
        }
    }

    protected void StrafePlayer()
    {
        if (IsFreeze != true)
        {
            Vector3 strafeDirection = new Vector3(-currentDirection.y, currentDirection.x, 0); // Perpendicular direction
            transform.position += strafeDirection * moveSpeed * Time.deltaTime;
            spriteRenderer.flipX = strafeDirection.x < 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (currentDirection != Vector3.zero)
        {
            // Movement direction
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + currentDirection * 2f);
        }

        // Optional: draw line to player
        if (playerTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, playerTransform.position);
        }

        
    }

    public void CreatePath()
    {
        if (path.Count > 0)
        {
            // Move towards the next node in the path
            Node nextNode = path[0];
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(nextNode.transform.position.x, nextNode.transform.position.y, -2), 3 * Time.deltaTime);

            // If close enough to the node, move to the next one
            if (Vector2.Distance(transform.position, nextNode.transform.position) < 0.1f)
            {
                currentNode = nextNode;
                path.RemoveAt(0); // Remove the node from the path once reached
            }
        }

        else
        {
            Node[] nodes = FindObjectsOfType<Node>();
            while (path == null || path.Count == 0)
            {
                path = AStarManager.instance.GeneratePath(currentNode, GetClosestNodeToPlayer(nodes));
            }
        }
    }

    private Node GetClosestNodeToPlayer(Node[] nodes)
    {
        Node closestNode = nodes[0];
        float closestDistance = Vector2.Distance(playerTransform.position, closestNode.transform.position);

        foreach (var node in nodes)
        {
            float distanceToPlayer = Vector2.Distance(playerTransform.position, node.transform.position);
            if (distanceToPlayer < closestDistance)
            {
                closestNode = node;
                closestDistance = distanceToPlayer;
            }
        }

        return closestNode;
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
        if (lootTable.Count == 0)
        {
            Debug.LogWarning("Loot table is empty. No items to drop.");
            return;
        }
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
                Instantiate(lootItem.itemPrefab, transform.position + offset, Quaternion.identity);
            }
        }
    }

    public override void Die()
    {
        DropItems();  // Call DropItems method
        if (deadBodyPrefab != null)
        {
            GameObject deadBody = Instantiate(deadBodyPrefab, transform.position, Quaternion.identity);
            Destroy(deadBody, 3f); // Destroy after a few seconds
        }
        base.Die();   // Call base class's Die() method
    }

    protected override void Initialise() //called in Start() method of superclass Entity
    {
        maxHealth = Mathf.RoundToInt(baseMaxHealth * GameManager.Instance.difficultyMultiplier);
        collideDamage = Mathf.RoundToInt(baseCollideDamage * GameManager.Instance.difficultyMultiplier);
        moveSpeed = baseMoveSpeed * GameManager.Instance.difficultyMultiplier;
        base.Initialise();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Update to use playerTransform
    }
}