using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectBehavior : MonoBehaviour
{    
    private LaneObject lane = null;
    public Transform leftLimit, rightLimit;

    protected Rigidbody rb;
    protected float colliderScale;
    protected bool goingRight = false;

    protected Vector3 startingPos;

    protected virtual void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        lane = transform.parent.GetComponent<LaneObject>();
    }

    private void Start()
    {
        goingRight = this.transform.localRotation.y > 0;       
        colliderScale = this.GetComponent<BoxCollider>().size.z;

        Vector3 token = rb.position;
        if (goingRight)
        {
            token.x = leftLimit.position.x - (colliderScale / 2);
            startingPos = token;
        }
        else
        {
            token.x = rightLimit.position.x + (colliderScale / 2);
            startingPos = token;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (transform.forward * lane.generalSpeed * Time.deltaTime));
        if(goingRight)
        {
            if(rb.position.x - colliderScale/2 > rightLimit.transform.position.x)
            {                
                rb.MovePosition(startingPos);
            }
        }
        else
        {
            if (rb.position.x + colliderScale / 2 < leftLimit.transform.position.x)
            {
                rb.MovePosition(startingPos);
            }
        }
    }

    public virtual void ChangeMode()
    {
        //
    }
}
