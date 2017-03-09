& dotnet restore
& dotnet build

mkdir ..\..\bin\BuildTasks\, ..\..\bin\BuildTasks\net46, ..\..\bin\BuildTasks\netcoreapp1.1 -ea 0 > $null
copy bin\Debug\net46\Win32UI.*.dll ..\..\bin\BuildTasks\net46
copy bin\Debug\netcoreapp1.1\Win32UI.*.dll ..\..\bin\BuildTasks\netcoreapp1.1
copy Sunburst.Win32UI.Build.targets ..\..\bin\BuildTasks
