using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject animObj;
    public float speed;
    [SerializeField]
    private int health = 100;
    [SerializeField]
    private float healthRegenRate = 10.2f;
    [SerializeField]
    private int healthRegenAmount = 5;

    [SerializeField]
    private Text hpText;

    [SerializeField]
    private GameObject deathParticles;
    [SerializeField]
    private AudioClip hitClip;

    void Start()
    {
        StartCoroutine(Indestructable());
        hpText.text = "Health " + health;
    }

    private float startRegen;
    void Update()
    {
        Death();
        transform.position += new Vector3(speed, 0, 0);

        if (Time.time > startRegen + healthRegenRate && health <= 100 - healthRegenAmount)
        {
            startRegen = Time.time;
            health += healthRegenAmount;
            hpText.text = "Health " + health;
        }

//        if (Input.GetKeyDown(KeyCode.Space))
//           PhysicsController();

    }

    bool blinking;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (!blinking)
        {
            this.GetComponentInChildren<AudioSource>().PlayOneShot(hitClip);
            Instantiate(deathParticles, this.transform.position, Quaternion.identity);
            health -= 30;
            hpText.text = "Health " + health;
            StartCoroutine(Indestructable());
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (!blinking)
        {
            health -= 1;
            hpText.text = "Health " + health;

        }
    }

    public IEnumerator Indestructable()
    {
        animObj.GetComponent<Animator>().enabled = true;
        blinking = true;
        //this.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        //this.GetComponent<Collider2D>().enabled = true;
        animObj.GetComponent<Animator>().enabled = false;
        animObj.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 1);
        blinking = false;

    }

    private void Death()
    {
        if(health <= 0)
            SceneManager.LoadScene("demo", LoadSceneMode.Single);
    }

    private void PhysicsController()
    {
        Rigidbody2D rg = this.GetComponent<Rigidbody2D>();
        rg.AddForce(new Vector2(0, 1) * 5);
        rg.velocity = new Vector2(1, 0);
    }
}
