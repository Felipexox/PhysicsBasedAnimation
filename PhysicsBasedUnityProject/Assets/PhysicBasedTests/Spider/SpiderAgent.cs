using System;
using System.Collections;
using System.Collections.Generic;
using MLAgents;
using TMPro;
using UnityEngine;

public class SpiderAgent : Agent
{
    [System.Serializable]
    public class Joints
    {
        public TorqueJoint m_Paw;
        public TorqueJoint m_Feet;
    }

    private SpiderAcademy m_Academy;

    private Vector3 m_InitPosi;
    
    public float timeBetweenDecisionsAtInference;
    private float m_TimeSinceDecision;
    
    [SerializeField] private Joints[] m_Legs;

    [SerializeField] public float m_MaxDistance = 0.3f;
    private bool m_ReceiveReward = false;

    [SerializeField] private bool m_TouchGround;
    
    private void Start()
    {
        m_Academy = FindObjectOfType(typeof(SpiderAcademy)) as SpiderAcademy;
        m_InitPosi = transform.position;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
//        for (int i = 0; i < vectorAction.Length; i++)
//        {
//            Debug.Log("Vector "+i+": "+vectorAction[i]);
//        }
        for (int i = 0; i < m_Legs.Length; i += 1)
        {
            m_Legs[i].m_Paw.TorqueJointValue =  (vectorAction[i] + 0.2f) * 300;
        }
        for (int i = 0; i < m_Legs.Length; i += 1)
        {
            m_Legs[i].m_Feet.TorqueJointValue = (vectorAction[i+4] + 0.2f) * 300;
        }
        
    }

    public override void AgentReset()
    {
      

        Debug.Log("Agent reseted");
        for (int i = 0; i < m_Legs.Length; i += 2)
        {
            m_Legs[i].m_Paw.TorqueJointValue = 0;
            m_Legs[i].m_Feet.TorqueJointValue = 0;
        }

        transform.position = m_InitPosi;
        m_MaxDistance = 0;
        m_ReceiveReward = false;
    }

    public override void CollectObservations()
    {
        for (int i = 0; i < m_Legs.Length; i += 1)
        { 
            AddVectorObs(transform.position - m_Legs[i ].m_Feet.RigidBodyValue.transform.position);
            
            AddVectorObs(m_Legs[i ].m_Feet.RigidBodyValue.transform.rotation);


        }
        AddVectorObs(transform.position - m_InitPosi);
        AddVectorObs(transform.position.y);
        AddVectorObs(transform.forward);
        AddVectorObs(m_MaxDistance);
    }
    public void FixedUpdate()
    {
        WaitTimeInference();
    }

    private void WaitTimeInference()
    {
        if (!m_Academy.GetIsInference())
        {
            RequestDecision();
        }
        else
        {
            if (m_TimeSinceDecision >= timeBetweenDecisionsAtInference)
            {
                m_TimeSinceDecision = 0f;
                RequestDecision();
            }
            else
            {
                m_TimeSinceDecision += Time.fixedDeltaTime;
            }
        }
    }

    private void Update()
    {
        if (Vector3.Distance(m_InitPosi, transform.position) > m_MaxDistance)
        {
            m_MaxDistance = Vector3.Distance(m_InitPosi, transform.position);
            AddReward(0.01f * m_MaxDistance);
        }

        if (!m_ReceiveReward && !m_TouchGround)
        {
            AddReward(3);
            m_ReceiveReward = true;
        }

        if (m_ReceiveReward && m_TouchGround)
        {
            AddReward(-3);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.name == "Ground")
        {
            m_TouchGround = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.name == "Ground")
        {
            m_ReceiveReward = false;
            m_TouchGround = false;
        }
    }
}