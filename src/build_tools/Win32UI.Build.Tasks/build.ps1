& dotnet restore
& dotnet publish

mkdir ..\..\bin\BuildTasks -ea 0 > $null
copy bin\Debug\netstandard1.3\publish\System.*.dll, `
     bin\Debug\netstandard1.3\publish\Win32UI.Build.*.dll, `
     Sunburst.Win32UI.Build.targets `
     ..\..\bin\BuildTasks
