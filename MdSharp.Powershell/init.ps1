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

	Write-Output "Registering tab expansion"
    Register-TabExpansion 'Get-Markdown' @{
      'name' = { GetDocumentAssemblies | foreach { $_ } }
    }
}
else {
    Write-Warning "Could not locate MdSharp module"
}

function GetDocumentAssemblies(){
    $selectedProject = Get-Project
    $projectPath = Split-Path $selectedProject.FullName

	$binPath = [System.IO.Path]::Combine($projectPath, 'bin')

    # Get the debug path if exists, because it is mo betta
	if (Test-Path ([System.IO.Path]::Combine($binPath, 'Debug'))) {
		$binPath = [System.IO.Path]::Combine($binPath, 'Debug')
	}

    return [System.IO.Directory]::GetFiles($binPath, "*.xml")
}
