using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * Author: Pantelis Andrianakis
 * Date: December 24th 2018
 */
public class TabBetweenInputFields : MonoBehaviour
{
    public Selectable[] selectables;
    public int startIndex = 0;

    private void Start()
    {
        ApplyEnterSelect(selectables[startIndex]);
    }

    private void Update()
    {
        if (InputManager.TAB_DOWN)
        {
            startIndex++;

            if (startIndex >= selectables.Length)
            {
                startIndex = 0;
            }

            if (selectables[startIndex] != null)
            {
                selectables[startIndex].Select();
            }
        }

        if (InputManager.RETURN_DOWN)
        {
            startIndex = 2; // Login button.
            ApplyEnterSelect(selectables[startIndex]);
        }
    }

    private void ApplyEnterSelect(Selectable selectable)
    {
        if (selectable != null)
        {
            if (selectable.GetComponent<TMP_InputField>() != null)
            {
                selectable.Select();
            }
            else
            {
                Button selectedButton = selectable.GetComponent<Button>();
                if (selectedButton != null)
                {
                    selectable.Select();
                    selectedButton.OnPointerClick(new PointerEventData(EventSystem.current));
                }
            }
        }
    }
}
