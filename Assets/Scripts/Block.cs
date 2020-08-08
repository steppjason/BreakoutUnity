using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] AudioClip blockSound;
    [SerializeField] GameObject blockParticles;
    [SerializeField] Sprite[] hitSprites;

    [SerializeField] int health;
    [SerializeField] int blockPoints;

    Level level;
    GameStatus gameStatus;

    void Start(){
        
        health = hitSprites.Length;

        level = FindObjectOfType<Level>();
        gameStatus = FindObjectOfType<GameStatus>();

        if(tag == "Breakable"){
            level.CountBlock();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        
        health--;

        if(health == 0 && tag == "Breakable"){
            TriggerParticle();
            AudioSource.PlayClipAtPoint(blockSound,Camera.main.transform.position, 0.4f);
            Destroy(gameObject);

            gameStatus.AddToScore(blockPoints);
            gameStatus.AddMultipler();

            level.RemoveBlock();

        } else if(tag == "Breakable"){
            UpdateHitSprite();
        }
    }

    private void UpdateHitSprite(){
        int spriteIndex = health - 1;
        GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
    }

    private void TriggerParticle(){
        GameObject particle = Instantiate(blockParticles, transform.position, transform.rotation);
        var main = particle.gameObject.GetComponent<ParticleSystem>().main;
        main.startColor = gameObject.GetComponent<SpriteRenderer>().color;
        Destroy(particle, 2f);
    }
}
