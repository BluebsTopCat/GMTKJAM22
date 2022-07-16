using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Die")] 
    public GameObject deathprefab;
    public int dicebroken;
    public int damagetaken;
    [Header("Audio")] 
    public AudioSource hurt;
    public AudioSource sfx;
    public AudioSource rolling;
    public AudioClip collide;
    public AudioSource pullback;
    public AudioSource levelups;
    [Header("Other")]
    public int experience;
    public int currentexpgoal;
    public int level;
    public int[] leveldice;

    public LineRenderer lr;
    
    private DisplayStuff ds;
    private DiceMaster dm;
    public SpriteRenderer player;
    public enum state{standing, rolling, prepping}
    public state playerstate;
    public Die d;
    public int currentnumber;
    public int _health = 6;
    public float impulse = 2;
    private Rigidbody2D _rigidbody2D;
    public GameObject mousepos;
    public SpriteRenderer mousesprite;
    public Sprite closed;
    public Sprite open;
    public GameObject lockedaimpos;
    public Collider2D circleattackcollider;
    public float timesincelastchange;
    public float iframes;
    public GameObject lookaheader;

    // Update is called once per frame

    private void Start()
    {
        ds = FindObjectOfType<DisplayStuff>();
        dm = FindObjectOfType<DiceMaster>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        levelup();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        rollitagain();
    }

    public void levelup()
    {
        levelups.Play();
        level += 1;
        _health += 2;
        experience = 0;
        currentexpgoal = 5 * (int)Mathf.Pow(1.5f, level);
        d = dm.fetchdie(leveldice[Mathf.Clamp(level-1, 0, leveldice.Length)]);
        rollitagain();
    }
    public void rollitagain()
    {
        currentnumber = Random.Range(1, d.count + 1);
        player.sprite = d.spritesheet[(currentnumber -1)%d.spritesheet.Length];
    }
    void Update()
    {
        if (_health <= 0)
        {
            die();
        }
        iframes -= Time.deltaTime;   
        if(experience >= currentexpgoal)
            levelup();
        ds.updateui(_health,experience, currentexpgoal, level);
        _rigidbody2D.velocity *= .95f;

        Vector3 mousetopos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousetopos =  new Vector3(mousetopos.x, mousetopos.y, 0f);
        if (playerstate == state.standing && _rigidbody2D.velocity.magnitude > 10)
        {
            playerstate = state.rolling;
        }
        switch (playerstate)
        {
            case state.rolling:
                rolling.volume = Mathf.Clamp(_rigidbody2D.velocity.magnitude / 10, .25f, 1);
                pullback.volume = 0;
                lockedaimpos.SetActive(false);
                lookaheader.SetActive(false);
                lr.enabled = false;
                mousesprite.sprite = open;
                mousepos.transform.position = new Vector3(mousetopos.x, mousetopos.y, 0f);
                mousepos.transform.eulerAngles = Vector3.zero;
                circleattackcollider.enabled = true;
                if (timesincelastchange <= .1)
                {
                    timesincelastchange += Time.deltaTime * _rigidbody2D.velocity.magnitude;
                }
                else
                {
                    timesincelastchange = 0;
                    rollitagain();
                }

                if (_rigidbody2D.velocity.magnitude < 5f)
                    playerstate = state.standing;
                break;
            case state.standing:
                rolling.volume = 0;
                pullback.volume = 0;
                mousesprite.sprite = open;
                lr.enabled = false;
                circleattackcollider.enabled = false;
                mousepos.transform.position = this.transform.position;
                lockedaimpos.SetActive(false);
                lookaheader.SetActive(false);
                mousepos.transform.position = new Vector3(mousetopos.x, mousetopos.y, 0f);
                mousepos.transform.eulerAngles = Vector3.zero;
                _rigidbody2D.velocity *= .95f;
                break;
            case state.prepping:
                rolling.volume = 0;
                pullback.volume = .5f;
                pullback.pitch = .5f + Vector3.Distance(mousetopos, this.transform.position) / 4;
                lockedaimpos.SetActive(true);
                lookaheader.SetActive(true);
                lr.enabled = true;
                mousesprite.sprite = closed;
                mousepos.transform.position = mousetopos;
                Vector3 dir = mousepos.transform.position - this.transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
                mousepos.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                lockedaimpos.transform.position = mousetopos;
                lookaheader.transform.position = this.transform.position + (this.transform.position - mousetopos);
                if(Input.GetMouseButtonUp(0)) rollit();
                break;
        }
    }

    public void rollit()
    {
        playerstate = state.rolling;
        mousepos.transform.position = this.transform.position;
        circleattackcollider.enabled = true;
        _rigidbody2D.AddForce((lookaheader.transform.position - this.transform.position) * impulse, ForceMode2D.Impulse);
    }

    private void OnMouseDown()
    {
        if (playerstate == state.standing)
        {
            _rigidbody2D.velocity = Vector2.zero;
            playerstate = state.prepping;
        }
    }

    public void die()
    {
        Instantiate(deathprefab, this.transform.position, new quaternion(0, 0, 0, 0));
        ds.gameover();
        this.gameObject.SetActive(false);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Collider2D>().isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_rigidbody2D.velocity.magnitude > 1f)
        {
            sfx.pitch = Random.Range(.8f, 1.2f);
            sfx.volume = Mathf.Clamp(_rigidbody2D.velocity.magnitude / 10, .25f, 1);
            sfx.PlayOneShot(collide);
        }
    }

    public void gethurt(int i, Vector3 pos)
    {
        if (playerstate == state.rolling) return;
        _rigidbody2D.AddForce((- this.transform.position + pos).normalized * 5, ForceMode2D.Impulse);
        
        if (iframes >= 0) return;
        hurt.Play();
        _health -= i;
        damagetaken++;
        iframes = .5f;
    }
        
}

