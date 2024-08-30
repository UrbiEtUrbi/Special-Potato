using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasSizeHelp : MonoBehaviour
{

    AudioConfiguration a;
    TextMeshProUGUI l;
    
    private void Awake()
    {
        l = GetComponent<TextMeshProUGUI>();
        a = AudioSettings.GetConfiguration();
    }

    private void Update()   
    {
       

        var s = a.numRealVoices.ToString();
        s += "\n";

        s += a.numVirtualVoices.ToString();
       
        l.text = s;


    }


}
