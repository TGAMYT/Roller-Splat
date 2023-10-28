using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody rb;
    public ParticleSystem moveParticle;
    public float speed;
    private bool isTravelling;
    private Vector3 travelDirection;
    private Vector3 nextCollisionPosition;
    public int minSwipeRecognition = 700;
    private Vector2 swipePositionLastFrame;
    private Vector2 swipePositionCurrentFrame;
    private Vector2 CurrentSwipe; 
    private Color solveColor;

    private void Start() 
    {
         solveColor = Random.ColorHSV(0.5f,1);
         GetComponent<MeshRenderer>().material.color=solveColor;
    }
    private void FixedUpdate() 
    {
        if (isTravelling)
        {
            rb.velocity = speed*travelDirection;
        }
        
        Collider[] hitcolliders = Physics.OverlapSphere(transform.position - (Vector3.up/2), 0.1f);
        int i = 0;
        while (i<hitcolliders.Length)
        {
            GroundModulator ground =hitcolliders[i].transform.GetComponent<GroundModulator>();
            if (ground && !ground.isColoured)
            {
                ground.ChangeColour(solveColor);
            }
            i++;
        }
        if (nextCollisionPosition!=Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollisionPosition)<1)
            {
                isTravelling=false;
                travelDirection=Vector3.zero;
                nextCollisionPosition=Vector3.zero;
            }
        }
        if(isTravelling)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            swipePositionCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (swipePositionLastFrame != Vector2.zero)
            {
                CurrentSwipe = swipePositionCurrentFrame - swipePositionLastFrame;

                if (CurrentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    return;
                }

                CurrentSwipe.Normalize();

                //  UP/DOWN
                if(CurrentSwipe.x > -0.5 && CurrentSwipe.x < 0.5)
                {
                    //GO UP/DOWN
                    SetDestination(CurrentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                    moveParticle.Play();
                    if (CurrentSwipe.y<0)
                    {
                        moveParticle.transform.Rotate(0,0,0);
                    }else
                    {
                        moveParticle.transform.Rotate(0,0,180);
                    }
                }

                if(CurrentSwipe.y > -0.5 && CurrentSwipe.y < 0.5)
                {
                    //GO LEFT/RIGHT
                   SetDestination(CurrentSwipe.x > 0 ? Vector3.right : Vector3.left); 
                   moveParticle.Play();
                   if (CurrentSwipe.x<0)
                    {
                        moveParticle.transform.Rotate(0,0,90);
                    }else
                    {
                        moveParticle.transform.Rotate(0,0,270);
                    }
                }
            }
            swipePositionLastFrame = swipePositionCurrentFrame;
        }
    
        if (Input.GetMouseButtonUp(0))
        {
            swipePositionLastFrame=Vector2.zero;
            CurrentSwipe=Vector2.zero;
        }
        if (isTravelling==false)
        {
            moveParticle.Stop();
        }
    }

    void SetDestination(Vector3 direction)
    {
        travelDirection=direction;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point;
            isTravelling=true;
        }
    }
}
