using TMPro;
using UnityEngine;

public class GetValueFromDropdown : MonoBehaviour
{
    [SerializeField] private NavController navController;
    [SerializeField] private TMP_Dropdown dropdown;

    private int lastValue = -1;

    private void Update()
    {
        int currentValue = dropdown.value;
        if (currentValue != lastValue)
        {
            lastValue = currentValue;
            OnDropdownValueChanged();
        }
    }

    public int GetDropdownValue()
    {
        return dropdown.value;
    }

    private void OnDropdownValueChanged()
    {
        navController.StopNavigation(); // Stop any current navigation
        navController.SetActiveDestination(dropdown.value); // Set new destination
        navController.InitNav(); // Restart navigation with the new selection
    }
}
