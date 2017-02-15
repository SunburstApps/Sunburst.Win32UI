& dotnet restore
& dotnet build

mkdir ..\..\bin\BuildTasks -ea 0 > $null
copy bin\Debug\net45\Win32UI.*.dll, `
     Sunburst.Win32UI.Build.targets `
     ..\..\bin\BuildTasks
