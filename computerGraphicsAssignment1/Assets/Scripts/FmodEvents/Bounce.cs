using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSounds
{
   public FmodCarSoundManagerSetParameters bounceSound;
   public FmodCarSoundManagerSetParameters rollSound;
   public BallSounds()
   {
        bounceSound = new FmodCarSoundManagerSetParameters("event:/Bounce", "BounceIntensity",0,1);
        rollSound = new FmodCarSoundManagerSetParameters("event:/Roll", "RollSpeed", 0,1);
   }

   public void ManageBounceIntensity(float force, Vector3 pos)
   {
        float maxForce = 10;
        if (force > maxForce)
        {
            force = maxForce;
        }
        force = force / maxForce;
        force = Mathf.Clamp01(force);
        bounceSound.setSoundPlayPosition(pos);
        bounceSound.SetContinuousValue(force);

   }
    public void ManageRollSpeedSound(float speed, Vector3 pos)
    {
        float maxSpeed = 8;
        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
        speed = speed / maxSpeed;
        speed = Mathf.Clamp01(speed);
        Debug.Log(speed);
        rollSound.setSoundPlayPosition(pos);
        rollSound.SetContinuousValue(speed);

    }
}
