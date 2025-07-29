using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Ship
{
    public ShipClassification classification;
    public int mass;
    public int weaponSlots;
    public int utilitySlots;
    public int reactorSlots;
    public int armorRating;
    public int speed;

    public List<Subsystem> subsystems = new();
}

public class ShipGenerator : MonoBehaviour
{
    Ship ship = new();

    private List<ShipBaseStats> shipStatsList = new();

    private List<ArtificialIntelligence> aiSubsystems = new();
    private List<Armor> armorSubsystems = new();
    private List<LifeSupport> lifeSupportSubsystems = new();
    private List<Reactor> reactorSubsystems = new();
    private List<Shielding> shieldingSubsystems = new();
    private List<FTLDrive> ftlSubsystems = new();
    private List<Thrusters> thrusterSubsystems = new();

    private ShipClassification shipClass;

    private void Start()
    {
        shipStatsList = Resources.LoadAll<ShipBaseStats>("ScriptableObjects/Ships").ToList();
        aiSubsystems = Resources.LoadAll<ArtificialIntelligence>("ScriptableObjects/Subsystems").ToList();
        armorSubsystems = Resources.LoadAll<Armor>("ScriptableObjects/Subsystems").ToList();
        lifeSupportSubsystems = Resources.LoadAll<LifeSupport>("ScriptableObjects/Subsystems").ToList();
        reactorSubsystems = Resources.LoadAll<Reactor>("ScriptableObjects/Subsystems").ToList();
        shieldingSubsystems = Resources.LoadAll<Shielding>("ScriptableObjects/Subsystems").ToList();
        ftlSubsystems = Resources.LoadAll<FTLDrive>("ScriptableObjects/Subsystems").ToList();
        thrusterSubsystems = Resources.LoadAll<Thrusters>("ScriptableObjects/Subsystems").ToList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ship ship = GenerateShipDesign();
            print("Ship design generated!");
            print($"Classification: {ship.classification}, utility slots: {ship.utilitySlots}, weapon slots: {ship.weaponSlots}, reactor slots: {ship.reactorSlots}, mass: {ship.mass}");
        }
    }

    private void AddCoreSystems(Ship ship)
    {
        ship.classification = Utilities.GetRandomEnumValue<ShipClassification>();

        ShipBaseStats baseStats = shipStatsList.FirstOrDefault(stats => stats.shipClass == ship.classification);
        ship.utilitySlots = baseStats.utilitySlots;
        ship.weaponSlots = baseStats.weaponSlots;
        ship.reactorSlots = baseStats.reactorSlots;
        ship.mass = baseStats.baseMass;
    }

    private void AddThrusters(Ship ship)
    {
        if (ship.speed > 0)
        {
            foreach (Thrusters thruster in thrusterSubsystems)
            {

            }
        }
    }

    private Ship GenerateShipDesign()
    {
        Ship ship = new();
        AddCoreSystems(ship);



        return ship;
    }
}
