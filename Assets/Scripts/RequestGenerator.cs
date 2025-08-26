using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RequestGenerator : MonoBehaviour
{
    public TMP_Text descriptionText;

    private void Start()
    {
        ShipContract contract = GenerateContract();
        
        descriptionText.text = contract.description;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Changing description!");
            descriptionText.text = GenerateContract().description;
        }
    }

    private enum Demeanor
    {
        Friendly,
        Aggressive,
        Psychotic,
        Layman
    }
    
    private readonly string[] names =
    {
        "Among Us Crewmate", 
        "Chad Thundercock", 
        "Liam McPlease", 
        "Hut", 
        "Nick", 
        "Radius Gordello",
        "Carl Fredrickson",
        "Potatocouch",
        "thrat",
        "Doofenshmertz"
    };

    private readonly Dictionary<Demeanor, string> introductions = new Dictionary<Demeanor, string>()
    {
        { Demeanor.Aggressive, "Design a ship for me. I want it to have" },
        { Demeanor.Friendly, "Hello! I would like for you to design a ship for me. I would like it to have " },
        { Demeanor.Psychotic, "DESIGN A FUCKING SHIP FOR ME YOU SHIT-EATING MAGGOT! IT NEEDS " },
        { Demeanor.Layman, "me need ship. ship need make enemies go <i>bye-bye</i>." },
    };
    
    public ShipContract GenerateContract()
    {
        ShipContract contract = new ShipContract
        {
            name = names[Random.Range(0, names.Length)],
            description = GenerateDescription(name)
        };

        return contract;
    }

    private string GenerateDescription(string name)
    {
        Demeanor demeanor = Utilities.GetRandomEnumValue<Demeanor>();
        string description = introductions[demeanor];
        
        // Grug need not say anything more.
        if (demeanor == Demeanor.Layman)
        {
            return description;
        }

        description += " FTL capabilities, a huge fucking laser, and high-speed internet.";

        return description;
    }
}
