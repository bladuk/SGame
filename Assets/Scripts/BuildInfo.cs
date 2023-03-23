using UnityEngine;

[ExecuteInEditMode]
public class BuildInfo : MonoBehaviour
{
    public static BuildInfo Singleton;
    [Header("General information")]
    [SerializeField] internal bool IsBetaBuild;
    [SerializeField] internal bool IsDevelopmentBuild;
    [SerializeField] internal bool IsDebugMode;
    [SerializeField] internal string BuildHash;

    private void Awake()
    {
        Singleton = this;
    }

    private void Update()
    {
        if (IsDevelopmentBuild && IsBetaBuild)
        {
            IsBetaBuild = false;
            Debug.LogWarning("Only one setting can be selected.");
        }
    }
}
