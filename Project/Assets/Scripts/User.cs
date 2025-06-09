using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class User : Player
{
    [SerializeField] SoundData gainKillSound;
    TMP_Text killCountText;
    TMP_Text killedText;
    Camera cam;

    public override void Awake()
    {
        base.Awake();
        GameObject.Find("MenuUI").transform.GetChild(0).gameObject.SetActive(false);
        killCountText = GameObject.Find("KillCount").GetComponent<TMP_Text>();
        killedText = GameObject.Find("KilledTextHolder").transform.GetChild(0).GetComponent<TMP_Text>();

        UserSkillManager usm = FindObjectOfType<UserSkillManager>();
        usm.user = this;
        sm = usm;

        cam = Camera.main;
        cam.GetComponent<CameraFollowWithBarriers>().followObject = transform;
    }

    public override void Update()
    {
        base.Update();

        Vector2 inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.linearVelocity = Vector2.zero;

        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            usernameHolder.gameObject.SetActive(!usernameHolder.gameObject.activeSelf);
        }

        Aim(cam.ScreenToWorldPoint(Input.mousePosition));

        rb.linearVelocity = inputs * (SkillManager.BASE_SPEED + (sm.speed.Value / 5f));
    }

    IEnumerator KilledText()
    {
        killedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        killedText.gameObject.SetActive(false);
    }

    public override void Killed(int exp, Player player = null)
    {
        base.Killed(exp, player);

        if (player)
        {
            //Increase kill counter
            killCountText.text = $"Kills: {kills}";
            //Shot on screen who you killed
            killedText.text = $"Killed {player.username}";
            //special sound effect play
            a.PlayOneShot(gainKillSound.sound.RandomSound());
            StopCoroutine(KilledText());
            StartCoroutine(KilledText());
        }
    }

    public override void Die()
    {
        GameObject.Find("MenuUI").transform.GetChild(0).gameObject.SetActive(true);
        base.Die();
    }
}
