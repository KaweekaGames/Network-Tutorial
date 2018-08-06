using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance = null;

    [SyncVar]
    public int colorPlace = 0;

    [SyncVar]
    public int playerTurn = 0;

    [SerializeField]
    private List<Player> playerList;

    [SerializeField]
    int numberConnections = 0;

    GameObject[] players;
    GameObject[] cubes;

    private void Start()
    {
        if (!isServer)
        {
            return;
        }
        else
        {
            instance = this;
            playerList = new List<Player>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
        }

        numberConnections = NetworkServer.connections.Count;

        if(playerList.Count != numberConnections)
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            cubes = GameObject.FindGameObjectsWithTag("Cube");

            foreach (GameObject ob in players)
            {
                Player newPlayer = ob.GetComponent<Player>();

                if (!playerList.Contains(newPlayer))
                {
                    playerList.Add(newPlayer);

                    int playerInt = playerList.Count - 1;

                    newPlayer.RpcGetNumber(playerInt);
                    newPlayer.RpcGetLocalGM();

                    Debug.Log("new player");
                }
            }

            foreach (GameObject ob in cubes)
            {
                Cube newCube = ob.GetComponent<Cube>();

                newCube.RpcGetLocalGM();
            }

            if (playerTurn >= playerList.Count)
            {
                playerTurn = 0;
            }
        }
    }

    public int GetColorNumber(int maxNumber)
    {
        colorPlace++;

        if(colorPlace >= maxNumber)
        {
            colorPlace = 0;
        }

        return colorPlace;
    }

    public void NextTurn(int pTrn)
    {
        if(pTrn >= playerList.Count - 1)
        {
            playerTurn = 0;
        }
        else
        {
            playerTurn++;
        }
    }

}
