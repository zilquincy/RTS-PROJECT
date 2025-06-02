using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private float unitHealth;
    public float unitMaxHealth;

    public HealthTracker healthTracker;
    void Start()
    {
        UnitSelectManager.Instance.allUnitsList.Add(gameObject);

        unitHealth = unitMaxHealth;
        updateHealthUI  ();
    }

    

    private void OnDestroy()
    {
        UnitSelectManager.Instance.allUnitsList.Remove(gameObject);
    }
    private void updateHealthUI()
    {
        healthTracker.UpdateSliderValue(unitHealth, unitMaxHealth);

        if (unitHealth <= 0)
        {
            //dying logic

            Destroy(gameObject);


        }
    }
    internal void TakeDamage(int damageToInflict)
    {
        unitHealth -= damageToInflict;

        updateHealthUI();
    }
}
