﻿using ColossalFramework.UI;
using ICities;
using System.IO;
using UnityEngine;

namespace CSURToolBox.UI
{
    public class OptionUI : MonoBehaviour
    {
        public static bool isShortCutsToPanel = false;
        public static bool isCSURSSmooth = true;
        public static bool isCSURRSmooth = true;
        public static int smoothLevel = 1;
        public static void makeSettings(UIHelperBase helper)
        {
            // tabbing code is borrowed from RushHour mod
            // https://github.com/PropaneDragon/RushHour/blob/release/RushHour/Options/OptionHandler.cs
            LoadSetting();
            UIHelper actualHelper = helper as UIHelper;
            UIComponent container = actualHelper.self as UIComponent;

            UITabstrip tabStrip = container.AddUIComponent<UITabstrip>();
            tabStrip.relativePosition = new Vector3(0, 0);
            tabStrip.size = new Vector2(container.width - 20, 40);

            UITabContainer tabContainer = container.AddUIComponent<UITabContainer>();
            tabContainer.relativePosition = new Vector3(0, 40);
            tabContainer.size = new Vector2(container.width - 20, container.height - tabStrip.height - 20);
            tabStrip.tabPages = tabContainer;

            int tabIndex = 0;
            // Lane_ShortCut

            AddOptionTab(tabStrip, "Lane ShortCut");
            tabStrip.selectedIndex = tabIndex;

            UIPanel currentPanel = tabStrip.tabContainer.components[tabIndex] as UIPanel;
            currentPanel.autoLayout = true;
            currentPanel.autoLayoutDirection = LayoutDirection.Vertical;
            currentPanel.autoLayoutPadding.top = 5;
            currentPanel.autoLayoutPadding.left = 10;
            currentPanel.autoLayoutPadding.right = 10;

            UIHelper panelHelper = new UIHelper(currentPanel);

            var generalGroup = panelHelper.AddGroup("Lane Button ShortCut") as UIHelper;
            var panel = generalGroup.self as UIPanel;

            panel.gameObject.AddComponent<OptionsKeymappingLane>();

            var generalGroup1 = panelHelper.AddGroup("ShortCuts Control") as UIHelper;
            generalGroup1.AddCheckbox("ShortCuts will be used for ToPanel Button", isShortCutsToPanel, (index) => isShortCutsToPanelEnable(index));
            SaveSetting();

            // Function_ShortCut
            ++tabIndex;

            AddOptionTab(tabStrip, "Function ShortCut");
            tabStrip.selectedIndex = tabIndex;

            currentPanel = tabStrip.tabContainer.components[tabIndex] as UIPanel;
            currentPanel.autoLayout = true;
            currentPanel.autoLayoutDirection = LayoutDirection.Vertical;
            currentPanel.autoLayoutPadding.top = 5;
            currentPanel.autoLayoutPadding.left = 10;
            currentPanel.autoLayoutPadding.right = 10;

            panelHelper = new UIHelper(currentPanel);

            generalGroup = panelHelper.AddGroup("Function Button ShortCut") as UIHelper;
            panel = generalGroup.self as UIPanel;

            panel.gameObject.AddComponent<OptionsKeymappingFunction>();

            ++tabIndex;

            AddOptionTab(tabStrip, "Experimental Function");
            tabStrip.selectedIndex = tabIndex;

            currentPanel = tabStrip.tabContainer.components[tabIndex] as UIPanel;
            currentPanel.autoLayout = true;
            currentPanel.autoLayoutDirection = LayoutDirection.Vertical;
            currentPanel.autoLayoutPadding.top = 5;
            currentPanel.autoLayoutPadding.left = 10;
            currentPanel.autoLayoutPadding.right = 10;

            panelHelper = new UIHelper(currentPanel);
            var generalGroup2 = panelHelper.AddGroup("Experimental Function") as UIHelper;
            generalGroup2.AddCheckbox("Enable CSUR-S road lane smooth", isCSURSSmooth, (index) => isCSURSSmoothEnable(index));
            generalGroup2.AddCheckbox("Enable CSUR-R road lane smooth", isCSURRSmooth, (index) => isCSURRSmoothEnable(index));
            generalGroup2.AddDropdown("smooth level", new string[] { "Low", "Medium", "High" }, smoothLevel, (index) => GetSmoothLevel(index));
        }
        private static UIButton AddOptionTab(UITabstrip tabStrip, string caption)
        {
            UIButton tabButton = tabStrip.AddTab(caption);

            tabButton.normalBgSprite = "SubBarButtonBase";
            tabButton.disabledBgSprite = "SubBarButtonBaseDisabled";
            tabButton.focusedBgSprite = "SubBarButtonBaseFocused";
            tabButton.hoveredBgSprite = "SubBarButtonBaseHovered";
            tabButton.pressedBgSprite = "SubBarButtonBasePressed";

            tabButton.textPadding = new RectOffset(10, 10, 10, 10);
            tabButton.autoSize = true;
            tabButton.tooltip = caption;

            return tabButton;
        }

        public static void SaveSetting()
        {
            //save langugae
            FileStream fs = File.Create("CSUR_UI_setting.txt");
            StreamWriter streamWriter = new StreamWriter(fs);
            streamWriter.WriteLine(isShortCutsToPanel);
            streamWriter.WriteLine(isCSURRSmooth);
            streamWriter.WriteLine(isCSURSSmooth);
            streamWriter.WriteLine(smoothLevel);
            streamWriter.Flush();
            fs.Close();
        }

        public static void LoadSetting()
        {
            if (File.Exists("CSUR_UI_setting.txt"))
            {
                FileStream fs = new FileStream("CSUR_UI_setting.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string strLine = sr.ReadLine();

                if (strLine == "True")
                {
                    isShortCutsToPanel = true;
                }
                else
                {
                    isShortCutsToPanel = false;
                }

                strLine = sr.ReadLine();

                if (strLine == "False")
                {
                    isCSURRSmooth = false;
                }
                else
                {
                    isCSURRSmooth = true;
                }

                strLine = sr.ReadLine();

                if (strLine == "False")
                {
                    isCSURSSmooth = false;
                }
                else
                {
                    isCSURSSmooth = true;
                }

                strLine = sr.ReadLine();

                if (strLine == "2")
                {
                    smoothLevel = 2;
                }
                else if (strLine == "0")
                {
                    smoothLevel = 0;
                }
                else
                {
                    smoothLevel = 1;
                }
                sr.Close();
                fs.Close();
            }
        }
        public static void isShortCutsToPanelEnable(bool index)
        {
            isShortCutsToPanel = index;
            SaveSetting();
        }
        public static void isCSURRSmoothEnable(bool index)
        {
            isCSURRSmooth = index;
            SaveSetting();
        }
        public static void isCSURSSmoothEnable(bool index)
        {
            isCSURSSmooth = index;
            SaveSetting();
        }
        public static void GetSmoothLevel(int index)
        {
            smoothLevel = index;
            SaveSetting();
        }
    }
}