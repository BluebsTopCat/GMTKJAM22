using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayStuff : MonoBehaviour
{
    private Player p;
    public gBrade[] graded;
    [Header("GoodEnding")] 
    public GameObject goodui;
    public TextMeshProUGUI Dicekilled;
    public TextMeshProUGUI Levelsgotten;
    public TextMeshProUGUI DamageTaken;
    public TextMeshProUGUI Grade;
    [Header("Stuff")]
    public bool isplaying;
    public GameObject leveluihead;
    public GameObject gameoveruihead;
    public GameObject music;
    
    public TextMeshProUGUI time;
    public int hp;
    public Image[] hearts;
    public Sprite fullheart;
    public Sprite midheart;
    public Sprite emptyheart;

    public int exp;
    public int expgoal;
    public Slider sl;

    private float starttime;
    public float t;
    public void Start()
    {
        p = GameObject.FindObjectOfType<Player>();
        starttime = Time.time;
    }

    public void Update()
    {
        if (!isplaying) return;
        t = 60 + starttime - Time.time;
        int minutes = (int) t / 60;
        int seconds = (int) t % 60;
        time.text = $"{minutes:00}:{seconds:00}";

        if (t <= 0)
        {
            win();
        }
    }

    public void quitgame()
    {
        SceneManager.LoadScene(0);
    }

    public void restart()
    {
        SceneManager.LoadScene(1);
    }
    public void gameover()
    {
        Cursor.visible = true;
        isplaying = false; 
        Destroy(music);
       leveluihead.SetActive(false);
       gameoveruihead.SetActive(true);
    }

    public void win()
    {
        goodui.SetActive(true);
        Destroy(gameoveruihead);
        Dicekilled.text = "Dice Broken: " + p.dicebroken;
        Levelsgotten.text = "Levels Gained: " + p.level;
        DamageTaken.text = "Damage Taken: " + p.damagetaken;
        int score = p.dicebroken * 3 + p.level * 10 - p.damagetaken * 5;
        
        string grade = "";
        foreach(gBrade g  in graded)
        {
            if (g.score >= score)
            {
                grade = g.name;
                break;
            }
        }
        Grade.text = "Grade: " + grade;

        Destroy(p.gameObject);
        Cursor.visible = true;
        isplaying = false;
        leveluihead.SetActive(false);
        gameoveruihead.SetActive(true);
    }
    public void updateui(int hip, int expe, int expgl, int leveld)
    {
        hp = hip;
        exp = expe;
        expgoal = expgl;
        int temphp = hp;
        foreach (Image i in hearts)
        {
            if (temphp - 2 >= 0)
            {
                i.sprite = fullheart;
            }
            else if (temphp - 1 >= 0)
            {
                i.sprite = midheart;
            }
            else
            {
                i.sprite = emptyheart;
            }
            temphp -= 2;
        }
        sl.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Clamp(3*(12 + (25*expgoal/10)), 10, 800), 75);
        sl.value = (float)exp / expgoal;
    }
}

[System.Serializable]
public class gBrade
{
    public string name;
    public int score;
}