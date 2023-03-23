using UnityEngine;

// Work in progress
public class EditorScript : MonoBehaviour
{
    public static EditorScript Singleton;

    private void Awake()
    {
        Singleton = this;
    }
}
