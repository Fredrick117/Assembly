using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RequestGenerator : MonoBehaviour
{
    public TMP_Text descriptionText;
    public TMP_Text customerInformationText;

    public ShipContract GenerateContract()
    {
        ShipContract contract = new ShipContract();
        
        contract.description = GenerateDescription();
        contract.customerAffiliation = Utilities.GetRandomEnumValue<Affiliation>();
        
        if (contract.customerAffiliation == Affiliation.LunaCorp)
            contract.customerName = lunacorpNames[Random.Range(0, lunacorpNames.Length)];
        else
            contract.customerName = names[Random.Range(0, names.Length)];

        return contract;
    }
    
    private enum Demeanor
    {
        Friendly,
        Aggressive,
        Psychotic,
        Layman
    }
    
    private void Start()
    {
        UpdateContractWindowText(GenerateContract());
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateContractWindowText(GenerateContract());
        }
    }

    private void UpdateContractWindowText(ShipContract contract)
    {
        descriptionText.text = contract.description;
        customerInformationText.text = 
            $"{contract.customerName}, {contract.customerAffiliation}";
    }
    
    private readonly string[] lunacorpNames =
    {
        "LC_Tyson",
        "LC_Oppenheimer",
        "LC_Einstein",
        "LC_Sagan",
        "LC_Aristotle",
        "LC_Aristotle",
        "LC_Copernicus",
        "LC_Galilei",
        "LC_Kepler",
        "LC_Bacon",
        "LC_Boyle",
        "LC_Newton",
        "LC_Franklin",
        "LC_Darwin",
        "LC_Mendeleev",
        "LC_Faraday",
        "LC_Turing",
        "LC_Hawking",
    };
    
    private readonly string[] names =
    {
        "Chad Thundercock", 
        "Liam", 
        "Hut", 
        "Nick", 
        "Radius Gordello",
        "Carl Fredrickson",
        "Potatocouch",
        "thrat",
        "Doofenshmertz",
        "BigBalls69",
        "SnoopDogg420",
        "BootyLover",
        "Daniel",
        "dmoney665",
        "yzzgra66666",
        "JackSparrow191",
        "xX_Huntr_K1llr_Xx",
        "xX_FatBalls111_Xx",
        "Niceguy63",
        "Ganondalf",
        "HobbitSlayer",
        "Master Chief",
        "Shepard",
        "SugarFreek",
    };

    private readonly Dictionary<Demeanor, string> introductions = new Dictionary<Demeanor, string>()
    {
        { Demeanor.Aggressive, "Design a ship for me. I want it to have" },
        { Demeanor.Friendly, "Hello! I would like for you to design a ship for me. I would like it to have " },
        { Demeanor.Psychotic, "DESIGN A FUCKING SHIP FOR ME!! IT NEEDS " },
        { Demeanor.Layman, "me need ship. ship need make enemies go <i>bye-bye</i>." },
    };

    private string GenerateDescription()
    {
        Demeanor demeanor = Utilities.GetRandomEnumValue<Demeanor>();
        string description = introductions[demeanor];
        
        // Grug need not say anything more.
        if (demeanor == Demeanor.Layman)
        {
            return description;
        }

        description += " FTL capabilities, a huge fucking laser, and high-speed internet.";

        if (demeanor == Demeanor.Psychotic)
            description = description.ToUpper();

        return description;
    }
}
