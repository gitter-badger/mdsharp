param($rootPath, $toolsPath, $package, $project)

foreach ($_ in Get-Module | ?{$_.Name -eq 'MdSharpModule'}){
    Remove-Module 'MdSharpModule'
}

$modulePath = Join-Path $toolsPath MdSharp.psm1
$assemblyPath = Join-Path $toolsPath "MdSharp.Core.dll"
$global:MdSharpToolsPath = $toolsPath

if (Test-Path $modulePath) {

	Write-Output "Importing MdSharp module"
    Import-Module ($modulePath)

	Write-Output "Adding MdSharp types"
	Add-Type -Path $assemblyPath
}
else {
    Write-Warning "Could not locate MdSharp module"
}
