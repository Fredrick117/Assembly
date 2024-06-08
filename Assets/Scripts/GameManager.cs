using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum WeaponType
{ 
    Kinetic,
    Energy,
    Rocket,
}

public class Weapon
{
    public Weapon(float damagePerSec, WeaponType weaponType, string name, string description)
    {
        this.damagePerSec = damagePerSec;
        this.weaponType = weaponType;
        this.name = name;
        this.description = description;
    }

    public float damagePerSec { get; set; }
    public WeaponType weaponType { get; set; }
    public string name { get; set; }
    public string description { get; set; }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ShipRequest CurrentShipRequest;

    delegate void ShipRequestDelegate(ShipRequest Request);
    ShipRequestDelegate CreateNewShipRequest;

    [SerializeField]
    private int numSubsystemTypes = 4;

    public GameObject RequestUI;

    public GameObject MoneyCounter;

    public ShipClass currentShipClass;

    public List<GameObject> corvetteSpawnPoints = new List<GameObject>();

    public List<GameObject> destroyerSpawnPoints = new List<GameObject>();

    public List<Weapon> allWeapons = new List<Weapon>();


    //
    [HideInInspector]
    public bool isDragging;

    public GameObject currentlyDraggedObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        CurrentShipRequest = CreateRandomRequest(numSubsystemTypes);

        // All weapons
        allWeapons.Add(new Weapon(10, WeaponType.Kinetic, "Machine Gun", "A basic and uninteresting weapon that fires small ballistic projectiles."));
        allWeapons.Add(new Weapon(100, WeaponType.Energy, "Laser", "A weapon that fires superheated beams of energy."));
        allWeapons.Add(new Weapon(1000, WeaponType.Rocket, "Armageddon-class Nuclear Warhead", "A small yet effective warhead mounted on a small missile that deals incredible damage."));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            currentlyDraggedObject.transform.position = mousePosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnSubmission += CheckAgainstRequest;
    }

    // Check to see if the ship in the assembly area matches the requirements provided
    void CheckAgainstRequest()
    {

    }

    ShipRequest CreateRandomRequest(int numTypes)
    {
        List<SubsystemType> list = new List<SubsystemType>();

        // Add a few random required subsystems
        for (int i = 0; i < numTypes; i++)
        {
            SubsystemType subsystem = GetRandomSubsystem();
            if (!list.Contains(subsystem))
            {
                list.Add(subsystem);
            }
        }

        ShipRequest request = new ShipRequest();
        request.shipClass = (ShipClass)Random.Range(0, (int)System.Enum.GetValues(typeof(ShipClass)).Cast<ShipClass>().Max());

        return request;
    }

    SubsystemType GetRandomSubsystem()
    {
        return (SubsystemType)Random.Range(0, (int)System.Enum.GetValues(typeof(SubsystemType)).Cast<SubsystemType>().Max());
    }

    void CleanUpObjects()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ModuleBase"))
        {
            GameObject.Destroy(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Reactor"))
        {
            GameObject.Destroy(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ShieldGen"))
        {
            GameObject.Destroy(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("LifeSup"))
        {
            GameObject.Destroy(go);
        }
    }

    public void SpawnShipSection(GameObject prefab)
    {
        if (prefab.GetComponent<ShipSection>().shipClass == ShipClass.Corvette)
        {
            foreach (GameObject spawnPoint in corvetteSpawnPoints)
            {
                SectionSpawnPoint section = spawnPoint.GetComponent<SectionSpawnPoint>();
                
                if (prefab.GetComponent<ShipSection>().shipSectionType == section.sectionType)
                {
                    if (spawnPoint.transform.childCount > 0)
                    {
                        GameObject.Destroy(spawnPoint.transform.GetChild(0).gameObject);
                    }

                    GameObject spawnedPrefab = GameObject.Instantiate(prefab, spawnPoint.transform.position, prefab.transform.rotation);
                    spawnedPrefab.transform.SetParent(spawnPoint.transform);
                }
            }
        }
        else if (prefab.GetComponent<ShipSection>().shipClass == ShipClass.Destroyer)
        {
            // TODO
        }
    }
}
