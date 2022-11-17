using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{ 
    public TextMeshProUGUI timesHitText;
    public TextMeshProUGUI victoryText;
    public TextMeshProUGUI putText;

    public GameObject victoryScreen;
    public GolfBallController golfBallController;
    public GameObject golfBall;

    public Tilemap tilemap;
    public TileBase holeTile;

    // Update is called once per frame
    void Update()
    { 
        // update the times hit on the screen
        timesHitText.text = "Puts: " + golfBallController.timesHit.ToString();

        // check if the ball is in the hole
        if (tilemap.GetTile(tilemap.WorldToCell(golfBall.transform.position)) == holeTile)
        {
            // if the golf ball is not moving
            if (golfBall.GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f)
            {
                // show the victory screen
                victoryScreen.SetActive(true);
                if (golfBallController.timesHit == 1) 
                {
                    victoryText.text = "Hole in one!";
                } else 
                {
                    victoryText.text = "Victory!";
                }
                putText.text = "Puts: " + golfBallController.timesHit.ToString();

                // unlock the cursor
                Cursor.lockState = CursorLockMode.None;
                
                // dont read any button presses
                golfBallController.enabled = false;
            }
        }
    }
}