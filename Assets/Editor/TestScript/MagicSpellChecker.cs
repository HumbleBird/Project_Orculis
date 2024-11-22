using Meta.XR.ImmersiveDebugger.UserInterface;
using System;
using System.Linq;
using UnityEngine;

[ExecuteAlways] // 에디터와 실행 모드 모두에서 실행
public class MagicSpellChecker : MonoBehaviour
{
    [ContextMenu("Test CheckMagicSpells")]
    public void CheckMagicSpells(string[] vars)
    {
        // 유효한 값만 추출
        var validValues = vars
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToArray();

        // 비었는지 확인
        if (validValues.Any() == false)
            return;

        // 첫 번째 단어 주문
        string spellString = validValues[0];

        // 동적 메서드 호출 (Reflection 활용)
        try
        {
            var methodName = $"CastSpell_{spellString}";
            var method = GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (method != null)
            {
                method.Invoke(this, null);
            }
            else
            {
                Debug.LogWarning($"알 수 없는 주문: {spellString}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"주문 처리 중 오류 발생: {ex.Message}");
        }
    }

    private void CastSpell_Incendio()
    {
        Debug.Log("Incendio 주문 시전!");
    }

    private void CastSpell_Accio()
    {
        Debug.Log("Accio 주문 시전!");
    }

    private void CastSpell_Repellcio()
    {
        Debug.Log("Repellcio 주문 시전!");
    }
}
