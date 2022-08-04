using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBump : MonoBehaviour
{
    [SerializeField] bool canAddOnBumps;
    [SerializeField] float bumpAmount;
    [SerializeField] float speed;
    Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (Vector3.Distance(transform.localScale, initialScale) > .01f)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, initialScale, Time.deltaTime * speed);
        }
    }

    public void Bump(float bumpAmount = 0, float speed = 0)
    {
        if (bumpAmount != 0)
        {
            transform.localScale += new Vector3(bumpAmount, bumpAmount, 0);

            if (!canAddOnBumps)
            {
                transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0, initialScale.x + bumpAmount), Mathf.Clamp(transform.localScale.y, 0, initialScale.y + bumpAmount));
            }
        }
        else
        {
            transform.localScale += new Vector3(this.bumpAmount, this.bumpAmount, 0);

            if (!canAddOnBumps)
            {
                transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0, initialScale.x + this.bumpAmount), Mathf.Clamp(transform.localScale.y, 0, initialScale.y + this.bumpAmount));
            }
        }
        if (speed != 0)
        {
            this.speed = speed;
        }

    }
}
