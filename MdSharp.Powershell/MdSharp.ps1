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
		$documentContext = New-Object -TypeName MdSharp.Core.DocumentContext -ArgumentList $xmlPath, $binPath
		Write-Output "MdSharp - Generating markdown"
		$documentContext.CreateMarkdown()
		Write-Output "MdSharp - Markdown generated!"
    }
	Catch
	{
		$ErrorMessage = $_.Exception.Message
		Write-Warning "Failed to create markdown"
		Write-Warning $ErrorMessage
	}
}

function GetBinPath{
	$selectedProject = Get-Project
	$projectPath = Split-Path $selectedProject.FullName

	return [System.IO.Path]::Combine($projectPath, 'bin', 'Debug')
}
