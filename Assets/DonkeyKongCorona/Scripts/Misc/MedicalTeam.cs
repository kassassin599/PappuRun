using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalTeam : MonoBehaviour
{
    public void PickUp()
    {
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();

        foreach (EnemyAI enemy in enemies)
        {
            if (enemy.reachedToEnemy)
            {
                enemy.PickedUp();
            }
        }
    }
}
