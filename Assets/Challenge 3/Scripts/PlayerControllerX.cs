﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip jump;

    public bool isLowEnough = false;
    public float heightLimit;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();
        heightLimit = GameObject.Find("Background").GetComponent<BoxCollider>().size.y;

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver )
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        }
        if (transform.position.y > heightLimit)
        {
            playerRb.Sleep();
            transform.position = new Vector3(transform.position.x, heightLimit- 2.5f , transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

        else if (other.gameObject.CompareTag("Ground"))
        {
            if (gameOver == false)
            {
                playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
                playerAudio.PlayOneShot(jump, 1.0f);
            }
            else
            {
                playerRb.Sleep();
            }
        }

    }

}
