using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FmodCarSoundManager
{
    [EventRef]
    protected string fmodEvent;
    protected FMOD.Studio.EventInstance eventInstance;
    protected bool loops;
    public FmodCarSoundManager(string fmodEvent)
    {
        this.loops = false;
        this.fmodEvent = fmodEvent;
        CreateInstance();
    }
    public FmodCarSoundManager(string fmodEvent, bool loops)
    {
        this.loops = false;
        this.loops = loops;
        this.fmodEvent = fmodEvent;
        CreateInstance();
    }
    public FmodCarSoundManager(FmodCarSoundManager fmodCarSoundManager)
    {
        this.loops = fmodCarSoundManager.loops;
        this.fmodEvent = fmodCarSoundManager.fmodEvent;
        CreateInstance();
    }


    public string GetFmodEvent()
    {
        return fmodEvent;
    }
    public bool getLoops() => loops;

    protected void SetFmodEvent(string fmodEvent)
    {
        this.fmodEvent = fmodEvent;
    }

    private void CreateInstance()
    {
        if (!eventInstance.isValid())
        {
            eventInstance = RuntimeManager.CreateInstance(fmodEvent);
        }
    }

    public void StartEventSound()
    {
        if (!eventInstance.isValid())
        {
            CreateInstance();
        }
        eventInstance.start();
        
    }

    public void setSoundPlayPosition(Vector3 position)
    {
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));

        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D();
        eventInstance.get3DAttributes(out attributes);
        Vector3 pos = new Vector3(attributes.position.x, attributes.position.y, attributes.position.z);

    }
    public void PauseEventSound()
    {
        if (eventInstance.isValid() && IsEventPlaying())
        {
            eventInstance.setPaused(true);
        }
    }

    public void ResumeEventSound()
    {

        if (eventInstance.isValid() && IsEventPlaying())
        {
            eventInstance.setPaused(false);
        }
        
    }
    public void stopSound()
    {
        if (eventInstance.isValid())
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
    public bool IsEventPlaying()
    {
        if (eventInstance.isValid())
        {
            FMOD.Studio.PLAYBACK_STATE playbackState;
            eventInstance.getPlaybackState(out playbackState);
            return playbackState == FMOD.Studio.PLAYBACK_STATE.PLAYING;
        }
        return false;
    }
    public void EndSoundInstance()
    {
        if (eventInstance.isValid())
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }
    ~FmodCarSoundManager()
    {
        if (eventInstance.isValid())
        {
            eventInstance.release();
        }
    }
}
public class FmodCarSoundManagerSetParameters : FmodCarSoundManager
{
    private float MinParameterRange;
    private float MaxParameterRange;
    private string parameterName;

    public FmodCarSoundManagerSetParameters(string fmodEvent, string parameterName)
        : base(fmodEvent)
    {
        MinParameterRange = 0f;
        MaxParameterRange = 100f;
        this.parameterName = parameterName;
    }
    public FmodCarSoundManagerSetParameters(FmodCarSoundManager fmodCarSoundManager, string parameterName)
        : base(fmodCarSoundManager)
    {
        MinParameterRange = 0f;
        MaxParameterRange = 100f;
        this.parameterName = parameterName;
    }

    public FmodCarSoundManagerSetParameters(string fmodEvent, string parameterName, float min, float max)
        : base(fmodEvent)
    {
        MinParameterRange = min;
        MaxParameterRange = max;
        this.parameterName = parameterName;
    }
    public FmodCarSoundManagerSetParameters(FmodCarSoundManager fmodCarSoundManager, string parameterName, float min, float max)
        : base(fmodCarSoundManager)
    {
        MinParameterRange = min;
        MaxParameterRange = max;
        this.parameterName = parameterName;
    }
    public FmodCarSoundManagerSetParameters(FmodCarSoundManagerSetParameters fmodCarSoundManagerSetParameters)
        : base(fmodCarSoundManagerSetParameters)
    {
        MinParameterRange = fmodCarSoundManagerSetParameters.MinParameterRange;
        MaxParameterRange = fmodCarSoundManagerSetParameters.MaxParameterRange;
        this.parameterName = fmodCarSoundManagerSetParameters.parameterName;
    }


    public string GetParameterName()
    {
        return parameterName;
    }

    public void SetParameterName(string parameterName)
    {
        this.parameterName = parameterName;
    }

    public float GetMinParameterRange()
    {
        return MinParameterRange;
    }

    public float GetMaxParameterRange()
    {
        return MaxParameterRange;
    }

    protected void SetMinParameterRange(float min)
    {
        MinParameterRange = min;
    }

    protected void SetMaxParameterRange(float max)
    {
        MaxParameterRange = max;
    }

    public void SetDiscreteParameter(int value)
    {
        float discVal = (float)value;
        SetParameterValue(discVal);
    }

    public void SetContinuousValue(float value)
    {
        SetParameterValue(value);
    }

    public void SetLabeledParameter(string label)
    {
        if (eventInstance.isValid())
        {
            FMOD.RESULT result = eventInstance.setParameterByNameWithLabel(parameterName, label);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogError($"Failed to set labeled parameter {parameterName}: {result}");
            }
        }
    }

    private void SetParameterValue(float value)
    {
        value = Mathf.Clamp(value, MinParameterRange, MaxParameterRange);

        if (eventInstance.isValid())
        {
            FMOD.RESULT result = eventInstance.setParameterByName(parameterName, value);
            if (result != FMOD.RESULT.OK)
            {
                Debug.LogError($"Failed to set parameter {parameterName}: {result}");
            }
        }
    }

    public int GetDiscreteParameterValue()
    {
        float parameterValue;
        if (eventInstance.isValid())
        {
            FMOD.RESULT result = eventInstance.getParameterByName(parameterName, out parameterValue);
            if (result == FMOD.RESULT.OK)
            {
                return (int)parameterValue;
            }
        }
        return 0;
    }

    public float GetContinuousParameterValue()
    {
        float parameterValue = 0f;
        if (eventInstance.isValid())
        {
            FMOD.RESULT result = eventInstance.getParameterByName(parameterName, out parameterValue);
            if (result != FMOD.RESULT.OK)
            {
            }
        }
        return parameterValue;
    }
}
public class FmodEvent{
    public string fmodEvent;
    public FmodEvent(string fmodEvent){
        this.fmodEvent = fmodEvent;
    }
    public FmodEvent(FmodEvent fEvent){
        this.fmodEvent = fEvent.fmodEvent;
    }
}
public class Parameter: FmodEvent{
    public string parameterName;

    public Parameter(string fmodEvent, string parameterName) : base (fmodEvent){
        this.parameterName = parameterName;
    }
    public Parameter(FmodEvent fEvent, string parameterName) : base(fEvent){
        this.parameterName = parameterName;
    }
    public Parameter(Parameter parameter) : base(parameter.fmodEvent){
        this.parameterName = parameter.parameterName;
    }
    
}
public class DiscreteParameter: Parameter {
    public int minParameter;
    public int maxParameter;
    public DiscreteParameter(string fEvent, string parameterName, int min, int max) : base (fEvent,parameterName){
        this.minParameter = min;
        this.maxParameter = max;
    }
    public DiscreteParameter(FmodEvent fEvent, string parameterName, int min, int max) : base (fEvent,parameterName){
        this.minParameter = min;
        this.maxParameter = max;
        
    }
    public DiscreteParameter(Parameter parameter, int min, int max) : base(parameter){
        this.minParameter = min;
        this.maxParameter = max;
        
    }
    public DiscreteParameter(DiscreteParameter parameter) : base(parameter){
        this.minParameter = parameter.minParameter;
        this.maxParameter = parameter.maxParameter;
        
    }
}
public class ContinuousParameter: Parameter {
    public float minParameter;
    public float maxParameter;
    public ContinuousParameter(string fEvent, string parameterName, float min, float max) : base (fEvent,parameterName){
        this.minParameter = min;
        this.maxParameter = max;
    }
    public ContinuousParameter(FmodEvent fEvent, string parameterName, float min, float max) : base (fEvent,parameterName){
        this.minParameter = min;
        this.maxParameter = max;
        
    }
    public ContinuousParameter(Parameter parameter, float min, float max) : base(parameter){
        this.minParameter = min;
        this.maxParameter = max;
        
    }
    public ContinuousParameter(ContinuousParameter parameter) : base(parameter){
        this.minParameter = parameter.minParameter;
        this.maxParameter = parameter.maxParameter;
        
    }
}
public class LabelledParameter: Parameter {
    public string Label;
    public LabelledParameter(string fEvent, string parameterName, string label) : base (fEvent,parameterName){
        this.Label = label;       
    }
    public LabelledParameter(FmodEvent fEvent, string parameterName, string label) : base (fEvent,parameterName){
        this.Label = label;               
    }
    public LabelledParameter(Parameter parameter, string label) : base(parameter){
        this.Label = label;       
    }
    public LabelledParameter(LabelledParameter parameter) : base(parameter){
        this.Label = parameter.Label;
        
    }

    
}

public class FmodCarSoundManagerSetGlobalParameters
{
    public FmodCarSoundManagerSetGlobalParameters()
    {

    }
    public void SetGlobalParameter(string parameterName, float value)
    {
        RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
    }
}