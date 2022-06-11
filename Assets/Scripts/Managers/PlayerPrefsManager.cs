using UnityEngine;

namespace Managers
{
public class PlayerPrefsManager : MonoBehaviour
{
    [SerializeField] private float defaultSensitivity = 0.05f;
    public float Sensitivity
    {
        get
        {
            if (!PlayerPrefs.HasKey("Sensitivity"))
            {
                Sensitivity = defaultSensitivity;
            }
            return PlayerPrefs.GetFloat("Sensitivity");
        }
        set => PlayerPrefs.SetFloat("Sensitivity", value);
    }
}
}
