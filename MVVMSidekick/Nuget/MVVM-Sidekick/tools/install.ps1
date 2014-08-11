
 
param($installPath, $toolsPath, $package, $project)



$vsVersions = @("2012","2013")  



$MYDOC =[Environment]::GetFolderPath([Environment+SpecialFolder]::MyDocuments)
$source ="$toolsPath\MVVM-Sidekick.snippet"


    
Foreach ($vsVersion in $vsVersions)  
{  
    $myCodeSnippetsFolder = "$MYDOC\Visual Studio $vsVersion\Code Snippets\Visual C#\My Code Snippets\"  
    if (Test-Path $myCodeSnippetsFolder)  
    {  
			echo $source
			echo $myCodeSnippetsFolder 
            copy $source $myCodeSnippetsFolder  
    }  
}  

 