using Oculus.Voice;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Ref")]
    InputHandler m_InputHandler;
    public AppVoiceExperience voiceExperience;

    [Header("Magic")]
    [SerializeField] private Transform RightHandStickTransform;

    [SerializeField] private Orb m_OrbPrefab;

    private void Awake()
    {
        m_InputHandler = GetComponent<InputHandler>();
    }

    void Start()
    {
    }

    void Update()
    {
        voiceExperience.Activate();
    }

    public void CheckMagicSpells(string[] vars)
    {
        string spellString = vars[0];

        if (string.IsNullOrEmpty(spellString)) return;

        print(spellString);

        if (spellString == "Incendio")
            CastSpell_Orb();
        else if (spellString == "Accio")
            CastSpell_Accio();
    }
        

    public void CastSpell_Orb()
    {
        Orb orb = Instantiate(m_OrbPrefab, RightHandStickTransform.position, Quaternion.identity);
        orb.SetInfo(this, RightHandStickTransform);
        m_InputHandler.right_select = false;
    }

    public void CastSpell_Accio()
    {

    }
}
