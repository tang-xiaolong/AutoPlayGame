using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//定义DNA的基本信息，同时，作为整个实体的控制器
public class BirdBrain : MonoBehaviour
{
    int dnaLength = 4;
    public BirdDNA dna;
    public GameObject eyes;
    private bool seeDownWall = false;
    private bool seeUpWall = false;
    private bool seeUp = false;
    private bool seeDown = false;
    private Vector3 startPos = Vector3.zero;
    public float aliveTime = 0;
    public float distanceTravel = 0;
    private bool alive = true;
    Rigidbody2D rb;

    public void Init()
    {
        dna = new BirdDNA(dnaLength,100);
        transform.Translate(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "top" ||
            collision.gameObject.tag == "bottom" ||
            collision.gameObject.tag == "upWall" ||
            collision.gameObject.tag == "downWall")
        {
            alive = false;
            Debug.Log(collision.gameObject.name);
        }
    }

    private void Update()
    {
        if (!alive) return;

        seeUp = seeDown = seeUpWall = seeDownWall = false;
        RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.forward, 1.0f);
        if(hit.collider != null)
        {
            seeUpWall = hit.collider.gameObject.tag == "upWall";
            seeDownWall = hit.collider.gameObject.tag == "downWall";
        }
        hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.up, 1.0f);
        seeUp = hit.collider != null && hit.collider.gameObject.tag == "top";
        hit = Physics2D.Raycast(eyes.transform.position, -eyes.transform.up, 1.0f);
        seeDown = hit.collider != null && hit.collider.gameObject.tag == "bottom";
        aliveTime = BirdPopulationManager.elapsed;
    }

    private void FixedUpdate()
    {
        if (!alive) return;
        float upForce = 0;
        float forwardForce = 1;

        if (seeUpWall)
            upForce = dna.GetGene(0);
        else if(seeDownWall)
            upForce = dna.GetGene(1);
        else if(seeUp)
            upForce = dna.GetGene(2);
        else if(seeDown)
            upForce = dna.GetGene(3);
        rb.AddForce(transform.right * forwardForce);
        rb.AddForce(transform.up * upForce);
        distanceTravel = Vector3.Distance(startPos, transform.position);
    }
}
