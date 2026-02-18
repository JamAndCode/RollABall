using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float moveX;
    private float moveY;
    private float moveZ;
    private Rigidbody playerRb;
    public float speed = 10.0f;

    public int count;
    public TMP_Text countText;
    public TMP_Text winText;
    private bool isOnGround = false;
    public float jumpForce = 5.0f;
    private GameManager gameManager;
    private AudioSource collectibleSound;
    public ParticleSystem collectibleEffect;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.gameObject.SetActive(false);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        collectibleSound = GetComponent<AudioSource>();
    }

    public void OnMove(InputValue moveValue)
    {
        Vector2 moveVector = moveValue.Get<Vector2>();

        moveX = moveVector.x;
        moveZ = moveVector.y;
    }

    public void OnJump()
    {
        if (isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);   
            isOnGround = false;
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveX, moveY, moveZ);
        playerRb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            collectibleSound.Play();
            ParticleSystem effect = Instantiate(collectibleEffect, other.transform.position, other.transform.rotation);
            
            //Imported effect has camera shake. Removed because it sucks
            CartoonFX.CFXR_Effect cfxrEffect = effect.GetComponent<CartoonFX.CFXR_Effect>();
            if (cfxrEffect != null && cfxrEffect.cameraShake != null)
            {
                cfxrEffect.cameraShake.enabled = false;
            }
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        
        //TODO: fix this. Only decreases on first enterance
        }else if (other.CompareTag("Trap"))
        {
            playerRb.linearVelocity = new Vector3(0.5f, 0, 0.5f);
        }
        else if (other.CompareTag("Enemy"))
        {
            //Random player tp was disorientating, changing to decrease score and teleport enemy away
            count--;
            SetCountText();

            //attachedRigidbody doesn't work :(
            //I'm just going to destroy and respawn elsewhere
            Destroy(GameObject.Find("Enemy(Clone)"));
            Instantiate(enemyPrefab, new Vector3(Random.Range(-9, 9), 1f, Random.Range(-9, 9)), Quaternion.identity);
            other.attachedRigidbody.linearVelocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //this isn't the right way to do this, since collision != ground, but if player can utilize it thats pretty funny
        bool isGroundCollision = collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("ColumnCollectible") || collision.gameObject.CompareTag("BridgeCollectible");
        if (isGroundCollision)
        {
            isOnGround = true;
        }else if (collision.gameObject.CompareTag("Bumper"))
        {
            Vector3 bumperDirection = (transform.position - collision.transform.position).normalized;
            playerRb.AddForce(bumperDirection * jumpForce * 1.5f, ForceMode.Impulse);
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (GameObject.FindGameObjectsWithTag("PickUp").Length <= 0)
        {
            gameManager.GameOver();
        }
    }
}
