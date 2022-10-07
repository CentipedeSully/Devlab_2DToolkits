using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XAxisPlayerMovement : MonoBehaviour
{
    ///This script moves the player left and right based on input from a controller script. It also features other edge case events for extra polish. 
    ///Features: 
    ///> Disabling Movement when against a wall (Needs Wall Detection)
    ///> OnStarting and OnStopping Events (w/adjustable velocity buffer)
    #region Declarations
    //Base Utilities
    [Header("Base Movement Utilities")]
    [SerializeField]private float _moveSpeed = 0;

    [Tooltip("Whether or not to flip the sprite based on the directional Input recieved.")]
    [SerializeField]private bool _toggleSpriteFlipping = true;

    private float _currentMoveDirection = 0;


    //Events
    public delegate void HorizontalMoveEvent();
    #endregion
    //======================================================================================================================================
    #region Monobehavior Methods
    private void FixedUpdate()
    {
        MovePlayer();
    }
    #endregion
    //======================================================================================================================================
    #region Custom Functions
    public void ReadInput()
    {
        //Get Input
        _currentMoveDirection = 0;
    }

    private void FlipSprite()
    {
        if (_currentMoveDirection != 0)
            transform.localScale= new Vector3(_currentMoveDirection, transform.localScale.y, transform.localScale.z);
    }

    private void ApplyMoveForce()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(_currentMoveDirection, 0) * _moveSpeed * Time.fixedDeltaTime);
    }

    private void MovePlayer()
    {
        if (_toggleSpriteFlipping == true)
            FlipSprite();
        ApplyMoveForce();
    }
    #endregion
    //======================================================================================================================================
}
