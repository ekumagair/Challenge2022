using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particula : MonoBehaviour
{
    ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject criarNoImpacto;
    public string criarNoImpactoTag;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (criarNoImpacto != null && other.gameObject.tag == criarNoImpactoTag)
        {
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

            int i = 0;

            while (i < numCollisionEvents)
            {
                //Instantiate(criarNoImpacto, collisionEvents[i].intersection - (transform.up / 50), new Quaternion(transform.rotation.x, Random.Range(0, 359), transform.rotation.z, transform.rotation.w));
                CriarDecalque(i, other);
                i++;
            }
        }
    }

    void CriarDecalque(int colEvent, GameObject attachTo)
    {
        //Debug.DrawRay(hitLocation, hitNormal, Color.blue, 99999999f);
        GameObject impactDecal = Instantiate(criarNoImpacto, collisionEvents[colEvent].intersection, Quaternion.LookRotation(collisionEvents[colEvent].normal));
        impactDecal.transform.Rotate(90, 0, 0);

        //Vector3 scale = new Vector3(0.15f, 0.15f, 0.15f);
        //impactDecal.transform.localScale = scale;

        if (attachTo != null)
        {
            impactDecal.transform.SetParent(attachTo.transform);
        }

        impactDecal.GetComponent<Renderer>().enabled = true;
        impactDecal.GetComponent<Renderer>().receiveShadows = true;
    }
}