param($installPath, $toolsPath, $package)



$vsVersions = @("2012","2013","2015","2017","2019")  



$MYDOC =[Environment]::GetFolderPath([Environment+SpecialFolder]::MyDocuments)
$source ="$toolsPath\MVVM-Sidekick.snippet"


    
Foreach ($vsVersion in $vsVersions)  
{  
    $myCodeSnippetsFolder = "$MYDOC\Visual Studio $vsVersion\Code Snippets\Visual C#\My Code Snippets\"  
    if (Test-Path $myCodeSnippetsFolder)  
    {  
			echo 'Copying From ' + $source
			echo 'Copying To ' $myCodeSnippetsFolder 
            copy $source $myCodeSnippetsFolder  
    }  
}  

 