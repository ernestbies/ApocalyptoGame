using UnityEngine;
using UnityEngine.UI;

public class HealthBarCamera : MonoBehaviour
{
    
    protected Vector3 position;
    public Image Bar;
    //[HideInInspector]
    public float fill;
    Transform player;

    // Use this for initialization
    void Start()
    {
        //localStartPosition = transform.localPosition;
        fill = 1.0f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.LookAt(player);        
    }

    // Update is called once per frame
    void Update()
    {        
        Bar.fillAmount = fill;
    }
}
