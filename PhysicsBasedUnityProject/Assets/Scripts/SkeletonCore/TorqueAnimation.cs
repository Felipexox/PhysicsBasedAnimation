using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueAnimation : MonoBehaviour
{
    [SerializeField] private TorqueJoint m_TorqueHead;
    [SerializeField] private TorqueJoint m_TorqueBody;
    [SerializeField] private TorqueJoint m_TorqueFeet;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(JumpAnimation());
        }
    }


    IEnumerator JumpAnimation()
    {
        float tHeadValue = m_TorqueHead.TorqueJointValue;
        float tBodyValue = m_TorqueBody.TorqueJointValue;
        float tFeetValue = m_TorqueFeet.TorqueJointValue;


        m_TorqueHead.TorqueJointValue = 0;
        
        m_TorqueBody.TorqueJointValue = 0;
        
        m_TorqueHead.RigidBodyValue.AddForce(Vector3.right * 900);
        
        m_TorqueHead.RigidBodyValue.AddTorque(Vector3.forward * 1600);

        m_TorqueBody.RigidBodyValue.AddTorque(Vector3.back * 1600);
        
        yield return  new WaitForSeconds(1.5f);

        m_TorqueFeet.TorqueJointValue = 0;
        
        yield return new WaitForSeconds(1.4f);
        
        m_TorqueHead.TorqueJointValue = tHeadValue;
        
        m_TorqueBody.TorqueJointValue = tBodyValue;
        
        yield return new WaitForSeconds(0.4f);
        
        m_TorqueFeet.TorqueJointValue = tFeetValue;


    }
}
