namespace SatisfactoryApp.Utils;

public static class ResourceDisplayNames
{
    private static readonly Dictionary<string, string> DisplayNames = new(StringComparer.OrdinalIgnoreCase)
    {
        ["oreiron"] = "Iron Ore",
        ["orecopper"] = "Copper Ore",
        ["stone"] = "Limestone",
        ["coal"] = "Coal",
        ["oregold"] = "Caterium Ore",
        ["rawquartz"] = "Raw Quartz",
        ["sulfur"] = "Sulfur",
        ["orebauxite"] = "Bauxite",
        ["oreuranium"] = "Uranium",
        ["sam"] = "SAM",
        ["liquidoil"] = "Crude Oil",
        ["water"] = "Water",
        ["nitrogengas"] = "Nitrogen Gas",
        ["plastic"] = "Plastic",
        ["rubber"] = "Rubber",
        ["modularframefused"] = "Fused Modular Frame",
        ["computer"] = "Computer",
        ["modularframeheavy"] = "Heavy Modular Frame",
        ["aluminumcasing"] = "Aluminum Casing",
        ["aluminumplate"] = "Alclad Aluminum Sheet",
        ["ficsiteingot"] = "Ficsite Ingot",
        ["fluidcanister"] = "Empty Canister",
        ["gastank"] = "Empty Fluid Tank",
        ["fuel"] = "Fuel",
        ["quartzcrystal"] = "Quartz Crystal",
        ["singularitycell"] = "Singularity Cell",
        ["computersuper"] = "Supercomputer",
        ["turbofuel"] = "Turbofuel",
        ["samingot"] = "SAM Ingot",
        ["nuclearwaste"] = "Uranium Waste",
        ["timecrystal"] = "Time Crystal",
        ["crystaloscillator"] = "Crystal Oscillator",
        ["coppersheet"] = "Copper Sheet",
        ["packagedbiofuel"] = "Packaged Liquid Biofuel",
        ["biofuel"] = "Biofuel",
        ["wire"] = "Wire",
        ["cable"] = "Cable",
        ["motorlightweight"] = "Turbo Motor",
        ["ficsitemesh"] = "Ficsite Trigon",
        ["ironplatereinforced"] = "Reinforced Iron Plate",
        ["ironplate"] = "Iron Plate",
        ["ironrod"] = "Iron Rod",
        ["steelplate"] = "Steel Plate",
        ["ironscrew"] = "Screws",
        ["modularframe"] = "Modular Frame",
        ["portableminer"] = "Portable Miner",
        ["electromagneticcontrolrod"] = "Electromagnetic Control Rod",
        ["motor"] = "Motor",
        ["steelpipe"] = "Steel Pipe",
        ["steelplatereinforced"] = "Reinforced Steel Plate",
        ["cement"] = "Concrete",
        ["stator"] = "Stator",
        ["packagedionizedfuel"] = "Packaged Ionized Fuel",
        ["rotor"] = "Rotor",
        ["highspeedwire"] = "Quickwire",
        ["modularframelightweight"] = "Lightweight Modular Frame",
        ["hazmatfilter"] = "Iodine-Infused Filter",
        ["filter"] = "Gas Filter",
        ["fabric"] = "Fabric",
        ["circuitboardhighspeed"] = "High-Speed Connector",
        ["circuitboard"] = "Circuit Board",
        ["crystalshard"] = "Power Shard",
        ["samfluctuator"] = "SAM Fluctuator",
        ["silica"] = "Silica",
        ["highspeedconnector"] = "High-Speed Connector",
        ["coolingsystem"] = "Cooling System",
        ["ironingot"] = "Iron Ingot",
        ["copperingot"] = "Copper Ingot",
        ["steelingot"] = "Steel Ingot",
        ["cateriumingot"] = "Caterium Ingot",
        ["aluminumingot"] = "Aluminum Ingot",
        ["liquidfuel"] = "Fuel",
        ["packagednitrogengas"] = "Packaged Nitrogen Gas",
        ["spaceelevatorpart1"] = "Smart Plating",
        ["spaceelevatorpart2"] = "Versatile Framework",
        ["spaceelevatorpart3"] = "Automated Wiring",
        ["spaceelevatorpart4"] = "Modular Engine",
        ["spaceelevatorpart5"] = "Adaptive Control Unit",
        ["spaceelevatorpart6"] = "Magnetic Field Generator",
        ["spaceelevatorpart7"] = "Assembly Director System",
        ["spaceelevatorpart8"] = "Thermal Propulsion Rocket",
        ["spaceelevatorpart9"] = "Nuclear Pasta",
        ["spaceelevatorpart10"] = "Biochemical Sculptor",
        ["spaceelevatorpart11"] = "Ballistic Warp Drive",
        ["spaceelevatorpart12"] = "AI Expansion Server",
        ["goldingot"] = "Caterium Ingot",
        ["darkenergy"] = "Dark Matter Residue",
        ["darkmatter"] = "Dark Matter",
        ["quantumoscillator"] = "Superposition Oscillator",
        ["quantumenergy"] = "Excited Photonic Matter",
        ["alienpowerfuel"] = "Alien Power Matrix",
        ["polymerresin"] = "Polymer Resin",
        ["plutoniumwaste"] = "Plutonium Waste",
        ["ficsoniumfuelrod"] = "Ficsonium Fuel Rod",
        ["nitricacid"] = "Nitric Acid",
        ["candycane"] = "Candy Cane",
        ["gift"] = "FICSMAS Gift",
        ["xmasballcluster"] = "FICSMAS Ornament Bundle",
        ["xmasbranch"] = "FICSMAS Tree Branch",
        ["xmaswreath"] = "FICSMAS Wreath",
        ["snow"] = "Actual Snow",
        ["xmasball1"] = "Red FICSMAS Ornament",
        ["xmasball2"] = "Blue FICSMAS Ornament",
        ["xmasball3"] = "Copper FICSMAS Ornament",
        ["xmasball4"] = "Iron FICSMAS Ornament",
        ["xmasbow"] = "FICSMAS Bow",
        ["xmasstar"] = "FICSMAS Wonder Star",
        ["cartridgestandard"] = "Rifle Ammo",
        ["copperdust"] = "Copper Powder",
        ["gunpowdermk2"] = "Smokeless Powder",
        ["nobeliskexplosive"] = "Nobelisk",
        ["pressureconversioncube"] = "Pressure Conversion Cube",
        ["cartridgechaos"] = "Turbo Rifle Ammo",
        ["cartridgesmartprojectile"] = "Homing Rifle Ammo",
        ["compactedcoal"] = "Compacted Coal",
        ["gunpowder"] = "Black Powder",
        ["nobeliskcluster"] = "Cluster Nobelisk",
        ["nobeliskshockwave"] = "Pulse Nobelisk",
        ["packagedrocketfuel"] = "Packaged Rocket Fuel",
        ["rebarexplosive"] = "Explosive Rebar",
        ["rebarspreadshot"] = "Shatter Rebar",
        ["rebarstunshot"] = "Stun Rebar",
        ["rocketfuel"] = "Rocket Fuel",
        ["spikedrebar"] = "Iron Rebar",
        ["uraniumcell"] = "Encased Uranium Cell",
        ["nuclearfuelrod"] = "Uranium Fuel Rod",
        ["plutoniumfuelrod"] = "Plutonium Fuel Rod",
        ["crystal"] = "Blue Power Slug",
        ["crystalmk2"] = "Yellow Power Slug",
        ["crystalmk3"] = "Purple Power Slug",
        ["hatcherparts"] = "Hatcher Remains",
        ["hogparts"] = "Hog Remains",
        ["leaves"] = "Leaves",
        ["mycelia"] = "Mycelia",
        ["spitterparts"] = "Spitter Remains",
        ["stingerparts"] = "Stinger Remains",
        ["wood"] = "Wood",
        ["aliendnacapsule"] = "Alien DNA Capsule",
        ["alienprotein"] = "Alien Protein",
        ["genericbiomass"] = "Biomass",
        ["liquidbiofuel"] = "Liquid Biofuel",
        ["diamond"] = "Diamonds",
        ["aluminumscrap"] = "Aluminum Scrap",
        ["packagedoilresidue"] = "Packaged Heavy Oil Residue",
        ["heavyoilresidue"] = "Heavy Oil Residue",
        ["packagedoil"] = "Packaged Oil",
        ["aluminumplatereinforced"] = "Reinforced Aluminum Plate",
        ["aluminasolution"] = "Alumina Solution",
        ["petroleumcoke"] = "Petroleum Coke",
        ["ficsonium"] = "Ficsonium",
        ["ionizedfuel"] = "Ionized Fuel",
        ["liquidturbofuel"] = "Liquid Turbo Fuel",
        ["nonfissibleuranium"] = "Non-Fissile Uranium",
        ["plutoniumcell"] = "Plutonium Cell",
        ["plutoniumpellet"] = "Plutonium Pellet",
        ["sulfuricacid"] = "Sulfuric Acid",

        // Machines / buildings (used in factory summaries)
        ["constructor"] = "Constructor",
        ["smelter"] = "Smelter",
        ["foundry"] = "Foundry",
        ["assembler"] = "Assembler",
        ["manufacturer"] = "Manufacturer",
        ["refinery"] = "Refinery",
        ["blender"] = "Blender",
        ["packager"] = "Packager",
        ["converter"] = "Converter",
        ["hadroncollider"] = "Particle Accelerator",
        ["quantumencoder"] = "Quantum Encoder",
        ["minermk1"] = "Miner Mk.1",
        ["minermk2"] = "Miner Mk.2",
        ["minermk3"] = "Miner Mk.3",
        ["waterextractor"] = "Water Extractor",
        ["oilextractor"] = "Oil Extractor",
        ["resourcesink"] = "AWESOME Sink",
        ["trainstation"] = "Train Station",
        ["truckstation"] = "Truck Station",
        ["dronestation"] = "Drone Port",
        ["generatorbiomass"] = "Biomass Burner",
        ["generatorcoal"] = "Coal-Powered Generator",
        ["generatorfuel"] = "Fuel-Powered Generator",
        ["generatornuclear"] = "Nuclear Power Plant",
        ["generatorgeothermal"] = "Geothermal Generator",
        ["powerstorage"] = "Power Storage",
        ["frackingextractor"] = "Resource Well Extractor",
        ["assemblermk1"] = "Assembler",
        ["manufacturermk1"] = "Manufacturer",
        ["foundrymk1"] = "Foundry",
        ["oilrefinery"] = "Refinery",
        ["spaceelevator"] = "Space Elevator",
        ["pipehyperstart"] = "Hypertube Entrance",

        // Additional aliases for older/internal building class names
        ["waterpump"] = "Water Extractor",
        ["oilpump"] = "Oil Extractor",
        ["radartower"] = "Radar Tower",
        ["constructormk1"] = "Constructor",
        ["smeltermk1"] = "Smelter",
        ["frackingsmasher"] = "Resource Well Extractor",
        ["elevator"] = "Space Elevator",
        ["alienpower"] = "Alien Power Matrix",
    };

    private static readonly HashSet<string> _loggedMissing = new(StringComparer.OrdinalIgnoreCase);

    public static string GetDisplayName(string? itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
            return "Unknown";

        var key = NormalizeKey(itemName);
        if (DisplayNames.TryGetValue(key, out var displayName))
            return displayName;

        if (_loggedMissing.Add(key))
            Console.WriteLine($"[ResourceDisplayNames] Missing mapping for: \"{itemName}\" (key: \"{key}\")");
        return itemName;
    }

    public static string GetItemIconUrl(string? itemName)
    {
        var displayName = GetDisplayName(itemName);
        if (string.IsNullOrEmpty(displayName) || displayName == "Unknown") return string.Empty;
        return $"/icons/{GetIconSlug(displayName)}.png";
    }

    public static string GetIconSlug(string displayName)
    {
        if (string.IsNullOrEmpty(displayName)) return string.Empty;
        return displayName.Replace(" ", "_");
    }

    private static string NormalizeKey(string value)
    {
        if (value.StartsWith("Desc_", StringComparison.OrdinalIgnoreCase) &&
            value.EndsWith("_C", StringComparison.OrdinalIgnoreCase) &&
            value.Length > "Desc__C".Length)
        {
            var inner = value[5..^2]; // between Desc_ and _C
            return new string(inner.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
        }

        if (value.StartsWith("Build_", StringComparison.OrdinalIgnoreCase) &&
            value.EndsWith("_C", StringComparison.OrdinalIgnoreCase) &&
            value.Length > "Build__C".Length)
        {
            var inner = value[6..^2]; // between Build_ and _C
            return new string(inner.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
        }

        return new string(value.Where(c => char.IsLetterOrDigit(c)).ToArray()).ToLowerInvariant();
    }
}
