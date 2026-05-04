$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$outDir   = Join-Path $repoRoot "SatisfactoryApp\wwwroot\icons"

if (-not (Test-Path $outDir)) {
    New-Item -ItemType Directory -Path $outDir -Force | Out-Null
}

$base  = "https://satisfactory.wiki.gg/images"
$names = @(
    "Resource_Well_Extractor",
    "Assembler",
    "Converter",
    "Space_Elevator",
    "Coal",
    "Plastic",
    "Rubber",
    "Limestone",
    "Wire",
    "Cable",
    "Concrete",
    "Water",
    "Quickwire",
    "Fuel",
    "Turbo_Motor",
    "Refinery",
    "Manufacturer",
    "Blender",
    "Hypertube_Entrance",
    "Drone_Port",
    "SAM",
    "Dark_Matter_Residue",
    "Superposition_Oscillator",
    "Alien_Power_Matrix",
    "Alclad_Aluminum_Sheet",

    # Additional missing buildings/items from network errors
    "Foundry",
    "AWESOME_Sink",
    "Radar_Tower",
    "ConstructorMk1",
    "SmelterMk1",
    "Coal-Powered_Generator",
    "Packager",
    "Fuel-Powered_Generator",

    "Bauxite",
    "Rotor",
    "Stator",
    "Computer",
    "Fabric",
    "Gas_Filter",
    "Iodine-Infused_Filter",
    "Excited_Photonic_Matter",
    "Shatter_Rebar",
    "Sulfur",

    # Additional common items that were still missing
    "Biomass",
    "Blue_Power_Slug",
    "Yellow_Power_Slug",
    "Purple_Power_Slug",
    "Leaves",
    "Mycelia",
    "Wood",
    "Biofuel",
    "Packaged_Liquid_Biofuel",
    "Screws",
    "Silica",
    "Turbofuel",
    "Uranium_Waste",
    "Encased_Uranium_Cell",

    # FICSMAS items (canonical wiki filenames)
    "FICSMAS_Gift",
    "FICSMAS_Tree_Branch",
    "FICSMAS_Wreath",
    "Actual_Snow",
    "FICSMAS_Bow",
    "FICSMAS_Wonder_Star",
    "Red_FICSMAS_Ornament",
    "Blue_FICSMAS_Ornament",
    "Copper_FICSMAS_Ornament",
    "Iron_FICSMAS_Ornament",
    "FICSMAS_Ornament_Bundle",

    # Additional missing icons from latest run
    "Constructor",
    "Smelter",
    "Nobelisk",
    "Motor",
    "Diamonds"
)

Write-Host "Downloading specific missing icons into $outDir"

foreach ($n in $names) {
    $url  = "$base/$n.png"
    $dest = Join-Path $outDir "$n.png"

    if (Test-Path $dest) {
        Write-Host "Skip $n (already exists)"
        continue
    }

    try {
        Invoke-WebRequest -Uri $url -OutFile $dest -UseBasicParsing -TimeoutSec 20
        Write-Host "OK   $n"
    }
    catch {
        Write-Host "FAIL $n : $($_.Exception.Message)" -ForegroundColor Yellow
    }

    Start-Sleep -Milliseconds 200
}

