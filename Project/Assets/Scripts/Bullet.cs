using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Player player;
    [SerializeField] ParticleSystem ps;
    [HideInInspector] public float damage;
    [HideInInspector] public float range;
    [SerializeField] SoundData hitCip;
    [SerializeField] AudioSource a;
    Vector2 startingPos;

    private void Awake()
    {
        startingPos = transform.position;
    }

    void Die()
    {
        ps.Emit(5);
        ps.transform.parent = null;
        ps.transform.localScale = Vector3.one;

        gameObject.SetActive(false);
        Invoke("Dest", .5f);
    }

    void Dest()
    {
        Destroy(ps.gameObject);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Entity hit = collision.gameObject.GetComponent<Entity>();
        if (hit)
        {
            if (hit == player) { return; }

            int expGiven = hit.ExpGiven;

            if (hit.TakeDamage(damage) && player)
            {
                Player killed = hit.GetComponent<Player>();
                this.player.Killed(expGiven, killed);
            }
        }

        a.PlayOneShot(hitCip.sound.RandomSound());
        Die();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, startingPos) > range)
        {
            Dest();
        }
    }
}
