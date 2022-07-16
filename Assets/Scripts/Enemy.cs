using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public int experience;
    public SpriteRenderer spr;
    public Sprite move1;
    public Sprite move2;
    public Sprite ouch;
    public Player p;
    public Rigidbody2D rb2d;
    public int hp;
    public float speed = 1;
    public int weakmultiples;
    public int str = 1;
    public GameObject number;
    public float damagecooldown;
    public float timetotoggle = .25f;
    public bool flflop = false;
    public AudioSource hurtsound;
    public GameObject deathobj;
    void Start()
    {
        p = FindObjectOfType<Player>();
        rb2d = this.GetComponent<Rigidbody2D>();
    }

    public void gethurt(int damage, Vector3 position)
    {
        if(damagecooldown >= 0 ) return;
        hurtsound.Play();
        timetotoggle = .25f;
        spr.sprite = ouch;
        GameObject g = Instantiate(number);
        g.transform.position = this.transform.position;
        string input = damage + "";
        int type = 0;
        if (weakmultiples != 0 && damage % weakmultiples == 0)
        {
            damage = 2 * damage;
            input = damage/2 + "- Weak!";
            type = 1;
        }

        if (type == 0 && damage == p.d.count || type == 1 && damage/2 == p.d.count)
        {
            input += "!!";
            type = 2;
        }

        g.GetComponent<VisualText>().enabled(input, type);

        rb2d.AddForce(this.transform.position - position, ForceMode2D.Impulse);
        
        hp -= damage;
        if(hp <= 0) die();
        damagecooldown = .15f;
    }
    

    public void die()
    {
        p.dicebroken += 1;
        Instantiate(deathobj, this.transform.position, new Quaternion(0,0,0,0));
        Destroy(gameObject);
        p.experience += experience;
    }

    
    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collided!");
        if (other.gameObject == p.gameObject)
        {
            
            p.gethurt(str, this.transform.position);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == p.gameObject)
        {
            gethurt(p.currentnumber, p.transform.position);
        }
            
    }
}
