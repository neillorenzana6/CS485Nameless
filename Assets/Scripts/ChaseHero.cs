using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseHero : MonoBehaviour
{
    public Transform Player;
    public float moveSpeed = 5.0f;
    public int maxDist = 10;
    public int minDist = 2;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player);
        if (Vector3.Distance(transform.position, Player.position) <= maxDist && Vector3.Distance(transform.position, Player.position) >= minDist)
        {
            anim.SetBool("isWalking", true);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            transform.Rotate(0, 180, 0);
        }
        else
        {
                anim.SetBool("isWalking", false);    
        }

    }

}
