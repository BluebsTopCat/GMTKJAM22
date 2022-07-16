using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lr;

    public GameObject start;
    public GameObject end;
    public Material mat;
   
    // Update is called once per frame
    void Update()
    {
        mat.mainTextureScale = new Vector2(Vector3.Distance(start.transform.position,end.transform.position), 1);
      lr.SetPosition(0,start.transform.position);
      lr.SetPosition(1,end.transform.position);  
    }
}
