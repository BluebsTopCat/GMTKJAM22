using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnMaster : MonoBehaviour
{
    public DisplayStuff ds;
    public List<Transform> validspawn;
    public List<GameObject> baddies;
    public bool spawndelay = false;
    // Start is called before the first frame update
    void Start()
    {
        ds = FindObjectOfType<DisplayStuff>();
        validspawn = new List<Transform>();
        for(int i = 0; i < transform.childCount; i++)
        {
            validspawn.Add(transform.GetChild(i));
        }
    }

    private void Update()
    {
        if (!spawndelay) StartCoroutine(spawn());
    }

    public IEnumerator spawn()
    {
        Instantiate(baddies[Random.Range(0, baddies.Count)], validspawn[Random.Range(0, validspawn.Count)].transform.position, new Quaternion(0,0,0,0));
        spawndelay = true;
        yield return new WaitForSeconds(Mathf.Clamp(3 - (60-ds.t)/20, .05f, 20));
        spawndelay = false;
    }
}
