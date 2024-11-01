using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowing : MonoBehaviour
{
    public Transform m_FollowTarget;
    public float followingSpeed = 0.01f;

    private Vector3 startingOffset;
    private Vector3 cachedPosition;

    private void Start()
    {
        startingOffset = m_FollowTarget.position - transform.position;
        cachedPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_FollowTarget != null && Vector3.Magnitude(m_FollowTarget.position - transform.position - startingOffset) > 0.05f)
        {
            transform.position += Vector3.Normalize(m_FollowTarget.position - transform.position - startingOffset) * followingSpeed;
        }
    }
}
