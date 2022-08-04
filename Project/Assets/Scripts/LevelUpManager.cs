using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    [SerializeField] LevelUpData[] levels;
    static LevelUpManager instance;

    public static LevelUpManager I
    {
        get
        {
            if (instance == null)
            {
                instance = new LevelUpManager();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance == this) { return; }
        else if (this != instance)
        {
            Destroy(gameObject);
            return;
        }
    }

    public LevelUpData GetLevel(int level)
    {
        return levels[Mathf.Clamp(level - 1, 0, levels.Length - 1)];
    }
}