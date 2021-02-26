using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class PlayerState
{
    public int playerIndex;
    public Rigidbody agentRb;
    public Vector3 startingPos;
    // public playercontroller Playercontroller;
    public AirHockeyAgent Playercontroller;

}
public class GameManager : MonoBehaviour
{
    private playercontroller p1scr, p2scr;
    public GameObject GOP1,GOP2;
    
    public GameObject ball;
    [FormerlySerializedAs("ballRB")]
    [HideInInspector]
    public Rigidbody ballRb;
    public bool m_ballRandomInit;
    public GameObject ground;
    public ballController m_BallController;
    private scoreScript scoreScript;
    public List<PlayerState> playerStates = new List<PlayerState>();
    [HideInInspector]
    public Vector3 ballStartingPos;
    [HideInInspector]
    public bool canResetBall;
    private int epnum = 0;
    private bool isRandomInit;
    private Vector3 m_ballInitPosition = new Vector3(0f, 0.66705f, 0);

    
    private void Awake()
    {
        p1scr = (playercontroller)GOP1.GetComponent(typeof(playercontroller));
        p2scr = (playercontroller)GOP2.GetComponent(typeof(playercontroller));
        m_BallController = ball.GetComponent<ballController>();
        scoreScript = this.GetComponent<scoreScript>();
        
        // TODO: why this doesn't work?
        // Academy.Instance.OnEnvironmentReset +=  EnvironmentReset;

    }

    void Start()
    {
        ballRb = ball.GetComponent<Rigidbody>();
    }

    public void goalTouched(int winner)
    {
        scoreScript.addScore(winner);
        foreach (var ps in playerStates)
        {
            //TODO: add reward
            if (ps.playerIndex == winner)
            {
                ps.Playercontroller.AddReward(1); // + ps.Playercontroller.timePenalty);
            }
            else
            {
                ps.Playercontroller.AddReward(-1);
            }


            ps.Playercontroller.EndEpisode();
        }

    }
    
    public void BallInitialize()
    {
        Debug.Log("puck reinitialized"+ epnum.ToString());
        epnum++;
        if (isRandomInit)
        {
            ball.gameObject.transform.position = new Vector3(
                Random.Range(-0.186f, 0.186f), 
                m_ballInitPosition.y, 
                Random.Range(-0.186f, 0.186f)
            );
            ball.transform.rotation = Quaternion.Euler(0, 0, 0);
            ballRb.velocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
        }
        else
        {
            ball.gameObject.transform.position = m_ballInitPosition;
            ball.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            ballRb.velocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
            // Instantiate(ballGenerate, m_initPosition,  Quaternion.Euler(270,0,0));	
        }
        ball.gameObject.SetActive(true);
    }
}
