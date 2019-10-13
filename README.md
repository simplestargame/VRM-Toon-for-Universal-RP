# VRM Toon Shader Graph for Universal RP

# Requirements
Unity 2019.3.0b6  
Universal RP template project  
 - Universal RP 7.1.2
 - Shader Graph 7.1.2

# Usage of this Toon Shader  
1. Craete Universaal RP Template project or Open this project and set Universal Render Assets to ProjectSettings > Graphics
1. Update Universal RP versions to v7.1.2 in Menu Window > Package Manager
1. Move UnityProjectDir/Library/PackageCache to UnityProjectDir/Packages of ...  
  1. com.unity.render-pipelines.core@7.1.2
  1. com.unity.render-pipelines.universal@7.1.2
  1. com.unity.shadergraph@7.1.2  

  Now, you can see <font color="Red">pen icon</font> in Menu Window > Package Manager  
  ![packageManager](README/PackageManager2019-10-13120125.png)  
  Ready to edit package shader files.

1. Move my repository scripts and shader files  
  UnityProjectDir/Assets/MoveFiles child folders name indicates move destination folder paths.

1. Use "Shader Graphs/ToonShaderGraph" for your game!
  1. The file location is VRM-Toon-for-Universal-RP\Assets\UniToon\ToonShaderGraph.shadergraph

# Check Example VRM Scene

1. Find VRMMaterialImporter if you use [UniVRM package](https://github.com/vrm-c/UniVRM)  
  Edit a line as follows.
 ```cs
 var shaderName = "Shader Graphs/ToonShaderGraph";//item.shader;
 ```

# Result of My Universal RP Toon Shader
VRoid SDK Load Result Image.  
![warabeda](README/universalRP_Toon.jpg)
