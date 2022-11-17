using UnityEngine;
using System.Collections;

public class GolfBallController : MonoBehaviour
{
    LineRenderer lr;

    public float angle = 90.0f;
    public float power = 1.0f;
    public int resolution = 10;

    public float powerIncrement = 0.1f;
    public float angleIncrement = 0.1f;

    float g;
    float radianAngle;

    public int timesHit = 0;
    float maxAngle = 180.0f;
    float minAngle = 0.0f;
    float maxPower = 100.0f;
    float minPower = 0.0f;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y);
    }

    //initialization
    void RenderArc()
    {
        // obsolete: lr.SetVertexCount(resolution + 1);
        lr.positionCount = resolution + 1;
        lr.SetPositions(CalculateArcArray());
    }
    //Create an array of Vector 3 positions for the arc
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (power * power * Mathf.Sin(2 * radianAngle)) / g;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        // set all arc points to allign with the ball
        for (int i = 0; i < arcArray.Length; i++)
        {
            arcArray[i] = new Vector3(arcArray[i].x + transform.position.x, arcArray[i].y + transform.position.y, 0);
        }

        // remove the last 10 points of the arc from the array
        Vector3[] arcArray2 = new Vector3[arcArray.Length - 10];
        for (int i = 0; i < arcArray2.Length; i++)
        {
            arcArray2[i] = arcArray[i];
        }

        lr.positionCount = resolution - 10;
        return arcArray2;
    }

    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * power * power * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }

    void Update()
    {
        // if the ball isn't moving, render the arc
        if (GetComponent<Rigidbody2D>() != null) {
            if (GetComponent<Rigidbody2D>().velocity.magnitude == 0)
            {
                RenderArc();
            }
            else
            {
                // obsolete: lr.SetVertexCount(0);
                lr.positionCount = 0;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            // change the angle of the ball to the left
            if (angle > minAngle)
            {                
                angle -= angleIncrement;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // change the angle of the ball to the right
            if (angle < maxAngle)
            {
                angle += angleIncrement;
            }            
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            // increase the power to be applied to the ball
            if (power < maxPower)
            {
                power += powerIncrement;
            }            
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // decrease the power to be applied to the ball
            if (power > minPower)
            {
                power -= powerIncrement;
            }          
        }

        // if - is pressed slow down time
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            Time.timeScale = Time.timeScale / 2;
        }

        // if = is pressed speed up time
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            Time.timeScale = Time.timeScale * 2;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            timesHit++;
            
            Time.timeScale = 1.0f;
            // apply a force to the ball that is equal to the angle and power
            if (GetComponent<Rigidbody2D>() != null)
            {                
                GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)) * power, ForceMode2D.Impulse);
            }
        }
    }
}
