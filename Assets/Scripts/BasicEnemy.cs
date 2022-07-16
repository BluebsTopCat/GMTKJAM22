using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    // Start is called before the first frame update

    // Update is called once per frame
    void OnEnable()
    {
        p = FindObjectOfType<Player>();
        rb2d = this.GetComponent<Rigidbody2D>();
    }
    public void Update()
    {

        if (timetotoggle <= 0)
        {
            timetotoggle = .25f;
            if (flflop)
            {
                spr.sprite = move1;
            }
            else
            {
                spr.sprite = move2;
            }

            flflop = !flflop;
        }
        else
        {
            timetotoggle -= Time.deltaTime;    
        }
        
        damagecooldown -= Time.deltaTime;
        if (p == null) return;
        rb2d.velocity = (p.transform.position - this.transform.position).normalized * speed * 2.5f;
    }
}
