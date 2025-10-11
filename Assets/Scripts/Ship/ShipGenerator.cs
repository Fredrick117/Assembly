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
    public int speed;
    public int utilitySlots;
    public int reactorSlots;
    public int armorRating;

    public bool isAutonomous;
    public bool isFTL;
    public bool isAtmospheric;

    public List<Subsystem> subsystems = new();
}

// Ship generation:
// 1) Select a random classification (corvette, destroyer, carrier)
// 2) Select a random set of thrusters (also decides atmospheric entry capabilities) (future: choose thrusters based on mass)
// 3) Shielding subsystem (if ship is supposed to be energy shielded)
// 4) FTL drive (if enough remaining slots)
// 5) AI (if enough slots)
// 6) Pick the reactor with the minimum power necessary to make the above work

public class ShipGenerator : MonoBehaviour
{
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

        reactorSubsystems.OrderBy(reactor => reactor.powerOutput);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ship ship = GenerateShip();
            print("Ship design generated!");
            print($"Classification: {ship.classification}, utility slots: {ship.utilitySlots}, weapon slots: {ship.weaponSlots}, reactor slots: {ship.reactorSlots}, mass: {ship.mass}");
            foreach (Subsystem s in ship.subsystems)
            {
                print($"Subsystem: {s.displayName}");
            }
        }
    }

    private void SetBaseStats(Ship ship)
    {
        ship.classification = Utilities.GetRandomEnumValue<ShipClassification>();

        ShipBaseStats baseStats = shipStatsList.First(stats => stats.shipClass == ship.classification);

        if (baseStats == null)
        {
            Debug.LogError($"Could not find base stats for {ship.classification.ToString()}");
        }

        ship.utilitySlots = baseStats.utilitySlots;
        ship.weaponSlots = baseStats.weaponSlots;
        ship.reactorSlots = baseStats.reactorSlots;
        ship.mass = baseStats.baseMass;
    }

    private void AddThrusters(Ship ship)
    {
        if (ship.subsystems.Count < ship.utilitySlots - 1)
        {
            Thrusters selectedThruster = thrusterSubsystems[Random.Range(0, thrusterSubsystems.Count)];
            ship.subsystems.Add(selectedThruster);
            ship.speed += selectedThruster.speed;
        }
    }

    private void AddShielding(Ship ship)
    {
        if (ship.subsystems.Count < ship.utilitySlots - 1)
        {
            ship.subsystems.Add(shieldingSubsystems[Random.Range(0, shieldingSubsystems.Count)]);
        }
    }

    private void AddFTL(Ship ship)
    {
        bool ftl = Random.Range(0, 7) > 2;
        if (!ftl)
        {
            ship.isFTL = false;
            return;
        }

        if (ship.subsystems.Count < ship.utilitySlots - 1)
        {
            FTLDrive selectedFTL = ftlSubsystems[Random.Range(0, ftlSubsystems.Count)];
            ship.subsystems.Add(selectedFTL);
            ship.isFTL = true;
        }
    }

    private void AddAI(Ship ship)
    {
        bool ai = Random.Range(0, 7) > 5;

        if (!ai)
        {
            ship.isAutonomous = false;
            return;
        }

        if (ship.subsystems.Count < ship.utilitySlots - 1)
        {
            ArtificialIntelligence selectedAI = aiSubsystems[Random.Range(0, aiSubsystems.Count)];
            ship.subsystems.Add(selectedAI);
            ship.isAutonomous = true;
        }
    }

    private void AddReactor(Ship ship)
    {
        foreach (Reactor reactor in reactorSubsystems)  // what order?
        {
            if (reactor.powerOutput > GetTotalPowerConsumption(ship))
            {
                ship.subsystems.Add(reactor);
                return;
            }
        }

        Debug.LogWarning("ShipGenerator: Could not find suitable reactor for this design!");
    }

    private void AddArmor(Ship ship)
    {
        Armor armor = armorSubsystems[Random.Range(0, armorSubsystems.Count())];
        ship.subsystems.Add(armor);
        ship.armorRating = armor.armorRating;
    }

    private int GetTotalPowerConsumption(Ship ship)
    {
        int power = 0;
        foreach (Subsystem subsystem in ship.subsystems)
        {
            power += subsystem.powerDraw;
        }

        return power;
    }

    public Ship GenerateShip()
    {
        Ship ship = new();
        SetBaseStats(ship);
        AddThrusters(ship);
        AddShielding(ship);
        AddFTL(ship);
        AddAI(ship);
        AddArmor(ship);
        AddReactor(ship);

        return ship;
    }
}
