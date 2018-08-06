using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Cube : NetworkBehaviour
{
    [SerializeField]
    GameManager localGameManager;

    Renderer rend;
    Color newColor;
    Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow };
    int numCol;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();

        numCol = colors.Length;

    }

   [ClientRpc]
   public void RpcChangeColor()
    {
        newColor = colors[localGameManager.GetColorNumber(numCol)];

        rend.material.color = newColor;
    }

    [ClientRpc]
    public void RpcGetLocalGM()
    {
        GameObject gm = GameObject.FindGameObjectWithTag("GameManger");

        localGameManager = gm.GetComponent<GameManager>();

        newColor = colors[localGameManager.colorPlace];

        if(rend == null)
        {
            rend = GetComponent<Renderer>();
        }

        rend.material.color = newColor;
    }

    public void ChangeMyColor()
    {
        RpcChangeColor();
        GameManager.instance.NextTurn(localGameManager.playerTurn);
        Debug.Log("turn over");
    }
}
