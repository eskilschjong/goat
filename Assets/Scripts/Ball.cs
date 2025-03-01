using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    public TextMeshProUGUI directionText;
    public TextMeshProUGUI forceText;
    public GameObject arrowObject;

    public float forceLevel = 0f;
    public float maxForceLevel = 20f;
    public float direction1 = 5f, direction2 = 5f;

    private Rigidbody2D rb;

    // Audio
    public AudioClip backboardHit;
    public AudioClip rimHit;
    public AudioClip netHit;
    public AudioClip floorHit;

    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0; // Disable gravity at the start
        }
        directionText.text = direction1.ToString("F2");
        forceText.text = forceLevel.ToString("F2");

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    

    void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            if (forceLevel < maxForceLevel)
            {
                forceLevel += Time.deltaTime * 5;
            }
            else
            {
                forceLevel = maxForceLevel;
            }
            forceText.text = forceLevel.ToString("F2");
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (rb != null)
            {
                Vector2 force = new Vector2(direction1, direction2).normalized * forceLevel;
                rb.AddForce(force, ForceMode2D.Impulse);
                rb.AddTorque(0.1f, ForceMode2D.Impulse);
                forceLevel = 0f;
                rb.gravityScale = 1; // Enable gravity when the mouse button is released
                if (arrowObject != null)
                {
                    arrowObject.SetActive(false);
                }
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction1 = Mathf.Clamp(direction1 + Time.deltaTime * 5, 0, 10);
            directionText.text = direction1.ToString("F2");
        }

        if (Input.GetKey(KeyCode.W))
        {
            direction1 = Mathf.Clamp(direction1 - Time.deltaTime * 5, 0, 10);
            directionText.text = direction1.ToString("F2");
        }
        UpdateArrowDirection();
    }

    void UpdateArrowDirection()
    {
        if (arrowObject != null)
        {
            // Calculate the angle in radians, then convert to degrees
            float angle = Mathf.Atan2(direction2, direction1) * Mathf.Rad2Deg;
            
            // Apply rotation to the arrow object
            arrowObject.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Optionally, you can move the arrow to always be near the ball
            arrowObject.transform.position = transform.position;
        }
    }
    private bool netCollided = false;
    private bool stopBouncing = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        if (collision.gameObject.CompareTag("Backboard"))
        {
            PlaySound(backboardHit);
        }
        else if (collision.gameObject.CompareTag("Rim"))
        {
            PlaySound(rimHit);
        }
        else if (collision.gameObject.CompareTag("Net") && !netCollided)
        {
            PlaySound(netHit);
            netCollided = true;
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            if (!stopBouncing)
            {
                PlaySound(floorHit);
            }
            // Makes the ball stop bouncing forever
            if (rb.linearVelocity.magnitude < 2f && !stopBouncing) {
                Debug.Log("stop bouncing");
                rb.linearVelocity = Vector2.zero;
                stopBouncing = true;
            }
        }

    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
