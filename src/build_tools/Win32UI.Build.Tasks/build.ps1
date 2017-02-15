& dotnet restore
& dotnet build

mkdir ..\..\bin\BuildTasks\, ..\..\bin\BuildTasks\net45, ..\..\bin\BuildTasks\netcoreapp1.1 -ea 0 > $null
copy bin\Debug\net45\Win32UI.*.dll ..\..\bin\BuildTasks\net45
copy bin\Debug\netcoreapp1.1\Win32UI.*.dll ..\..\bin\BuildTasks\netcoreapp1.1
copy Sunburst.Win32UI.Build.targets ..\..\bin\BuildTasks
