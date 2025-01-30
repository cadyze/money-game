using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletDamage;
    public float bulletSpeed;
    public float decayFactor;
    public float bulletLifetime;
    public bool fromPlayer;

    private Vector2 direction;
    private float currentLifetime;

    public void UpdateBullet(Vector2 direction, float bulletDamage, float bulletSpeed, float decayFactor, float bulletLifetime, bool fromPlayer)
    {
        this.direction = direction;
        this.bulletDamage = bulletDamage;
        this.bulletSpeed = bulletSpeed;
        this.decayFactor = decayFactor;
        this.bulletLifetime = bulletLifetime;
        this.fromPlayer = fromPlayer;
        currentLifetime = 0f;

        // Update velocity and shot
        GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;

        // Calculate the angle between the upward direction (Vector2.up) and the target direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the sprite
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void Update()
    {
        // Decrease speed over time based on decayFactor
        if (decayFactor > 0)
        {
            bulletSpeed = Mathf.Max(0, bulletSpeed - decayFactor * Time.deltaTime);
            GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
        }

        // Destroy bullet after lifetime expires
        currentLifetime += Time.deltaTime;
        if (currentLifetime >= bulletLifetime)
        {
            Destroy(gameObject);
        }
    }
}
