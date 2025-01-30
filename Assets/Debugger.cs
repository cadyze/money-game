using TMPro;
using UnityEngine;

public class Debugger : MonoBehaviour
{

    private PlayerStats playerStats;
    private TextMeshProUGUI tUGUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
        tUGUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        string debugOutput = "";
        debugOutput += $"HEALTH: {playerStats.health}\n";
        debugOutput += $"MONEY: {playerStats.money}\n";
        debugOutput += $"POWER: {playerStats.attackPower}\n";
        debugOutput += $"FIRERATE: {playerStats.fireRate}\n";

        tUGUI.text = debugOutput;
    }
}

