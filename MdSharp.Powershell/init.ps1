param($rootPath, $toolsPath, $package, $project)
$dllPath = Join-Path $toolsPath ObjectExporter.PowerShell.dll
if (Test-Path $dllPath) {
	Import-Module ($dllPath)
}
