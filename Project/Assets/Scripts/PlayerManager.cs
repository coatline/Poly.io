using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInputField;
    [SerializeField] List<string> usernames;
    [SerializeField] TMP_Text leaderboard;
    [SerializeField] User userPrefab;
    [SerializeField] AI AIPrefab;
    Vector2Int mapSize;
    public User user;

    [SerializeField] int cap;
    List<Player> players;
    List<Player> lb;

    void Awake()
    {
        players = new List<Player>();
        mapSize = new Vector2Int(Litterbug.MAPX, Litterbug.MAPY);

        Initialize();
        StartCoroutine(Tic());
        StartCoroutine(UpdateLeaderBoard());
    }

    IEnumerator UpdateLeaderBoard()
    {
        yield return new WaitForEndOfFrame();

        lb = new List<Player>();

        for (int i = -1; i < players.Count; i++)
        {
            Player player;

            if (i == -1)
            {
                if (!user) { continue; }
                player = user;
            }
            else
            {
                player = players[i];
            }

            for (int j = 0; j < 5; j++)
            {
                if (j > lb.Count - 1)
                {
                    lb.Add(player);
                    break;
                }
                else if (player.kills > lb[j].kills)
                {
                    if (j < 4)
                    {
                        if (j + 1 > lb.Count - 1)
                        {
                            lb.Add(lb[j]);
                        }
                        else
                        {
                            lb[j + 1] = lb[j];
                        }
                    }

                    lb[j] = player;

                    break;
                }
            }
        }

        leaderboard.text =
            $"{lb[0].username} - {lb[0].kills} kills lvl. {lb[0].Level}\n" +
            $"{lb[1].username} - {lb[1].kills} kills lvl. {lb[1].Level}\n" +
            $"{lb[2].username} - {lb[2].kills} kills lvl. {lb[2].Level}\n" +
            $"{lb[3].username} - {lb[3].kills} kills lvl. {lb[3].Level}\n" +
            $"{lb[4].username} - {lb[4].kills} kills lvl. {lb[4].Level}\n";

        yield return new WaitForSeconds(2);
        StartCoroutine(UpdateLeaderBoard());
    }

    AI SpawnAi()
    {
        var p = Instantiate(AIPrefab, new Vector3(Random.Range((float)-mapSize.x / 2, mapSize.x / 2), Random.Range((float)-mapSize.y / 2, mapSize.y / 2)), Quaternion.identity);
        p.username = usernames[Random.Range(0, usernames.Count)];
        return p;
    }

    public void SpawnUser()
    {
        user = Instantiate(userPrefab, new Vector3(Random.Range((float)-mapSize.x / 2, mapSize.x / 2), Random.Range((float)-mapSize.y / 2, mapSize.y / 2)), Quaternion.identity);
        user.username = usernameInputField.text;
    }

    void Initialize()
    {
        for (int i = 0; i < cap; i++)
        {
            players.Add(SpawnAi());
        }
    }

    IEnumerator Tic()
    {
        yield return new WaitForSeconds(5);

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null)
            {
                players[i] = SpawnAi();
            }
        }

        StartCoroutine(Tic());
    }
}
