using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver=false;
    private float floatForce = 0.5f;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip GroundSound;

    private bool outarange=false;
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();
        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !gameOver && !outarange){
            playerRb.AddForce(Vector3.up * floatForce*2, ForceMode.Impulse);
        }
        // While space is pressed and player is low enough, float up
        if (transform.position.y < 2){
            //outarange=true;
            //transform.Translate(Vector3.up * (1.0f - transform.position.y));
            playerRb.AddForce(Vector3.up*floatForce, ForceMode.Impulse);
        }else if(transform.position.y > 16){
            outarange=true;
            playerRb.AddForce(Vector3.down*floatForce, ForceMode.Impulse);
            //transform.Translate(Vector3.down * (transform.position.y-15.0f));
        }else{
            outarange=false;
        }
        /*if ((outarange)&&((transform.position.y >= 2)&&(transform.position.y <= 16)))
        {
          //Debug.Log("Player is within the range");
          //playerRb.velocity = playerRb.velocity/10;
          StartCoroutine("slowDown"); 
          outarange=false;
        }*/
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
        else if(other.gameObject.CompareTag("Ground")){
            Debug.Log("Touched the grass!");
            playerAudio.PlayOneShot(moneySound,1.0f);
        }
            
    }

    IEnumerator slowDown(){
        while(playerRb.velocity.y>0.1f){
            playerRb.velocity = playerRb.velocity/2.5f;
            yield return new WaitForSeconds(0.01f);
            playerRb.AddForce(Vector3.up*floatForce*5, ForceMode.Impulse);
        }
    }
}
