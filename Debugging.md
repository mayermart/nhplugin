Under the project properties of the plugin (NHibernatePlugin.csproj) set the folowing values:
  * Debug | Start external program
    * `C:\Program Files\Microsoft Visual Studio 9.0\Common7\IDE\devenv.exe`
  * Debug | Comand line arguments
    * `/ReSharper.Plugin "C:\Code\NHibernatePlugin\NHibernatePlugin\bin\Debug\NHibernatePlugin.dll"`

Note that the plugin must not be loaded by ReSharper on startup of visual studio (remove it from ReSharpers plugin directory).