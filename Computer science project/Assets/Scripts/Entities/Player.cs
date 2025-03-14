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
    public CrosshairController crosshair;

    public int killCount = 0;
    [Header("Level + Currency")]
    public int level = 0;
    public int currentXP = 0;
    public int xpToLevelUp = 100;
    public int currentCurrency = 0;

    [Header("Weapon handling")]
    public Gun currentGun;
    public List<GameObject> avalibleWeapons;
    [SerializeField] private int currentGunIndex = 0;


    // Update is called once per frame
    protected void Update()
    {
        base.Update();
        PlayerMovement();
        HandleShooting();
        HandleAiming();
        HandleHealthRegen();
        HandleWeaponSwitching();
        PlayerOpenShop();
        PlayerOpenPauseScreen();
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


    private void HandleWeaponSwitching()
    {
        if (avalibleWeapons.Count <= 1) return; // Prevent errors if no weapons exist

        if (controls.Player.WeaponLeft.triggered)
        {
            currentGunIndex = (currentGunIndex - 1 + avalibleWeapons.Count) % avalibleWeapons.Count;
            SwitchGun();
        }

        if (controls.Player.WeaponRight.triggered)
        {
            currentGunIndex = (currentGunIndex + 1) % avalibleWeapons.Count;
            SwitchGun();
        }
    }

    public void PlayerOpenShop()
    {
        if (controls.Player.OpenShop.triggered)
        {
            Debug.Log("Shop button pressed");
            if (GameManager.Instance.shopEnabled == true)
            {
                GameManager.Instance.DisableShop();
            }
            else
            {
                GameManager.Instance.EnableShop();
            }
        }
    }

    public void PlayerOpenPauseScreen()
    {
        if (controls.Player.Pause.triggered)
        {
            Debug.Log("Pause button pressed");
            if (GameManager.Instance.pauseScreenEnabled == true)
            {
                GameManager.Instance.DisablePauseScreen();
            }
            else
            {
                GameManager.Instance.EnablePauseScreen();
            }
        }
    }

    public void SwitchGun()
    {
        if (currentGun != null)
        {
            Destroy(currentGun.gameObject);
        }

        // Instantiate the new gun from the list
        GameObject newGunObject = Instantiate(avalibleWeapons[currentGunIndex], transform.position, Quaternion.identity, transform);
        currentGun = newGunObject.GetComponent<Gun>();

        // Make sure to assign the crosshairTransform (and any other properties you need to assign)
        if (currentGun != null)
        {
            GameObject crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshair != null)
            {
                currentGun.crosshairTransform = crosshair.transform;
            }
            else
            {
                Debug.LogError("Crosshair not found!");
            }
        }

        Debug.Log("Switched to new gun: " + avalibleWeapons[currentGunIndex]);
    }

    public void AddGun(GameObject gun)
    {
        if (gun != null)
        {
            avalibleWeapons.Add(gun.gameObject); // Add the GameObject of the Gun to the list
            Debug.Log("Added new gun: " + gun.name); 
        }
        else
        {
            Debug.LogError("Attempted to add a null gun.");
        }
    }

    private void HandleAiming()
    {
        // Get the crosshair's position in world space
        Vector3 crosshairWorldPosition = crosshair.transform.position;

        // Pass the crosshair position to the gun's aiming function
        currentGun.RotateAndPositionGun(crosshairWorldPosition);
    }


    private void HandleShooting()
    {
        if (currentGun != null && controls.Player.Shoot.triggered)
        {
            currentGun.Shoot();
        }
    }

    private void HandleHealthRegen()
    {
        regenTimer += Time.deltaTime;

        if (regenTimer >= 1f) // Run every 1 second
        {
            regenTimer = 0f;
            Heal(regenRate); 
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


    public override void Die()
    {
        Debug.Log("Player has Died");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnableDeathScreen();
        }
        else
        {
            Debug.LogError("GameManager instance is not initialized!");
        }
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

    public void UseCurrency(int currencyLost)
    {
        currentCurrency -= currencyLost;
    }


    protected override void Initialise() //called in the Entitys Start() method
    {
        base.Initialise();
        animator = GetComponent<Animator>();
        controls = new PlayerControls();
        controls.Player.Enable(); // Enable the controls
    }
}


/*   private void HandleWeaponSwitching()
    {
        if (controls.Player.WeaponLeft.triggered)
        {
            currentGunIndex--;

            // Wrap around if out of bounds
            if (currentGunIndex < 0)
            {
                currentGunIndex = avalibleWeapons.Count - 1; // Go to last weapon
            }

            SwitchGun();
        }

        if (controls.Player.WeaponRight.triggered)
        {
            currentGunIndex++;

            // Wrap around if out of bounds
            if (currentGunIndex >= avalibleWeapons.Count)
            {
                currentGunIndex = 0; // Go to first weapon
            }

            SwitchGun();
        }
    }
*/