using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    [SerializeField] ParticleSystem levelUpParticles;
    [SerializeField] TMP_Text usernameTextPrefab;
    [SerializeField] TMP_Text levelTextPrefab;
    [SerializeField] Turret turretPrefab;
    [SerializeField] Color levelUpColor;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Color baseColor;
    public Transform usernameHolder;
    TMP_Text usernameText;
    public Rigidbody2D rb;
    List<Turret> turrets;
    TMP_Text levelText;
    Color targetColor;

    public SkillManager sm;

    [HideInInspector] public string username;
    [HideInInspector] public int kills;
    int currentExpCap;
    int level;
    int exp;

    public override int ExpGiven
    {
        get
        {
            return baseExpGiven + (currentExpCap * 2);
        }
    }

    public override float Health
    {
        get
        {
            return base.Health;
        }
        set
        {
            base.Health = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;

            LevelUpData data = LevelUpManager.I.GetLevel(level);

            currentExpCap += data.expCapAdded;

            if (level > 1 && levelUpParticles)
            {
                levelUpParticles.Play();
                Health += maxHealth / 4f;
            }

            for (int i = 0; i < data.newTurrets; i++)
            {
                AddTurretAt(Vector3.zero, true);
            }

            levelText.text = level.ToString();
            sm.SkillPoints += data.skillPoints;
        }
    }

    int Experience
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;

            if (exp >= currentExpCap)
            {
                exp -= currentExpCap;
                Level++;
            }

            targetColor = Color.Lerp(baseColor, levelUpColor, (float)exp / (float)currentExpCap);
        }
    }

    public virtual void Start()
    {
        targetColor = baseColor;
        usernameHolder = worldSpaceCanvas.transform.GetChild(0);
        usernameText = Instantiate(usernameTextPrefab, transform.position, Quaternion.identity, usernameHolder);
        levelText = Instantiate(levelTextPrefab, transform.position, Quaternion.identity, worldSpaceCanvas.transform);
        usernameText.text = username.ToString();

        turrets = new List<Turret>();
        maxHealth = SkillManager.BASE_HEALTH;
        Health = maxHealth;

        AddTurretAt(transform.position + transform.up);

        Level = 1;
        StartCoroutine(Regen());
    }

    public override void Update()
    {
        base.Update();

        sr.color = Color.Lerp(sr.color, targetColor, Time.deltaTime * 3.5f);

        if (usernameText)
        {
            usernameText.transform.position = transform.position;
        }

        if (levelText)
        {
            levelText.transform.position = transform.position;
        }

        if (sm.health.Value > prevHealthAtt)
        {
            AddHealth();
        }

        prevHealthAtt = sm.health.Value;
    }

    int prevHealthAtt;

    public void AddHealth()
    {
        maxHealth += 2;
        Health += 2;
    }

    public void AddTurretAt(Vector3 facingPos, bool random = false)
    {
        if (random)
        {
            facingPos = transform.position + new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y);
        }

        turrets.Add(Instantiate(turretPrefab, transform.position + ((facingPos - transform.position).normalized) / 2.5f, Quaternion.Euler(0, 0, Extensions.AngleFromPosition(transform.position, facingPos)), transform));
    }

    public void Shoot()
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            turrets[i].Shoot(SkillManager.BASE_DAMAGE + (sm.damage.Value / 7f), sm.rateOfFire.Value, SkillManager.BASE_RANGE);
        }
    }

    IEnumerator Regen()
    {
        yield return new WaitForSeconds(9f / (Mathf.Clamp(sm.regeneration.Value / 4f, 1.2f, 100)));
        Health += 1 + (((int)(sm.regeneration.Value / 5)));
        StartCoroutine(Regen());
    }

    public override void Die()
    {
        if (usernameText)
        {
            Destroy(usernameText.gameObject);
        }

        Destroy(levelText.gameObject);
        base.Die();
    }

    public virtual void Killed(int exp, Player player = null)
    {
        Experience += exp;

        if (player)
        {
            kills++;
        }
    }

    public void Aim(Vector3 pos)
    {
        transform.rotation = Quaternion.Euler(0, 0, Extensions.AngleFromPosition(transform.position, pos));
    }
}
