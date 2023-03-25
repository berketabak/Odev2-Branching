using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;

    public int healPoint;
    public int maxHP;
    public int currentHP;

    private void Update()
    {
        
    }

    public bool TakeDamage(int dmg)
    {
        dmg = (dmg+unitLevel);
       // dmg = Random.Range(dmg - 3 + unitLevel, dmg + 3 + unitLevel);
       // Debug.Log("Damage point:" + dmg);
        currentHP -= dmg;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal (int amount)
    {
        amount = amount + 5;
       // amount = Random.Range(amount - 5, amount + 5);
        currentHP += amount;

        if (currentHP >= maxHP)
            currentHP = maxHP;
    }
}
