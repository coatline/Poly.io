using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [HideInInspector] public List<Attribute> attributes;
    public const int BASE_SHOTFORCE = 260;
    public const float BASE_SHOTRATE = .65f;
    public const float BASE_SPEED = 2.4f;
    public const float BASE_HEALTH = 6;
    public const float BASE_DAMAGE = 1;
    public const float BASE_RANGE = 7;

    public Attribute regeneration;
    public Attribute rateOfFire;
    public Attribute shotForce;
    public Attribute health;
    public Attribute speed;
    public Attribute damage;

    int sp;

    public virtual int SkillPoints
    {
        get
        {
            return sp;
        }
        set
        {
            sp = value;
        }
    }

    private void Start()
    {
        attributes.Add(regeneration);
        attributes.Add(rateOfFire);
        attributes.Add(shotForce);
        attributes.Add(health);
        attributes.Add(damage);
        attributes.Add(speed);
    }

    public Attribute GetRandAttribute()
    {
        return attributes[Random.Range(0, attributes.Count)];
    }
}

[System.Serializable]
public class Attribute
{
    [SerializeField] TMP_Text countText;
    [SerializeField] int val;

    public int Value
    {
        get
        {
            return val;
        }
        set
        {
            val = value;

            if (countText)
            {
                countText.text = $"{val}";
            }
        }
    }
}