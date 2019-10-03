using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabilitySckeleton : MonoBehaviour
{
    public enum TypeHability
    {
        Torque,
        Force
    }
    public enum Direction
    {
        Forward,
        Right
    }
    [Serializable]
    public class Hability
    {
        public float m_Force;
        public Vector3 m_RelativePoint;
        public Direction m_Direction;
        public TypeHability m_Type;
        public KeyCode m_Input;
    }

    [SerializeField] private Hability[] m_Habilities;
    
    
    private void Update()
    {
        for (int i = 0; i < m_Habilities.Length; i++)
        {
            if(Input.GetKeyDown(m_Habilities[i].m_Input))
            {
                RaycastHit tHit;
                Physics.Raycast(transform.position + m_Habilities[i].m_RelativePoint , GetRelativeDirection(m_Habilities[i].m_Direction),
                    out tHit);
                if (tHit.collider != null)
                {
                    Debug.Log(tHit.collider.name);
                    
                    Rigidbody tRigidBody = tHit.collider.GetComponent<Rigidbody>();

                    if (m_Habilities[i].m_Type == TypeHability.Force)
                    {
                        tRigidBody.AddForceAtPosition(GetRelativeDirection(m_Habilities[i].m_Direction) * m_Habilities[i].m_Force,tHit.point);
                        
                    }else if (m_Habilities[i].m_Type == TypeHability.Torque)
                    {
                        tRigidBody.AddTorque(GetRelativeDirection(m_Habilities[i].m_Direction) * m_Habilities[i].m_Force, ForceMode.Impulse);
                    }
                }
            }
        }
    }

    private Vector3 GetRelativeDirection(Direction pDirection)
    {
        switch (pDirection)
        {
            case Direction.Forward:
                return transform.forward;
            
            case Direction.Right:
                return transform.right;
        }

        return Vector3.zero;
    }
}
