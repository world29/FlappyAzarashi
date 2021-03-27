// GENERATED AUTOMATICALLY FROM 'Assets/System/PlayerAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerAction : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerAction"",
    ""maps"": [
        {
            ""name"": ""PlatformAction"",
            ""id"": ""be848f74-60cc-43fd-a2ab-ed7b4c484515"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Value"",
                    ""id"": ""c5b37822-c472-482e-b4e7-07c1b229515c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Value"",
                    ""id"": ""4e552261-5a37-432c-bd27-cc86cf36e5a2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""729061f2-ebfe-46d0-b9c7-80d35129d0f3"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""02f8fd54-698b-48fd-bc43-c9b6564e01a6"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c483e210-09ec-4cd5-9ad0-d20b72fa1ce1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3cc6b240-437e-447e-976b-df7abea234e0"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7916a2cc-261a-462b-ac0e-135c805f3ada"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7266aaaf-979a-48f1-bb84-c068c784f631"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlatformAction
        m_PlatformAction = asset.FindActionMap("PlatformAction", throwIfNotFound: true);
        m_PlatformAction_Jump = m_PlatformAction.FindAction("Jump", throwIfNotFound: true);
        m_PlatformAction_Dash = m_PlatformAction.FindAction("Dash", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // PlatformAction
    private readonly InputActionMap m_PlatformAction;
    private IPlatformActionActions m_PlatformActionActionsCallbackInterface;
    private readonly InputAction m_PlatformAction_Jump;
    private readonly InputAction m_PlatformAction_Dash;
    public struct PlatformActionActions
    {
        private @PlayerAction m_Wrapper;
        public PlatformActionActions(@PlayerAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_PlatformAction_Jump;
        public InputAction @Dash => m_Wrapper.m_PlatformAction_Dash;
        public InputActionMap Get() { return m_Wrapper.m_PlatformAction; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlatformActionActions set) { return set.Get(); }
        public void SetCallbacks(IPlatformActionActions instance)
        {
            if (m_Wrapper.m_PlatformActionActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_PlatformActionActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlatformActionActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlatformActionActionsCallbackInterface.OnJump;
                @Dash.started -= m_Wrapper.m_PlatformActionActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlatformActionActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlatformActionActionsCallbackInterface.OnDash;
            }
            m_Wrapper.m_PlatformActionActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
            }
        }
    }
    public PlatformActionActions @PlatformAction => new PlatformActionActions(this);
    public interface IPlatformActionActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
    }
}
