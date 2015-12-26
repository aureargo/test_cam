﻿using UnityEngine;
using System.Collections;

/**déplacement d'une caméra sur un objet se déplaçant sur l'axe Z*/
public class DepCamera : MonoBehaviour {

    public GameObject obj;

    public Vector3 vue = new Vector3(10, 5,0);
    public float zAvantMult = 128, xReculMult = 64;
    public float speedCam = 1.0f;
    private Vector3 ancienPos;
    private float distZ = 0, distAncien = 0;
    private bool dir = true;
    
    private Vector3 posVoulu;

	void Start()
    {
        ancienPos = obj.transform.position;
        posVoulu = ancienPos + vue;
        this.transform.position = posVoulu;
    }
	// Update is called once per frame
	void Update ()
    {
        if (ancienPos != obj.transform.position)
        {
            Vector3 v = obj.transform.position - ancienPos; //calcul du vecteur de déplacement de l'objet suivit, sachant que les Updates des différents scripts de la scene ne sont pas ordonnée comme on veut.
            if (v.z < 0)                dir = false;
            else if (v.z > 0)           dir = true;
            //else if(v.z == 0) ne rien faire; sans ça, la caméra fait des acoup car les update de l'objet et de la caméra ne sont pas appelé successivement l'un après l'autre et v.z est parfois à 0 en plein déplacement. //n'est plus trop utile depuis que dire n'est plus qu'utilisé qu'une fois.

            distZ = Vector3.SqrMagnitude(v);
            if (dir == false)
                distZ = -distZ;

            distZ += distAncien * 99;
            distZ /= 100;   //évite les effets de vibration déplaisant avec un déplacement progressif.

            posVoulu = vue + obj.transform.position;
            posVoulu.x += Mathf.Abs(distZ) * xReculMult;    posVoulu.z += distZ * zAvantMult;  //demande de placer la caméra pas directement sur l'objet mais un peu en avant et avec un peu de recul en fonction de la vitesse.


            this.transform.position = Vector3.Lerp(this.transform.position, posVoulu, speedCam * Time.deltaTime);   //on déplace la caméra progressivement

            Vector3 at = obj.transform.position;
            at.z += distZ * zAvantMult; //on dirige la caméra un peu en avant vers la direction où l'objet va. 
            this.transform.LookAt(at);


            ancienPos = obj.transform.position;
            distAncien = distZ;
        }
        else if (distZ != 0 || posVoulu != this.transform.position) //si l'objet ne bouge plus, on déplace quand même la caméra progressivement vers la destination voulue
        {
            print(distZ + " / " + Vector3.SqrMagnitude(this.transform.position - posVoulu));
            if (Vector3.SqrMagnitude(this.transform.position - posVoulu) < 0.0001f)
                this.transform.position = posVoulu;
            else
                this.transform.position = Vector3.Lerp(this.transform.position, posVoulu, speedCam * Time.deltaTime);

            if (Mathf.Abs(distZ) < 0.001f)
            {
                distZ = 0;
                this.transform.LookAt(obj.transform.position);
            }
            else
            {
                distZ *= 0.99f;
                Vector3 at = obj.transform.position;
                at.z += distZ* zAvantMult;
                this.transform.LookAt(at);
            }
            distAncien = distZ;
        }
	}
}

