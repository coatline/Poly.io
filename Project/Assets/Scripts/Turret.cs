using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] SoundData fireClip;
    [SerializeField] Transform muzzle;
    [SerializeField] AudioSource a;
    Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    float timer;

    public void Shoot(float damage, int shotRate, float range)
    {
        if (timer > SkillManager.BASE_SHOTRATE - (float)(Mathf.Clamp(shotRate / 75f, 0, SkillManager.BASE_SHOTRATE - .1f)))
        {
            Fire(damage, range);
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void Fire(float damage, float range)
    {
        a.PlayOneShot(fireClip.sound.RandomSound());
        var bullet = Instantiate(bulletPrefab, muzzle.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * ((player.sm.shotForce.Value * 25) + SkillManager.BASE_SHOTFORCE));
        bullet.damage = damage;
        bullet.range = range;
        bullet.player = player;
    }
}
