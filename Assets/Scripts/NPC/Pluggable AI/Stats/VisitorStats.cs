using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Stats/VisitorStats")]
public class VisitorStats : ScriptableObject
{
    public GameObject speciesPrefab;
    //public string visitorName;
    //public string activites;
    public float lookSphere = 10f, turnSpeed = 25f; // stroll stats
    public float minStateTime = 30f, maxStateTime = 120f;
    public int minWait, maxWait;
    public float waitTime, newStateTime;
    

}
