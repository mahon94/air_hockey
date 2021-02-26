using System;
using UnityEngine;
using System.Collections;

public class playercontroller : MonoBehaviour
{   
    
    public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    public float m_curSpeed = 60f;                 // How fast the tank moves forward and back.
    public float m_maxSpeed = 14f;
    private Vector3 m_MaxSpeedVector;
    
    private string m_VerticalAxisName;          // The name of the input axis for moving forward and back.
    private string m_HorizontalAxisName;        // The name of the input axis for turning.
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private float m_VerticalInputValue;         // The current value of the movement input.
    private float m_HorizontalInputValue;       // The current value of the turn input.
    
    private void Awake ()
    {
        m_Rigidbody = GetComponent<Rigidbody> ();
    }

    private void OnEnable ()
    {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        // Also reset the input values.
        m_VerticalInputValue = 0f;
        m_HorizontalInputValue = 0f;
    }
    private void OnDisable ()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
    }
    void Start()
    {
        m_VerticalAxisName = "Vertical" + m_PlayerNumber;
        m_HorizontalAxisName = "Horizontal" + m_PlayerNumber;
        m_MaxSpeedVector = new Vector3(m_maxSpeed, 0f, m_maxSpeed);
    }
    
    // Update is called once per frame
    private void Update()
    {
        m_VerticalInputValue = Input.GetAxis(m_VerticalAxisName);
        m_HorizontalInputValue = Input.GetAxis(m_HorizontalAxisName);
        
    }
    
    private void FixedUpdate ()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move();
    }
    
    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        // if (m_PlayerNumber == 1)
        // {
        //     Debug.Log(new Vector2(m_VerticalInputValue, m_HorizontalInputValue));
        //     Debug.Log(new Vector2(m_MaxSpeedVector.x, m_MaxSpeedVector.z));
        // }
        Vector3 movement = new Vector3(Time.deltaTime * m_VerticalInputValue * m_curSpeed, 0f , 
            m_HorizontalInputValue * m_curSpeed * Time.deltaTime);
        

        // movement.x = Mathf.Clamp(movement.x, -1 * 0.2f * playerWidth, 0.2f * playerWidth);
        // movement.z = Mathf.Clamp(movement.z, -1 * 0.2f * playerWidth, 0.2f * playerWidth);
        /*if (movement.x != 0 || movement.z != 0)
        {
         Debug.Log(movement);   
        }
        // Apply this movement to the rigidbody's position.
        // m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        */
        if (m_PlayerNumber == 1)
            movement.x = movement.x * -1f;
        if (m_PlayerNumber == 2)
            movement.z = movement.z * -1f;

        
        if (Mathf.Abs(m_Rigidbody.velocity.x) < m_MaxSpeedVector.x )
            m_Rigidbody.AddForce( movement.x, 0, 0, ForceMode.VelocityChange );
        if(Mathf.Abs(m_Rigidbody.velocity.z) < m_MaxSpeedVector.z )
            m_Rigidbody.AddForce(0,0, movement.z, ForceMode.VelocityChange);
    }

    private void Move2()
    {
        if (Input.GetKey(KeyCode.W))
            if (Mathf.Abs(m_Rigidbody.velocity.x) < m_MaxSpeedVector.x)
                m_Rigidbody.AddForce(-2, 0, 0, ForceMode.Acceleration);

        if (Input.GetKey(KeyCode.S))
            if (m_Rigidbody.velocity.x < m_MaxSpeedVector.x)
                m_Rigidbody.AddForce(2, 0, 0, ForceMode.Acceleration);

        if (Input.GetKey(KeyCode.A))
            if (Mathf.Abs(m_Rigidbody.velocity.z) < m_MaxSpeedVector.z)
                m_Rigidbody.AddForce(0, 0, -2, ForceMode.Acceleration);

        if (Input.GetKey(KeyCode.D))
            if (m_Rigidbody.velocity.z < m_MaxSpeedVector.z)
                m_Rigidbody.AddForce(0, 0, 2, ForceMode.Acceleration);
    }
    


}
