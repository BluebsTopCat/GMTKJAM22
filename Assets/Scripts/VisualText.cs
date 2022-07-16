using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisualText : MonoBehaviour
{
    public float starttimetodie = 3f;
    public float timetodie;
    public TextMeshPro tmp;

    public TMP_ColorGradient standard;

    public TMP_ColorGradient rage;

    public TMP_ColorGradient gold;
    // Start is called before the first frame update


    public void enabled(string num, int type)
    {
        tmp.text = num;
        if (type == 0)
            tmp.colorGradientPreset = standard;
        else if (type == 1)
            tmp.colorGradientPreset = rage;
        else if(type == 2)
            tmp.colorGradientPreset = gold;
        timetodie = starttimetodie;
        StartCoroutine(fadeout()); 
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator fadeout()
    {
        while (timetodie > 0)
        {
            tmp.color = new Color(1,1,1, timetodie/starttimetodie);
            timetodie -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
