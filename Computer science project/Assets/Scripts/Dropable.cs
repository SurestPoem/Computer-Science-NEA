using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropable : MonoBehaviour
{
    public static float pickupSpeed = 4.5f; // Speed at which the item moves toward the player
    public Transform playerTransform; // Reference to the player for movement
    public Player player;
    private bool isMovingTowardPlayer = false; // Flag to indicate the item should move
    private static float pickupDistanceThreshold = 0.5f; // Distance at which the item "reaches" the player

    // Start is called before the first frame update
    protected void Start()
    {
        // Find player by tag
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = FindObjectOfType<Player>(); // Find the Player object in the scene

    }

    // Update is called once per frame
    protected void Update()
    {
        if (isMovingTowardPlayer)
        {
            MoveTowardPlayer(); // Move the item
        }
    }

    // OnTriggerEnter2D is called when the player enters the item's trigger collider
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) // Only start moving if the player is the one triggering
        {
            isMovingTowardPlayer = true; // Start the movement when the player enters the trigger
        }
    }

    // This method moves the item towards the player
    private void MoveTowardPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized; // Calculate direction to the player
        transform.position += direction * pickupSpeed * Time.deltaTime; // Move the item

        // Check if the item is close enough to the player
        if (Vector3.Distance(transform.position, playerTransform.position) <= pickupDistanceThreshold)
        {
            OnPickup(); // Trigger pickup behavior
            Destroy(gameObject); // Destroy item once it reaches the player
        }
    }
    protected virtual void OnPickup()
    {
        Debug.Log(name + " item picked up");
        //Override this in subclasses to suit individual pickup items need
    }
}
