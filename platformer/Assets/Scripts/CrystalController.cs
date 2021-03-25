using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour {

    private int distance = 10;
    private float movement = 0.5f;

    // Use this for initialization
	void Start () {
        StartCoroutine("MoveObject");
	}
	
    // this coroutine moves the crystal up and down over several frames
    IEnumerator MoveObject()
    {
        while (true)
        {
            transform.position = Vector2.Lerp(transform.position, transform.position + new Vector3(0, movement, 0), 1f * Time.deltaTime);
            distance--;
            // reverse the direction of the movement
            if (distance <= 0)
            {
                distance = 10;
                movement = movement * -1;
            }
            yield return null;
        }
    }

    // if the player picks this up, destroy the instance
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameManager.instance.IncrementCrystalCount();
        }
    }
}
