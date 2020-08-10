using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerController player;
    public float speed = 5f;
    private bool canMove = true; //using boolean to decide whether the enemy can move or not

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        /*********************************************************************************/
        //Here, I am not using transform.lookat or any direction vector to make this simple
        /*********************************************************************************/



        if (player.transform.position.x > this.transform.position.x-2 && player.transform.position.x < this.transform.position.x + 2)
        {
            StartCoroutine(holdEnemy()); //if the player is very near the enemy, the enemy will stop moving
        }

        if(player.transform.position.x > this.transform.position.x) //if the player is right side of enemy
        {
            this.transform.localEulerAngles = new Vector3(0, 180, 0);
            if (canMove)
            {
                this.transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0); 
            } 
        }
        else
        {
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
            if (canMove)
            {
                this.transform.position = transform.position - new Vector3(speed * Time.deltaTime, 0, 0);
            } 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy")){ //if enemy collides with other enemy, it will stop moving
            StartCoroutine(holdEnemy());
        }
    }

    private IEnumerator holdEnemy()
    {
        canMove = false;
        yield return new WaitForSeconds(2f);
        canMove = true;
    }

}


