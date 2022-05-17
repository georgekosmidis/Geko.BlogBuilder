
[string]$tmpFolder = 'tmp'

# Prepare the fodler where the build will go
if (Test-Path $tmpFolder) { 
    Remove-Item $tmpFolder -Recurse -Force 
}
New-Item -Path $tmpFolder -ItemType Directory

#Build and publish the solution to the $tmpFolder
dotnet publish _src/Blog.Builder.sln -c Release -o $tmpFolder

#Change location to the build fodler
[string]$cl = Get-Location
Set-Location -Path $tmpFolder

#Execute the builder with the two relative paths
dotnet Blog.Builder.dll --workables ..\workables --output ..\_output

#Go back and cleanup
Set-Location $cl 
Remove-Item $tmpFolder -Recurse -Force 