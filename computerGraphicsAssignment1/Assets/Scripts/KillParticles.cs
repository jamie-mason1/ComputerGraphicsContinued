using UnityEngine;
[ExecuteInEditMode]
public class ParticleKillOnContact : MonoBehaviour
{
    private ParticleSystem particleSystem;
    Popping[] pops = new Popping[26];
    //snap crackle sound instance


    void Start()
    {
        // Get the particle system attached to the object
        particleSystem = GetComponent<ParticleSystem>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("box"))
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            for(int i = 0; i<pops.Length; i++)
            {
                if (pops[i] == null)
                {
                    pops[i] = new Popping();
                }
                else
                {
                    if (!pops[i].pop.IsEventPlaying())
                    {
                        pops[i].pop.setSoundPlayPosition(collision.contacts[0].point);
                        pops[i].pop.StartEventSound();
                        break;
                    }
                    else
                    {
                        if (i < pops.Length-1)
                        {
                            i++;
                        }
                        pops[i].pop.setSoundPlayPosition(collision.contacts[0].point);
                        pops[i].pop.StartEventSound();
                        break;

                    }
                }
            }
            
            //play sound event
        }
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("box"))
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        }

        
    }
    
}
