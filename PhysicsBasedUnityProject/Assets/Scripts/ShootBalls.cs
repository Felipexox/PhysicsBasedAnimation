using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ShootBalls : MonoBehaviour
{
    [SerializeField] private GameObject m_Prefab;

    [SerializeField] private float m_Force;

    [SerializeField] private KeyCode m_KeyToCreate;
    
    private void Update()
    {
        if (Input.GetKeyDown(m_KeyToCreate))
        {
            Ray tRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            GameObject tObjectInstantiated = Instantiate(m_Prefab, tRay.origin, m_Prefab.transform.rotation);
            
            tObjectInstantiated.GetComponent<Rigidbody>().AddForce(tRay.direction * m_Force);
        }
    }
}
