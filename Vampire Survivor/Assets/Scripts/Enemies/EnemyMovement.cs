using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;

    Vector2 knockbackVelocity;
    float knockbackDuration;
    
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        // If we are currently being knocked back, then process knockback.
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            // Otherwise, constantly move the enemy toward the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime); // Constantly have the enemy move towards the player.
        }
    }

    // This is meant to be called from other scripts to create knockback
    public void Knockback(Vector2 velocity, float duration)
    {
        //Ignore the knockback if the duration is greater than 0
        if (knockbackDuration > 0) return;

        // Begins the knockback
        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}
