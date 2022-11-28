using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NetManager : MonoBehaviour
{
    public int finishedNets = 0;
    public int generation = 0;

    public float bestFitness = 0;
    public float worstFitness = 0;
    public int bestFitnessTimesHit = 0;

    public int population = 10;
    public int maxHitAttempts = 5;
    public float maxPower = 1000f;
    public float maxAngle = 180.0f;

    private int[] layers = {484, 10, 10, 3 };

    public GameObject netBall;

    public List<NeuralNetwork> nets = new List<NeuralNetwork>();
    public List<GameObject> netBalls = new List<GameObject>();

    private Tilemap _tilemap;

    public int[] worldState = new int[482];
    public float goalPosX;
    public float goalPosY;

    private void Awake()
    {
        _tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }
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
            newNetBall.GetComponent<NetGolfBallController>().Init(net, this);
            netBalls.Add(newNetBall);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SuperSpeed"))
        {
            Time.timeScale = 20;
        }
        if (Input.GetButtonDown("SpeedUp"))
        {
            Time.timeScale = 5;
        }
        if (Input.GetButtonDown("NormalSpeed"))
        {
            Time.timeScale = 0.5f;
        }
        if (Input.GetButtonDown("SlowDown"))
        {
            Time.timeScale = 0.5f;
        }

        if (finishedNets >= population)
        {
            finishedNets = 0;
            nets.Sort();

            bestFitness = nets[population - 1].GetFitness();
            worstFitness = nets[0].GetFitness();
            bestFitnessTimesHit = netBalls[population - 1].GetComponent<NetGolfBallController>().timesHit;

            for (int i = 0; i < population / 2; i++)
            {
                nets[i] = new NeuralNetwork(nets[i + (population / 2)], worldState);
                nets[i].Mutate();

                nets[i + (population / 2)] = new NeuralNetwork(nets[i + (population / 2)], worldState);
            }

            for (int i = 0; i < population; i++)
            {
                nets[i].SetFitness(0f);
            }

            CreateNetBalls();

            generation += 1;
        }
    }

    private void CreateNetBalls()
    {
        if (netBalls != null)
        {
            for (int i = 0; i < netBalls.Count; i++)
            {
                Destroy(netBalls[i].gameObject);
            }

        }

        netBalls = new List<GameObject>();

        for (int i = 0; i < population; i++)
        {
            GameObject newNetBall = Instantiate(netBall, new Vector3(-14.75f, -7.75f, 0), new Quaternion(0, 0, 0, 1));
            newNetBall.GetComponent<NetGolfBallController>().Init(nets[i], this);
            netBalls.Add(newNetBall);
        }

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
                    worldState[(x * 16) + y + 2] = 1;
                }
                else if (tile != null && tile.name == "Goal")
                {
                    worldState[0] = x;
                    worldState[1] = y;

                    worldState[(x * 16) + y + 2] = 0;
                }
                else
                {
                    worldState[(x * 16) + y + 2] = 0;
                }
            }
        }

        Vector3 goalInLocal = _tilemap.GetCellCenterWorld(new Vector3Int(worldState[0], worldState[1], 0));
        Vector3 goalInWorld = goalInLocal + _tilemap.origin;

        goalPosX = goalInWorld.x;
        goalPosY = goalInWorld.y;
    }

}
