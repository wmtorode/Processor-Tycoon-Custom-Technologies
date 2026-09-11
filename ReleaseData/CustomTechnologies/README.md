# Processor Tycoon Custom Technologies

> [!WARNING]
> Adding or removing custom technologies requires a new game. Do not add or remove technologies while using an existing save game, as this will likely cause issues or crashes.

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

### [Companies]
* **Companies**: Semicolon-separated list of directories where custom companies are located. Default: `CustomTechnologies/Companies`.

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
  "Branch": 0,
  "DependencyIds": [
    "1nm"
  ]
}
```

### Fields
* **BaseId**: The ID of the technology this one is based on. The base technology supplies the icon and, when `Branch` is omitted, the research-tree branch.
* **Year**: The year this technology becomes available for research.
* **ResearchDays**: Total number of days required to complete the research.
* **MonthlyCost**: The monthly cost in currency during the research period.
* **TreeYOffset**: Vertical offset in the research tree UI.
> [!NOTE]
> The horizontal offset is automatically calculated based on the year of the technology's availability.
* **Branch**: Optional research-tree branch override. Use the following values:

| Value | Use |
| --- | --- |
| `0` | Every non-package technology, and packages before `PLCC 1980`. |
| `1` | Package upgrades on the `EMD`/`PGA 2000` branch after `PGA 1995`. |
| `2` | Mobile-segment package upgrades after `PGA 1995`. |
| `3` | Industrial-segment package upgrades after `PLCC 1980`. |

> [!IMPORTANT]
> Set `Branch` to `0` for all non-package research. For package research, use `0` before `PLCC 1980`, then select the branch matching the package line from the table. If `Branch` is omitted, the cloned `BaseId` technology's branch is retained instead.
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
    "Branch": 0,
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
    "Branch": 0,
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
    "Branch": 0,
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
    "Branch": 0,
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
    "Branch": 0,
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
    "Branch": 0,
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
    "Branch": 0,
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

---

## Custom Companies

> [!CAUTION]
> Custom Companies is a work in progress and is a considered a pre-release feature. Use with caution.

This feature allows you to spawn custom AI-controlled companies into the game.

### Example
```json
{
  "BaseCompanyName": "Inlet",
  "CompanyName": "Moon",
  "FullName": "Moon Macro Systems",
  "CompanyId": "MMS",
  "InitialCash": 1000000000,
  "StartingTechYear": 1975,
  "InitialFactoryCapacity": 16,
  "FoundingYear": 1977,
  "FoundingMonth": 2,
  "SpawnYear": 1977,
  "SpawnMonth": 2,
  "Colour": "#e8600c",
  "DarkModeColour": "#e8600c",
  "CpuNamePatterns": [
    {
      "PatternType": "GenerationRomanModel",
      "BaseName": "UltraSPARK ",
      "IntroductionYear": 1995,
      "GenerationYear": 0,
      "UseCustomGenerator": true,
      "SegmentSuffixes": [
        { "Segment": "DesktopHigh", "Suffix": " Pro" },
        { "Segment": "DesktopMid", "Suffix": "" },
        { "Segment": "DesktopLow", "Suffix": " LP" }
      ]
    }
  ]
}
```

### Fields
* **BaseCompanyName**: The internal name of the base company this one should mimic for initial behavior or data.
* **CompanyName**: The short name of the company used in most UI elements.
* **FullName**: The full name of the company.
* **CompanyId**: A unique, stable ID used to identify and restore the company in save games. Do not reuse an ID belonging to another company.
* **InitialCash**: The amount of cash the company starts with when it spawns.
* **StartingTechYear**: The technology level the company starts with (expressed as a year).
* **InitialFactoryCapacity**: The initial production capacity of the company's factory.
* **FoundingYear**: The year the company was founded.
* **FoundingMonth**: The month the company was founded.
* **SpawnYear**: The year the company should spawn into the game world.
* **SpawnMonth**: The month the company should spawn into the game world.
* **Colour**: Optional HTML color used for the company in the normal theme, such as `#e8600c`.
* **DarkModeColour**: Optional HTML color used for the company in the dark theme.
* **CpuNamePatterns**: Optional list that replaces the base company's CPU naming patterns. If omitted, the inherited generator remains unchanged.

### CPU name patterns

Patterns should be listed in chronological order. The generator starts with the first entry and then selects the last entry in the list whose `IntroductionYear` is less than or equal to the current year.

* **PatternType**: One of the name types in the table below. Enum names are case-sensitive.
* **BaseName**: Text placed at the beginning of generic names. Some company-specific name types instead generate their own fixed brand text; their descriptions identify this behavior.
* **IntroductionYear**: The first year this pattern can be selected.
* **GenerationYear**: The year from which generation counting starts. Set it to `0` or a negative value to use `IntroductionYear`.
* **UseCustomGenerator**: When `true`, enables configurable `SegmentSuffixes` for `Generation` and `GenerationRomanModel`. Other name types continue to use the standard game algorithm even when this is `true`.
* **SegmentSuffixes**: Segment/suffix pairs used by the custom generator. Missing segments receive an empty suffix. Spaces are not inserted automatically, so include any desired spaces in `BaseName` or `Suffix`.

Generation numbers are based on the company's qualifying main-market CPUs since `GenerationYear`. A CPU targeting the company's main market advances the generation number; other target segments use the current main-market generation.

#### Segment values

All valid `Segment` strings are:

* `DesktopHigh`
* `DesktopMid`
* `DesktopLow`
* `Industrial`
* `MobileHigh`
* `MobileMid`
* `MobileLow`

Every value can be used in `SegmentSuffixes`. The standard name types do not all have special formatting for every segment: desktop-oriented types generally specialize the desktop and industrial values, while `Samsuno`, `Quantoom`, and `MidasTek` types specialize the mobile values.

#### Name type values

For the generic types, `generation` means the calculated generation number. Unless noted otherwise, the generated portion is appended directly to `BaseName`.

| `PatternType` | Generated name behavior |
| --- | --- |
| `ThousandAndBits` | Produces an era-dependent four-digit generation/bit-style model. Standard suffixes are `M` for `DesktopMid`, `U` for `DesktopLow`, and `LP` for `Industrial`. |
| `HundredAndBits` | Produces a shorter generation/bit-style model. Standard suffixes are `GX` for `DesktopHigh`, `U` for `DesktopLow`, and `LP` for `Industrial`. |
| `ThousandAndHundred` | Produces `<generation><tier>00`, where the tier is `7`, `5`, `3`, or `0` for high, mid, low, or industrial desktop targets. |
| `Generation` | Standard generator: produces `<generation><tier>` using tiers `7`, `5`, `3`, and `0`. Custom generator: produces `<generation><configured suffix>`. |
| `ThousandHundredModel` | Produces `<tier> <generation><model>`, using tier/model families `7`/`700`, `5`/`600`, `3`/`200`, and the industrial family. |
| `GenerationNoModel` | Produces only the numeric generation, with no tier/model number. |
| `GenerationRomanModel` | Standard generator: produces a Roman-numeral generation followed by ` Extreme`, ` Pro`, no suffix, or ` LP` for high, mid, low, or industrial desktop targets. Custom generator: produces the Roman numeral followed by the configured suffix. |
| `ThousandHundredModelNoSpace` | Produces `<generation><model>` without the leading tier or separating space used by `ThousandHundredModel`. |
| `CoreGeneration` | Produces `<core count><generation><tier code>`, with tier codes `70`, `50`, and `00` for high, mid, and low desktop targets. |
| `InletPentum` | Generates the built-in `Pentum G...` family; multicore designs switch to fixed `Kore 2 Duo`, `Quad`, `Hexa`, or `Octa` branding. `BaseName` is ignored. |
| `InletKore` | Generates the built-in `Kore i7`, `i5`, or `i3` generation/model family, including its special eighth-generation high-end model. `BaseName` is ignored. |
| `InletKoreUltra` | Generates `Kore Ultra 7`, `5`, or `3` names using generations counted from 2023. `BaseName` is ignored. |
| `EMDAthon` | Generates the built-in `Athon`, `Phaenom`, `Semprion`, or `Dorun` family according to year, segment, and core count. `BaseName` is ignored. |
| `EMDPX` | Generates `PX-` names whose digits encode core count, SMT, generation, and desktop tier. `BaseName` is ignored. |
| `EMDRozan` | Generates `Rozan <tier> <generation><model>` names, with generations based on qualifying process nodes from 2015 onward. `BaseName` is ignored. |
| `Morotola` | Generates the built-in `MT-68...`/`MT-680...` generation sequence. `BaseName` is ignored. |
| `Samsuno2000` | Generates mobile `S7`, `S5`, or `S3` model families; high-end models receive a `P` suffix. `BaseName` is ignored. |
| `Samsuno2010` | Generates `<BaseName> <generation><tier>00`, with mobile tiers `9`, `8`, and `7`. |
| `Quantoom2000` | Generates built-in `FMP8...` names, switching to `SMP8...` for `MobileLow`. `BaseName` is ignored. |
| `Quantoom2010` | Generates `<BaseName> <tier><generation>0`, with mobile tiers `8`, `6`, and `2`. |
| `MidasTek2000` | Generates built-in `MI-<generation><tier>00` names, with mobile tiers `7`, `5`, and `3`. `BaseName` is ignored. |
| `MidasTek2015` | Generates `<BaseName> <suffix><generation><tier>`, using `XT`/`9`, `GX`/`5`, and `LP`/`1` for high, mid, and low mobile targets. |
