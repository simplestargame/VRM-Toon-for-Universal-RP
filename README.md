# VRM Toon Shader Graph for Universal RP

# Requirements
Unity 2019.3.0b1  
Universal RP template project  
 - Universal RP 7.0.1
 - Shader Graph 7.0.1

# Usage of this Toon Shader  
1. Move PackageCache to Packages of ...  
  1. Shader Graph
  1. Universal RP
1. Move this Project Assets
  1. MoveFiles child folder names indicates move destination.
1. Use valid ToonShaderGraph.shadergraph for your game!

# Example
1. Find VRMMaterialImporter if you use UniVRM package.  
 edit next line.
 ```cs
 var shaderName = "Shader Graphs/ToonShaderGraph";//item.shader;
 ```

# Result
VRoid SDK Load Result Image.  
![warabeda](README/universalRP_Toon.jpg)
