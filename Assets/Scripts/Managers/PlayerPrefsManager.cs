using System;
using UnityEngine;

namespace Managers
{
public class PlayerPrefsManager : MonoBehaviour
{
    [SerializeField, Range( 0.05f, 0.3f )] private float defaultSensitivity;

    private void Awake()
    {
        // TODO: Delete after adding sensitivity setting
        Sensitivity = defaultSensitivity;
    }

    public float Sensitivity
    {
        get
        {
            if ( !PlayerPrefs.HasKey( "Sensitivity" ) )
            {
                Sensitivity = defaultSensitivity;
            }

            return PlayerPrefs.GetFloat( "Sensitivity" );
        }
        set => PlayerPrefs.SetFloat( "Sensitivity", value );
    }
}
}