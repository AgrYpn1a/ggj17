using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratePiano : MonoBehaviour
{
    [SerializeField]
    GameObject start, end, pianoTip, player;

    private float currentStartX;

    float random;

    [SerializeField]
    private Text scoreText;
    private float score;

    [SerializeField]
    private Text swapText;

    // Use this for initialization
    void Start()
    {
        random = Random.Range(0, 34.45f);

        start.transform.position = new Vector3(0, 0, 0);

        // Initial generating
        InitPianoRow(64);
        UpdatePianoRow(start.transform.position.x - 16, 64);
        currentStartX = start.transform.position.x + 32;

        score = 0;
        scoreText.text = "Score: " + score;
        swapText.color = Color.green;
        swapText.text = "";
    }

    Queue<GameObject> pianoTips = new Queue<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > currentStartX)
        {
            UpdatePianoRow(currentStartX, 32);
            currentStartX += 16;
        }

        score += Time.deltaTime;
        scoreText.text = "Score: " + score.ToString("0.00");
        if ((int)score % 10 == 0 && (int)score != 0)
        {
            scoreText.color = Color.green;
            swapText.text = "Swap!";
        }
        else
        {
            scoreText.color = Color.black;
            swapText.text = "";
        }
    }


    void InitPianoRow(int numOfTips = 16)
    {
        for (float i = 0; i < numOfTips * 2; i++)
        {
            GameObject pianoTipInstance;
            pianoTipInstance = Instantiate(pianoTip, new Vector3(i, 10, 0), start.transform.rotation) as GameObject;

            pianoTips.Enqueue(pianoTipInstance);
        }
    }

    public float frequency = 0.2f;
    public float gain = 20;
    public float frequency2 = 0.1f;
    public float gain2 = 50;

    void UpdatePianoRow(float startX, int numOfTips = 16)
    {
        for (float i = startX; i < startX + numOfTips; i++)
        {
            float noise = GetNoise(i);

            GameObject pianoTipInstance1 = pianoTips.Dequeue();
            pianoTipInstance1.transform.position = new Vector3(i, 10 + noise, 0);
            pianoTips.Enqueue(pianoTipInstance1);

            GameObject pianoTipInstance2 = pianoTips.Dequeue();
            pianoTipInstance2.transform.position = new Vector3(i, -10 + noise, 0);
            pianoTips.Enqueue(pianoTipInstance2);
        }
    }

    float GetNoise(float i)
    {
        float noise = (-0.5f + Mathf.PerlinNoise(i * frequency, random)) * gain;
        noise += (-0.5f + Mathf.PerlinNoise(i * frequency2, random)) * gain2;

        return noise;
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < 400; i++)
        {
            float noise = GetNoise(i);
            float nextNosie = GetNoise(i + 1);

            Gizmos.DrawLine(
                new Vector3(i, noise, 0), 
                new Vector3(i + 1, nextNosie, 0));
        }
    }
}