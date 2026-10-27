using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip bounceSound;

    [Header("Properties")]
    public float speed = 8f;
    public float speedMultiplier = 1.1f;

    [Header("Other Scripts")]
    public Player player;

    private float currentSpeed;

    private AudioSource audioSource;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        SetBall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBall()
    {
        StartCoroutine(LaunchBall());
    }

    private IEnumerator LaunchBall()
    {
        transform.position = Vector2.zero;
        rb.linearVelocity = Vector2.zero; 
        currentSpeed = 0f;

        yield return new WaitForSeconds(1.5f);

        currentSpeed = speed;

        float xDirection = Random.Range(0, 2) == 0 ? -1f : 1f;
        float yDirection = Random.Range(-0.5f, 0.5f);

        Vector2 launchDirection = new Vector2(xDirection, yDirection).normalized;
        rb.linearVelocity = launchDirection * currentSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "LeftScore":
                player.AddScore("left");
                break;
            case "RightScore":
                player.AddScore("right");
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            currentSpeed *= speedMultiplier;

            float ballY = transform.position.y;
            float platformY = collision.transform.position.y; 
            float platformHeight = collision.collider.bounds.size.y;
            float bounceAngleFactor = (ballY - platformY) / platformHeight;

            float xDirection = transform.position.x > collision.transform.position.x ? 1f : -1f;

            Vector2 newDirection = new Vector2(xDirection, bounceAngleFactor).normalized;
            rb.linearVelocity = newDirection * currentSpeed;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            float yDirection = transform.position.y > collision.transform.position.y ? 1f : -1f;
            float dynamicY = Mathf.Abs(rb.linearVelocity.y);

            if (dynamicY < 0.1f) 
            {
                dynamicY = 2f; 
            }

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, dynamicY * yDirection);
        }

        audioSource.clip = bounceSound;
        audioSource.Play();
    }
}
