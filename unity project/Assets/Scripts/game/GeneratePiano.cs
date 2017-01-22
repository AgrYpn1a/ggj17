using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratePiano : MonoBehaviour
{
    [SerializeField]
    GameObject start, end, pianoTip, player;

    [SerializeField]
    private Sprite white;
    [SerializeField]
    private Sprite black;

    private float currentStartX;

    float random;

    [SerializeField]
    private Text scoreText;
    private float score;

    [SerializeField]
    private Text swapText;
    [SerializeField]
    private Text lvlText;

    public float frequency = 0.2f;
    public float gain = 20;
    public float frequency2 = 0.1f;
    public float gain2 = 50;

    private Queue<GameObject> pianoTips = new Queue<GameObject>();

    private float timer;
    private float timeScale = 1.6f;

    private Vector3 lastPos;

    [SerializeField]
    private PlayerController playerController;

    private bool firstUpdate = true;

    [SerializeField]
    private Sprite[] sprites;

    void Start()
    {
        random = Random.Range(0, 34.45f);

        start.transform.position = new Vector3(0, 0, 0);

        // Initial generating
        InitPianoRow(64);
        UpdatePianoRow(start.transform.position.x - 20, 64);
        currentStartX = start.transform.position.x + 32;

        score = 0;
        scoreText.text = "Score: " + score;
        swapText.color = Color.green;
        swapText.text = "";
        lvlText.text = "Level " + 1;

    }


    void Update()
    {
        if (player.transform.position.x > lastPos.x - 16)
        {
            //UpdatePianoRow(currentStartX - 16, 64);
            UpdatePianoRow(lastPos.x - 32, 64);
            //currentStartX = lastPos.x - 32;
        }

        score += Time.deltaTime;
        scoreText.text = "Score: " + score.ToString("0.00");
        if ((int)score % 30 == 0 && (int)score != 0)
        {
            scoreText.color = Color.green;
            swapText.text = "Swap!";
        }
        else
        {
            scoreText.color = Color.black;
            swapText.text = "";
        }

        ChangeLevel();

    }

    int bottomCount = 0;
    int topCount = 0;

    void InitPianoRow(int numOfTips = 16)
    {
        for (float i = 0; i < numOfTips * 2; i++)
        {
            if (i % 2 == 0)
                bottomCount++;
            else
                topCount++;


            if ((1 + i) % 4 == 0)
                pianoTip.GetComponent<SpriteRenderer>().sprite = black;
            else
                pianoTip.GetComponent<SpriteRenderer>().sprite = white;

            GameObject pianoTipInstance = Instantiate(pianoTip, new Vector3(i, 10, 0), start.transform.rotation) as GameObject;

            pianoTips.Enqueue(pianoTipInstance);
            lastPos = pianoTipInstance.transform.position;
        }
    }

    void UpdatePianoRow(float startX, int numOfTips = 16)
    {
        float scaleDownFactor = 1;
        if (firstUpdate)
        {
            firstUpdate = false;
            scaleDownFactor = 6;
        }
        for (float i = startX; i < startX + numOfTips; i++)
        {
            if (i > 15)
                scaleDownFactor = 1;
            float noise = GetNoise(i) / scaleDownFactor;

            GameObject pianoTipInstance1 = pianoTips.Dequeue();
            pianoTipInstance1.transform.position = new Vector3(i, 10 + noise, 0);


            pianoTips.Enqueue(pianoTipInstance1);

            GameObject pianoTipInstance2 = pianoTips.Dequeue();
            pianoTipInstance2.transform.position = new Vector3(i, -10 + noise, 0);


            pianoTips.Enqueue(pianoTipInstance2);
            lastPos = pianoTipInstance2.transform.position;
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

    private void ChangeLevel()
    {
        int sc = (int)score;

        if (sc > 30)
        {
            lvlText.text = "Level " + 2;
            gain2 = 30;
            playerController.animObj.GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        if (sc > 60)
        {
            lvlText.text = "Level " + 3;
            playerController.speed = 0.3f;
            playerController.animObj.GetComponent<SpriteRenderer>().sprite = sprites[1];

        }
        if (sc > 90)
        {
            lvlText.text = "Level " + 4;
            playerController.speed = 0.35f;
            //gain2 = 50;
            playerController.animObj.GetComponent<SpriteRenderer>().sprite = sprites[2];

        }
        if (sc > 120)
        {
            lvlText.text = "Level " + 5;
            playerController.speed = 0.38f;
            //gain2 = 60;
            playerController.animObj.GetComponent<SpriteRenderer>().sprite = sprites[3];
        }

        if (sc > 200)
        {
            lvlText.text = "Level " + 6;
            gain2 = 36;
            playerController.animObj.GetComponent<SpriteRenderer>().sprite = sprites[4];

        }
    }
}