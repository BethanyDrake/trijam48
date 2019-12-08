using UnityEngine;
using System.Collections;

public class PrefabReplacer : MonoBehaviour {

    public GameObject fixer;
    public GameObject[] prefabs = new GameObject[10];
    public string[] layers = new string[10];


    GameObject updatedObject;

	// Use this for initialization
	void Start () {
        updatedObject = new GameObject();
        updatedObject.name = fixer.name + "(Updated)";
        RemakeObject();
	
	}
    public void RemakeObject()
    {
        print("remaking level");
        Transform[] objects = fixer.GetComponentsInChildren<Transform>();

        foreach (Transform o in objects)
        {
            //print(o.name);
            foreach (GameObject prefab in prefabs)
            {   if (prefab == null) continue;
                if (o.name == prefab.name + "(Clone)")
                {
                    
                    Replace(o.gameObject, prefab);
                    return;

                }
            }
            //Replace(o.gameObject, o.gameObject);
        }
    }

    private ArrayList sections = new ArrayList();


    void Organise(GameObject o)
    {
        print("organising");
        if (GetSection(o.tag) == null)
        {
            AddSection(o.tag);
        }

        o.transform.parent = GetSection(o.tag).transform;
    }
    void AddSection(string tag)
    {
        GameObject s = new GameObject();
        s.transform.parent = updatedObject.transform;
        s.name = tag;
        sections.Add(s);
    }

    GameObject GetSection(string tag)
    {
        foreach(GameObject section in sections)
        {
            if (section.name == tag)
            {
                print("section found: " + section.name);
                return section;
            }
            else
            {
                print(section.name + "  " + tag);
            }
        }
        return null;
    }

    void Replace(GameObject original, GameObject replacementPrefab)
    {
        GameObject replacement = Instantiate(replacementPrefab);
        replacement.transform.position = original.transform.position;
        replacement.transform.rotation = original.transform.rotation;
        Organise(replacement);
        //Destroy(original);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
