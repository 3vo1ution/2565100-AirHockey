using UnityEngine;

public class AiScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // AI movement speed
    [SerializeField] public Rigidbody2D puck; // Reference to the puck Rigidbody2D
    private Rigidbody2D rb; // Reference to the AI paddle Rigidbody2D
    [SerializeField] private float minX, maxX, minY, maxY; // Movement constraints

    private bool isContactingPuck = false;
    private float contactTime = 0f;
    private float maxContactTime = 1f; // Maximum time AI can stay in contact with puck

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Ensure the AI paddle stays within the defined movement constraints
        Vector2 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        // Calculate direction vector from AI to Puck
        Vector2 direction = (puck.position - rb.position).normalized;

        if (!isContactingPuck)
        {
            // Move the AI towards the Puck
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
        else
        {
            // If contacting puck for more than one second, briefly move away
            contactTime += Time.fixedDeltaTime;
            if (contactTime >= maxContactTime)
            {
                isContactingPuck = false;
                contactTime = 0f;
            }
            else
            {
                // Briefly move away from puck
                rb.MovePosition(rb.position - direction * speed * Time.fixedDeltaTime);
            }
        }

        // Bounce effect when the puck collides with the AI paddle
        if (Mathf.Abs(rb.position.x - puck.position.x) < 0.5f && Mathf.Abs(rb.position.y - puck.position.y) < 0.5f)
        {
            isContactingPuck = true;
            contactTime = 0f; // Reset contact time
            Vector2 reflectedDirection = Vector2.Reflect(direction, rb.position - puck.position).normalized;
            puck.velocity = reflectedDirection * speed;
        }
    }
}
