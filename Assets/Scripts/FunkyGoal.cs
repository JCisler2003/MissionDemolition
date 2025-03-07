using UnityEngine;
using UnityEngine.Video;


[RequireComponent(typeof(Renderer))]
public class FunkyGoal : MonoBehaviour
{

    static public bool goalMet = false;
    VideoPlayer video;

    void Awake()
    {
      video = GetComponent<VideoPlayer>();   
    }

    void OnDisable()
    {
        if (video != null)
        {
            video.Stop();
        }
    }

    void OnTriggerEnter(Collider other){
        Projectile proj = other.GetComponent<Projectile>();
        if (proj != null){
           // Goal.goalMet = true;

            //Material mat = GetComponent<Renderer>().material;
            video.Play();
    

        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
