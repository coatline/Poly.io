using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level ", menuName = "Level Up")]

public class LevelUpData : ScriptableObject
{
    public int expCapAdded;

    public int skillPoints;
    public int newTurrets;
}
