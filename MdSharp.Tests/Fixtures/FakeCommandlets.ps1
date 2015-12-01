function Get-Project(){
	return New-Object psobject -property @{FullName = [System.IO.Path]::Combine($pwd, "..", "..", "..")}
}