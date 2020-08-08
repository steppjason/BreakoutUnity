using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] int blocks;

    SceneLoader sceneLoader;
    GameStatus gameStatus;

    
    void Start(){
        sceneLoader = FindObjectOfType<SceneLoader>();
        gameStatus = FindObjectOfType<GameStatus>();
    }

    void Update(){
        
    }

    public void CountBlock(){
        blocks++;
    }

    public void RemoveBlock(){
        blocks--;

        if(blocks <= 0){
            gameStatus.ResetMultipler();
            sceneLoader.LoadNextScene();
        }
    }
}
