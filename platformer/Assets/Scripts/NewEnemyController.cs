using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyController : MonoBehaviour {

    private Animator anim;
    private int direction;
    private RaycastHit2D hitDown;
    private RaycastHit2D hitUp;
    private RaycastHit2D hitLeft;
    private RaycastHit2D hitRight;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        hitDown = Physics2D.Raycast(transform.position, Vector2.down, 0.9f);
        hitUp = Physics2D.Raycast(transform.position, Vector2.up, 0.9f);
        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.5f);
        hitRight = Physics2D.Raycast(transform.position, Vector2.right, 0.5f);
        //Debug.DrawRay(transform.position, new Vector2(0, -0.9f), Color.blue, 1f);
        //Debug.DrawRay(transform.position, new Vector2(0, 0.9f), Color.blue, 1f);

        if (hitDown.collider != null || hitUp.collider != null) //if were are about to hit a platform or wall
        {
            direction = direction * -1; //change direction
        }

        //is the player touching the enemy 
        if (hitLeft.collider != null && !GameManager.instance.playerDied)
            if (hitLeft.collider.gameObject.CompareTag("Player"))
                // kill off player 
                GameManager.instance.PlayerDeath();
            else if (hitLeft.collider.gameObject.CompareTag("Fire"))
                // player was hit by the fire
                Die();

        if (hitRight.collider != null && !GameManager.instance.playerDied)
            if (hitRight.collider.gameObject.CompareTag("Player"))
                // kill off player 
                GameManager.instance.PlayerDeath();
            else if (hitRight.collider.gameObject.CompareTag("Fire"))
                // player was hit by the fire
                Die();

        // make the enemy move if the player is still alive
        if (!GameManager.instance.playerDied)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, transform.position.y - 1 * direction), Time.deltaTime * 3f);
            Debug.Log(transform.position.y);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
            Destroy(gameObject);
    }

    public void PlayerDeath()
    {
        //set dragon to respong to player's death
        anim.SetBool("PlayerDead", true);
    }
    private void Die()
    {
        //play dragon death sound and then destroy enemy object
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, 0.3f);
    }
}
