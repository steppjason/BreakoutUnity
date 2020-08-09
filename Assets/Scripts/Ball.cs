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

    Gradient trailColor;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKeys;
    
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

        trailColor = new Gradient();
        colorKey = new GradientColorKey[1];
        colorKey[0].color = Color.white;
        colorKey[0].time = 0.0f;
        alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 0.0f;
        alphaKeys[1].time = 1.0f;
    }

    void Update(){
        if(!hasVelocity){
            LockBallToPaddle();
            LaunchBall();
        }

        if(hasVelocity){
            if(gameStatus.GetMultiplier() > 0){
                GetComponent<TrailRenderer>().time = 0.2f + gameStatus.GetMultiplier() * 0.05f;

                switch(gameStatus.GetMultiplier()){
                    case 1:
                        colorKey[0].color = new Color(1f,0.8353392f,0f,1f);
                        break;
                    case 2:
                        colorKey[0].color = new Color(1f,0.7282301f,0f,1f);
                        break;
                    case 3:
                        colorKey[0].color = new Color(1f,0.6082861f,0f,1f);
                        break;
                    default:
                        colorKey[0].color = new Color(1f,0.3112785f,0f,1f);
                        break;
                }

            } else {
                GetComponent<TrailRenderer>().time = 0.2f;
                colorKey[0].color = new Color(1f,1f,1f,1f);
            }
        }

        trailColor.SetKeys(colorKey, alphaKeys);
        GetComponent<TrailRenderer>().colorGradient = trailColor;

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
            GetComponent<TrailRenderer>().time = 0.2f;
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
