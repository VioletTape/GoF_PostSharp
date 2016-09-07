This folder contains the build-time and run-time components of PostSharp:

* 'tools' contains build-time components. 
   Edit your csproj/vbproj file with a text editor and add an <Import /> element for 'tools\PostSharp.targets'.

* 'lib' contains run-time components. 
   Add to your project a reference to PostSharp.dll for the proper framework family and version.

For more information, see http://doc.postsharp.net/.