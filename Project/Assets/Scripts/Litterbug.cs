using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Litterbug : MonoBehaviour
{
    public const int MAPX = 150;
    public const int MAPY = 150;

    [SerializeField] Entity circlePrefab;
    [SerializeField] Entity squarePrefab;
    [SerializeField] Entity hexPrefab;
    [SerializeField] int cap;
    List<Entity> shapes;

    void Start()
    {
        shapes = new List<Entity>();

        StartCoroutine(Initialize());
        StartCoroutine(Tic());
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    IEnumerator Initialize()
    {
        for (int i = 0; i < cap; i++)
        {
            shapes.Add(Instantiate(RandShape(), new Vector3(Random.Range((float)-MAPX / 2, MAPX / 2), Random.Range((float)-MAPY / 2, MAPY / 2)), Quaternion.identity, transform));
            yield return new WaitForSeconds(Random.Range(.2f, SkillManager.BASE_SHOTRATE - .2f));
        }
    }

    IEnumerator Tic()
    {
        yield return new WaitForSeconds(10);

        for (int i = 0; i < shapes.Count; i++)
        {
            if (shapes[i] == null)
            {
                shapes[i] = (Instantiate(RandShape(), new Vector3(Random.Range((float)-MAPX / 2, MAPX / 2), Random.Range((float)-MAPY / 2, MAPY / 2)), Quaternion.identity, transform));
                yield return new WaitForSeconds(Random.Range(.2f, SkillManager.BASE_SHOTRATE - .2f));
            }
        }

        StartCoroutine(Tic());
    }

    Entity RandShape()
    {
        int rand = Random.Range(0, 11);

        if (rand <= 1)
        {
            return hexPrefab;
        }
        else if (rand <= 3)
        {
            return squarePrefab;
        }
        else
        {
            return circlePrefab;
        }
    }
}
