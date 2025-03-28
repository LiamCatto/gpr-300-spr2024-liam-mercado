using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class SkeletonManager : MonoBehaviour
{
    //Joint and Skeleton represent the hierarchy structure, but do not hold poses directly. 
    public struct Joint
    {
        public char m_name;    //Human-readable joint name
        public int m_iParent;  //Parent index (-1 if root)
    }
    public struct Skeleton
    {
        public List<Joint> m_aJoint;   //List of joints
        public SkeletonPose m_pose;
    }
    //Represents the local transformation of a single joint
    public struct JointPose
    {
        public Quaternion m_rotation;  //Euler angle
        public Vector3 m_translation;  //Translation
        public Vector3 m_scale;        //Scale
    }
    //Represents a pose for an entire skeleton
    public struct SkeletonPose
    {
        public List<JointPose> m_aLocalPose;   //List of joint poses
        public List<Matrix4x4> m_aGlobalPose;  //Global joint poses. This is what is calculated using FK!
    };

    private SkeletonPose defaultPose = new SkeletonPose()
    {
        // NOTE: add the rest of the data for a T pose
        m_aLocalPose = new List<JointPose>
        {
            new JointPose() { m_rotation = Quaternion.Euler(Vector3.zero), m_translation = new Vector3(1, 0, 0), m_scale = Vector3.one }
        },
        m_aGlobalPose = new List<Matrix4x4>
        {

        }
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
