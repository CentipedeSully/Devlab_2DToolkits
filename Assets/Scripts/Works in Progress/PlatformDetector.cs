using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDetector : MonoBehaviour
{
    ///This script signals events based on where and when a platform is detected in reference from the parent gameObject.
    ///Features:
    ///> Below, Above, Left and Right Platform Detection
    //Base utilities
    [Header("Debug Logs")]
    [SerializeField] private bool _enableDebugMessages = true;
    [Header("Platform Detection Utilities")]
    [SerializeField] private LayerMask _platformLayer;
    [Tooltip("The delay in seconds of the object still detecting the ground (after the object legally leaves the ground). Grants a more forgiving jump if the object tries to jump after it left the platform.")]
    [SerializeField] private float _platformLeniencyDuration = .15f;
    [Header("Above Detection")]
    [SerializeField] private float _aboveDetectionFieldWidth = 1.0f;
    [SerializeField] private float _aboveDetectionFieldHeight = 1.0f;
    [SerializeField] private float _aboveDetectDistance = 1.0f;
    [SerializeField] private Color _aboveGizmoColor = Color.red;
    [Header("Below Detection")]
    [SerializeField] private float _belowDetectionFieldWidth = 1.0f;
    [SerializeField] private float _belowDetectionFieldHeight = 1.0f;
    [SerializeField] private float _belowDetectDistance = 1.0f;
    [SerializeField] private Color _belowGizmoColor = Color.green;
    [Header("LefSide Detection")]
    [SerializeField] private float _leftSideDetectionFieldWidth = 1.0f;
    [SerializeField] private float _leftSideDetectionFieldHeight = 1.0f;
    [SerializeField] private float _leftSideDetectDistance = 1.0f;
    [SerializeField] private Color _leftSideGizmoColor = Color.yellow;
    [Header("RightSide Detection")]
    [SerializeField] private float _rightSideDetectionFieldWidth = 1.0f;
    [SerializeField] private float _rightSideDetectionFieldHeight = 1.0f;
    [SerializeField] private float _rightSideDetectDistance = 1.0f;
    [SerializeField] private Color _rightSideGizmoColor = Color.blue;

    //Collision Information
    private RaycastHit2D _aboveColliderScan;
    private RaycastHit2D _belowColliderScan;
    private RaycastHit2D _leftSideColliderScan;
    private RaycastHit2D _rightSideColliderScan;

    //Additional Utilities
    private IEnumerator _leniencyTimerReference;
    private StateMachine _stateMachineReference;
    private bool _stateMachineExists = false;
    [SerializeField]private bool _leniencyExpired = true;


    //==============================================================================================================================
    private void Awake()
    {
        TryToFindStateMachine();
    }
    private void Update()
    {
        DetectAllDirectionsForPlatform();
        TrySharingStatesWithStateMachine();
    }
    private void OnDrawGizmos()
    {
        DrawAllDirectionalDetections();
    }
    //==============================================================================================================================
    //Check for the State Machine
    private void TryToFindStateMachine()
    {
        _stateMachineReference = GetComponent<StateMachine>();
        if (_stateMachineReference != null)
            _stateMachineExists = true;
        else
            _stateMachineExists = false;
    }
    private void LogMissingStateMachine()
    {
        Debug.Log("StateMachine Component Missing on " +  gameObject + ".");
    }
    private void TrySharingStatesWithStateMachine()
    {
        if (_stateMachineExists)
            UpdateStateMachine();
        else if (_enableDebugMessages)
            LogMissingStateMachine();
    }

    //Detection Functions
    private void DetectAboveForPlatform()
    {
        _aboveColliderScan = Physics2D.BoxCast(GetComponent<Collider2D>().bounds.center, new Vector2(_aboveDetectionFieldWidth, _aboveDetectionFieldHeight), 0, Vector2.up, _aboveDetectDistance, _platformLayer);
    }
    private void DetectBelowForPlatform()
    {
        _belowColliderScan = Physics2D.BoxCast(GetComponent<Collider2D>().bounds.center, new Vector2(_belowDetectionFieldWidth, _belowDetectionFieldHeight), 0, Vector2.down, _belowDetectDistance, _platformLayer);
    }
    private void DetectLeftSideForPlatform()
    {
        _leftSideColliderScan = Physics2D.BoxCast(GetComponent<Collider2D>().bounds.center, new Vector2(_leftSideDetectionFieldWidth, _leftSideDetectionFieldHeight), 0, Vector2.left, _leftSideDetectDistance, _platformLayer);
    }
    private void DetectRightSideForPlatform()
    {
        _rightSideColliderScan = Physics2D.BoxCast(GetComponent<Collider2D>().bounds.center, new Vector2(_rightSideDetectionFieldWidth, _rightSideDetectionFieldHeight), 0, Vector2.right, _rightSideDetectDistance, _platformLayer);
    }
    private void DetectAllDirectionsForPlatform()
    {
        DetectAboveForPlatform();
        DetectBelowForPlatform();
        DetectLeftSideForPlatform();
        DetectRightSideForPlatform();
    }

    //Draw Functions
    private void DrawAboveDetections()
    {
        //Set the gizmo Color
        Gizmos.color = _aboveGizmoColor;

        if (_aboveColliderScan)
        {
            //Draw a Ray from the center of the collider upwards to the hit
            Gizmos.DrawRay(GetComponent<Collider2D>().bounds.center, Vector2.up * _aboveColliderScan.distance);

            //Draw the Detection Square at the maximum distance
            Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center + Vector3.up * _aboveColliderScan.distance, new Vector3(_aboveDetectionFieldWidth, _aboveDetectionFieldHeight));
        }
        else
        {
            //Draw a Ray from the center of the collider upwards by the upwards detection distance
            Gizmos.DrawRay(GetComponent<Collider2D>().bounds.center, Vector2.up * _aboveDetectDistance);

            //Draw the Detection Square at the maximum upwards distance
            Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center + Vector3.up * _aboveDetectDistance, new Vector3(_aboveDetectionFieldWidth, _aboveDetectionFieldHeight));
        }
    }
    private void DrawBelowDetections()
    {
        //Set the gizmo Color
        Gizmos.color = _belowGizmoColor;

        if (_belowColliderScan)
        {
            //Draw a Ray from the center of the collider downwards to the hit
            Gizmos.DrawRay(GetComponent<Collider2D>().bounds.center, Vector2.down * _belowColliderScan.distance);

            //Draw the Detection Square at the maximum downwards distance
            Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center + Vector3.down * _belowColliderScan.distance, new Vector3(_belowDetectionFieldWidth, _belowDetectionFieldHeight));
        }
        else
        {
            //Draw a Ray from the center of the collider downwards by the downwards detection distance
            Gizmos.DrawRay(GetComponent<Collider2D>().bounds.center, Vector2.down * _belowDetectDistance);

            //Draw the Detection Square at the maximum downwards distance
            Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center + Vector3.down * _belowDetectDistance, new Vector3(_belowDetectionFieldWidth, _belowDetectionFieldHeight));
        }
    }
    private void DrawLeftSideDetections()
    {
        //Set the gizmo Color
        Gizmos.color = _leftSideGizmoColor;

        if (_leftSideColliderScan)
        {
            //Draw a Ray from the center of the collider leftwards to the hit
            Gizmos.DrawRay(GetComponent<Collider2D>().bounds.center, Vector2.left * _leftSideColliderScan.distance);

            //Draw the Detection Square at the maximum leftwards detection distance
            Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center + Vector3.left * _leftSideColliderScan.distance, new Vector3(_leftSideDetectionFieldWidth, _leftSideDetectionFieldHeight));
        }
        else
        {
            //Draw a Ray from the center of the collider leftwards by the leftwards detection distance
            Gizmos.DrawRay(GetComponent<Collider2D>().bounds.center, Vector2.left * _leftSideDetectDistance);

            //Draw the Detection Square at the maximum leftwards detection distance
            Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center + Vector3.left * _leftSideDetectDistance, new Vector3(_leftSideDetectionFieldWidth, _leftSideDetectionFieldHeight));
        }
    }
    private void DrawRightSideDetections()
    {
        //Set the gizmo Color
        Gizmos.color = _rightSideGizmoColor;

        if (_rightSideColliderScan)
        {
            //Draw a Ray from the center of the collider rightwards to the hit
            Gizmos.DrawRay(GetComponent<Collider2D>().bounds.center, Vector2.right * _rightSideColliderScan.distance);

            //Draw the Detection Square at the maximum distance
            Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center + Vector3.right * _rightSideColliderScan.distance, new Vector3(_rightSideDetectionFieldWidth, _rightSideDetectionFieldHeight));
        }
        else
        {
            //Draw a Ray from the center of the collider rightwards by the rightwards detection distance
            Gizmos.DrawRay(GetComponent<Collider2D>().bounds.center, Vector2.right * _rightSideDetectDistance);

            //Draw the Detection Square at the maximum distance
            Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center + Vector3.right * _rightSideDetectDistance, new Vector3(_rightSideDetectionFieldWidth, _rightSideDetectionFieldHeight));
        }
    }
    private void DrawAllDirectionalDetections()
    {
        DrawAboveDetections();
        DrawBelowDetections();
        DrawLeftSideDetections();
        DrawRightSideDetections();
    }

    //Share Results with the StateMachine
    private void UpdateAboveDetectionState()
    {
        if (_aboveColliderScan)
            _stateMachineReference.SetPlatformAboveState(true);
        else _stateMachineReference.SetPlatformAboveState(false);

    }
    private void UpdateBelowDetectionState()
    {
        if (_belowColliderScan)
        {
            //If there's platform underneath and a counting timer is already active, then stop the timer and reset the leniency mechanic's values
            if (_leniencyTimerReference != null)
            {
                StopCoroutine(_leniencyTimerReference);
                _leniencyTimerReference = null;
                _leniencyExpired = false;
            }
            else if (_leniencyExpired == true)
                _leniencyExpired = false;
            //Update the state machine
            _stateMachineReference.SetPlatformBelowState(true);
        }
        else
        {
            //If there isn't a platform underneath and the leniency mechanic's expired, then just update the state machine
            if (_leniencyExpired)
                _stateMachineReference.SetPlatformBelowState(false);
            //else if there isn't a platform underneath and the leniency timer's inactive and the leniency mechanic isn't expired, then start the timer. Tell the state machine that there's still a platform underneath.
            else if (_leniencyTimerReference == null && _leniencyExpired == false)
            {
                _leniencyTimerReference = CountPlatformLeniencyDuration();
                StartCoroutine(_leniencyTimerReference);
            }
        }
    }
    private void UpdateLeftSideDetectionState()
    {
        if (_leftSideColliderScan)
            _stateMachineReference.SetPlatformOnLeftSideState(true);
        else _stateMachineReference.SetPlatformOnLeftSideState(false);

    }
    private void UpdateRightSideDetectionState()
    {
        if (_rightSideColliderScan)
            _stateMachineReference.SetPlatformOnRightSideState(true);
        else _stateMachineReference.SetPlatformOnRightSideState(false);

    }
    private void UpdateStateMachine()
    {
        UpdateAboveDetectionState();
        UpdateBelowDetectionState();
        UpdateLeftSideDetectionState();
        UpdateRightSideDetectionState();
    }

    private IEnumerator CountPlatformLeniencyDuration()
    {
        yield return new WaitForSeconds(_platformLeniencyDuration);
        _leniencyTimerReference = null;
        _leniencyExpired = true;

    }
    //==============================================================================================================================
}
