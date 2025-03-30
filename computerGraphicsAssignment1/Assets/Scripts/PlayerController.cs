using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float force = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private ParticleSystem explosion;

    bool grounded;
    bool wasExploding;
    bool wasRolling;
    bool wasBouncing;
    bool justStarted;

    Explosion explosionSound;
    BackgroundMusic music;
    BallSounds[] bounce = new BallSounds[3];
    BallSounds roll;
    Camera cam;
    Rigidbody rb;
    Vector3 movementDir;
    float currentTransitionMusic;
    float t;
    [SerializeField] PauseMenu pause;
    bool lastPausedState;
    private void Start()
    {
        cam = Camera.main;
        explosionSound = new Explosion();
        rb = GetComponent<Rigidbody>();
        grounded = false;
        bounce = new BallSounds[3];
        roll = new BallSounds();
        music = new BackgroundMusic();
        justStarted = true;
        wasBouncing = false;
        wasRolling = false;
        wasExploding = false;
        currentTransitionMusic = 0;
        t = 0;
        music.StrollMusic.StartEventSound();
        music.ManageMusic(0);

    }

    private void Update()
    {
        if (explosionSound == null)
        {
            explosionSound = new Explosion();
        }
        if (bounce == null)
        {
            bounce = new BallSounds[3];
        }
        if (roll == null)
        {
            roll = new BallSounds();
        }
        if(music == null)
        {
            music = new BackgroundMusic();
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float distanceToTarget = Vector3.Distance(transform.position, GameObject.FindWithTag("Push").transform.position);
        if (!pause.paused)
        {
            movementDir = (cam.transform.right * horizontalInput) + (cam.transform.forward * verticalInput);
            movementDir.y = 0f;

            if (grounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                    //jump sound instance
                }
            }
            movementDir.Normalize();
            roll.ManageRollSpeedSound(rb.angularVelocity.magnitude, transform.position);

            if (lastPausedState != pause.paused)
            {
                if (wasExploding)
                {
                    if (!explosion.isPlaying)
                    {
                        explosion.Play();
                        explosionSound.explode.ResumeEventSound();
                        wasExploding = false;
                    }

                }
                if (wasBouncing)
                {
                    for(int i = 0; i<bounce.Length; i++)
                    {
                        if (bounce[i] == null)
                        {
                            bounce[i] = new BallSounds();
                        }
                        else
                        {
                            bounce[i].bounceSound.ResumeEventSound();
                        }
                    }
                    wasBouncing = false;
                }
                if (wasRolling)
                {
                    roll.rollSound.ResumeEventSound();
                    wasRolling = false;
                }
                music.BattleMusic.ResumeEventSound();
                music.StrollMusic.ResumeEventSound();
            }
            if(distanceToTarget <= 4f)
            {
                if (t < 1)
                {
                    if (!music.BattleMusic.IsEventPlaying())
                    {
                        music.BattleMusic.StartEventSound();
                    }
                    t += Time.deltaTime;
                }
            }
            else
            {
                if (t > 0)
                {
                    if (t < 0.5f)
                    {
                        if (music.BattleMusic.IsEventPlaying())
                        {
                            music.BattleMusic.stopSound();
                        }
                    }
                    t -= Time.deltaTime;
                }
            }
            t = Mathf.Clamp01(t);
            music.ManageMusic(t);
            rb.AddForce(movementDir * speed * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            if (explosion.isPlaying)
            {
                explosion.Pause();
                wasExploding = true;
            }

            managePausedSounds();
        }
        lastPausedState = pause.paused;
        if (justStarted)
        {
            justStarted = false;
        }
    }
    void managePausedSounds()
    {
        if (explosionSound.explode.IsEventPlaying())
        {
            explosionSound.explode.PauseEventSound();
        }
        if (roll.rollSound.IsEventPlaying())
        {
            roll.rollSound.PauseEventSound();
        }
        for (int i = 0; i < bounce.Length; i++)
        {
            if (bounce[i] == null)
            {
                bounce[i] = new BallSounds();
            }

            if (bounce[i].bounceSound.IsEventPlaying())
            {
                bounce[i].bounceSound.PauseEventSound();
            }
        }
        if (music.BattleMusic.IsEventPlaying())
        {
            music.BattleMusic.PauseEventSound();
        }
        if (music.StrollMusic.IsEventPlaying())
        {
            music.StrollMusic.PauseEventSound();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if ("Push" == collision.gameObject.tag)
        {
            if (collision.gameObject.GetComponent<Rigidbody>() != null)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(movementDir * force, ForceMode.Impulse);
                collision.gameObject.GetComponent<Rigidbody>().AddRelativeTorque(movementDir * force, ForceMode.Impulse);
                explosion.transform.position = collision.contacts[0].point;
                explosion.Play();
                if (!explosionSound.explode.IsEventPlaying())
                {
                    explosionSound.explode.setSoundPlayPosition(collision.contacts[0].point);
                    explosionSound.explode.StartEventSound();
                }
                else
                {
                    Explosion newEx = new Explosion();
                    if (!newEx.explode.IsEventPlaying())
                    {
                        newEx.explode.setSoundPlayPosition(collision.contacts[0].point);
                        newEx.explode.StartEventSound();
                    }
                }
                for (int i = 0; i < collision.contacts.Length; i++)
                {
                    if ((collision.contacts[i].point - transform.position).y < 0)
                    {
                        grounded = true;
                    }
                }
                //play explosion sound
                //instance rolling sound

            }

        }
        if ("Floor" == collision.gameObject.tag)
        {
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                if ((collision.contacts[i].point - transform.position).y < 0)
                {
                    grounded = true;
                }
                Vector3 impulse = collision.impulse;
                Vector3 force = impulse / Time.fixedDeltaTime;
                if (!justStarted)
                {
                    int j = 0;
                    while (j < bounce.Length)
                    {
                        if (bounce[j] == null)
                        {
                            bounce[j] = new BallSounds();
                        }
                        else
                        {
                            bounce[j].ManageBounceIntensity(force.magnitude, collision.contacts[0].point);

                            if (!bounce[j].bounceSound.IsEventPlaying())
                            {
                                bounce[j].bounceSound.StartEventSound();
                                wasBouncing = true;
                                break;
                            }
                            j++;
                        }
                    }
                }
            }
        }


    }
    void OnCollisionStay(Collision collision)
    {
        if ("Push" == collision.gameObject.tag)
        {
            if (collision.gameObject.GetComponent<Rigidbody>() != null)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(movementDir * force, ForceMode.Force);
            }
        }
        if ("Floor" == collision.gameObject.tag)
        {
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                if ((collision.contacts[i].point - transform.position).y < 0)
                {
                    grounded = true;
                }
                if (!pause.paused)
                {
                    Debug.Log(collision.contacts[0].point);

                    if (!roll.rollSound.IsEventPlaying())
                    {
                        roll.rollSound.StartEventSound();

                    }
                    wasRolling = true;

                }

                //instanced bounce sound
            }
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if ("Push" == collision.gameObject.tag)
        {
            grounded = false;
            //instance rolling sound
        }
        else if ("Floor" == collision.gameObject.tag)
        {
            grounded = false;

            roll.rollSound.stopSound();

            //instance rolling sound

        }

    }
    private void OnDestroy()
    {
        explosionSound.explode.EndSoundInstance();
        roll.rollSound.EndSoundInstance();
        for (int i = 0; i < bounce.Length; i++)
        {
            if (bounce[i] == null)
            {
                bounce[i] = new BallSounds();
            }
            bounce[i].bounceSound.EndSoundInstance();
            music.BattleMusic.EndSoundInstance();
            music.StrollMusic.EndSoundInstance();
        }
    }
    private void OnDisable()
    {
        explosionSound.explode.EndSoundInstance();
        roll.rollSound.EndSoundInstance();
        for (int i = 0; i < bounce.Length; i++)
        {
            if (bounce[i] == null)
            {
                bounce[i] = new BallSounds();
            }
            bounce[i].bounceSound.EndSoundInstance();
        }
        music.BattleMusic.EndSoundInstance();
        music.StrollMusic.EndSoundInstance();
    }
}
