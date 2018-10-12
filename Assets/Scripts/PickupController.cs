using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {
    [SerializeField]
    private int pickupScore = 10;
    [SerializeField]
    private ParticleSystem pickupParticles;
    private GameController gameControl;
    private AudioSource chimeSound;
    private SpriteRenderer sr;
    private Collider2D boxCollider;
    
    private void Start()
    {
        gameControl = FindObjectOfType<GameController>();
        chimeSound = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(pickupParticles, transform.position, Quaternion.identity);
            gameControl.AddScore(pickupScore);
            chimeSound.Play();
            StartCoroutine(DestroyObject());
            boxCollider.enabled = false;
            sr.enabled = false;
            //gameObject.SetActive(false);

        }
        else if (collision.gameObject.CompareTag("GarbageCollection"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        yield return null;
    }
}
