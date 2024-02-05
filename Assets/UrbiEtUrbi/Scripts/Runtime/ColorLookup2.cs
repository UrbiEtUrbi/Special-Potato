using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/CustomEffectComponent", typeof(UniversalRenderPipeline))]
public class ColorLookup4 : VolumeComponent, IPostProcessComponent
{

    public ClampedFloatParameter intensity = new ClampedFloatParameter(value: 0, min: 0, max: 1, overrideState: true);
    // A color that is constant even when the weight changes
    public NoInterpColorParameter fromColor1 = new NoInterpColorParameter(Color.black);
    public NoInterpColorParameter fromColor2 = new NoInterpColorParameter(Color.black);

    public NoInterpColorParameter toColor1 = new NoInterpColorParameter(Color.black);
    public NoInterpColorParameter toColor2 = new NoInterpColorParameter(Color.black);


    public bool IsActive()
    {
        return intensity.value > 0;
      
    }

    public bool IsTileCompatible()
    {
        return true;
    }
}
