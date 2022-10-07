using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{

    /// <Responsibility>
    /// This class defines the size of the spawn position and decides whether or not this spawn position is available to use.
    /// The spawnPosition uses the gameobject's transform.position as the spawn's origin
    /// </Responsibility>


    //Declarations
    [Header("Spawn Position Utilities")]
    [Min(.5f)][SerializeField] private float _spawnPositionWidth = 1;
    [Min(.5f)][SerializeField] private float _spawnPositionHeight = 1;
    [SerializeField] private LayerMask _layerMask;
    [Tooltip("Viewable only for Debugging. Don't touch")]
    [SerializeField] private bool _isPositionAvailable = false;
    [SerializeField] private Color _gizmoColor = Color.blue;

    //Readablility Variables
    private Vector2 _boxCastPosition;
    private Vector2 _spawnDimensions;

    //events
    public delegate void SpawnPositionEvent(SpawnPosition positionReference);
    public event SpawnPositionEvent OnSpawnAvailabilityChanged;

    //references
    //[SerializeField] private SpawnPositionManager _spawnPositionManager;


    //Monobehaviors
    private void Awake()
    {
        InitializeReadabilityVariables();
    }

    private void Update()
    {
        //Continuously checks the position for any colliders.
        CheckSpawnPosition();
    }

    private void OnDrawGizmos()
    {
        DrawOverlapBoxForDebugging();
    }

    //Utilities
    //getters & setters
    public bool GetPositionAvailability()
    {
        return _isPositionAvailable;
    }

    public float GetSpawnWidth()
    {
        return _spawnPositionWidth;
    }
    
    public float GetSpawnHeight()
    {
        return _spawnPositionHeight;
    }

    public void SetPositionAvailability(bool newValue)
    {
        //if there's a difference, then something changed. Raise a signal
        if (newValue != _isPositionAvailable)
        {
            //Be sure to update the info first!
            _isPositionAvailable = newValue;
            OnSpawnAvailabilityChanged?.Invoke(this);
        }
        else
            _isPositionAvailable = newValue;

    }

    /* Removed to decouple this class from the spawnPositionManager
    public void SetSpawnPositionManager(SpawnPositionManager spawnPositionManager)
    {
        _spawnPositionManager = spawnPositionManager;
    }

    public SpawnPositionManager GetSpawnPositionManager()
    {
        return _spawnPositionManager;
    }
    */

    //Check Position for a change
    public void CheckSpawnPosition()
    {
        //Make a quick box check over this gameObject's x,z by the spawn position's dimensions, if the returned collider is null, the position is available
        if (Physics2D.OverlapBox(_boxCastPosition, _spawnDimensions, 0, _layerMask) == null)
        {
            SetPositionAvailability(true);
        }
        else
        {
            SetPositionAvailability(false);
        }
    }

    private void InitializeReadabilityVariables()
    {
        //Initialize the Readability Variables. These only exist to make the CheckSpawnPositon Utility more readable ^_^
        _boxCastPosition = new Vector2(transform.position.x, transform.position.y);
        _spawnDimensions = new Vector2(_spawnPositionWidth, _spawnPositionHeight);
    }

    //Debugging Utilities

    private void DrawOverlapBoxForDebugging()
    {
        float _debugBoxLeftOffset = transform.position.x - (_spawnPositionWidth / 2);
        float _debugBoxRightOffset = transform.position.x + (_spawnPositionWidth / 2);
        float _debugBoxBottomOffset = transform.position.y - (_spawnPositionHeight / 2);
        float _debugBoxTopOffset = transform.position.y + (_spawnPositionHeight / 2);
        Gizmos.color = _gizmoColor;

        //Bottom Line
        Gizmos.DrawLine(new Vector3(_debugBoxLeftOffset, _debugBoxBottomOffset, transform.position.z), new Vector3(_debugBoxRightOffset, _debugBoxBottomOffset, transform.position.z));

        //Top Line
        Gizmos.DrawLine(new Vector3(_debugBoxLeftOffset, _debugBoxTopOffset, transform.position.z), new Vector3(_debugBoxRightOffset, _debugBoxTopOffset, transform.position.z));

        //Left Line
        Gizmos.DrawLine(new Vector3(_debugBoxLeftOffset, _debugBoxBottomOffset, transform.position.z), new Vector3(_debugBoxLeftOffset, _debugBoxTopOffset, transform.position.z));

        //Right Line
        Gizmos.DrawLine(new Vector3(_debugBoxRightOffset, _debugBoxBottomOffset, transform.position.z), new Vector3(_debugBoxRightOffset, _debugBoxTopOffset, transform.position.z));

    }
}
