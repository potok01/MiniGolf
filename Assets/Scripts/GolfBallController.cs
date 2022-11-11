using UnityEngine;

public class GolfBallController : MonoBehaviour
{
    public float angle = 90.0f;
    public float power = 100.0f;
    public int timesHit = 0;

    float maxAngle = 180.0f;
    float minAngle = 0.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // change the angle of the ball to the left
            if (angle > minAngle)
            {
                angle -= 10.0f;
            } 
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // change the angle of the ball to the right
            if (angle < maxAngle)
            {
                angle += 10.0f;
            }            
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // increase the power to be applied to the ball
            power += 100.0f;            
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // decrease the power to be applied to the ball
            power -= 100.0f;           
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            timesHit++;
            // apply a force to the ball in the direction of the angle and power using vector2d
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            if (GetComponent<Rigidbody2D>() != null)
            {
                GetComponent<Rigidbody2D>().AddForce(direction * power);
            }
        }
    }
}