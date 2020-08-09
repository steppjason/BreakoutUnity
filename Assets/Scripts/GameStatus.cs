using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameStatus : MonoBehaviour
{
    [Range(0.1f, 2f)][SerializeField] float gameSpeed = 1f;
    [SerializeField] Text scoreText;
    [SerializeField] Text scoreMultipler;
    [SerializeField] ParticleSystem scoreMultiplerFX;

    [SerializeField] int currentScore = 0;
    [SerializeField] int multiplier = -1;

    void Awake()
    {
        int gameStatusCount = FindObjectsOfType<GameStatus>().Length;
        
        if(gameStatusCount > 1) {
            gameObject.SetActive(false);
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start(){
        scoreText.text = currentScore.ToString();
        ResetMultipler();
    }

    void Update(){
        Time.timeScale = gameSpeed;
    }

    public void AddToScore(int blockpoints){
        if(multiplier > 0){
            blockpoints *= multiplier;
        }
        currentScore += blockpoints;
        scoreText.text = String.Format("{0:n0}",currentScore);
    }

    public void ResetGameStatus(){
        Destroy(gameObject);
    }

    public void AddMultipler(){
        multiplier++;
        if(multiplier > 0){
            scoreMultipler.text = "x" + multiplier.ToString();
            scoreMultiplerFX.Play();
        }
    }

    public void ResetMultipler(){
        multiplier = -1;
        scoreMultipler.text = "";
        scoreMultiplerFX.Stop();
    }

    public int GetMultiplier(){
        return multiplier;
    }
}
