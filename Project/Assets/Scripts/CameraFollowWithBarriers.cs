using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowWithBarriers : MonoBehaviour
{
    [Header("Automatically offsets barrier positions by size of camera")]
    public Transform bottomLeftBarrier;
    public Transform topRightBarrier;
    public Transform followObject;
    public Vector2 cameraSizeInUnits;
    Camera cam;

    [Range(.01f, 1f)]
    [SerializeField] float speed;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        originalBgColor = cam.backgroundColor;
        Vector2 mapSize = new Vector2(Litterbug.MAPX, Litterbug.MAPY);
        topRightBarrier.transform.position = mapSize / 2;
        bottomLeftBarrier.transform.position = -mapSize / 2;

        cameraSizeInUnits.x = cam.orthographicSize * cam.aspect;
        cameraSizeInUnits.y = cam.orthographicSize;
    }

    private void Start()
    {
        bottomLeftBarrier.transform.position += new Vector3((cameraSizeInUnits.x), cameraSizeInUnits.y, 0);
        topRightBarrier.transform.position -= new Vector3((cameraSizeInUnits.x), cameraSizeInUnits.y, 0);
    }

    Color originalBgColor;
    bool white;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!white)
            {
                cam.backgroundColor = Color.white;
                white = true;
            }
            else
            {
                cam.backgroundColor = originalBgColor;
                white = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (!bottomLeftBarrier) { return; }
        if (!followObject)
        {
            Player player = FindObjectOfType<Player>();

            if (player)
            {
                followObject = player.transform;
            }
            else
            {
                Debug.LogWarning("No Target For Camera to Follow!"); return;
            }
        }

        Vector3 movement = new Vector3(followObject.position.x - transform.position.x, followObject.position.y - transform.position.y);

        if (transform.position.x <= bottomLeftBarrier.position.x)
        {
            if (movement.x < 0)
            {
                transform.position = new Vector3(bottomLeftBarrier.position.x, transform.position.y, -10);
                movement.x = 0;
            }
        }
        if (transform.position.y <= bottomLeftBarrier.position.y)
        {
            if (movement.y < 0)
            {
                transform.position = new Vector3(transform.position.x, bottomLeftBarrier.position.y, -10);
                movement.y = 0;
            }
        }
        if (transform.position.x >= topRightBarrier.position.x)
        {
            if (movement.x > 0)
            {
                transform.position = new Vector3(topRightBarrier.position.x, transform.position.y, -10);
                movement.x = 0;
            }
        }
        if (transform.position.y >= topRightBarrier.position.y)
        {
            if (movement.y > 0)
            {
                transform.position = new Vector3(transform.position.x, topRightBarrier.position.y, -10);
                movement.y = 0;
            }
        }

        transform.Translate(movement * speed);
    }
}
