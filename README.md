# UnityOpenworldSandbox


To make sure that git merge will work fine, add following code to your .gitconfig file (/c/Users/user_name/.gitconfig):

```
[merge "unityyamlmerge"]
	name = Unity SmartMerge (UnityYamlMerge)
	# example: "/c/Dev/2020.3.26f1/Editor/Data/Tools/UnityYAMLMerge.exe\"
	driver = \"/PATH_TO_YOUR_UNITY_EDITOR/Editor/Data/Tools/UnityYAMLMerge.exe\" merge -h -p --force --fallback none %O %B %A %A
	recursive = binary
```