function Get-Markdown{
    param(
		[Parameter(Position=0, 
				   Mandatory=$true, 
				   ValueFromPipeline=$true,
				   ValueFromPipelineByPropertyName=$true)]
		[string]$assemblyName
    )
    Write-Output "MdSharp - Fetching XML for $assemblyName"
    $binPath = GetBinPath
    $xmlPath = Join-Path $binPath ($assemblyName + ".xml")
    Write-Output "MdSharp - Loading Document Context for $xmlPath"

    Try{
		$documentContext = New-Object -TypeName MdSharp.Core.DocumentContext -ArgumentList $xmlPath, $global:MdSharpToolsPath
		Write-Output "MdSharp - Generating markdown"
		$documentContext.CreateMarkdown()
		Write-Output "MdSharp - Markdown generated!"
    }
	Catch
	{
		$ErrorMessage = $_.Exception.Message
		Write-Error "Failed to generate markdown: $ErrorMessage"
	}
}

function GetBinPath{
	$selectedProject = Get-Project
	$projectPath = Split-Path $selectedProject.FullName
	
	$binPath = [System.IO.Path]::Combine($projectPath, 'bin')

    # Get the debug path if exists, because it is mo betta
	if (Test-Path ([System.IO.Path]::Combine($binPath, 'Debug'))) {
		$binPath = [System.IO.Path]::Combine($binPath, 'Debug')
	}
	return $binPath
}

function GetDocumentAssemblies(){
	$binPath = GetBinPath
    return [System.IO.Directory]::GetFiles($binPath, "*.xml")
}

Write-Output "Exporting module member 'Get-Markdown'"
Export-ModuleMember Get-Markdown


Write-Output "Registering tab expansion"
Register-TabExpansion 'Get-Markdown' @{
    'assemblyName' = { GetDocumentAssemblies | foreach { [System.IO.Path]::GetFileNameWithoutExtension($_) } }
}