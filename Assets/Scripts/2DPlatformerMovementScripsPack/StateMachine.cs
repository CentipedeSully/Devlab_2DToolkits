using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    [Header("Platform Detector Utilities (Read Only)")]
    //Platform Detector Module Utilities
    [SerializeField]private bool _isPlatformAbove = false;
    [SerializeField] private bool _isPlatformBelow = false;
    [SerializeField] private bool _isPlatformOnLeftSide = false;
    [SerializeField] private bool _isPlatformOnRightSide = false;

    [Header("Player Jump Utilities (Read Only)")]
    //Player Jump Module Utilities
    //Input States
    [SerializeField] private bool _jumpCommand = false;
    //Jump Functionality States
    [SerializeField] private bool _jumpReady = true;
    [SerializeField] private bool _isJumping = false;
    //Momentum States
    [SerializeField] private bool _isAscending = false;
    [SerializeField] private bool _isDescending = false;





    //Platform Detector Setters & Getters
    public void SetPlatformAboveState(bool newState)
    {
        _isPlatformAbove = newState;
    }
    public void SetPlatformBelowState(bool newState)
    {
        _isPlatformBelow = newState;
    }
    public void SetPlatformOnLeftSideState(bool newState)
    {
        _isPlatformOnLeftSide = newState;
    }
    public void SetPlatformOnRightSideState(bool newState)
    {
        _isPlatformOnRightSide = newState;
    }


    public bool GetPlatformAboveState()
    {
        return _isPlatformAbove;
    }
    public bool GetPlatformBelowState()
    {
        return _isPlatformBelow;
    }
    public bool GetPlatformOnLeftSideState()
    {
        return _isPlatformOnLeftSide;
    }
    public bool GetPlatformOnRightSideState()
    {
        return _isPlatformOnRightSide;
    }


    //JumpModule Setters & Getters
    public void SetJumpInputState(bool jumpInput)
    {
        _jumpCommand = jumpInput;
    }
    public void SetJumpReadyState(bool jumpReadyState)
    {
        _jumpReady = jumpReadyState;
    }
    public void SetActiveJumpingState(bool jumpingState)
    {
        _isJumping = jumpingState;
    }
    public void SetAscendingState(bool newState)
    {
        _isAscending = newState;
    }
    public void SetDescendingState(bool newState)
    {
        _isDescending = newState;
    }

    public bool GetJumpInputState()
    {
        return _jumpCommand;
    }
    public bool GetJumpReadyState()
    {
        return _jumpReady;
    }
    public bool GetActiveJumpState()
    {
        return _isJumping;
    }
    public bool GetAscendingState()
    {
        return _isAscending;
    }
    public bool GetDescendingState()
    {
        return _isDescending;
    }


    #region Scrapped Work
    //Declarations FirstAttempt Stuff
    /*
    //State-Machine Base Utilities
    private bool _modulePlatformDetectorEnabled = false;
    private bool _modulePlayerJumpEnabled = false;
    private Color _originalSpriteColor;

    //Platform Detector Module Fields
    [Header("Platform Detector Module")]
    [Tooltip("This'll be detected automatically. If it doesn't exist then this aspect of the state machine won't be used.")]
    [SerializeField] private PlatformDetector platformDetectorReference;
    [SerializeField] private bool _showPlatformingStateViaSpriteColor = true;
    [SerializeField] private Color _spriteColorIfPlatformAbove = Color.red;
    [SerializeField] private Color _spriteColorIfPlatformBelow = Color.green;
    [SerializeField] private Color _spriteColorIfPlatformOnLeftSide = Color.yellow;
    [SerializeField] private Color _spriteColorIfPlatformOnRightSide = Color.blue;

    private bool _isPlatformAbove = false;
    private bool _isPlatformBelow = false;
    private bool _isPlatformOnLeftSide = false;
    private bool _isPlatformOnRightSide = false;
    private bool _platformDetectorModifiedSpriteColor = false;



    //PlayerJumpModule
    [Header("Player Jump Module")]
    [Tooltip("This'll be detected automatically. If it doesn't exist then this aspect of the state machine won't be used.")]
    [SerializeField] private YAxisPlayerJump _playerJumpReference;
    [SerializeField] private bool _showJumpingStateViaSpriteColor = true;
    private bool _isAscending = false;
    private bool _isDescending = false;
    private bool _jumpCommand = false;
    //private bool _isJumpAvailable = false;
    //_
    */

    //Custom Function FirstAttempt stuff
    /*
    //StateMachine-Wide Utilities
    private void SaveOriginalSpriteColor()
    {
        _originalSpriteColor = GetComponent<SpriteRenderer>().color;
    }
    private void ResetSpriteColor(ref bool colorModifiedFlag)
    {
        if (colorModifiedFlag == true)
            GetComponent<SpriteRenderer>().color = _originalSpriteColor;
        colorModifiedFlag = false;
    }
    private void LogMissingModule(string missingModuleName)
    {
        Debug.Log("Component '" + missingModuleName + "' not found. StateMachine Component in " + gameObject + " disabling '" + missingModuleName + "' utilities.");
    }
    private void LogMissingDependency(string dependentName, string missingDependencyName)
    {
        Debug.Log("Component '" + dependentName + "' relies on missing Module '" + missingDependencyName + "'. StateMachine Component in " + gameObject + " disabling '" + dependentName + "' utilities.");
    }

    //Platform Detector Module
    private void GetAndTryEnablingPlatformDetectorModule()
    {
        platformDetectorReference = GetComponent<PlatformDetector>();
        if (platformDetectorReference == null)
        {
            LogMissingModule("PlatformDetector");
            _modulePlatformDetectorEnabled = false;
        }
        else _modulePlatformDetectorEnabled = true;
    }
    private void RunPlatformDetectionModule()
    {
        GetPlatformDetectorStates();
        if (_showPlatformingStateViaSpriteColor)
            ShowCurrentStateViaColor();
        else if (_platformDetectorModifiedSpriteColor)
            ResetSpriteColor(ref _platformDetectorModifiedSpriteColor);
    }
    private void GetPlatformDetectorStates()
    {
        _isPlatformAbove = platformDetectorReference.GetAboveDetection();
        _isPlatformBelow = platformDetectorReference.GetBelowDetection();
        _isPlatformOnLeftSide = platformDetectorReference.GetLeftSideDetection();
        _isPlatformOnRightSide = platformDetectorReference.GetRightSideDetection();
    }
    private void ShowCurrentStateViaColor()
    {
        //Flag the sprite's color as modified by the platform detector.
        _platformDetectorModifiedSpriteColor = true;

        //Now Change the color based off of the current state
        if (_isPlatformAbove)
            GetComponent<SpriteRenderer>().color = _spriteColorIfPlatformAbove;
        else if (_isPlatformBelow)
            GetComponent<SpriteRenderer>().color = _spriteColorIfPlatformBelow;
        else if (_isPlatformOnLeftSide)
            GetComponent<SpriteRenderer>().color = _spriteColorIfPlatformOnLeftSide;
        else if (_isPlatformOnRightSide)
            GetComponent<SpriteRenderer>().color = _spriteColorIfPlatformOnRightSide;
        else GetComponent<SpriteRenderer>().color = _originalSpriteColor;
    }

    //Player Jump Module
    private void GetAndTryEnablingPlayerJumpModule()
    {
        _playerJumpReference = GetComponent<YAxisPlayerJump>();
        if (_playerJumpReference == null)
        {
            LogMissingModule("PlayerJump");
            _modulePlayerJumpEnabled = false;
        }
        else
        {
            //Quick Dependency Check. The Jump Module Depends on the Platform Detector Module.
            _modulePlayerJumpEnabled = IsDependencyEnabledForPlayerJump();
            

        }
    }
    private bool IsDependencyEnabledForPlayerJump()
    {
        if (_modulePlatformDetectorEnabled)
            return true;
        else
        {
            LogMissingDependency("PlayerJump", "PlatformDetector");
            return false;
        }
    }
    private void RunPlayerJumpModule()
    {
        GetPlayerJumpStates();
    }
    private void GetPlayerJumpStates()
    {
        //Get Timer-Based States


        //Get Input-Based States
        _jumpCommand = _playerJumpReference.GetInputBasedStates();

        //Get Momentum-Based States
        _playerJumpReference.GetMomentumBasedStates(ref _isAscending, ref _isDescending, in _isPlatformBelow);
    }
    //ShowCurrentJumpStateViaColor()
    */
    #endregion

    //====================================================================================================
}
