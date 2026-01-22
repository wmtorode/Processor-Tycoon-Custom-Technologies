# Processor Tycoon Custom Technologies

This mod allows you to create and add custom technologies to Processor Tycoon. You can define new process nodes, packages, memory types, frequencies, wafer sizes, core counts, and cache upgrades using JSON files.

## Configuration (.cfg)

The mod generates a configuration file (usually found in `BepInEx/config/CustomTechnologies.cfg`) with the following parameters:

### [Tech]
* **Packages**: Semicolon-separated list of directories where custom package types are located. Default: `CustomTechnologies/Packages`.
* **ProcessNodes**: Semicolon-separated list of directories where custom process nodes are located. Default: `CustomTechnologies/ProcessNodes`.
* **Memory**: Semicolon-separated list of directories where custom memory types are located. Default: `CustomTechnologies/Memory`.
* **Frequency**: Semicolon-separated list of directories where custom frequency upgrades are located. Default: `CustomTechnologies/Frequency`.
* **Wafer**: Semicolon-separated list of directories where custom wafer sizes are located. Default: `CustomTechnologies/Wafer`.
* **Cores**: Semicolon-separated list of directories where custom core counts are located. Default: `CustomTechnologies/Cores`.
* **Cache**: Semicolon-separated list of directories where custom cache upgrades are located. Default: `CustomTechnologies/Cache`.
* **Patches**: Semicolon-separated list of directories where technology patch JSON files are located. Default: `CustomTechnologies/Patches`.

### [Research]
* **YearAiCanResearchProjects**: The year the AI starts being able to use research projects. Default: `2030`.
* **YearPlayerCanResearchProjects**: The year the player starts being able to use research projects. Default: `2030`.

---

## Research Object

Every custom technology JSON file must include a `Research` object. This object defines how the technology appears in the research tree.

### Example
```json
"Research" : {
  "BaseId": "1nm",
  "Year": 2035,
  "ResearchDays": 250,
  "MonthlyCost": 800000000,
  "TreeYOffset": -4,
  "DependencyIds": [
    "1nm"
  ]
}
```

### Fields
* **BaseId**: The ID of the technology this one is based on this will cause that technology's Icon and Branch (only for the PGA/LGA Split) to be used for your new tehcnology.
* **Year**: The year this technology becomes available for research.
* **ResearchDays**: Total number of days required to complete the research.
* **MonthlyCost**: The monthly cost in currency during the research period.
* **TreeYOffset**: Vertical offset in the research tree UI.
> [!NOTE]
> The horizontal offset is automatically calculated based on the year of the technology's availability.
* **DependencyIds**: A list of technology IDs that must be researched before this one becomes available.

---

## Process Node Technology

Defines a new manufacturing process node.

### Example
```json
{
  "Research" : {
    "BaseId": "1nm",
    "Year": 2035,
    "ResearchDays": 250,
    "MonthlyCost": 800000000,
    "TreeYOffset": -4,
    "DependencyIds": ["1nm"]
  },
  "Name": "0.7nm",
  "MinimumYieldRate": 5,
  "MaximumYieldRate": 95,
  "LearningRequirement": 50,
  "TransistorDensity": 155000000,
  "PowerConsumptionReduction": 15000,
  "CacheCostReduction": 18,
  "MulticorePenaltyReduction": 0.12,
  "ProjectCost": 800000000,
  "ProjectTime": 100,
  "FailureRateOffset": 0.085
}
```

### Fields
* **Name**: The display name and unique identifier for this process node.
* **MinimumYieldRate**: The baseline yield rate for new projects.
* **MaximumYieldRate**: The maximum achievable yield rate after learning.
* **LearningRequirement**: Affects how quickly yield improves, higher values mean slower improvement rates.
* **TransistorDensity**: Number of transistors per square millimeter.
* **PowerConsumptionReduction**: Impact on power efficiency.
* **CacheCostReduction**: Percentage reduction in cache manufacturing cost.
* **MulticorePenaltyReduction**: Reduction in the yield penalty for multicore designs.
* **ProjectCost**: Cost to develop a project using this node.
* **ProjectTime**: Time required to develop a project using this node.
* **FailureRateOffset**: Impact on the project failure rate.
> [!NOTE]
> This is only used by the AI that is not Industrial Focused.

---

## Package Technology

Defines a new CPU package type.

### Example
```json
{
  "Research" : {
    "BaseId": "DIP 1975",
    "Year": 1980,
    "ResearchDays": 105,
    "MonthlyCost": 3000000,
    "TreeYOffset": -3,
    "DependencyIds": ["DIP 1975"]
  },
  "Name": "DIP 1980",
  "BaseName": "DIP",
  "MinSize": 10,
  "MaxSize": 28,
  "MinPinCount": 18,
  "MaxPinCount": 64,
  "MinCacheKB": 4,
  "MaxCacheKB": 8,
  "ConsumptionMultiplier": 0.25,
  "SafeTemperature": 60.0,
  "ThermalEfficiency": 0.6,
  "BaseUnitCost": 14,
  "ProjectCost": 4000000,
  "ProjectTime": 35,
  "SupportsMultipleCores": false
}
```

### Fields
* **Name**: The display name and unique identifier for this package.
* **BaseName**: The internal category for the package (e.g., "DIP", "PGA", "LGA").
* **MinSize** / **MaxSize**: The supported physical size range for the die in square millimeters.
* **MinPinCount** / **MaxPinCount**: The supported range of pins/connections.
* **MinCacheKB** / **MaxCacheKB**: The supported range of integrated cache.
* **ConsumptionMultiplier**: Modifier for power consumption.
* **SafeTemperature**: Maximum safe operating temperature in Celsius.
* **ThermalEfficiency**: How well the package dissipates heat.
* **BaseUnitCost**: Baseline manufacturing cost per unit.
* **ProjectCost**: Cost to design a new CPU using this package.
* **ProjectTime**: Time required to design a new CPU using this package.
* **SupportsMultipleCores**: Boolean indicating if this package supports multicore CPUs.

---

## Memory Technology

Defines new RAM types (e.g., DDR5, DDR6).

### Example
```json
{
  "Research" : {
    "BaseId": "DDR6",
    "Year": 2034,
    "ResearchDays": 250,
    "MonthlyCost": 400000000,
    "TreeYOffset": 2,
    "DependencyIds": ["DDR6"]
  },
  "Name": "DDR7",
  "IpsThreshold": 96000000,
  "CacheIps": 4500000,
  "CacheIpcBoost": 3.75,
  "UnitCostPerCache": 1,
  "ProjectCost": 450000000,
  "ProjectTime": 55
}
```

### Fields
* **Name**: The display name and unique identifier.
* **IpsThreshold**: Base performance metric for this memory type.
* **CacheIps**: Performance contribution from memory cache.
* **CacheIpcBoost**: IPC (Instructions Per Cycle) boost provided.
* **UnitCostPerCache**: Additional cost per unit of cache.
* **ProjectCost**: Cost to design a new CPU using this memory.
* **ProjectTime**: Time required to design a new CPU using this memory.

---

## Cache Technology

Defines cache capacity upgrades.

### Example
```json
{
  "Research" : {
    "BaseId": "L2 1 MB",
    "Year": 2026,
    "ResearchDays": 150,
    "MonthlyCost": 600000000,
    "TreeYOffset": 3,
    "DependencyIds": ["L2 1 MB"]
  },
  "Name": "L2 1.5 MB",
  "L1Steps4K": 0,
  "L2Steps16K": 96,
  "L3Steps64K": 0
}
```

### Fields
* **Name**: The display name and unique identifier.
* **L1Steps4K**: Increments of 4KB for Level 1 cache.
* **L2Steps16K**: Increments of 16KB for Level 2 cache.
* **L3Steps64K**: Increments of 64KB for Level 3 cache.

---

## Wafer Technology

Defines new silicon wafer sizes.

### Example
```json
{
  "Research" : {
    "BaseId": "300 mm",
    "Year": 2029,
    "ResearchDays": 300,
    "MonthlyCost": 1000000000,
    "TreeYOffset": -5,
    "DependencyIds": ["300 mm"]
  },
  "Name": "450 mm Wafer",
  "NormalName": "450 mm",
  "WaferSize": 450,
  "ConstructionCostMultiplier": 1.1,
  "MaintainanceCostMultiplier": 1.2,
  "UpgradeCost": 75000000
}
```

### Fields
* **Name**: Unique identifier for the technology.
* **NormalName**: The display name for the wafer size.
* **WaferSize**: Physical diameter of the wafer in millimeters.
* **ConstructionCostMultiplier**: Multiplier for fab construction costs.
* **MaintainanceCostMultiplier**: Multiplier for fab maintenance costs.
* **UpgradeCost**: Monthly cost to upgrade a fab to this wafer size.

---

## Multicore Technology

Defines new CPU core count capabilities.

> [!WARNING]
> Core Counts must always be in increasing order. Having a core count of 16 and then a later core count of 12 will cause incorrect results.

### Example
```json
{
  "Research" : {
    "BaseId": "Octa Core",
    "Year": 2022,
    "ResearchDays": 150,
    "MonthlyCost": 200000000,
    "TreeYOffset": -1,
    "DependencyIds": ["Octa Core"]
  },
  "Name": "12 Core",
  "EnablesSmt": false,
  "CoreCount": 12
}
```

### Fields
* **Name**: Unique identifier and display name.
* **EnablesSmt**: Boolean indicating if this technology enables Simultaneous Multithreading.
> [!NOTE]
> The game doesn't seem to actually use this currently.
* **CoreCount**: The number of cores this technology enables.

---

## Frequency Technology

Defines CPU frequency milestones.

### Example
```json
{
  "Research" : {
    "BaseId": "5.00 GHz",
    "Year": 2023,
    "ResearchDays": 45,
    "MonthlyCost": 125000000,
    "TreeYOffset": 0,
    "DependencyIds": ["5.00 GHz"]
  },
  "Name": "5.20 GHz",
  "Frequency": 5200000
}
```

### Fields
* **Name**: Unique identifier and display name.
* **Frequency**: The frequency value in kHz.

---

## Technology Patches

Technology patches allow you to modify the research tree properties of existing technologies (both built-in and custom).

### Example
```json
{
  "TechId": "DIP 1975",
  "Year": 1974,
  "TreeYOffset": -2,
  "DependencyIds": ["SomeOtherTech"]
}
```

### Fields
* **TechId** (Required): The unique ID of the technology you wish to patch.
* **Year** (Optional): Overrides the availability year. If omitted, the original value is kept.
* **TreeYOffset** (Optional): Overrides the vertical offset in the research tree UI. If omitted, the original value is kept.
* **DependencyIds** (Optional): Overrides the list of required technologies. If omitted, the original dependencies are kept.
