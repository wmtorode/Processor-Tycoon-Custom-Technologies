using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx.Logging;
using CustomTechnologies.data;
using CustomTechnologies.data.companies;
using Newtonsoft.Json;
using ProcessorTycoon.CompanySystem;
using ProcessorTycoon.PopupSystem;
using ProcessorTycoon.Save;
using ProcessorTycoon.TimeSystem;
using UnityEngine;

namespace CustomTechnologies.features;

public class CompaniesInjector
{
    public static CompaniesInjector Instance
    {
        get
        {
            if (_instance == null) _instance = new CompaniesInjector();
            if (!_instance.HasInitialized) _instance.Initialize();
            return _instance;
        }
    }
    
    private static CompaniesInjector _instance;
    private bool HasInitialized = false;
    private ManualLogSource Logger;

    private List<CustomCompany> CustomCompanies;
    
    private void Initialize()
    {
        CustomCompanies = new List<CustomCompany>();
        Logger = CustomTechnologiesPlugin.Logger;
        HasInitialized = true;
    }

    public void LoadCompanies(string directory)
    {
        Logger.LogInfo($"Loading Custom Companies from {directory}");
        
        if (!Directory.Exists(directory))
            return;

        foreach (var customCompFile in Directory.GetFiles(directory, "*.json"))
        {
            try
            {
                var company = JsonConvert.DeserializeObject<CustomCompany>(File.ReadAllText(customCompFile));  
                CustomCompanies.Add(company);
                Logger.LogInfo($"Loaded Custom Company {company.CompanyName} ({company.FullName})");
            } 
            catch (Exception ex)
            {
                Logger.LogError($"Failed to load Custom Company from {customCompFile}: {ex.Message}");
            }
        }
    }

    public void InjectCompanies(CompanySpawner companySpawner)
    {
        var currentDate = DateController.Instance.CurrentDate;
        foreach (var customCompany in CustomCompanies)
        {

            if (customCompany.hasSpawned)
            {
                continue;
            }

            if (customCompany.SpawnYear != currentDate.Year || customCompany.SpawnMonth != currentDate.Month)
            {
                continue;
            }
            
            bool exists = companySpawner.historicalCompanies.Exists(
                t => t.companyPrefab.Name == customCompany.CompanyName);
            if (exists)
            {
                Logger.LogWarning($"Custom Company {customCompany.CompanyName} is already injected");
                continue;
            }
            
            var baseCompany = companySpawner.historicalCompanies.FirstOrDefault(
                t => t.companyPrefab.Name == customCompany.BaseCompanyName);

            if (baseCompany == null)
            {
                Logger.LogWarning($"Custom Company {customCompany.CompanyName} Base Company {customCompany.BaseCompanyName} not found");
                continue;
            }
            
            var prefab = UnityEngine.Object.Instantiate<AICompany>(baseCompany.companyPrefab);
            prefab.Name = customCompany.CompanyName;
            prefab.FullName = customCompany.FullName;
            prefab.initialData.money = customCompany.InitialCash;
            prefab.initialData.factory.ProductionCapacity = customCompany.InitialFactoryCapacity;
            prefab.initialData.technologyYear = customCompany.StartingTechYear;

            var company = new CompanySpawner.HistoricalCompany();
            company.companyPrefab = prefab;
            if (customCompany.SpawnYear == customCompany.FoundingYear &&
                customCompany.SpawnMonth == customCompany.FoundingMonth)
            {
                company.realLifeDate = new Date(0, 0);
            }
            else
            {
                company.realLifeDate = new Date(customCompany.FoundingYear, customCompany.FoundingMonth);
            }
            
            company.spawnDate = new Date(customCompany.SpawnYear, customCompany.SpawnMonth);
            companySpawner.historicalCompanies.Add(company);
            
            Date date = company.realLifeDate;
            if (company.realLifeDate.Year <= 0 || company.realLifeDate.Month <= 0)
                date = company.spawnDate;
            prefab.FoundationDate = new Date(1971, 1)
            {
                Month = date.Month,
                Year = date.Year
            };
            
            prefab.transform.SetParent(companySpawner.aiCompanies.transform, false);
            prefab.SaveID = SaveIDHandler.Instance.NewID();
            prefab.Initialize(companySpawner.historicalCompanies.IndexOf(company));
            if (prefab.IsFoundry)
                PopupManager.Instance.InstantiateCompetitorNotification((ICompany) prefab, Popup.CompetitorNofication.OfferingFoundryServices);
            
            // have to fallback to reflection here as the Publicizer seems to expose multiple copies of the action, creating ambiguity
            var onCompanySpawnedField = typeof(CompanySpawner).GetField("OnCompanySpawned", 
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance 
                                                      | System.Reflection.BindingFlags.NonPublic);
            if (onCompanySpawnedField != null)
            {
                var onCompanySpawned = onCompanySpawnedField.GetValue(companySpawner) as Action;
                if (onCompanySpawned != null)
                    onCompanySpawned();
            }
            customCompany.hasSpawned = true;

            Logger.LogInfo($"Spawned Custom Company {customCompany.CompanyName} ({customCompany.FullName})");

        }
    }
}