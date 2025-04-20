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
    protected virtual void Start()
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

    //Called when the player enters the item's trigger collider  (Set this trigger collider as a few times larger than the item actual size itself)
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) 
        {
            isMovingTowardPlayer = true; 
        }
    }

    // This method moves the item towards the player
    private void MoveTowardPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized; //Calculates the direction towards the player
        transform.position += direction * pickupSpeed * Time.deltaTime; //Moves items towards the direction

        // Check if the item is close enough to the player
        if (Vector3.Distance(transform.position, playerTransform.position) <= pickupDistanceThreshold)
        {
            OnPickup();
            Destroy(gameObject); // Destroys item
        }
    }
    protected virtual void OnPickup()
    {
        Debug.Log(name + " item picked up");
        //Overrided in subclasses to suit the item needs
    }
}
