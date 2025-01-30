using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private enum PlayerState { Normal, Rolling }
    private PlayerState currentState = PlayerState.Normal;

    private Rigidbody2D playerRb;
    private Vector2 input = Vector2.zero;
    private Vector2 rollDirection = Vector2.zero;
    private Interactable[] interactables;
    private bool canFire = true;
    private bool isInvulnerable = false;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float interactRange = 7f;

    private PlayerStats playerStats;
    private float originalAttackPower;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>(); // Ensure PlayerStats is on the same GameObject
        originalAttackPower = playerStats.attackPower;
    }

    private void Update()
    {
        if (currentState == PlayerState.Normal)
        {
            playerRb.linearVelocity = input.normalized * playerStats.playerSpeed;
        }
        interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None);
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }

    public void OnRoll(InputValue value)
    {
        if (currentState == PlayerState.Normal && input.sqrMagnitude > 0)
        {
            rollDirection = input.normalized;
            StartCoroutine(RollCoroutine());
        }
    }

    private IEnumerator RollCoroutine()
    {
        GetComponent<SpriteRenderer>().color = Color.blue;
        currentState = PlayerState.Rolling;
        isInvulnerable = true;

        float originalSpeed = playerStats.playerSpeed * playerStats.rollSpeedMultiplier;
        float minSpeed = playerStats.playerSpeed * 0.1f;
        float elapsedTime = 0f;

        while (elapsedTime < playerStats.rollDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / playerStats.rollDuration;
            float rollSpeed = Mathf.Lerp(originalSpeed, minSpeed, t * t);

            playerRb.linearVelocity = rollDirection * rollSpeed;
            yield return null;
        }

        isInvulnerable = false;
        currentState = PlayerState.Normal;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnInteract(InputValue value)
    {
        Interactable nearestInteractable = FindNearestInteractable();
        if (nearestInteractable != null)
        {
            Debug.Log("GO");
            nearestInteractable.Interact();
        }
    }

    private Interactable FindNearestInteractable()
    {
        Interactable closest = null;
        float closestDistance = interactRange;

        foreach (Interactable interactable in interactables)
        {
            float distance = Vector2.Distance(transform.position, interactable.transform.position);
            if (distance < closestDistance)
            {
                closest = interactable;
                closestDistance = distance;
            }
        }
        return closest;
    }

    public void OnAttack(InputValue value)
    {
        if (canFire)
        {
            Vector3 playerPos = transform.position;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0;

            GameObject bullet = Instantiate(bulletPrefab, playerPos, Quaternion.identity);
            bullet.GetComponent<BulletController>().UpdateBullet(
                (mousePos - playerPos).normalized,
                playerStats.attackPower,
                playerStats.bulletSpeed,
                0,
                5f,
                true
            );

            bullet.GetComponent<Rigidbody2D>().linearVelocity = (mousePos - playerPos).normalized * playerStats.bulletSpeed;

            StartCoroutine(FireCooldown());
        }
    }

    private IEnumerator FireCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(playerStats.fireRate);
        canFire = true;
    }

    /// <summary>
    /// Temporarily increases player's attack power.
    /// </summary>
    public IEnumerator IncreaseDamage(float multiplier, float duration)
    {
        Debug.Log("Increased Damage!");
        playerStats.attackPower *= multiplier;
        yield return new WaitForSeconds(duration);
        playerStats.attackPower = originalAttackPower;
        Debug.Log("Damage Reset!");
    }

    /// <summary>
    /// Temporarily grants the player invulnerability.
    /// </summary>
    public IEnumerator ActivateShield(float duration)
    {
        Debug.Log("Shield Activated!");
        isInvulnerable = true;
        GetComponent<SpriteRenderer>().color = Color.cyan;
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        Debug.Log("Shield Deactivated!");
    }

    /// <summary>
    /// Destroys all active bullets in the scene.
    /// </summary>
    public void ClearBullets()
    {
        Debug.Log("Clearing all bullets...");
        BulletController[] bullets = FindObjectsByType<BulletController>(FindObjectsSortMode.None);
        foreach (BulletController bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }
    }
}
