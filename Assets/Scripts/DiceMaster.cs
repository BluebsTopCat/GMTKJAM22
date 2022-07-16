using System.Collections.Generic;
using UnityEngine;


public class DiceMaster : MonoBehaviour
{
    public List<Die> Dice;

    public Die fetchdie(int id)
    {
        return Dice[id];
    }
}
[System.Serializable]
public class Die
{
    public string name;
    public Sprite[] spritesheet;
    public int count;

    public Die(int _count, Sprite[] _spritesheet, string _name)
    {
        count = _count;
        spritesheet = _spritesheet;
        name = _name;
    }
}