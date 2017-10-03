using System.Collections.Generic;
using UnityEngine;

public class MJPlayer : MonoBehaviour
{
    private List<IWeapon> activeWeaponList;

    private int totalScore;

    private PlaqueManager plaque;
    
    void Awake()
    {
        activeWeaponList = new List<IWeapon>();
        plaque = FindObjectOfType<PlaqueManager>();
    }

    void Start()
    {
        var weapons = GetComponentsInChildren<IWeapon>();
        foreach (var weapon in weapons)
        {
            activeWeaponList.Add(weapon);
        }
        plaque.SetScore(totalScore);
    }

    public void AddScore(int score)
    {
        totalScore += score;
        plaque.SetScore(totalScore);
    }

}