# Download Satisfactory item icons from the wiki into wwwroot/icons.
# Run from repo root: .\scripts\DownloadItemIcons.ps1
# Icons are derived from ResourceDisplayNames.cs display-name values (slug = name with spaces replaced by underscores).

$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent $PSScriptRoot
$csPath = Join-Path $repoRoot "SatisfactoryApp\Utils\ResourceDisplayNames.cs"
$outDir = Join-Path $repoRoot "SatisfactoryApp\wwwroot\icons"
$baseUrl = "https://satisfactory.wiki.gg/images"

if (-not (Test-Path $csPath)) {
    Write-Error "ResourceDisplayNames.cs not found at $csPath"
}
if (-not (Test-Path $outDir)) {
    New-Item -ItemType Directory -Path $outDir -Force | Out-Null
}

$content = Get-Content $csPath -Raw
$matches = [regex]::Matches($content, '=\s*"([^"]+)"')
$displayNames = $matches | ForEach-Object { $_.Groups[1].Value } | Where-Object { $_ -notmatch '^[a-z0-9]+$' -and $_.Length -gt 1 } | Sort-Object -Unique
$slugs = $displayNames | ForEach-Object { $_ -replace ' ', '_' } | Sort-Object -Unique

Write-Host "Found $($slugs.Count) unique icon slugs. Output: $outDir"
$downloaded = 0
$failed = @()

foreach ($slug in $slugs) {
    $url = "$baseUrl/$slug.png"
    $outPath = Join-Path $outDir "$slug.png"
    try {
        Invoke-WebRequest -Uri $url -OutFile $outPath -UseBasicParsing -TimeoutSec 10
        $downloaded++
        Write-Host "  OK $slug"
    } catch {
        $failed += $slug
        Write-Host "  FAIL $slug" -ForegroundColor Yellow
    }
    Start-Sleep -Milliseconds 200
}

Write-Host "`nDone. Downloaded: $downloaded. Failed: $($failed.Count)"
if ($failed.Count -gt 0) {
    Write-Host "Failed slugs: $($failed -join ', ')"
}
