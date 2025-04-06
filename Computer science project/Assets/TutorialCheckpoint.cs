using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCheckpoint : MonoBehaviour
{
    public LevelManager levelManager;
    public bool isTriggered = false;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        isTriggered = false;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isTriggered)
        {
            if (collider.CompareTag("Player"))
            {
                isTriggered = true;
                levelManager.EndStage();
            }
        }
    }
}
