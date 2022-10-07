using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionManager : MonoBehaviour
{
    /// <Responsibility>
    /// This class listens to each spawn position, and it returns a valid spawn position upon request
    /// </Responsibility>
    
    //Declarations
    [Header("Spawn Position Manager Utilitites")]
    [Tooltip("Put SpawnPositions here. Name them too. They'll be managed at runtime")]
    [SerializeField] private List<SpawnPosition> _allPositions;
    [Tooltip("Read Only. Open positions are here. They'll move when they get blocked by something. Only visible for debugging.")]
    [SerializeField] private List<SpawnPosition> _availablePositions;
    [Tooltip("Read Only. Closed off positions are here. They'll move back to the available list when they get freed up. Only visible for debugging.")]
    [SerializeField] private List<SpawnPosition> _unavailablePositions;
    [SerializeField] private bool _showDebug = false;

    private int _positionCount;
    private bool _isStartupValidationComplete = false;


    //Monobehaviors
    private void OnEnable()
    {
        //Kep track of the total number of managed spawns
        RecordPositionCount();

        //Have each spawnPosition setup its data. 
        //Do this before subscribing, so the events wont fire before we're ready for em
        ValidateEachSpawnPosition();

        //Subscribe to each spawnPosition's update event
        InitializeSubscriptions();

        //Send each spawnPosition to its proper list and flag the completed load
        LoadSpawnPositionsintoTheirCollections();
        _isStartupValidationComplete = true;
    }

    private void OnDisable()
    {
        ResetLists(); //Requred to prevent memory leaks (& keep this classes exit/enter state clean)
        InitializeUnsubscriptions();
    }




    //Utilities
    /* Removed to Decouple the spawnPosition class from this class
    private void InitializeManagement()
    {
        foreach (SpawnPosition spawnPosition in _allPositions)
        {
            if (spawnPosition.GetSpawnPositionManager() == null)
                spawnPosition.SetSpawnPositionManager(this);
        }
    }
    */
    //Setup each spawnPosition's data
    private void ValidateEachSpawnPosition()
    {
        //Makes each spawnPosition in the collection check and update its own data.
        for (int i = 0; i < _positionCount; i++)
        {
            _allPositions[i].CheckSpawnPosition();
        }
    }

    private void LoadSpawnPositionsintoTheirCollections()
    {
        //Moves backwards from the end to the beginning of the Lists. It's a bit faster this way, I think.
        for (int i = _positionCount - 1; i >= 0; i--)
        {
            SendPositionToProperCollection(_allPositions[i]);
        }
    }

    //Subscribe to each SpawnPosition's OnSpawnChangedEvent
    private void InitializeSubscriptions()
    {
        foreach (SpawnPosition spawnPosition in _allPositions)
        {
            spawnPosition.OnSpawnAvailabilityChanged += SendPositionToProperCollection;
        }
    }

    //Unsubscribe to each SpawnPosition's OnSpawnChangedEvent
    private void InitializeUnsubscriptions()
    {
        foreach (SpawnPosition spawnPosition in _allPositions)
        {
            spawnPosition.OnSpawnAvailabilityChanged -= SendPositionToProperCollection;
        }
    }

    //Check & assign each position to the proper collection on startup
    private void SendPositionToProperCollection(SpawnPosition spawnPosition)
    {
        if (_isStartupValidationComplete)
        {
            if (spawnPosition.GetPositionAvailability() == true)
            {
                MovePostionToNewCollection(spawnPosition, _unavailablePositions, _availablePositions);
            }
            else
            {
                MovePostionToNewCollection(spawnPosition, _availablePositions, _unavailablePositions);
            }
        }
        else
        {
            if (spawnPosition.GetPositionAvailability() == true)
            {
                MovePostionToNewCollection(spawnPosition, _allPositions, _availablePositions);
            }
            else
            {
                MovePostionToNewCollection(spawnPosition, _allPositions, _unavailablePositions);
            }
        }
    }

    private void MovePostionToNewCollection(SpawnPosition spawnPosition, List<SpawnPosition> currentCollection, List<SpawnPosition> newCollection)
    {
        //Throw error if the current position isn't in the current collection. Weird.
        if ( !currentCollection.Contains(spawnPosition))
        {
            Debug.Log("ERROR in SpawnPositionManager script of " + gameObject.name + ": attempted spawnPosition move when spawnPositon isn't in original collection");
        }
        //Throw error if the current position is already in the target collection. 
        else if (newCollection.Contains(spawnPosition))
        {
            Debug.Log("ERROR in SpawnPositionManager script of " + gameObject.name + ": attempted spawnPosition move when spawnPositon is already in target collection ");
        }
        else 
        {
            currentCollection.Remove(spawnPosition);
            newCollection.Add(spawnPosition);
            if (_showDebug == true)
            {
                Debug.Log("SpawnPosition Move Successful");
            }
        }
    }

    private void RecordPositionCount()
    {
        _positionCount = _allPositions.Count;
    }

    //Cleanup for Disable
    private void ResetLists()
    {
        //Dump each list back into the _allList Category. Very necessary to make sure this class enters/exits in a safe state.
        //NOT DOING THIS WILL RESULT IN A MEMORY LEAK!!!! InitializeUnsubscriptions() Requires that the _allActions list be identical to when it was called in InitializeSubscriptions() 
        //I may get around to designing a less-volatile method later, if my time budget allows.
       while (_unavailablePositions.Count > 0)
       {
           MovePostionToNewCollection(_unavailablePositions[0], _unavailablePositions, _allPositions);
       }
       while (_availablePositions.Count > 0)
       {
           MovePostionToNewCollection(_availablePositions[0], _availablePositions, _allPositions);
       }

        //Also Reset the Validation flag
        _isStartupValidationComplete = false;
    }


    public SpawnPosition GetRandomAvailableSpawnPosition()
    {
        //Return null if no position is available
        if (_availablePositions.Count == 0)
            return null;
        //Return the only position if only one is available
        else if (_availablePositions.Count == 1)
        {
            return _availablePositions[0];
        }
        //Else be RANDOM BABY!
        else
        {
            return _availablePositions[Random.Range(0, _availablePositions.Count)];
        }
    }

    //Testing Utilities
    public void TestMovePosition()
    {
        //Note this test uses the first element in _allPositions and attempts a couple invalid moves followed by 4 valid ones.
        if (_showDebug == true && _allPositions.Count > 0)
        {
            SpawnPosition anomolySpawnPosition = new SpawnPosition();
            SpawnPosition testSpawnPosition = _allPositions[0];

            //Case 1: Expected outcome "Not in Original Collection"
            Debug.Log("MoveTest Case1: Expected outcome -> Error, spawnPosition isn't in original collecion");

            MovePostionToNewCollection(anomolySpawnPosition, _allPositions, _availablePositions); //error expected, anomoly doesn't exist in original collection

            //Case 2: Expected outcome "Already in target Collection"
            Debug.Log("MoveTest Case2: Expected outcome -> Error, spawnPosition is already in target collecion");

            MovePostionToNewCollection(testSpawnPosition, _allPositions, _allPositions); //error expected. collection already holds the current position

            //Case 3: Success, Move success 4 times, allPosition-> availaiblePositions -> unavialablePositions -> avialablePositions -> allPositons
            Debug.Log("MoveTest Case3: Expected outcome -> Move Successful (x4)");

            MovePostionToNewCollection(testSpawnPosition, _allPositions, _availablePositions);
            MovePostionToNewCollection(testSpawnPosition, _availablePositions, _unavailablePositions);
            MovePostionToNewCollection(testSpawnPosition, _unavailablePositions, _availablePositions);
            MovePostionToNewCollection(testSpawnPosition, _availablePositions, _allPositions);



        }
    }
}
