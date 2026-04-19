$ErrorActionPreference = 'Stop'
$root = "Z:\Timtek\Timtek.SAR\src"

Write-Host "=== .NET SDKs ===" -ForegroundColor Cyan
dotnet --list-sdks

if (-not (Test-Path $root)) {
    New-Item $root -ItemType Directory -Force | Out-Null
}
Set-Location $root

# Solution
Write-Host "`nCreating solution..." -ForegroundColor Cyan
dotnet new sln --name Timtek.SAR

# Projects
$projects = @(
    @{ Name = "Timtek.SAR.Domain"; Template = "classlib" }
    @{ Name = "Timtek.SAR.Application"; Template = "classlib" }
    @{ Name = "Timtek.SAR.Infrastructure"; Template = "classlib" }
    @{ Name = "Timtek.SAR.Web"; Template = "web" }
    @{ Name = "Timtek.SAR.Api"; Template = "webapi" }
    @{ Name = "Timtek.SAR.Tests"; Template = "classlib" }
)

foreach ($p in $projects) {
    Write-Host "  Creating $($p.Name)..." -ForegroundColor Cyan
    dotnet new $p.Template --name $p.Name --framework net10.0
    dotnet sln add "$($p.Name)/$($p.Name).csproj"
}

# Project references
Write-Host "`nAdding references..." -ForegroundColor Cyan
dotnet add Timtek.SAR.Application reference Timtek.SAR.Domain/Timtek.SAR.Domain.csproj
dotnet add Timtek.SAR.Infrastructure reference Timtek.SAR.Domain/Timtek.SAR.Domain.csproj
dotnet add Timtek.SAR.Infrastructure reference Timtek.SAR.Application/Timtek.SAR.Application.csproj
dotnet add Timtek.SAR.Web reference Timtek.SAR.Application/Timtek.SAR.Application.csproj
dotnet add Timtek.SAR.Api reference Timtek.SAR.Application/Timtek.SAR.Application.csproj
dotnet add Timtek.SAR.Tests reference Timtek.SAR.Application/Timtek.SAR.Application.csproj
dotnet add Timtek.SAR.Tests reference Timtek.SAR.Domain/Timtek.SAR.Domain.csproj

# Test packages
Write-Host "`nAdding test packages..." -ForegroundColor Cyan
$tp = "Timtek.SAR.Tests/Timtek.SAR.Tests.csproj"
dotnet add $tp package Machine.Specifications
dotnet add $tp package Machine.Specifications.Should
dotnet add $tp package Machine.Specifications.Runner.VisualStudio
dotnet add $tp package FakeItEasy
dotnet add $tp package Microsoft.NET.Test.Sdk

# Clean up placeholder files
Get-ChildItem -Path $root -Filter "Class1.cs" -Recurse | Remove-Item -Force

Write-Host "`n=== Solution contents ===" -ForegroundColor Green
dotnet sln list
Write-Host "`nScaffolding complete." -ForegroundColor Green
