using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float moveSpeedInAir = 1000f;
    [SerializeField]
    private float jumpStrength = 100f;
    [SerializeField]
    private List<Sprite> playerSprites;
    [SerializeField]
    private ParticleSystem resurrectParticles;
    [SerializeField]
    private int invulnerableFlashes = 20;
    [SerializeField]
    private float invulnerableFlashTime = 0.1f;
    [SerializeField]
    private AudioSource playerAudio;


    private GameController gc;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    private bool isDead;
    private bool inputDisabled;
    private int playerLayer;
    private int enemyLayer;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        gc = FindObjectOfType<GameController>();
        playerAudio = GetComponent<AudioSource>();
        isGrounded = true;
        inputDisabled = false;
        isDead = false;

        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");

        EnableCollision();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputDisabled)
        {
            if (isGrounded)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    rb.angularVelocity += moveSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    rb.angularVelocity -= moveSpeed * Time.deltaTime;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.velocity = new Vector2(rb.velocity.x, Vector2.up.y * jumpStrength);
                    isGrounded = false;
                    playerAudio.Stop();
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    rb.velocity = new Vector2(Mathf.Max(-4f, rb.velocity.x - (moveSpeedInAir * Time.deltaTime)), rb.velocity.y);
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    rb.velocity = new Vector2(Mathf.Min(4f, rb.velocity.x + (moveSpeedInAir * Time.deltaTime)), rb.velocity.y);
                }
            }
        }

        if(Mathf.Abs(rb.angularVelocity) > 10f)
        {
           // Debug.Log("Velocity " + Mathf.Abs(rb.angularVelocity).ToString());
            if(!playerAudio.isPlaying && isGrounded)
            {
                playerAudio.Play();
            }
            playerAudio.volume = Mathf.Abs(rb.angularVelocity) / 450f;
        }
        else
        {
            playerAudio.Stop();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            HandleDeath();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Wall")) && Mathf.Abs(rb.velocity.y) > 0.5f)
        {
            isGrounded = false;
        }
    }

    private void HandleDeath()
    {
        if (isDead)
            return;

        PlayerDie();
        if (gc.TakeLife())
        {
            //Resurrect
            StartCoroutine(Resurrect());
        }
    }

    private void EnableCollision()
    {
        int mask = Physics2D.GetLayerCollisionMask(playerLayer);

        mask |= (1 << enemyLayer);
        Physics2D.SetLayerCollisionMask(playerLayer, mask);

        mask = Physics2D.GetLayerCollisionMask(enemyLayer);

        mask |= (1 << playerLayer);
        Physics2D.SetLayerCollisionMask(enemyLayer, mask);
    }

    private void DisableCollision()
    {
        int mask = Physics2D.GetLayerCollisionMask(playerLayer);

        mask &= ~(1 << enemyLayer);
        Physics2D.SetLayerCollisionMask(playerLayer, mask);

        mask = Physics2D.GetLayerCollisionMask(enemyLayer);

        mask &= ~(1 << playerLayer);
        Physics2D.SetLayerCollisionMask(enemyLayer, mask);
    }

    IEnumerator Resurrect()
    {
        //yield return new WaitForSeconds(2f);
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, -1f);
        Instantiate(resurrectParticles, spawnPos, Quaternion.identity, transform);
        yield return new WaitForSeconds(1f);
        PlayerAlive();

        for(int i = 0; i < invulnerableFlashes; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(invulnerableFlashTime);
        }
        sr.enabled = true;

        EnableCollision();

        yield return null;
    }
    private void PlayerDie()
    {
        sr.sprite = playerSprites[1];
        inputDisabled = true;
        isDead = true;

        DisableCollision();
    }

    private void PlayerAlive()
    {
        sr.sprite = playerSprites[0];
        inputDisabled = false;
        isDead = false;

        //Player starts invulnerable
    }
}
