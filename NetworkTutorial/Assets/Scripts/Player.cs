using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    Cube cube;

    public Text name;

    [SerializeField]
    GameManager localGameManager;

    public int myInt = 0;

    public bool accessedGM = false;

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
    }

    public void ChangeCubeColor(GameObject cb)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        CmdChangeColor(cb);
    }

    [Command]
    void CmdChangeColor(GameObject cb)
    {
        cube = cb.GetComponent<Cube>();
        cube.ChangeMyColor();
    }


    [ClientRpc]
    public void RpcGetNumber (int number)
    {
        myInt = number;
    }

    [ClientRpc]
    public void RpcGetLocalGM()
    {
        GameObject gm = GameObject.FindGameObjectWithTag("GameManger");

        localGameManager = gm.GetComponent<GameManager>();

        int randNum = Random.Range(0, 100);

        name.text = gm.name + randNum.ToString();
    }

    public int GetPlayerTurn()
    {
        return localGameManager.playerTurn;
    }
}
