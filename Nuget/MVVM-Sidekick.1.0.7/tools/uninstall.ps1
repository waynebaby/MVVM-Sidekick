# Uninstall script for code snippets with NuGet
# =============================================
# Written by Geert van Horrik (see http://blog.catenalogic.com)
#
# Version 1.0 / 2011-02-18
#
# Required call to get environment variables
 
param($installPath, $toolsPath, $package, $project)
 
# You only have to customize the $snippetFolder name below,
# don't forget to rename the $snippetFolder of the other file
# ("install.ps1") as well
 
$snippetFolder = "MVVMSidekick"
 
# Actual script start
$source = "$toolsPath\*.snippet"
$vsVersions = @("2012")
 
Foreach ($vsVersion in $vsVersions)
{
    $myCodeSnippetsFolder = "$HOME\My Documents\Visual Studio $vsVersion\Code Snippets\Visual C#\My Code Snippets\"
    if (Test-Path $myCodeSnippetsFolder)
    {
        $destination = "$myCodeSnippetsFolder$snippetFolder"
        if (!($myCodeSnippetsFolder -eq $destination))
        {        
            if (Test-Path $destination)
            {
                "Uninstalling code snippets for Visual Studio $vsVersion"
                Remove-Item $destination -recurse -force
            }
        }
        else
        {
            "Define a value for snippetFolder variable, cannot be empty"
        }        
    }
}