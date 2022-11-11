using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GolfBallController golfBallController;

    void Update()
    {
        Debug.Log("Angle: " + golfBallController.angle);
        Debug.Log("Power: " + golfBallController.power);
        Debug.Log("Times Hit: " + golfBallController.timesHit);
    }
    
}