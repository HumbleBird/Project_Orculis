using Fusion;
using Fusion.XR.Host.Rig;
using Oculus.Voice;
using SimpleFPS;
using UnityEngine;

/// <summary>
/// Singleton on Runner used to obtain scene object references using lazy getters.
/// </summary>
public class SceneObjects : SimulationBehaviour
{
    // Use Runner.GetSingleton<SceneObjects>() to get SceneObjects instance.

    private AppVoiceExperience appVoiceExperience;
    public AppVoiceExperience m_AppVoiceExperience { get { return FindComponentInScene(ref appVoiceExperience); } }

    private Gameplay _gameplay;
    public Gameplay Gameplay { get { return FindComponentInScene(ref _gameplay); } }

    private Mivry _mivry;
    public Mivry m_Mivry { get { return FindComponentInScene(ref _mivry); } }

    private HardwareRig _rig;
    public HardwareRig m_Rig { get { return FindComponentInScene(ref _rig); } }

    private T FindComponentInScene<T>(ref T cachedComponent) where T : Component
    {
        if (cachedComponent == null && Runner != null && Runner.SceneManager != null && Runner.SceneManager.MainRunnerScene.IsValid())
        {
            var components = Runner.SceneManager.MainRunnerScene.GetComponents<T>(true);
            if (components.Length > 0)
            {
                cachedComponent = components[0];
            }
        }
        return cachedComponent;
    }

}
