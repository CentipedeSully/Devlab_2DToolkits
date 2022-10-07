using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YAxisPlayerJump : MonoBehaviour
{
  
    //References
    private Rigidbody2D _playerRigidbodyReference;
    private StateMachine _stateMachineReference;
    private IEnumerator _jumpDurationCounterReference;
    private IEnumerator _jumpCooldownCounterReference;
    //Core Jump Fields
    [Header("Base Jump Utilties")]
    [SerializeField] private float _initialJumpMagnitude = 1000;
    [SerializeField] private float _jumpCooldownDuration = 1;
    [SerializeField] private float _jumpMaxDuration = 0.5f;
    [SerializeField] private float _jumpBoostMagnitude = 100;
    private bool _jumpCommand = false;

    [Header("Wall Jump Utilities")]
    [SerializeField] private bool _enableWallJump = true;
    [SerializeField] private float _wallJumpUpMagnitude = 200;
    [SerializeField] private float _wallJumpSideMagnitude = 200;

    [Header("Additional Tweaks")]
    //Indirect Utilities
    [SerializeField] private float _momentumBufferDetectionDeadZone = 0.001f;



    //===========================================================================================

    private void Awake()
    {
        //Collect References
        _playerRigidbodyReference = GetComponent<Rigidbody2D>();
        _stateMachineReference = GetComponent<StateMachine>();
    }

    private void Update()
    {
        UpdateInputState();
        UpdateMomentumStates();
    }

    private void FixedUpdate()
    {
        ExecuteJumpSequence();
    }


    //===========================================================================================


    //Get Input
    public void ReadInput()
    {
        _jumpCommand = false;

    }


    //StateMachine Communication Functions
    private void UpdateInputState()
    {
        _stateMachineReference.SetJumpInputState(_jumpCommand);
    }
    private void UpdateMomentumStates()
    {
        if (_stateMachineReference.GetPlatformBelowState() == true)
        {
            _stateMachineReference.SetAscendingState(false);
            _stateMachineReference.SetDescendingState(false);
        }
        else if (_playerRigidbodyReference.velocity.y > _momentumBufferDetectionDeadZone)
        {
            _stateMachineReference.SetAscendingState(true);
            _stateMachineReference.SetDescendingState(false);
        }
        else if (_playerRigidbodyReference.velocity.y < -1 * _momentumBufferDetectionDeadZone)
        {
            _stateMachineReference.SetAscendingState(false);
            _stateMachineReference.SetDescendingState(true);
        }
        else
        {
            _stateMachineReference.SetAscendingState(false);
            _stateMachineReference.SetDescendingState(false);
        }
    }


    //private void UpdateJumpStates()
    private void UpdateActiveJumpState(bool newJumpState)
    {
        _stateMachineReference.SetActiveJumpingState(newJumpState);
    }
    private void UpdateJumpAvailabilityState(bool newState)
    {
        _stateMachineReference.SetJumpReadyState(newState);
    }


    //Jumping Functions
    private void ExecuteJumpSequence()
    {
        //Cancel Jumping if On ceiling or Released JumpCommand (Automatically cancels if Jump Reaches Maturity)
        if (_stateMachineReference.GetActiveJumpState() == true && (_stateMachineReference.GetPlatformAboveState() == true || _stateMachineReference.GetJumpInputState() == false))
            EndCurrentJump();

        //Boost Jump if JumpCommand and isJumping (Jump Will cancel automatically when the JumpDuration timer expires)
        else if (_stateMachineReference.GetJumpInputState() == true && _stateMachineReference.GetActiveJumpState() == true)
            BoostJump();

        //Start  a Jump if JumpCommand and JumpReady and NotOnCeiling (Walljump if on wall, ground jump if on ground)
        else if (_stateMachineReference.GetJumpInputState() == true &&  _stateMachineReference.GetJumpReadyState() == true && _stateMachineReference.GetPlatformAboveState() == false)
        {
            //If on ground, Jump
            if (_stateMachineReference.GetPlatformBelowState() == true)
                JumpPlayer();
            //If against a wall, but not in between opposing walls
            else if (_stateMachineReference.GetPlatformOnLeftSideState() == true || _stateMachineReference.GetPlatformOnRightSideState() == true && !(_stateMachineReference.GetPlatformOnLeftSideState() == true && _stateMachineReference.GetPlatformOnRightSideState() == true) && _enableWallJump)
                WallJumpPlayer();
        }
            
    }
    private void EndCurrentJump()
    {
        //Update the State Machine to stop jumping. This'll disable jump Boosting
        UpdateActiveJumpState(false);

        //Stop the JumpDurationCounter early, and clear the reference
        StopCoroutine(_jumpDurationCounterReference);
        _jumpDurationCounterReference = null;
    }
    private void BoostJump()
    {
        _playerRigidbodyReference.AddForce(Vector2.up * _jumpBoostMagnitude * Time.deltaTime);
    }
    private void JumpPlayer()
    {
        //Apply The Jump Force
        _playerRigidbodyReference.AddForce(Vector2.up * _initialJumpMagnitude * Time.deltaTime, ForceMode2D.Impulse);
        //Update the States
        _stateMachineReference.SetActiveJumpingState(true);
        _stateMachineReference.SetJumpReadyState(false);
        //Start the timers
        StartJumpTimers();
    }
    private void WallJumpPlayer()
    {
        //Apply the Up Wall Jump Force
        _playerRigidbodyReference.AddForce(Vector2.up * _wallJumpUpMagnitude * Time.deltaTime, ForceMode2D.Impulse);

        //Apply the Sidewards Wall Jump Force
        if (_stateMachineReference.GetPlatformOnLeftSideState() == true)
            _playerRigidbodyReference.AddForce(Vector2.right * _wallJumpSideMagnitude * Time.deltaTime, ForceMode2D.Impulse);
        else if (_stateMachineReference.GetPlatformOnRightSideState() == true)
            _playerRigidbodyReference.AddForce(Vector2.left * _wallJumpSideMagnitude * Time.deltaTime, ForceMode2D.Impulse);

        //Update States. This method should only run when the player's jump is ready AND when the player's jump Boost is over, so there isn't any need to cancel any timers.
        _stateMachineReference.SetActiveJumpingState(true);
        _stateMachineReference.SetJumpReadyState(false);

        //Start the timers
        StartJumpTimers();
    }

    //Jump Function Timers
    private IEnumerator JumpDurationCounter()
    {
        //Wait until the jump matures
        yield return new WaitForSeconds(_jumpMaxDuration);

        //Update the State Machine to end the jumping state
        UpdateActiveJumpState(false);

        //Free the jumpDurationCounter's Reference
        _jumpDurationCounterReference = null;
    }
    private IEnumerator JumpCooldownCounter()
    {
        //Wait until the cooldown expires
        yield return new WaitForSeconds(_jumpCooldownDuration);

        //Update the state machine: it's ready for a new jump
        UpdateJumpAvailabilityState(true);

        //Free the JumpcooldownCounter's reference
        _jumpCooldownCounterReference = null;
    }
    private void StartJumpTimers()
    {
        //Start Jump Cooldown
        _jumpCooldownCounterReference = JumpCooldownCounter();
        StartCoroutine(_jumpCooldownCounterReference);

        //Start Counting the Jump
        _jumpDurationCounterReference = JumpDurationCounter();
        StartCoroutine(_jumpDurationCounterReference);
    }
    #region Scrapped Stuff
    //Scrapped Stuff
    /*
    public bool GetInputBasedStates()
    {
        return _jumpCommand;
    }
    public void GetMomentumBasedStates(ref bool ascendingState, ref bool descendingState, in bool platformBelowState)
    {
        if (platformBelowState == true)
        {
            ascendingState = false;
            descendingState = false;
        }
        else if (_playerRigidbodyReference.velocity.y > _momentumBufferDetectionDeadZone)
        {
            ascendingState = true;
            descendingState = false;
        }
        else if (_playerRigidbodyReference.velocity.y < -1*_momentumBufferDetectionDeadZone)
        {
            ascendingState = false;
            descendingState = true;
        }
        else
        {
            ascendingState = false;
            descendingState = false;
        }
    }
    void JumpPlayer()
    {
        _playerRigidbodyReference.AddForce(Vector2.up * _initialJumpMagnitude * Time.deltaTime, ForceMode2D.Impulse);
    }
    */
    #endregion

    //===========================================================================================

}
