using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[SerializeField]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instant;

    public int speed = 5;
    bool isGrounded = true; //to check if the player is grounded

    float hMovement = 0f;

    public Animator animator;

    private bool canMove;

    private int currentScore = 0; 
    public Text scoreText;

    private int lives;
    public List<Image> heartList; 

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        lives = 3;
        instant = this;
    }

    // Update is called once per frame
    void Update()
    {
        hMovement = Input.GetAxis("Horizontal") * speed; //getting the horizontal input

        if (isGrounded) 
        {
            animator.SetFloat("move", Mathf.Abs(hMovement));
            animator.SetBool("isJumping", false);
        }

        if (Input.GetKey(KeyCode.UpArrow) && isGrounded == true)
        {
            isGrounded = false;
            animator.SetBool("isJumping", true);
            jump();
        }

        scoreText.text = currentScore.ToString();

        //Debug.Log(currentScore);
        //Debug.Log(lives);
    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    public void jump()
    {
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision) //to check if player is colliding with anything
    {
        if (collision.gameObject.CompareTag("ground")) //if player touches the ground, he is grounded
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
        else if (collision.gameObject.CompareTag("enemy"))
        {
            lives--;
            if (lives == 0)
            {
                gameOver(); //game will over if player run out of lives
            }
            heartList[lives].gameObject.SetActive(false);
            StartCoroutine(colEnemy());

            //if player touches the enemy, he will bounce back

            if (this.transform.position.x < collision.transform.position.x && isGrounded)
            {
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-10, 10), ForceMode2D.Impulse);
            }
            else if (this.transform.position.x > collision.transform.position.x && isGrounded)
            {
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 10), ForceMode2D.Impulse);
            }
            else 
            {
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 10), ForceMode2D.Impulse);
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("coin")) //if player collects coins, the score will increase
        {
            currentScore++;
            collision.gameObject.SetActive(false);
        }
    }

    private void movePlayer()
    {
        if (canMove)
        {
            if (hMovement < 0) //if the player is moving in left direction, we will rotate the player
            {
                if (this.transform.localEulerAngles.y != 180)
                {
                    this.transform.localEulerAngles = new Vector3(0, 180, 0);
                }
            }
            else
            {
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
            }

            this.transform.position = this.transform.position + new Vector3(hMovement * Time.deltaTime, 0, 0); //moving the player
        }
    }

    private IEnumerator colEnemy()
    {
        canMove = false;
        yield return new WaitForSeconds(1f);
        canMove = true;
    }

    private void gameOver() //if the game is over
    {
        if (currentScore > MainMenu.instant.highScore) //setting the high score
        { 
            MainMenu.instant.highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", MainMenu.instant.highScore);
        }
        SceneManager.LoadScene("GameOver");
    }

}
