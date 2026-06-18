using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SF.UIModule
{
    using Damageable;
    
    public class PlayerHUDDemo : MonoBehaviour
    {
        [SerializeField] private PanelRenderer _panelRenderer;
        
        [SerializeField] private VisualElementReference<ProgressBar> _healthBarRef = new VisualElementReference<ProgressBar>();
        [SerializeField] private AuthoringIdPath _healthBarID;
        private ProgressBar _healthBar;
        
        [SerializeField] private VisualElementReference<Button> _increaseButtonRef = new VisualElementReference<Button>();
        private readonly AuthoringIdPath _increaseButtonID = new AuthoringIdPath(2);
        private Button _increaseButton;
        
        private readonly VisualElementReference<Button> _decreaseButtonRef = new VisualElementReference<Button>();
        private readonly AuthoringIdPath _decreaseButtonID = new AuthoringIdPath(3);
        private Button _decreaseButton;
        
        [Header("Player Health")]
        [SerializeField] private HealthBase _playerHealth;
        [SerializeField] private int _healthChangeAmount;

        private void Start()
        {
            if (_panelRenderer == null)
                return;
            
            /* You can use a null-coalescing operator to guarantee the VisualElementReference is never null before using it.
             *  _healthBarRef ??= new VisualElementReference<ProgressBar>(); */ 
            
            _healthBarRef?.SetReference(_panelRenderer,_healthBarID);
            _healthBarRef?.RegisterReferenceResolvedCallback(OnHealthBarResolved);
            
            _increaseButtonRef?.SetReference(_panelRenderer,_increaseButtonID);
            _increaseButtonRef?.RegisterReferenceResolvedCallback(OnIncreaseButtonResolved);
            _increaseButtonRef?.RegisterReferenceUnloadedCallback(OnIncreaseButtonUnloaded);
            
            _decreaseButtonRef?.SetReference(_panelRenderer,_decreaseButtonID);
            _decreaseButtonRef?.RegisterReferenceResolvedCallback(OnDecreaseButtonResolved);
            _decreaseButtonRef?.RegisterReferenceUnloadedCallback(OnDecreaseButtonUnloaded);
        }
        

        private void OnDecreaseButtonResolved(Button decreaseButton)
        {
            decreaseButton.clicked        += OnDecreaseHealthClicked;
            decreaseButton.style.fontSize =  24;
        }
        
        private void OnDecreaseButtonUnloaded(Button decreaseButton)
        {
            decreaseButton.clicked -= OnDecreaseHealthClicked;
        }
        
        private void OnIncreaseButtonResolved(Button increaseButton)
        {
            increaseButton.clicked        += OnIncreaseHealthClicked;
            increaseButton.style.fontSize =  24;
        }
        
        private void OnIncreaseButtonUnloaded(Button increaseButton)
        {
            increaseButton.clicked -= OnIncreaseHealthClicked;
        }
        
        private void OnHealthBarResolved(ProgressBar healthBar)
        {
            if (_playerHealth == null) 
                return;

            _healthBar          = healthBar;
            _healthBar.lowValue  = 0;
            _healthBar.highValue = _playerHealth.MaxHealth;
            _healthBar.RegisterValueChangedCallback(OnHealthChanged);
            _healthBar.dataSource = _playerHealth;
        }

        private void OnHealthChanged(ChangeEvent<float> evt)
        {
            // Should almost be impossible, but better to check.
            if (_healthBar == null) return;
            _healthBar.title = $"{_playerHealth.CurrentHealth} / {_playerHealth.MaxHealth}";
        }

        private void OnDecreaseHealthClicked()
        {
            if (_playerHealth == null)
                return;
            
            _playerHealth.TakeDamage(_healthChangeAmount);
        }
        
        private void OnIncreaseHealthClicked()
        {
            if (_playerHealth == null)
                return;
            
            _playerHealth.RestoreHealth(_healthChangeAmount);
        }
        
        
        /*
/// <param name="panelRenderer"></param>
/// <param name="rootElement"></param>
/// <param name="version"></param>
private void OnUIReloadedCallback(PanelRenderer panelRenderer, VisualElement rootElement, int version)
{

    // We do the reference settings in the OnUIReloadedCallback to make sure the Panel is actually ready to have it's elements used.

}*/

    }
}
