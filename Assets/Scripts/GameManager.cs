using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GolfBallController golfBallController;
    string timesHitString = "Times Hit: ";

    void Update()
    {
        timesHitString = "Times Hit: " + golfBallController.timesHit;
        Debug.Log(timesHitString);
    }
    
}