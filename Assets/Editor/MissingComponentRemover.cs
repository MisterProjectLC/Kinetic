using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class MissingComponentRemover : MonoBehaviour
{
    public Transform[] mTargets;


    public void Clean()
    {
        for (int i = 0; i < mTargets.Length; i++)
        {
            Clean(mTargets[i]);

        }
    }

    public void Clean(Transform obj)
    {
        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj.gameObject);

        //Debug.Log(obj.childCount);
        for (int i = 0; i < obj.childCount; i++)
        {
            Clean(obj.GetChild(i));

        }

    }
}
#endif