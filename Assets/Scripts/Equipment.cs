using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "ActiveEquipment")]
public class Equipment : ScriptableObject
{
    [Header("Equipment Info")]
    public string equipmentName;
    public Sprite equipmentImage;
    public float abilityDuration;
    public float cooldownTime;
    [TextArea] public string equipmentDescription;

    [Header("Ability")]
    public AbilityType[] abilityType;

    private bool isOnCooldown = false;

    public void ActivateAbility(GameObject user)
    {
        if (isOnCooldown)
        {
            Debug.Log("Ability is on cooldown!");
            return;
        }

        PlayerController player = user.GetComponent<PlayerController>();
        PlayerStats stats = user.GetComponent<PlayerStats>();

        if (player == null || stats == null) return;

        foreach (var ability in abilityType)
        {
            switch (ability)
            {
                case AbilityType.Heal:
                    stats.health += 1;
                    Debug.Log("Player healed!");
                    break;

                case AbilityType.Shield:
                    player.StartCoroutine(player.ActivateShield(abilityDuration));
                    break;

                case AbilityType.IncDamage:
                    player.StartCoroutine(player.IncreaseDamage(2f, abilityDuration)); // Double damage for the duration
                    break;

                case AbilityType.ClearBullets:
                    player.ClearBullets();
                    break;

                default:
                    Debug.LogWarning($"Ability {ability} not implemented!");
                    break;
            }
        }

        user.GetComponent<MonoBehaviour>().StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
}

public enum AbilityType
{
    None,
    Heal,
    Shield,
    IncDamage,
    IncSpeed,
    ClearBullets,
    FireBigBullet
}
