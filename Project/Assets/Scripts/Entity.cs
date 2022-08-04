using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [HideInInspector] public Canvas worldSpaceCanvas;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] Vector3 healthBarScale;
    [SerializeField] Image healthBarPrefab;
    [SerializeField] ProceduralBump pb;
    [SerializeField] SoundData dieSound;
    public AudioSource a;
    Image healthBar;

    public float maxHealth;
    public int baseExpGiven;
    float targetFill;
    float health;

    public virtual void Awake()
    {
        if (healthBarPrefab)
        {
            worldSpaceCanvas = GameObject.Find("WorldSpaceCanvas").GetComponent<Canvas>();
            healthBarPrefab.rectTransform.sizeDelta = healthBarScale;
            healthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, worldSpaceCanvas.transform);
        }

        health = maxHealth;
    }

    public virtual int ExpGiven
    {
        get
        {
            return baseExpGiven;
        }
    }

    public virtual float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            if (health <= 0)
            {
                Die();
            }
            else if (health > maxHealth) { health = maxHealth; }

            if (healthBar)
            {
                targetFill = 1 - ((float)Health / (float)maxHealth);
            }
        }
    }

    public bool TakeDamage(float dmg)
    {
        if (deathParticles) { deathParticles.Emit(2); }
        if (pb) { pb.Bump(); }

        Health -= dmg;

        if (Health <= 0)
        {
            return true;
        }

        return false;
    }

    public virtual void Update()
    {
        if (healthBar)
        {
            healthBar.transform.position = transform.position;
            healthBar.transform.rotation = transform.rotation;

            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFill, Time.deltaTime * 4.5f);
        }

        if (deathParticles)
        {
            deathParticles.transform.rotation = Quaternion.identity;
        }
    }

    public virtual void Die()
    {
        if (deathParticles)
        {
            deathParticles.Emit(12);
            deathParticles.transform.parent = null;
            deathParticles.transform.localScale = Vector3.one;
        }

        if (healthBar)
        {
            Destroy(healthBar.gameObject);
        }

        a.PlayOneShot(dieSound.sound.RandomSound());
        StopAllCoroutines();
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Invoke("Destruct", .5f);
    }

    void Destruct()
    {
        Destroy(deathParticles.gameObject);
        Destroy(gameObject);
    }
}
