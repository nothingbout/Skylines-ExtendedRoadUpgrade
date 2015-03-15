using System;
using System.Collections.Generic;

using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace ExtendedRoadUpgrade {
    public class ModUI {
        public bool isVisible { get; private set; }

        ToolMode _toolMode = ToolMode.None;
        public ToolMode toolMode {
            get { return _toolMode; }
            set {
                _toolMode = value;
                if (tabstrip != null) {
                    tabstrip.selectedIndex = (int)_toolMode - 1;
                }

                if (builtinTabstrip != null) {
                    if (_toolMode != ToolMode.None) {
                        if (builtinTabstrip.selectedIndex >= 0) {
                            originalBuiltinTabsripSelectedIndex = builtinTabstrip.selectedIndex;
                        }

                        ignoreBuiltinTabstripEvents = true;
                        builtinTabstrip.selectedIndex = -1;
                        ignoreBuiltinTabstripEvents = false;
                    }
                    else if (builtinTabstrip.selectedIndex < 0 && originalBuiltinTabsripSelectedIndex >= 0) {
                        ignoreBuiltinTabstripEvents = true;
                        builtinTabstrip.selectedIndex = originalBuiltinTabsripSelectedIndex;
                        ignoreBuiltinTabstripEvents = false;
                    }
                }
            }
        }

        public event System.Action<ToolMode> selectedToolModeChanged;

        bool initialized {
            get { return tabstrip != null; }
        }

        bool ignoreBuiltinTabstripEvents = false;
        int originalBuiltinTabsripSelectedIndex = -1;
        UIComponent roadsOptionPanel = null;
        UITabstrip builtinTabstrip = null;
        UITabstrip tabstrip = null;

        public void Show() {
            if (!initialized) {
                if (!Initialize()) return;
            }

            isVisible = true;
        }

        PropertyChangedEventHandler<int> builtinModeChangedHandler = null;

        public void SetBuiltinMode(int mode) {
            builtinTabstrip.selectedIndex = mode;
        }

        public void DestroyView() {
            if (tabstrip != null) {
                if (builtinTabstrip != null) {
                    builtinTabstrip.eventSelectedIndexChanged -= builtinModeChangedHandler;
                }

                UIView.Destroy(tabstrip);
                tabstrip = null;
                isVisible = false;
            }
        }

        bool Initialize() {
            if (UIUtils.Instance == null) return false;

            roadsOptionPanel = UIUtils.Instance.FindComponent<UIComponent>("RoadsOptionPanel", null, UIUtils.FindOptions.NameContains);
            if (roadsOptionPanel == null) return false;

            builtinTabstrip = UIUtils.Instance.FindComponent<UITabstrip>("ToolMode", roadsOptionPanel);
            if (builtinTabstrip == null) return false;

            tabstrip = UIUtils.Instance.FindComponent<UITabstrip>("ExtendedRoadUpgradePanel");
            if (tabstrip != null) {
                DestroyView();
            }

            CreateView();
            if (tabstrip == null) return false; 

            return true;
        }

        void CreateView() {
            GameObject rootObject = new GameObject("ExtendedRoadUpgradePanel");
            tabstrip = rootObject.AddComponent<UITabstrip>();

            UIButton tabTemplate = (UIButton)builtinTabstrip.tabs[0];

            List<UIButton> tabs = new List<UIButton>();
            tabs.Add(tabstrip.AddTab("⇆", tabTemplate, true));
            tabs.Add(tabstrip.AddTab("⇉", tabTemplate, true));

            foreach (UIButton tab in tabs) {
                tab.name = "ExtendedRoadUpgradeButton";
                tab.normalFgSprite = null;
                tab.pressedFgSprite = null;
                tab.hoveredFgSprite = null;
                tab.focusedFgSprite = null;
                tab.textColor = new Color32(127, 130, 130, 255);

                tab.textScale = 2.0f;
                tab.textHorizontalAlignment = UIHorizontalAlignment.Center;
                tab.textVerticalAlignment = UIVerticalAlignment.Middle;
                tab.textPadding.left = -1;
                tab.textPadding.top = -5;
                tab.hoveredTextColor = tab.textColor;
                tab.focusedTextColor = tab.pressedTextColor = new Color32(190, 235, 255, 255);
                tab.playAudioEvents = true;
            }

            tabs[0].name = "ExtendedRoadUpgradeButtonTwoWay";
            tabs[0].tooltip = "Upgrade To Two-Way Road";
            tabs[1].name = "ExtendedRoadUpgradeButtonOneWay";
            tabs[1].tooltip = "Upgrade To One-Way Road";

            roadsOptionPanel.AttachUIComponent(tabstrip.gameObject);
            tabstrip.relativePosition = new Vector3(129, 35);
            tabstrip.width = 80;
            tabstrip.selectedIndex = -1;

            //UIButton.Destroy(tabTemplate.gameObject);

            if (builtinModeChangedHandler == null) {
                builtinModeChangedHandler = (UIComponent component, int index) => {
                    if (!ignoreBuiltinTabstripEvents) {
                        if (selectedToolModeChanged != null) selectedToolModeChanged(ToolMode.None);
                    }
                };
            }

            builtinTabstrip.eventSelectedIndexChanged += builtinModeChangedHandler;

            // Setting selectedIndex needs to be delayed for some reason
            tabstrip.StartCoroutine(FinishCreatingView());
        }

        System.Collections.IEnumerator FinishCreatingView() {
            yield return null;
            tabstrip.selectedIndex = -1;
            tabstrip.eventSelectedIndexChanged += (UIComponent component, int index) => {
                ToolMode newMode = (ToolMode)(index + 1);
                ModDebug.Log("tabstrip.eventSelectedIndexChanged: " + newMode);
                if (selectedToolModeChanged != null) selectedToolModeChanged(newMode);
            };
        }
    }
}
