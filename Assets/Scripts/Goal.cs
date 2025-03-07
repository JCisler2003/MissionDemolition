using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Goal : MonoBehaviour
{

    static public bool goalMet = false;

    void OnTriggerEnter(Collider other){
        Projectile proj = other.GetComponent<Projectile>();
        if (proj != null){
            Goal.goalMet = true;

            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 0.75f;
            mat.color = c;

        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
