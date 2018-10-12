
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private bool movingRight = true;
    [SerializeField]
    private AudioSource attackAudio;
    private int raycastMask;
    // Use this for initialization
    void Start () {
        float direction = Random.Range(0, 6);
        movingRight = direction > 2f;

        if(!movingRight)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        attackAudio = GetComponent<AudioSource>();

        int wallLayer = LayerMask.NameToLayer("Wall");
        int floorLayer = LayerMask.NameToLayer("Floor");
        raycastMask = (1 << floorLayer) | (1 << wallLayer);

    }
	
	// Update is called once per frame
	void Update () {
        float moveInDirection;

        if (!movingRight)
            moveInDirection = -1;
        else
            moveInDirection = 1;

        moveInDirection = moveInDirection * moveSpeed * Time.deltaTime;

        Vector3 NewPosition = new Vector3(transform.position.x + moveInDirection, transform.position.y, transform.position.z);
        transform.position = NewPosition;


        if (!movingRight)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void FixedUpdate()
    {
        Vector3 dir = (Quaternion.Euler(30, 60, 0) * transform.forward) * 2f;
        if (!movingRight)
        {
            dir = new Vector3(-dir.x, dir.y, dir.z);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.9f, raycastMask);
        //Debug.DrawRay(transform.position, dir, Color.red);

        if (hit.collider == null)
        {
            movingRight = !movingRight;
        }
        else if (hit.collider.CompareTag("Wall"))
            movingRight = !movingRight;


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GarbageCollection"))
        {
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Player"))
        {
            attackAudio.Play();
        }
    }
}
