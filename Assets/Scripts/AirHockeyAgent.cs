using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AirHockeyAgent : Agent
{
    public GameManager GameManager;
    public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    public float m_curSpeed = 50f;                 // How fast the tank moves forward and back.
    public float m_maxSpeed = 15f;
    public Rigidbody m_opponent;
    public Rigidbody m_puck;
    private Vector3 m_MaxSpeedVector;
    
    [HideInInspector]
    public float timePenalty;
    public float m_Existential;
    public float m_BallTouch;
    public float m_BallTouchDecay;
    private string m_VerticalAxisName;          // The name of the input axis for moving forward and back.
    private string m_HorizontalAxisName;        // The name of the input axis for turning.
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    // private float m_VerticalInputValue;         // The current value of the movement input.
    // private float m_HorizontalInputValue;       // The current value of the turn input.
    private float playerWidth = 0.118f;
    private ballController m_ballController;
    
    private List<Vector3> initPosList = new List<Vector3>()
                                        {
                                            new Vector3(0.6f, 0.6773136f, 0f),
                                            new Vector3(-0.6f, 0.6773136f, 0f)
                                        };
    
    // Start is called before the first frame update
    public override void Initialize()
    {
        MaxStep = 3000;
        m_Existential = 1f / MaxStep;
        m_BallTouch = 0.3f;
        m_BallTouchDecay = m_BallTouch * 0.2f;
        
        m_Rigidbody = GetComponent<Rigidbody>();
        m_VerticalAxisName = "Vertical" + m_PlayerNumber;
        m_HorizontalAxisName = "Horizontal" + m_PlayerNumber;
        m_MaxSpeedVector = new Vector3(m_maxSpeed, 0f, m_maxSpeed);

        PlayerState myState = new PlayerState()
        {
            playerIndex = m_PlayerNumber - 1,
            agentRb = this.m_Rigidbody,
            startingPos = initPosList[m_PlayerNumber - 1],
            Playercontroller = this,
        };
        GameManager.playerStates.Add(myState);
        

        m_ballController = m_puck.GetComponent<ballController>();

    }

    public override void OnEpisodeBegin()
    {
         // When the tank is turned on, make sure it's not kinematic.
         m_Rigidbody.isKinematic = false;

         // Also reset the input values.
         // m_VerticalInputValue = 0f;
         // m_HorizontalInputValue = 0f;
         // player initialization
         m_Rigidbody.velocity = Vector3.zero;
         m_Rigidbody.transform.localPosition = initPosList[m_PlayerNumber - 1];
         
         //Puck  random initialization 
         if (m_PlayerNumber == 2)
         {
             GameManager.BallInitialize();
         }
    }
    
    IEnumerator waitForGO(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    
    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        Vector3 myPos = transform.position;
        Vector3 myVel = m_Rigidbody.velocity;

        Vector3 oppPos = m_opponent.transform.position;
        Vector3 oppVel = m_opponent.velocity;

        Vector3 puckPos = m_puck.transform.position;
        Vector3 puckVel = m_puck.velocity;
        if (m_PlayerNumber == 1)
        {
            sensor.AddObservation(myPos.x);
            sensor.AddObservation(myPos.z);
            sensor.AddObservation(myVel.x);
            sensor.AddObservation(myVel.z);

            sensor.AddObservation(oppPos.x);
            sensor.AddObservation(oppPos.z);
            sensor.AddObservation(oppVel.x);
            sensor.AddObservation(oppVel.z);

            sensor.AddObservation(puckPos.x);
            sensor.AddObservation(puckPos.z);
            sensor.AddObservation(puckVel.x);
            sensor.AddObservation(puckVel.z);
            // Debug.Log(("p1",
            //             myPos.x,  
            //             myPos.z, 
            //             myVel.x, 
            //             myVel.z,
            //             oppPos.x, 
            //             oppPos.z, 
            //             oppVel.x, 
            //             oppVel.z,
            //             puckPos.x,
            //             puckPos.z,
            //             puckVel.x,
            //             puckVel.z));
        }
        else if (m_PlayerNumber == 2)
        {
            sensor.AddObservation(-1f * myPos.x);
            sensor.AddObservation(-1f * myPos.z);
            sensor.AddObservation(-1f * myVel.x);
            sensor.AddObservation(-1f * myVel.z);

            sensor.AddObservation(-1f * oppPos.x);
            sensor.AddObservation(-1f * oppPos.z);
            sensor.AddObservation(-1f * oppVel.x);
            sensor.AddObservation(-1f * oppVel.z);

            sensor.AddObservation(-1f * puckPos.x);
            sensor.AddObservation(-1f * puckPos.z);
            sensor.AddObservation(-1f * puckVel.x);
            sensor.AddObservation(-1f * puckVel.z);

            // Debug.Log(("p2",
            //     -1f * myPos.x,
            //     -1f * myPos.z,
            //     -1f * myVel.x,
            //     -1f * myVel.z,
            //
            //     -1f * oppPos.x,
            //     -1f * oppPos.z,
            //     -1f * oppVel.x,
            //     -1f * oppVel.z,
            //
            //     -1f * puckPos.x,
            //     -1f * puckPos.z,
            //     -1f * puckVel.x,
            //     -1f * puckVel.z));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Puck"))
        {
            if (m_BallTouch > 0)
            {
                AddReward(m_BallTouchDecay);
                m_BallTouch -= m_BallTouchDecay;
            }

        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
        timePenalty -= m_Existential;
        AddReward(-1f * m_Existential);
        Move(actions.DiscreteActions);


    }
    private void Move(ActionSegment<int> act)
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.


        var forwardForce = 0;
        var rightForce = 0;
        switch (act[0])
        {
            case 1:
                forwardForce = 1;
                break;
            case 2:
                forwardForce = -1;
                break;
                    
        }

        switch (act[1])
        {
            case 1:
                rightForce = 1;
                break;
            case 2:
                rightForce = -1;
                break;
        }

        // if (m_PlayerNumber == 2)
        // {
        //     Debug.Log("p1");
        //     Debug.Log(new Vector2(Input.GetAxis(m_VerticalAxisName), Input.GetAxis(m_HorizontalAxisName)));
        //     Debug.Log(new Vector2(forwardForce, rightForce));
        //
        // }
        
        Vector3 movement = new Vector3(Time.deltaTime * forwardForce * m_curSpeed, 0f , 
            rightForce * m_curSpeed * Time.deltaTime);
        
        if (m_PlayerNumber == 1)
            movement.x = movement.x * -1f;
        if (m_PlayerNumber == 2)
            movement.z = movement.z * -1f;

        
        if (Mathf.Abs(m_Rigidbody.velocity.x) < m_MaxSpeedVector.x )
            m_Rigidbody.AddForce( movement.x, 0, 0, ForceMode.VelocityChange );
        if(Mathf.Abs(m_Rigidbody.velocity.z) < m_MaxSpeedVector.z )
            m_Rigidbody.AddForce(0,0, movement.z, ForceMode.VelocityChange);
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;
        discreteActionsOut[1] = 0;
        var m_VerticalInputValue = Input.GetAxis(m_VerticalAxisName);
        var m_HorizontalInputValue = Input.GetAxis(m_HorizontalAxisName);
        if(m_VerticalInputValue > 0)
        {
            discreteActionsOut[0] = 1;
        }
        else if(m_VerticalInputValue < 0)
        {
            discreteActionsOut[0] = 2;
        }

        if (m_HorizontalInputValue > 0)
        {
            discreteActionsOut[1] = 1;
        }
        else if (m_HorizontalInputValue < 0)
        {
            discreteActionsOut[1] = 2;
        }
        
    }

}
    


// public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
// public float m_Speed = 6f;                 // How fast the tank moves forward and back.
// private Vector3 m_MaxSpeed;
//     
// private string m_VerticalAxisName;          // The name of the input axis for moving forward and back.
// private string m_HorizontalAxisName;        // The name of the input axis for turning.
// private Rigidbody m_Rigidbody;              // Reference used to move the tank.
// private float m_VerticalInputValue;         // The current value of the movement input.
// private float m_HorizontalInputValue;       // The current value of the turn input.
// private float playerWidth = 0.118f;
// private void Awake ()
// {
//     m_Rigidbody = GetComponent<Rigidbody> ();
// }
//
// private void OnEnable ()
// {
//     // When the tank is turned on, make sure it's not kinematic.
//     m_Rigidbody.isKinematic = false;
//
//     // Also reset the input values.
//     m_VerticalInputValue = 0f;
//     m_HorizontalInputValue = 0f;
// }
// private void OnDisable ()
// {
//     // When the tank is turned off, set it to kinematic so it stops moving.
//     m_Rigidbody.isKinematic = true;
// }
// void Start()
// {
//     m_VerticalAxisName = "Vertical" + m_PlayerNumber;
//     m_HorizontalAxisName = "Horizontal" + m_PlayerNumber;
//     m_MaxSpeed = new Vector3(m_Speed, 0f, m_Speed);
// }