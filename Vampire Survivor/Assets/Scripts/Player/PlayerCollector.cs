using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        playerCollector.radius = player.CurrentMagnet;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the other game object has the ICollectible interface
        if(collision.gameObject.TryGetComponent(out iCollectible collectible))
        {
            // Pulling animation
            //Gets the Rigidbody2D component on the item
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            //Vector2 pointing from item to player
            Vector2 forceDirection = (transform.position - collision.transform.position).normalized;
            //Applies force to the item in the forceDirection with pullSpeed
            rb.AddForce(forceDirection * pullSpeed);

            // If it does, call the collect method
            collectible.Collect();
        }
    }
}
