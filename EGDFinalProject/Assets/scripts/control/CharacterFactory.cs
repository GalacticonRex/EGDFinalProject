using UnityEngine;
using System.Collections;

public class CharacterFactory : MonoBehaviour {
    public GameObject[] Proficiency = new GameObject[5];
    public GameObject ToGenerate;

    public void Start()
    {
        Character ch = ToGenerate.GetComponent<Character>();
        if (ch == null)
            Debug.LogError("ToGenerate in character controller does not have Character component");
    }
 
    public Character Create(int prof, BuildingInstance build, PathInstance path)
    {
        /*
    if (prof < 1 || prof > 5)
    {
        Debug.LogError("Invalid proficiency value of " + prof.ToString());
        return null;
    }

    GameObject go = Instantiate(ToGenerate);
    GameObject child = Instantiate(Proficiency[prof - 1]);
    child.transform.parent = go.transform;
    child.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    go.transform.position = build.transform.position;

    Character ch = go.GetComponent<Character>();
    ch.path = path;
    ch.speed = path.GetDirectionFromSource(build);

    return ch;*/
        return null;
    }
}
