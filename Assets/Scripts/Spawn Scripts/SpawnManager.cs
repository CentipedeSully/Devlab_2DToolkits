using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    /// <responsibility>
    /// This class toggles spawning.
    /// </responsibility>

    //Declarations
    [Header("Spawn Manager Utilities")]
    [Tooltip("In Seconds")]
    [Min(.1f)][SerializeField] private float _spawnDelay = 1;
    [Tooltip("Read Only. Determines whether or not the class is actively spawning.")]
    [SerializeField] private bool _isSpawning = false;
    [SerializeField] private bool _showDebug = false;

    //references
    [Tooltip("All (random) spawnable objects go here. It's imperitive that the sum of each item's spawnChance equals 1, otherwise this script will complain. Also limit chances to the nearest hundreth.")]
    [SerializeField] private List<SpawnableObject> _spawnableObjectCollection;
    [SerializeField] private SpawnPositionManager _spawnPositionManager;
    private IEnumerator _spawnCooldownReference = null;
    private bool _isSpawnReady = true;
    private int[] _chanceArray;
    private bool _errorDetected = false;

    //events
    public delegate void SpawnManagerEvent();
    public event SpawnManagerEvent OnCooldownEntered;
    public event SpawnManagerEvent OnCooldownExited;
    public event SpawnManagerEvent OnSpawnTriggered;




    //Monobehaviors
    private void Awake()
    {
        //InitializeSpawnableCollection();
        ValidateSpawnChanceTotal();
        InitializeChanceArray();
    }

    private void Start()
    {
        StartSpawning();
    }

    private void OnEnable()
    {
        OnCooldownEntered += StartCooldown;
        OnCooldownExited += ExitCooldown;
        OnSpawnTriggered += SpawnThing;
    }

    private void OnDisable()
    {
        OnCooldownEntered -= StartCooldown;
        OnCooldownExited -= ExitCooldown;
        OnSpawnTriggered -= SpawnThing;
    }



    //Utilities
    //Getters and Setters
    public bool IsSpawnReady()
    {
        return _isSpawnReady;
    }

    public bool IsSpawning()
    {
        return _isSpawning;
    }

    //Control Utilities
    public void StartSpawning()
    {
        if (!_isSpawning && !_errorDetected)
        {
            //LogStart
            if (_showDebug)
                Debug.Log("SpawnManager Activated");

            //Set spawning flag
            _isSpawning = true;

            //Execute SpawnLoop
            OnSpawnTriggered?.Invoke();
        }
        else
        {
            Debug.Log("ERROR in SpawnManager Script: StartSpawning called while SpawnManager is already active");
        }

    }

    public void StopSpawning()
    {
        if (_isSpawning)
        {
            //LogHalt
            if (_showDebug)
                Debug.Log("SpawnManager Deactivated");

            //Set spawning flag
            _isSpawning = false;

            //Halt & Reset Spawning utilities
            CleanupCooldown();
        }
        else
        {
            Debug.Log("ERROR in SpawnManager Script: StopSpawning called while SpawnManager isn't active");
        }

    }



    //Cooldown-Related
    //Sets up the Cooldown utilities and starts the cooldown timer
    private void StartCooldown()
    {
        if (_spawnCooldownReference == null)
        {
            //Set the state
            _isSpawnReady = false;

            //Get the reference handle
            _spawnCooldownReference = CooldownTimer();

            //Start the Cooldown Timer
            StartCoroutine(_spawnCooldownReference);
        }
        else Debug.Log("ERROR in SpawnManager script: EnterCooldown Called when script already under Cooldown");
    }

    //Counts down by the spawnDelay (seconds) and then signals the CooldownExited 
    private IEnumerator CooldownTimer()
    {
        //Wait for the cooldown time
        yield return new WaitForSeconds(_spawnDelay);

        //Communicate Exit
        OnCooldownExited?.Invoke();

    }

    //Exits and Determines whether or not to continue spawning after the Cooldown is exited
    private void ExitCooldown()
    {
        //Clean up the Cooldown utilities
        CleanupCooldown();

        //If the spawnManager is still active, then trigger the spawn signal
        if (_isSpawning)
        {
            OnSpawnTriggered?.Invoke();
        }
        else
        {
            if (_showDebug)
                Debug.Log("SpawnManager Script Discontinuing Spawn Cycle");
        }
    }

    //Resets the Cooldown Utilities
    private void CleanupCooldown()
    {
        if (_spawnCooldownReference != null)
        {
            //Interrupt the cooldown(if necessary)
            StopCoroutine(_spawnCooldownReference);

            //Clear the reference handle
            _spawnCooldownReference = null;

            //Set the state
            _isSpawnReady = true;
        }
        else
        {
            Debug.Log("ERROR in SpawnManager Script: CleanupCooldown utility called when no cooldown is active");
        }
    }



    //Spawn Related
    private void SpawnThing()
    {
        //Pick a thing to spawn
        SpawnableObject selection = SelectRandomSpawnable();

        //Spawn thing at Random Spawnpoint
        selection.Spawn(_spawnPositionManager.GetRandomAvailableSpawnPosition());

        //Signal that the Cooldown is entered
        OnCooldownEntered?.Invoke();
    }

    private SpawnableObject SelectRandomSpawnable()
    {
        if (_spawnableObjectCollection.Count < 1)
            return null;
        else if (_spawnableObjectCollection.Count == 1)
        {
            return _spawnableObjectCollection[0];
        }
        else
        {
            //Pick from the chance array at random. It holds values that're meant to be used as indexes. Use the randomly selected index to index one of the spawnable objects
            return _spawnableObjectCollection[_chanceArray[Random.Range(0, 100)]];
        }
    }

    //Initializes the Chance array
    private void InitializeChanceArray()
    {
        if (!_errorDetected)
        {
            //initialize the array itself
            _chanceArray = new int[100];

            //keep track of the chanceArrays filled indexes 
            int chanceArrayBookmark = 0;

            //populate the chance array, using each item's spawnChance as the item's representational frequency(number of appearances) in the chance array
            for (int i = 0; i < _spawnableObjectCollection.Count; i++)
            {
                //Reset the occurance counter
                int currentItemOccuranceCount = 0;

                //truncate currrent value
                int editedSpawnChance = Mathf.FloorToInt(_spawnableObjectCollection[i].GetSpawnChance() * 100);
                //Debug.Log(editedSpawnChance);

                //populate the chance array by the same index as many times as it needs to occur (out of 100)
                while (currentItemOccuranceCount < editedSpawnChance)
                {
                    PopulateChanceArray(i, chanceArrayBookmark);

                    //move the bookmark forward to keep track of the filled indexes
                    chanceArrayBookmark++;

                    //count the number of times this current spawnableObject has entered this array
                    currentItemOccuranceCount++;
                }
            }

        }
    }

    //Fills the chance array with reoccuring indexes that trace back to each item in the spawnableCollection
    private void PopulateChanceArray(int itemIndexInCollection, int chanceIndex)
    {
        //The item's index in the Spawnable Collection is saved here and used to trace back to the item's position in the SpawnableCollection.
        //It's implied that there'll be duplicate values, which serve as the item's spawn likelihood. This array will be indexed at random.
        _chanceArray[chanceIndex] = itemIndexInCollection;
    }

    private void ValidateSpawnChanceTotal()
    {
        float totalSpawnChance = 0;
        if (_spawnableObjectCollection.Count > 0)
        {
            foreach (SpawnableObject item in _spawnableObjectCollection)
            {
                //Truncates anything after the hundreths place.
                //Debug.Log(item.GetSpawnChance() * 100);
                totalSpawnChance += Mathf.Floor(item.GetSpawnChance() * 100);
                //Debug.Log(totalSpawnChance);
            }

            if (totalSpawnChance != 100)
            {
                Debug.Log("Oi. SpawnChance total of all spawnable items don't add up to 1. SpawnManager upset >:(");
                Debug.Log("Keep SpawnChange values to the nearest hundredth (0.xx), thanks ;)");
                _errorDetected = true;
            }
        }
        else Debug.Log("ERROR in SpawnManager: No Spawnable prefabs supplied");
    }



    //Debugging Utilities
    private void LogSpawnPlaceholder()
    { 
        Debug.Log("Spawn Signal Recieved! Triggering Cooldown");
        OnCooldownEntered?.Invoke();
    }






}
