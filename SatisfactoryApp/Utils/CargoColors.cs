namespace SatisfactoryApp.Utils;

public static class CargoColors
{
    public static string GetCargoColor(string cargoType)
    {
        var cargoTypeLower = cargoType.ToLower();
        return cargoTypeLower switch
        {
            "coal" => "#000000",
            "plastic" => "#4169E1",
            "rubber" => "#F5F5DC",
            "modularframefused" => "#FFFF00",
            "computer" => "#00CED1",
            "oreuranium" => "#00FF00",
            "modularframeheavy" => "#F5F5F5",
            "aluminumcasing" => "darkgray",
            "aluminumplate" => "gray",
            "ficsiteingot" => "gold",
            "spaceelevatorpart_7" => "orange",
            "fluidcanister" or "gastank" => "#FF6B6B",
            "fuel" => "#FFD700",
            "quartzcrystal" => "#FF1493",
            "singularitycell" => "#E6E6FA",
            "computersuper" => "#00BFFF",
            "spaceelevatorpart_9" => "#FF4500",
            "turbofuel" => "#FF8C00",
            "sam" or "samingot" => "#9932CC",
            "nuclearwaste" => "#00FF00",
            "timecrystal" => "#FF69B4",
            "crystaloscillator" => "#FFB6C1",
            "spaceelevatorpart_8" => "#FF6347",
            "coppersheet" => "#CD7F32",
            "packagedbiofuel" => "#8B4513",
            "biofuel" => "#A0522D",
            "wire" => "#C0C0C0",
            "cable" => "#708090",
            "motorlightweight" => "#B0C4DE",
            "ficsitemesh" => "#9370DB",
            "ironplatereinforced" => "#696969",
            "ironplate" => "#A9A9A9",
            "ironrod" => "#808080",
            "steelplate" => "#2F4F4F",
            "ironscrew" => "#778899",
            "modularframe" => "#4682B4",
            "portableminer" => "#5F9EA0",
            "electromagneticcontrolrod" => "#20B2AA",
            "motor" => "#87CEEB",
            "steelpipe" => "#4682B4",
            "steelplatereinforced" => "#191970",
            "cement" => "#F5F5DC",
            "stator" => "#DDA0DD",
            "packagedionizedfuel" => "#FF1493",
            "rotor" => "#DA70D6",
            "xmasball1" or "xmasbow" or "candycane" or "snow" or "xmasbranch" or "xmasball3" or "xmasball4" or "xmasball2" => "#8B0000",
            "nobeliskexplosive" or "nobeliskshockwave" or "cartridgechaos" or "cartridgestandard" or "cartridgesmartprojectile" or "rebar_explosive" or "rebar_stunshot" or "rebar_spreadshot" or "spikedrebar" => "#FFD700",
            "highspeedwire" => "#FFA500",
            "modularframelightweight" => "#ADD8E6",
            "hazmatfilter" or "filter" or "fabric" => "#D3D3D3",
            "circuitboardhighspeed" or "circuitboard" => "#00CED1",
            "crystalshard" => "#BA55D3",
            "samfluctuator" => "#4B0082",
            "silica" => "#1E90FF",
            "highspeedconnector" => "#FF8C00",
            "coolingsystem" => "#6A5ACD",
            _ => "#FF0000"
        };
    }
}

