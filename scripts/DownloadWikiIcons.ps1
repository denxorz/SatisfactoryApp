<# 
 Download all Building, Item, and Fluid icons from the Satisfactory wiki into wwwroot/icons.

 Run from repo root:
   .\scripts\DownloadWikiIcons.ps1

 This script:
 - Scrapes these categories:
     https://satisfactory.wiki.gg/wiki/Category:Building_icons
     https://satisfactory.wiki.gg/wiki/Category:Item_icons
     https://satisfactory.wiki.gg/wiki/Category:Fluid_icons
 - Extracts all .png file names from links like /wiki/File:Iron_Ore.png
 - Downloads the raw images from https://satisfactory.wiki.gg/images/{FileName}.png
#>

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$outDir   = Join-Path $repoRoot "SatisfactoryApp\wwwroot\icons"

if (-not (Test-Path $outDir)) {
    New-Item -ItemType Directory -Path $outDir -Force | Out-Null
}

$categories = @(
    "https://satisfactory.wiki.gg/wiki/Category:Building_icons",
    "https://satisfactory.wiki.gg/wiki/Category:Item_icons",
    "https://satisfactory.wiki.gg/wiki/Category:Fluid_icons"
)

Write-Host "Fetching icon file lists from wiki categories..."

$fileNames = @()

foreach ($cat in $categories) {
    Write-Host "  Category: $cat"
    $nextUrl = $cat
    while ($nextUrl) {
        Write-Host "    Page: $nextUrl"
        $resp = Invoke-WebRequest -Uri $nextUrl -UseBasicParsing -TimeoutSec 30

        # Match /wiki/File:Something.png
        $matches = [regex]::Matches($resp.Content, "/wiki/File:([A-Za-z0-9_()%\-]+\.png)")
        foreach ($m in $matches) {
            $name = $m.Groups[1].Value
            if ($name) { $fileNames += $name }
        }

        # Look for a "next page" link in this category
        $nextMatch = [regex]::Match(
            $resp.Content,
            "href=""(/wiki/Category:[^""]+?pagefrom=[^""]+)\"".*?>\s*next page",
            [System.Text.RegularExpressions.RegexOptions]::IgnoreCase
        )

        if ($nextMatch.Success) {
            $href = $nextMatch.Groups[1].Value -replace "&amp;","&"
            $nextUrl = "https://satisfactory.wiki.gg$href"
        }
        else {
            $nextUrl = $null
        }
    }
}

$fileNames = $fileNames | Sort-Object -Unique

Write-Host "Found $($fileNames.Count) unique icon files."

$baseImageUrl = "https://satisfactory.wiki.gg/images"
$downloaded   = 0
$failed       = @()

foreach ($name in $fileNames) {
    $url     = "$baseImageUrl/$name"
    $outPath = Join-Path $outDir $name

    if (Test-Path $outPath) {
        Write-Host "  Skip (exists) $name"
        continue
    }

    try {
        Invoke-WebRequest -Uri $url -OutFile $outPath -UseBasicParsing -TimeoutSec 20
        $downloaded++
        Write-Host "  OK $name"
    }
    catch {
        $failed += $name
        Write-Host "  FAIL $name" -ForegroundColor Yellow
    }

    Start-Sleep -Milliseconds 200
}

Write-Host "`nDone. Downloaded: $downloaded. Failed: $($failed.Count)"
if ($failed.Count -gt 0) {
    Write-Host "Failed files: $($failed -join ', ')"
}

