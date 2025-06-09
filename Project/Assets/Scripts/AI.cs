using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player
{
    enum State
    {
        movingTowardPlayer,
        movingAwayFromPlayer,
        farming
    }

    [SerializeField] GameObject eyes;
    List<Player> playersLeaving;
    List<GameObject> seenFarms;
    List<Player> seenPlayers;
    GameObject currentFarm;
    Player targetPlayer;
    Vector2Int mapSize;
    State state;

    public override void Awake()
    {
        base.Awake();

        StartCoroutine(LockOntoLowestHealthInInterval());
        StartCoroutine(ChangeMovementAndLeadAmountOffset());
        StartCoroutine(Blink());

        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        mapSize = new Vector2Int(Litterbug.MAPX, Litterbug.MAPY);
        playersLeaving = new List<Player>();
        seenFarms = new List<GameObject>();
        seenPlayers = new List<Player>();
        state = State.farming;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player && !seenPlayers.Contains(player) && player != this)
            {
                seenPlayers.Add(player);
            }

            if (playersLeaving.Contains(player) && gameObject.activeSelf)
            {
                StopCoroutine(ForgetPlayer(4f, player));
            }
        }
        else if (state == State.farming && collision.gameObject.CompareTag("Farm"))
        {
            seenFarms.Add(collision.gameObject);

            if (!currentFarm)
            {
                currentFarm = collision.gameObject;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (seenPlayers.Count == 0)
            {
                Player player = collision.gameObject.GetComponent<Player>();

                if (player && !seenPlayers.Contains(player) && player != this)
                {
                    seenPlayers.Add(player);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.activeSelf)
        {
            Player p = collision.gameObject.GetComponent<Player>();

            playersLeaving.Add(p);
            StartCoroutine(ForgetPlayer(4f, p));
        }
        else if (collision.gameObject.CompareTag("Farm"))
        {
            if (seenFarms.Contains(collision.gameObject))
            {
                seenFarms.Remove(collision.gameObject);
            }
        }
    }

    IEnumerator Blink()
    {
        yield return new WaitForSeconds(.5f);
        eyes.SetActive(false);
        yield return new WaitForEndOfFrame();
        eyes.SetActive(true);
        StartCoroutine(Blink());
    }

    IEnumerator ForgetPlayer(float time, Player player)
    {
        yield return new WaitForSeconds(time);
        playersLeaving.Remove(player);
        seenPlayers.Remove(player);

        if (targetPlayer == player) { targetPlayer = null; }
    }

    IEnumerator LockOntoLowestHealthInInterval()
    {
        yield return new WaitForSeconds(.5f);

        LockOntoLowestPlayer();

        StartCoroutine(LockOntoLowestHealthInInterval());
    }

    void LockOntoLowestPlayer()
    {
        Player player = null;
        int least = 0;

        for (int i = seenPlayers.Count - 1; i >= 0; i--)
        {
            if (!seenPlayers[i]) { seenPlayers.RemoveAt(i); continue; }

            if (seenPlayers[i].Health > least)
            {
                player = seenPlayers[i];
            }
        }

        if (player == null) { return; }

        targetPlayer = player;
    }

    void Logic()
    {

        if (targetPlayer)
        {
            Aim(targetPlayer.transform.position + (new Vector3(targetPlayer.rb.linearVelocity.x, targetPlayer.rb.linearVelocity.y, 0) / leadAmount));

            // If I am low hp, move away
            if (Health < maxHealth / 2f)
            {
                state = State.movingAwayFromPlayer;
            }
            // if there is a target player Aim at it
            else
            {
                // attack pattern

                if (Vector2.Distance(transform.position, targetPlayer.transform.position) <= 3)
                {
                    state = State.movingAwayFromPlayer;
                }
                else if (Vector2.Distance(transform.position, targetPlayer.transform.position) >= 6.5f)
                {
                    state = State.movingTowardPlayer;
                }
                else if (state == State.farming)
                {
                    state = State.movingAwayFromPlayer;
                }
            }
        }
        // if there is no target player and there are players in the area lock onto the lowest player
        else if (seenPlayers.Count > 0)
        {
            LockOntoLowestPlayer();
        }
        else
        {
            state = State.farming;
        }
    }

    Vector3 targetPos;

    void States()
    {
        Logic();
        Shoot();

        switch (state)
        {
            case State.farming: Farm(); break;
            case State.movingAwayFromPlayer: if (targetPlayer) { MoveAwayFrom(targetPlayer.transform.position); } break;
            case State.movingTowardPlayer: if (targetPlayer) { MoveTowards(targetPlayer.transform.position); } break;
        }

        if (state != State.farming)
        {
            rb.angularVelocity = 0;
        }
    }

    void Farm()
    {
        if (!currentFarm)
        {
            if (seenFarms.Count > 0)
            {
                for (int i = 0; i < seenFarms.Count; i++)
                {
                    if (seenFarms[i])
                    {
                        currentFarm = seenFarms[i];
                        break;
                    }
                    else
                    {
                        seenFarms.RemoveAt(i);
                    }
                }
            }
            else
            {
                if (Vector2.Distance(transform.position, targetPos) < .5f || targetPos == Vector3.zero)
                {
                    targetPos = new Vector3(Random.Range(-(mapSize.x / 2), (mapSize.x / 2)), Random.Range(-(mapSize.y / 2), (mapSize.y / 2)));
                }

                MoveTowards(targetPos);
            }
        }
        else
        {
            Aim(currentFarm.transform.position);
            MoveTowards(currentFarm.transform.position);
        }
    }


    Vector3 movementOffset;
    float leadAmount;

    IEnumerator ChangeMovementAndLeadAmountOffset()
    {
        //movementOffset = Random.insideUnitCircle * 5;
        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            movementOffset = rb.linearVelocity;
        }
        else if (rand == 1)
        {
            movementOffset = -transform.right* 5.5f;
        }
        else
        {
            movementOffset = transform.right * 5.5f;
        }

        leadAmount = Random.Range(1.7f, 2.2f);
        yield return new WaitForSeconds(Random.Range(.25f, 3f));
        StartCoroutine(ChangeMovementAndLeadAmountOffset());
    }
   
    void MoveTowards(Vector3 pos)
    {
        rb.linearVelocity = ((pos + movementOffset) - transform.position).normalized * (SkillManager.BASE_SPEED + (sm.speed.Value / 5f));
    }

    void MoveAwayFrom(Vector3 pos)
    {
        rb.linearVelocity = ((transform.position + movementOffset) - pos).normalized * (SkillManager.BASE_SPEED + (sm.speed.Value / 5f));
    }

    public override void Update()
    {
        base.Update();

        States();
        eyes.transform.rotation = Quaternion.identity;

        if (sm.SkillPoints > 0)
        {
            sm.GetRandAttribute().Value++;
            sm.SkillPoints--;
        }
    }
}
