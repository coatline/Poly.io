using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSkillManager : SkillManager
{
    [HideInInspector] public User user;
    [SerializeField] GameObject holder;
    bool reset;

    public override int SkillPoints
    {
        get
        {
            return base.SkillPoints;
        }
        set
        {
            base.SkillPoints = value;

            if (value > 0)
            {
                holder.SetActive(true);
            }
            else
            {
                holder.SetActive(false);
            }
        }
    }


    private void Update()
    {
        if (!user)
        {
            if (!reset)
            {
                for (int i = 0; i < attributes.Count; i++)
                {
                    attributes[i].Value = 0;
                }

                SkillPoints = 0;
                reset = true;
            }
        }
        else
        {
            reset = false;
        }
    }

    public void AddDamage() { if (SkillPoints <= 0) { return; } SkillPoints--; damage.Value++; }
    public void AddHealth() { if (SkillPoints <= 0) { return; } SkillPoints--; health.Value++; }
    public void AddMovementSpeed() { if (SkillPoints <= 0) { return; } SkillPoints--; speed.Value++; }
    public void AddRateOfFire() { if (SkillPoints <= 0) { return; } SkillPoints--; rateOfFire.Value++; }
    public void AddShotForce() { if (SkillPoints <= 0) { return; } SkillPoints--; shotForce.Value++; }
    public void AddRegeneration() { if (SkillPoints <= 0) { return; } SkillPoints--; regeneration.Value++; }
}