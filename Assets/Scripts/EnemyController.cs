using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public float health;
    public List<AttackPattern> attackPatterns;
    public float timeBetweenAttacks;
    public GameObject bulletPrefab;

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);
            UseRandomAttack();
        }
    }

    private void UseRandomAttack()
    {
        if (attackPatterns.Count > 0)
        {
            AttackPattern selectedPattern = attackPatterns[Random.Range(0, attackPatterns.Count)];
            selectedPattern.SpawnBullets(transform.position, bulletPrefab, this);
        }
    }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }

    private void OnHit(BulletController bullet)
    {
        health -= bullet.bulletDamage;
        Destroy(bullet.gameObject);

        CheckHealth();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            GameObject cGameObject = collision.gameObject;
            if (cGameObject.CompareTag("Bullet"))
            {
                BulletController bullet = cGameObject.GetComponent<BulletController>();
                if (bullet.fromPlayer)
                {
                    OnHit(bullet);
                }
            }
        }
    }
}
