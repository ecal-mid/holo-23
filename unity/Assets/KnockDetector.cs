using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Subsystems;
using UnityEngine;
using UnityEngine.XR;

public class KnockDetector : MonoBehaviour
{
        public XRNode handNode = XRNode.LeftHand;

        HandsAggregatorSubsystem handsSubsystem;

        protected void OnEnable()
        {
            Debug.Assert(handNode == XRNode.LeftHand || handNode == XRNode.RightHand, $"HandVisualizer has an invalid XRNode ({handNode})!");

            handsSubsystem = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();

            if (handsSubsystem == null)
            {
                StartCoroutine(EnableWhenSubsystemAvailable());
            }
        }


        private IEnumerator EnableWhenSubsystemAvailable()
        {
            yield return new WaitUntil(() => XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>() != null);
            OnEnable();
        }

        private void Update()
        {
            // Query all joints in the hand.
            if (handsSubsystem == null || !handsSubsystem.TryGetEntireHand(handNode, out IReadOnlyList<HandJointPose> joints))
            {
                return;
            }
            
            if (handsSubsystem.TryGetJoint(TrackedHandJoint.MiddleProximal, handNode, out var knockFinger) &&
                handsSubsystem.TryGetJoint(TrackedHandJoint.Wrist, handNode, out var wrist) )
            {
                var knockToWrist = knockFinger.Position - wrist.Position;
                Debug.DrawLine(knockFinger.Position,wrist.Position);
                var knockDir = knockToWrist.normalized;
                var knockPos = knockFinger.Position;
                var maxDist = 0.1f;
                if (Physics.Raycast(knockPos, knockDir, out var hit, maxDist))
                {
                    // raycast again with hit normal
                    if (Physics.Raycast(knockPos, -hit.normal, out var hit2))
                    {
                        
                    }
                    
                 //   buffer.Add(hit.distance);
                }
                else
                {
                 //   buffer.Clear();
                }
            }
        }

    }