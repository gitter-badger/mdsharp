param($rootPath, $toolsPath, $package, $project)

foreach ($_ in Get-Module | ?{$_.Name -eq 'MdSharpModule'}){
    Remove-Module 'MdSharpModule'
}

$modulePath = Join-Path $toolsPath MdSharp
$assemblyPath = Join-Path $toolsPath "MdSharp.Core.dll"
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

    //Get the debug bin path because it is mo betta
    $binPath = [System.IO.Path]::Combine($projectPath, 'bin', 'Debug')

    return [System.IO.Directory]::GetFiles($binPath, "*.xml")
}
