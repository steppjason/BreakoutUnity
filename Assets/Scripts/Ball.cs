using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Paddle paddle;
    [SerializeField] AudioClip[] ballSounds;
    [SerializeField] float maxVelocityX;
    [SerializeField] float maxVelocityY;

    Vector2 velocity;

    bool hasVelocity = false;

    Vector2 paddleToBall;
    Vector2 paddlePosition;
    
    Rigidbody2D ballRigid;
    GameStatus gameStatus;

    AudioSource audioSource;
    AudioClip ballCollision;

    void Start(){
        paddlePosition = paddle.transform.position;
        paddleToBall = transform.position - paddle.transform.position;

        ballRigid = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        gameStatus = FindObjectOfType<GameStatus>();

        ballCollision = ballSounds[Random.Range(0,ballSounds.Length)];

        velocity = new Vector2(Random.Range(-5f,5f),15f);
    }

    void Update(){
        if(!hasVelocity){
            LockBallToPaddle();
            LaunchBall();
        }

        if(ballRigid.velocity.y > 0 && ballRigid.velocity.y < 3){
            ballRigid.gravityScale = -0.2f;
        } else if (ballRigid.velocity.y < 0 && ballRigid.velocity.y > -3){
            ballRigid.gravityScale = 0.2f;
        } else {
            ballRigid.gravityScale = 0f;
        }

        if(ballRigid.velocity.x > 0 && ballRigid.velocity.x < 3){
            ballRigid.velocity += new Vector2(0.5f, 0);
        } else if (ballRigid.velocity.x < 0 && ballRigid.velocity.x > -3){
            ballRigid.velocity += new Vector2(-0.5f, 0);
        }

        if(Mathf.Abs(ballRigid.velocity.y) > maxVelocityY){
            if(ballRigid.velocity.y > 0){
                ballRigid.velocity = new Vector2 (ballRigid.velocity.x, maxVelocityY);
            } else if(ballRigid.velocity.y < 0){
                ballRigid.velocity = new Vector2 (ballRigid.velocity.x, maxVelocityY * -1);
            }
        }

        if(Mathf.Abs(ballRigid.velocity.x) > maxVelocityX){
            if(ballRigid.velocity.x > 0){
                ballRigid.velocity = new Vector2 (maxVelocityX, ballRigid.velocity.y);
            } else if(ballRigid.velocity.y < 0){
                ballRigid.velocity = new Vector2 (maxVelocityX * -1, ballRigid.velocity.y);
            }
        }
    }

    private void LockBallToPaddle(){
        paddlePosition = paddle.transform.position;
        transform.position = paddlePosition + paddleToBall;
    }

    private void LaunchBall(){
        if(Input.GetMouseButtonDown(0)){
            hasVelocity = true;
            GetComponent<Rigidbody2D>().velocity = velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(hasVelocity){
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(ballCollision);
        }

        if(collision.gameObject.name == "Paddle"){
            gameStatus.ResetMultipler();
        }
    }
}
