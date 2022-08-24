using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.MyEditor
{
public static class FindMissingScripts
{
    [MenuItem( "My Menu/Find missing scripts in project" )]
    static void FindMissingScriptsInProjectMenuItem()
    {
        string[] prefabPaths = AssetDatabase.GetAllAssetPaths().Where(
            path => path.EndsWith( ".prefab", System.StringComparison.OrdinalIgnoreCase ) ).ToArray();

        foreach ( var path in prefabPaths )
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>( path );
            foreach ( var component in prefab.GetComponentsInChildren<Component>() )
            {
                if ( component == null )
                {
                    Debug.Log( $"Prefab found with missing script: {path} {prefab}" );
                    break;
                }
            }
        }
    }
}
}