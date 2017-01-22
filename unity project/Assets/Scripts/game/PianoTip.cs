using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoTip : MonoBehaviour
{

    private float targetPos;
    public bool animate = false;
    public bool externalAnimate = false;

    private float lerpPercentage;
    private float startTime;

    private float startPos;

    void Start()
    {
        startPos = this.transform.position.y;
        targetPos = startPos + 2;
    }

    void Update()
    {
        if(animate)
        {
            // animate
            lerpPercentage = Time.time - startTime;
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, targetPos, 0), lerpPercentage / 100);

            if (lerpPercentage >= 1)
                animate = false;
        }
    }

    public void Animate(float targetPos)
    {
        this.targetPos = targetPos;
        startTime = Time.time;
        animate = true;
    }
}
