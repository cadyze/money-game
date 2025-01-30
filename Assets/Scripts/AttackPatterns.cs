using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewAttackPattern", menuName = "Attack Pattern")]
public class AttackPattern : ScriptableObject
{
    public BulletDetails[] bulletsToSpawn;
    public AttackType attackType;
    public float fireRate; // Only used for Spray attacks
    public float duration; // Only used for Spray attacks

    public void SpawnBullets(Vector3 posToSpawnAt, GameObject bulletPrefab, MonoBehaviour caller)
    {
        if (attackType == AttackType.Burst)
        {
            SpawnBulletBatch(posToSpawnAt, bulletPrefab);
        }
        else if (attackType == AttackType.Spray)
        {
            caller.StartCoroutine(SprayAttack(posToSpawnAt, bulletPrefab));
        }
    }

    private void SpawnBulletBatch(Vector3 posToSpawnAt, GameObject bulletPrefab)
    {
        foreach (BulletDetails bullet in bulletsToSpawn)
        {
            GameObject bulletInstance = Instantiate(bulletPrefab, posToSpawnAt, Quaternion.identity);
            BulletController bulletController = bulletInstance.GetComponent<BulletController>();

            float randomizedSpeed = bullet.bulletSpeed + Random.Range(1 - bullet.bulletSpeedRandomness, 1 + bullet.bulletSpeedRandomness);
            float randomizedAngle = bullet.angleFromUp + Random.Range(1 - bullet.angleFromUpRandomness, 1 + bullet.angleFromUpRandomness);
            float randomizedDecay = bullet.decayFactor + Random.Range(1 - bullet.decayFactorRandomness, 1 + bullet.decayFactorRandomness);
            float randomizedLifeTime = bullet.bulletTimeAlive + Random.Range(1 - bullet.bulletTimeAliveRandomness, 1 + bullet.bulletTimeAliveRandomness);

            Vector2 direction = Quaternion.Euler(0, 0, randomizedAngle) * Vector2.up;
            Debug.Log(direction);
            bulletController.UpdateBullet(direction, 10f, randomizedSpeed, randomizedDecay, randomizedLifeTime, false);
        }
    }

    private IEnumerator SprayAttack(Vector3 posToSpawnAt, GameObject bulletPrefab)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            SpawnBulletBatch(posToSpawnAt, bulletPrefab);
            yield return new WaitForSeconds(fireRate);
            elapsedTime += fireRate;
        }
    }
}

public enum AttackType
{
    Burst,
    Spray
}

[System.Serializable]
public class BulletDetails
{
    [Tooltip("Angle offset from the upward direction in degrees")]
    public float angleFromUp;
    [Tooltip("Randomness applied to angle offset")]
    public float angleFromUpRandomness;

    [Tooltip("Speed of the bullet")]
    public float bulletSpeed;
    [Tooltip("Randomness applied to bullet speed")]
    public float bulletSpeedRandomness;

    [Tooltip("Time before the bullet despawns")]
    public float bulletTimeAlive;
    [Tooltip("Randomness applied to bullet lifetime")]
    public float bulletTimeAliveRandomness;

    [Tooltip("Rate at which speed decreases over time (0 means no decay)")]
    public float decayFactor;
    [Tooltip("Randomness applied to decay factor")]
    public float decayFactorRandomness;
}