using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace menus
{
    public class MenuNavigation : MonoBehaviour
    {
        public GameObject defaultSelectedButtonGob;
        
        private void Start()
        {
            EventSystem.current.SetSelectedGameObject(defaultSelectedButtonGob);
        }

        private void Update()
        {
            if (Gamepad.current != null && EventSystem.current.currentSelectedGameObject == null && Gamepad.current.wasUpdatedThisFrame)
            {
                EventSystem.current.SetSelectedGameObject(defaultSelectedButtonGob);
            }
        }
    }
}
