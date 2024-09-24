using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float moveSpeed = 0.1f;
    [SerializeField] int HP;

    [SerializeField] GameObject HpBar;

    [SerializeField] Text scoreText;

    [SerializeField] GameObject YouDied;
    [SerializeField] GameObject replayButton;

    int score;
    float scoreTime;
    GameObject currentFloor;

    Animator anim;

    AudioSource deathSound;
    AudioSource BackgroundMusic;
    void Start()
    {
        HP = 10;
        score = 0;
        scoreTime = 0f;
        anim = GetComponent<Animator>();
        deathSound = GetComponents<AudioSource>()[0];
        BackgroundMusic = GetComponents<AudioSource>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {    
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = true;
            anim.SetBool("run", true);
        }
        else if(Input.GetKey(KeyCode.D))
        {    
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = false;
            anim.SetBool("run", true);
        }
        else
        {
           anim.SetBool("run", false);
        }
        updateScore();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag =="normal-floor")
        {
            if(other.contacts[0].normal == new Vector2(0f, 1f))
            {
                Debug.Log("hitted normal floor!");
                currentFloor = other.gameObject;
                modifyHP(1);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if(other.gameObject.tag == "nails-floor")
        {
            if(other.contacts[0].normal == new Vector2(0f, 1f))
            {
                Debug.Log("hitted nails floor!");
                currentFloor = other.gameObject;
                modifyHP(-3);
                anim.SetTrigger("hurt");
                other.gameObject.GetComponent<AudioSource>().Play();
            }
            
        }
        else if(other.gameObject.tag == "ceiling")
        {
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            modifyHP(-3);
            anim.SetTrigger("hurt");
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Deathline")
        {
            Debug.Log("YOU DIED!");     
            Die();   
        }
    }

    void modifyHP(int num)
    {
        HP += num;
        if(HP > 10)
            HP = 10;
        else if(HP <= 0)
        {
            HP = 0;
            Die();
        }
        updateHpBar();
    }

    void updateHpBar()
    {

        for(int i = 0; i < HpBar.transform.childCount; i++)
        {
            if(HP > i)
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void updateScore()
    {
        scoreTime += Time.deltaTime;
        if(scoreTime > 0.1f)
        {
            score++;
            scoreTime = 0f;
            scoreText.text = score.ToString();
        }
    }

    public void Die()
    {
        deathSound.Play();
        BackgroundMusic.Stop();
        Time.timeScale = 0f;
        replayButton.SetActive(true);
        YouDied.SetActive(true);

    }

    public void replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Samplescene");
    }
}

