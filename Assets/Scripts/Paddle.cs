using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] float screenWidthUnits = 32f;
    [SerializeField] float minX = -5.25f;
    [SerializeField] float maxX = 5.25f;
    Vector2 paddlePosition = new Vector2();
    float mousePositionInUnits;

    // Start is called before the first frame update
    void Start()
    {
        mousePositionInUnits = (Input.mousePosition.x - (Screen.width / 2)) / Screen.width * screenWidthUnits;
        paddlePosition.x = mousePositionInUnits;
        paddlePosition.y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        mousePositionInUnits = (Input.mousePosition.x - (Screen.width / 2)) / Screen.width * screenWidthUnits;        
        paddlePosition.x = Mathf.Clamp(mousePositionInUnits, minX, maxX);
        transform.position = paddlePosition;
    }
}
 