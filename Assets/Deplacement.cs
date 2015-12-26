using UnityEngine;
using System.Collections;

public class Deplacement : MonoBehaviour {

    public float speed = 0;
    public float speedMax = 10;

    public float pas = 0.1f;

    public GameObject obj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.Q))
        {
            if (speed < speedMax)
                speed += 2*pas;         //2* pour que cela freine plus vite quand on appuie sur la touche inverse de la direction actuelle.
        }
        else if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.D))
        {
            if (speed > -speedMax)
                speed -= 2*pas;
        }
        else
        {
            if (speed > pas)
                speed -= pas;
            else if (speed < -pas)
                speed += pas;
            else
                speed = 0;
        }

        if (speed != 0)
        {
            transform.position = transform.position + (Vector3.forward * speed * Time.deltaTime);
            //deplaceCam();
        }
    }

    /*void deplaceCam()
    {
        float a = (speed / speedMax);
        cam.transform.localPosition = obj.transform.localPosition + new Vector3(10, 5, 0) + new Vector3((Mathf.Abs(a)*10),0,a*10);
    }*/

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(this.transform.position, Vector3.forward * 10);
    }
}
