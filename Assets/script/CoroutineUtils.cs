using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public static class CoroutineUtils 
{
    public static IEnumerator Lerp(float duration,
        Action<float> action, bool realTime = false,
        bool smooth = false, AnimationCurve curve = null,bool inverse = false )
    {
        float time = 0;
        Func<float, float> tEvel = t => t;
        if (smooth) tEvel = t => Mathf.SmoothStep(0, 1, t);
        if (curve != null) tEvel = t => curve.Evaluate(t);
        while(time< duration)
        {
            float delta = realTime ? Time.fixedDeltaTime : Time.deltaTime;
            float t = (time + delta > duration) ? 1 : (time / duration);
            if(inverse)
                t = 1 - t; 
            action(tEvel(t));
            time += delta; 
            yield return null;  
        }
        action(tEvel(inverse ? 0 : 1));
    }
    public static Coroutine Lerp(MonoBehaviour obj, float duration , Action<float> action)
    {
        return obj.StartCoroutine(Lerp(duration, action));
    }
}

