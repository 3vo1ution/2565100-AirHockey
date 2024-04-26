using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckScript : MonoBehaviour
{
    public ScoreScript ScoreScriptInstance;
    private Vector2 initialPosition;
    public static bool WasGoal { get; private set; }
    public float MaxSpeed;
    private Rigidbody2D rb;
    private bool playerLastHit; // To track the last player who hit the puck

    // Add a reference to the region colliders
    public Collider2D[] regionColliders;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        WasGoal = false;
        initialPosition = transform.position; // Store initial position of the puck

        // Start the coroutine to activate regions randomly
        StartCoroutine(ActivateRegionRandomly());
    }

    private IEnumerator ActivateRegionRandomly()
    {
        while (true)
        {
            // Choose a random time interval to activate a new region
            float randomTime = Random.Range(5f, 10f);
            yield return new WaitForSeconds(randomTime);

            // Activate a random region
            ActivateRandomRegion();
        }
    }

    private void ActivateRandomRegion()
    {
        // Disable all region colliders
        foreach (Collider2D collider in regionColliders)
        {
            collider.enabled = false;
        }

        // Choose a random region collider to activate
        int randomIndex = Random.Range(0, regionColliders.Length);
        Collider2D randomRegionCollider = regionColliders[randomIndex];

        // Enable the collider of the randomly chosen region
        randomRegionCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!WasGoal)
        {
            if (other.tag == "AiGoal")
            {
                ScoreScriptInstance.Increment(ScoreScript.Score.PlayerScore);
                playerLastHit = true;
                WasGoal = true;
                StartCoroutine(ResetPuck(false));
            }
            else if (other.tag == "PlayerGoal")
            {
                ScoreScriptInstance.Increment(ScoreScript.Score.AiScore);
                playerLastHit = false;
                WasGoal = true;
                StartCoroutine(ResetPuck(true));
            }
            else if (other.CompareTag("Region")) // Check if the puck hits a region
            {
                // Award one point to the player who last hit the puck
                if (playerLastHit)
                {
                    ScoreScriptInstance.Increment(ScoreScript.Score.PlayerScore);
                }
                else
                {
                    ScoreScriptInstance.Increment(ScoreScript.Score.AiScore);
                }

                // Reset puck position after scoring
                StartCoroutine(ResetPuck(playerLastHit));
            }
        }
    }

    private IEnumerator ResetPuck(bool didAiScore)
    {
        yield return new WaitForSecondsRealtime(1);
        WasGoal = false;
        rb.velocity = rb.position = new Vector2(0, 0);

        if (didAiScore)
            rb.position = new Vector2(0, 0);
        else
            rb.position = new Vector2(0, 0);
    }

    public void CenterPuck()
    {
        rb.position = new Vector2(0, 0);
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
    }
}