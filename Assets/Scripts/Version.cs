using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class Version : MonoBehaviour
{
    private void Start()
    {
        var component = gameObject.GetComponent<TMP_Text>();
        var version = Application.version;
#if UNITY_EDITOR
        version += "-EDITOR";
#else
        version += "-" + BuildInfo.Singleton.BuildHash;
#endif
        component.text = component.text.Replace("%version%", version);
    }
}
