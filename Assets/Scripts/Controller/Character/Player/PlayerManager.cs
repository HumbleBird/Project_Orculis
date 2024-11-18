using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Ref")]
    InputHandler m_InputHandler;

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
        CastSpell_Orb();
    }

    public void CastSpell_Orb()
    {
        if(m_InputHandler.right_select)
        { 
            Orb orb = Instantiate(m_OrbPrefab, RightHandStickTransform.position, Quaternion.identity);
            orb.SetInfo(this, RightHandStickTransform);
            m_InputHandler.right_select = false;
        }
    }
}
