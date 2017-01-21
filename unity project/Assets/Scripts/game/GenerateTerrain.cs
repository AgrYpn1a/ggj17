using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{

    [SerializeField]
    private Transform start;
    [SerializeField]
    private Transform end;

    [SerializeField]
    private Transform blackFret;
    [SerializeField]
    private Transform whiteFret;

    [SerializeField]
    private float fretWidth;

    private int changePoint;
    private int currentStep = 0;
    private int currentFret = 0;

    [SerializeField]
    private int numberOfFrets = 10;
    private Transform[] frets;
    private Transform[] fretsTop;

    [SerializeField]
    private Transform player;

    private int degrees = 0;

    [SerializeField]
    private int defHeight = 2;
    [SerializeField]
    private int defHeightTop = 2;

    void Start()
    {
        frets = new Transform[numberOfFrets];
        fretsTop = new Transform[numberOfFrets];

        changePoint = frets.Length / 2 - 1;

        generateFrets();
    }

    bool first = true;
    void Update()
    {
        if (currentStep > changePoint)
        {
            if (first)
            {
                RecycleFrets(0, changePoint, frets.Length - 1);
                first = false;
            }
            else
            {
                RecycleFrets(changePoint, frets.Length, changePoint - 1);
                first = true;
            }
        }

        if (player.position.x > frets[currentFret].position.x)
        {
            currentFret = addOne(currentFret, frets.Length);
            currentStep++;
            Debug.Log(currentStep);
        }
    }

    private int addOne(int n, int lnght)
    {
        return (n + 1) % lnght;
    }

    private void generateFrets()
    {
        Transform first = Instantiate(whiteFret,
            start.position,
            Quaternion.identity) as Transform;

        Transform firstTop = Instantiate(whiteFret,
            start.position,
            Quaternion.identity) as Transform;

        Vector3 lastPos = first.position;

        frets[0] = first;
        fretsTop[0] = firstTop;

        degrees = addOne(degrees, 360);

        for (int counter = 1; counter < frets.Length; counter++)
        {
            Transform current = Instantiate(whiteFret,
                lastPos + new Vector3(transform.position.x + fretWidth, 0, 0),
                Quaternion.identity) as Transform;

            Transform currentTop = Instantiate(whiteFret,
                lastPos + new Vector3(transform.position.x + fretWidth, 0, 0),
                Quaternion.identity) as Transform;

            lastPos = current.position;
            frets[counter] = current;
            fretsTop[counter] = currentTop;

            SetHeight(current, defHeight);


            // update top
            fretsTop[counter].position = new Vector3(fretsTop[counter].position.x, frets[counter].position.y + defHeightTop, 0);
        }
    }

    private void RecycleFrets(int begin, int end, int pos)
    {
        currentStep = 0;
        //start.position = frets[frets.Length - 1].position + new Vector3(fretWidth, 0, 0);

        // get position of the last
        Vector3 currentPos = frets[pos].position;

        for (int counter = begin; counter < end; counter++)
        {
            // always add to last
            frets[counter].position = currentPos + new Vector3(fretWidth, 0, 0);
            fretsTop[counter].position = currentPos + new Vector3(fretWidth, 0, 0);

            currentPos = frets[counter].position;

            Transform current = frets[counter];

            SetHeight(current, defHeight);
            
            // update top
            fretsTop[counter].position = new Vector3(fretsTop[counter].position.x, current.position.y + defHeightTop, 0);
        }

    }

    private void SetHeight(Transform t, float defHeight)
    {
        degrees = addOne(degrees, 360);
        float y = defHeight + Mathf.Sin(degrees) * UnityEngine.Random.Range(0.2f, 2.0f);
        t.transform.position = new Vector3(t.transform.position.x, y, 0);
    }
}
