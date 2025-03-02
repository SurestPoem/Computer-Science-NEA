using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    
    public Rigidbody2D rb2d;
    private Vector2 moveInput;
    private Animator animator;
    private PlayerControls controls;
    public SpriteRenderer playerSpriteRenderer;

    public int killCount = 0;
    [Header("Level + Currency")]
    public int level = 0;
    public int currentXP = 0;
    public int xpToLevelUp = 100;
    public int currentCurrency = 0;

    [Header("Weapon handling")]
    public Gun currentGun;
    public Transform gunHolder;
    public List<Gun> avalibleWeapons;

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        controls = new PlayerControls();
        controls.Player.Enable(); // Enable the controls
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
        PlayerMovement();
        HandleShooting();
        HandleAiming();
        HandleHealthRegen();
    }

    private void PlayerMovement()
    {
        if (IsFreeze != true)
        {

            moveInput = controls.Player.Move.ReadValue<Vector2>(); // Get movement input from either keyboard or controller
            moveInput.Normalize();

            rb2d.velocity = moveInput * moveSpeed;
            animator.SetFloat("moveSpeed", moveInput.magnitude);

            if (moveInput.x < 0)
            {
                playerSpriteRenderer.flipX = true;
            }
            else if (moveInput.x > 0)
            {
                playerSpriteRenderer.flipX = false;
            }

        }
    }


        private void HandleShooting()
    {
        if (currentGun != null && controls.Player.Shoot.triggered)
        {
            currentGun.Shoot();
        }
    }

    private void HandleAiming()
    {
        Vector2 aimDirection = controls.Player.Aim.ReadValue<Vector2>();  // Get aim input (mouse or controller right stick)

        // Convert to world space
        Vector3 shootPosition = (aimDirection != Vector2.zero) ? (Vector3)aimDirection : Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        currentGun.RotateAndPositionGun();
    }

    private void HandleHealthRegen()
    {
        regenTimer += Time.deltaTime;

        if (regenTimer >= 1f) // Run every 1 second
        {
            regenTimer = 0f;
            Heal(regenRate); // 1 regen = 0.2 HP per sec
        }
    }

    public void IncreaseStats(string statName, float value)
    {
        if (statName == "maxHealth")
        {
            maxHealth += value;
        }
        else if (statName == "moveSpeed")
        {
            moveSpeed += value;
        }
        else if (statName == "regenRate")
        {
            regenRate += value;
        }
        else
        {
            Debug.LogError("Unknown stat name: " + statName);
        }
    }

    public void SwitchGun(Gun newGun)
    {
        currentGun = newGun;
        Debug.Log("Switched to new gun: " + newGun.name);
    }

    public override void Die()
    {
        Debug.Log("Player has Died");
        //Add specfic player death code
    }

    public void IncreaseKills()
    {
        killCount++;
        Debug.Log("Kill count updated to " + killCount);
    }

    public void EarnXP(int xpGained)
    {
        currentXP += xpGained;

        while (currentXP >= xpToLevelUp) // Check for multiple level-ups
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        level++;
        Debug.Log("Level increased to " + level);

        currentXP -= xpToLevelUp; // Carry over excess XP
        xpToLevelUp = Mathf.CeilToInt(xpToLevelUp * 1.05f); // Increase XP needed for next level
    }

    public void EarnCurrency(int currencyGained)
    {
        currentCurrency += currencyGained;
    }
}
