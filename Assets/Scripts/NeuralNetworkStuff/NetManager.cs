using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NetManager : MonoBehaviour
{
    private int population = 10;

    private int[] layers = {580, 10, 10, 2 };

    public GameObject netBall;

    public List<NeuralNetwork> nets = new List<NeuralNetwork>();
    public List<GameObject> netBalls = new List<GameObject>();

    public Tilemap _tilemap;

    private int[] worldState = new int[578];
    private int goalPosX;
    private int goalPosY;

    // Start is called before the first frame update
    void Start()
    {
        parseWorld();
        for(int i = 0; i < population; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers, worldState);
            net.Mutate();
            nets.Add(net);

            GameObject newNetBall = Instantiate(netBall, new Vector3(-14.75f, -7.75f, 0), new Quaternion(0, 0, 0, 1));
            newNetBall.GetComponent<NetGolfBallController>().Init(net);
            netBalls.Add(newNetBall);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void parseWorld()
    {
        BoundsInt bounds = _tilemap.cellBounds;
        TileBase[] allTiles = _tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < 30; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null && tile.name == "BlackSquare")
                {
                    worldState[(x * 16) + y] = 1;
                }
                else if (tile != null && tile.name == "Goal")
                {
                    goalPosX = x;
                    goalPosY = y;
                    worldState[(x * 16) + y] = 0;
                }
                else
                {
                    worldState[(x * 16) + y] = 0;
                }
            }
        }

        worldState[576] = goalPosX;
        worldState[577] = goalPosY;


        //for (int x = 0; x < bounds.size.x; x++)
        //{
        //    for (int y = 0; y < bounds.size.y; y++)
        //    {
        //        TileBase tile = allTiles[x + y * bounds.size.x];
        //        if (tile != null)
        //        {
        //            Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
        //        }
        //        else
        //        {
        //            Debug.Log("x:" + x + " y:" + y + " tile: (null)");
        //        }
        //    }
        //}

    }

}
