using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walls : MonoBehaviour
{
    public float speed;
    public AudioSource slide;
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Rigidbody2D>())
        {
            if(other.gameObject.GetComponent<Player>())
                slide.volume = .5f;
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.down * speed, ForceMode2D.Impulse);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Player>())
            slide.volume = 0; 
    }
}
