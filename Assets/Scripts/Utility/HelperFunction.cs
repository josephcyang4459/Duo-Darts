#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __HELPER_FUNCTIONS {
    public static IEnumerator __delete_gameObject(GameObject g) {
        g.transform.parent = null;
        yield return new WaitForSeconds(.01f);//makes garbage but who cares
        Object.DestroyImmediate(g);
    }

    public static IEnumerator __delete_gameObject(Component g) {
        yield return new WaitForSeconds(.01f);//makes garbage but who cares
        Object.DestroyImmediate(g);
    }

    /// <summary>
    /// From all resources in project
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectName"></param>
    /// <returns></returns>
    public static Object FindObjectOfType<T>(string objectName) {
        Object[] temp = Resources.FindObjectsOfTypeAll(typeof(T));
        foreach (Object o in temp)
            if (o.name == objectName)
                return o;
        return 
            null;
    }

    /// <summary>
    /// From all resources in project
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectName"></param>
    /// <returns></returns>
    public static Object[] FindAllObjectsOfType<T>() {
        Object[] temp = Resources.FindObjectsOfTypeAll(typeof(T));
        return temp;
    }

    /// <summary>
    /// Get Object List from FindObjectsByType(typeof(MonoBehaviour), FindObjectsSortMode.None)
    /// </summary>
    /// <typeparam name="T">Interface or class that object can be casted to</typeparam>
    /// <param name="monoBehaviors"></param>
    /// <returns></returns>
    public static MonoBehaviour[] CollectMonoBehaviorsWithCast<T>(Object[] monoBehaviors) {
        List<MonoBehaviour> m = new();
        foreach(Object o in monoBehaviors) {
            if (typeof(T).IsAssignableFrom(o.GetType()))
                if(!((MonoBehaviour)o).gameObject.CompareTag("EditorOnly"))
                m.Add((MonoBehaviour)o);
        }
        return m.ToArray();
    }

    public static Vector3 __ClosestLocation(Vector3 origin) {
        const float DISTANCE = 15;
        float upD = -1;
        float downD = -1;

        //math to check if we are in something
        if (Physics.Raycast(origin + Vector3.down * DISTANCE * .95f, Vector3.up, out RaycastHit hitUp, DISTANCE)) {
            upD = Vector3.Distance(hitUp.point, origin);
        }
        //math to check if we are in something
        if (Physics.Raycast(origin + Vector3.up * DISTANCE * .95f, Vector3.down, out RaycastHit hitDown, DISTANCE)) {
            downD = Vector3.Distance(hitDown.point, origin);

        }
        //Debug.Log(upD + " " + downD);
        if (upD == -1 && downD == -1) {
            return origin;
        }

        if (upD == -1)
            return hitDown.point;
        if (downD == -1)
            return hitUp.point;

        return upD < downD ? hitUp.point : hitDown.point;
    }
}
#endif