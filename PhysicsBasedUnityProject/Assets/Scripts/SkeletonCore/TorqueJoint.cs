using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueJoint : MonoBehaviour
{
    [SerializeField] private float m_TorqueJoint;

    [SerializeField] private float m_DownForce;

    [SerializeField] private Rigidbody m_RigidBody;

    [SerializeField] private float m_DeltaTime;

    [SerializeField] private bool m_LocalAngle;

    [SerializeField] private Transform m_Joint;

    [SerializeField] private bool m_Debug;

    [SerializeField] private float m_Distance;

    [SerializeField] private bool m_StayHigh;

    [SerializeField] private bool m_UseAddTorque;

    [SerializeField] private bool m_Normal;

    [SerializeField] private float m_NormalValue;
    
    private Vector3 m_TorqueRotation;

    public float TorqueJointValue
    {
        get => m_TorqueJoint;
        set => m_TorqueJoint = value;
    }

    public Rigidbody RigidBodyValue
    {
        get => m_RigidBody;
        set => m_RigidBody = value;
    }

    private void Update()
    {
        float tHorizontal = Input.GetAxis("Horizontal");
        float tVertical = Input.GetAxis("Vertical");

        Vector3 m_NewTorque = Vector3.Lerp(m_TorqueRotation, GetRotationInverse(), Time.deltaTime * m_DeltaTime);

        if (m_Debug)
        {
            Debug.Log(m_TorqueRotation);
            Debug.Log(Vector3.Distance(m_NewTorque, m_TorqueRotation));
        }

        if (Vector3.Distance(m_NewTorque, m_TorqueRotation) >= m_Distance)
        {
            m_TorqueRotation = m_NewTorque;
            if(m_UseAddTorque)
                m_RigidBody.AddTorque( m_StayHigh?GetRotationInverse() * 1000 : m_TorqueRotation * m_TorqueJoint);
            else
                m_RigidBody.angularVelocity = ( m_StayHigh?GetRotationInverse() * 20 : m_TorqueRotation * m_TorqueJoint);

        }

        if(Input.GetKeyDown(KeyCode.Mouse0))    
            m_RigidBody.AddForce(new Vector3(tHorizontal, 1, tVertical) * Mathf.Abs(m_DownForce));
        if(m_Normal)
            m_RigidBody.AddForce(Vector3.down * m_NormalValue);

    }

    Vector3 GetRotationInverse()
    {
        if(!m_LocalAngle)
            return new Vector3(-GetAngleBetween(m_RigidBody.transform.up, Vector3.forward),0,GetAngleBetween(m_RigidBody.transform.up, Vector3.right));
        else
            return new Vector3(-GetAngleBetween(m_RigidBody.transform.up, m_Joint.forward),0,GetAngleBetween(m_RigidBody.transform.up, m_Joint.right));
    }

    float GetAngleBetween(Vector3 a, Vector3 b)
    {
        Vector3 multiple = Vector3.Scale(a, b);
        float scalar = multiple.x + multiple.y + multiple.z;
        float module = Vector3.Distance(a, Vector3.zero) * Vector3.Distance(b, Vector3.zero);

        return scalar / module;

    }
}
