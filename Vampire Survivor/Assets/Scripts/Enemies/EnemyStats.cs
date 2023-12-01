using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    //Current stats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;
    Transform player;

    [Header("Damage Feedback")]
    public Color damageColor = new Color(255, 255, 255, 1); // What the color of the damage flash should be
    public float damageFlashDuration = 0.2f; // How long the flash should last
    public float deathFadeTime = 0.6f; // How much time it takes for the enemy to fade.
    Color origianlColor;
    SpriteRenderer sr;
    EnemyMovement movement;

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        sr = GetComponent<SpriteRenderer>();
        origianlColor = sr.color;

        movement = GetComponent<EnemyMovement>();
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
    }

    // This function always needs at least 2 values, the amount of damage dealt <dmg>, as well as where the damage is
    // coming from, which is passed as <sourcePosition>. The <sourcePosition> is necessary because it is used to calculate
    // the directionn of the knockbac
    public void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5f, float knockbackDuration = 0.2f)
    {
        currentHealth -= dmg;
        StartCoroutine(DamageFlash());

        // Create the text popup when enemies take damage
        if (dmg > 0) GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);

        // Apply Knockback if it is not zero
        if(knockbackForce > 0)
        {
            // Gets the direction of knockback.
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            movement.Knockback(dir.normalized * knockbackForce, knockbackDuration);
        }

        if(currentHealth <= 0)
        {
            Kill();
        }
    }

    // This is a Coroutine function that makes the nemy flash when taking damage.
    IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = origianlColor;
    }

    public void Kill()
    {
        StartCoroutine(KillFade());
    }

    // This is a Coroutine function that fades the enemy away slowly.
    IEnumerator KillFade()
    {
        //Waits for a single frame.
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, origAlpha = sr.color.a;

        //This is a loop that fires every frame.
        while(t < deathFadeTime)
        {
            yield return w;
            t += Time.deltaTime;

            // Set the colour for this frame.
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1 - t / deathFadeTime) * origAlpha);
        }

        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
        }
    }

    private void OnDestroy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        es.onEnemyKilled();
    }

    private void ReturnEnemy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        transform.position = player.position + es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
    }
}
