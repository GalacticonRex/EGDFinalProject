using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public PathInstance path;
    public float speed;
    private float total;
    public int proficiency;
    public SpriteRenderer sprite;
    public const string LAYER_NAME = "Character";
    public bool alive;
    public float hunger;
    public float thirst;
    public float happiness;
    void Start()
    {
        alive = true;
        hunger = 100f;
        thirst = 100f;
        happiness = 80f;
        sprite = GetComponent<SpriteRenderer>();
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sprites)
        {
            if (s)
            {
                s.sortingOrder = 0;
                s.sortingLayerName = LAYER_NAME;

            }
        }

        StartCoroutine("checkHunger");
        if (speed < 0)
            total = path.Length;
        else
            total = 0.0f;
    }
    void calcProficiency()
    {
        //Consider proficiency to be related to HUNGRY + THIRST + HAPPINESS
        float averageStats = (hunger + thirst + happiness)/3;
        if (averageStats >= 0)
        {
            proficiency = 1;
        }
        if (averageStats >= 20)
        {
            proficiency = 2;
        }
        if (averageStats >= 40)
        {
            proficiency = 3;
        }
        if (averageStats >= 60)
        {
            proficiency = 4;
        }
        if (averageStats >= 80)
        {
            proficiency = 5;
        }
    }
    void Update()
    {
 
        total += speed * Time.deltaTime;
        if (path != null) { 
        Vector3 pt = path.GetPositionAlongPath(total);
        Vector3 move = pt - transform.position;
        if ( Mathf.Abs(move.x) > 0.01f )
        {
            if (move.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }

        if (pt == path.end || pt == path.start)
            Destroy(gameObject);
        transform.position = pt;
            }
    }
    IEnumerator checkHunger()
    {
        while (alive)
        {
            if (hunger <= 0 || thirst <= 0)
            {
                alive = !alive;
                Destroy(gameObject);
                Globals.currentPopulation -= 1;
            }
            if (Globals.resources[Globals.resourceTypes.FOOD] < Globals.currentPopulation)
            {
                hunger -= 1;
            }
            if (Globals.resources[Globals.resourceTypes.WATER] < Globals.currentPopulation)
            {
                thirst -= 1;
            }
            calcProficiency();
            yield return new WaitForSeconds(10f);
        }
    }
}
