#if UNITY_EDITOR
using System;
using UnityEditor;
using System.Text;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace AMADatePicker
{
    [CustomEditor(typeof(ADP)), CanEditMultipleObjects]
    public class ADPEditor : Editor
    {
        #region VARIABLES
        //---------------

        private ADP _ADP_I;
        private BackEndManager _BEM_I;

        private int _SmallSpace = 5;
        private int _SectionSpace = 5;

        public SerializedProperty _OnDateSelectProp;

        //--------
        #endregion

        #region ON_INSPECTOR_GUI
        //----------------------

        public override void OnInspectorGUI()
        {
            if (_ADP_I == null || _BEM_I == null)
            {
                _ADP_I = (ADP)target;
                _BEM_I = _ADP_I.transform.GetChild(0).GetComponent<BackEndManager>();

                _ADP_I._CheckInit();
            }

            _ProcessBasicSettings();
            GUILayout.Space(_SectionSpace);
            _ProcessLayoutSettings();
            GUILayout.Space(_SectionSpace);
            _ProcessVisualSettings();
            GUILayout.Space(_SectionSpace);
            _ProcessOnDateSelectEvent();
            GUILayout.Space(_SectionSpace);
            _ProcessInfoBox();

            if (GUI.changed && !Application.isPlaying)
            {
                EditorUtility.SetDirty(target);
                EditorSceneManager.MarkSceneDirty(_ADP_I.gameObject.scene);
            }
        }

        //--------
        #endregion

        #region BASIC_LAYOUT_OPTIONS
        //--------------------------

        private void _ProcessBasicSettings()
        {
            _ADP_I._ExpandBasicSettings = EditorGUILayout.BeginFoldoutHeaderGroup(_ADP_I._ExpandBasicSettings, "Basic Settings");

            if (_ADP_I._ExpandBasicSettings)
            {
                GUILayout.BeginVertical("Box");
                _GUILine(1, 0, 0);

                _ProcessEnableScrolling();
                _SetYearsRange();
                _ManageDefaultDate();

                _GUILine(1, 0, 0);
                GUILayout.EndVertical();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void _ProcessEnableScrolling()
        {
            GUILayout.BeginHorizontal();

            _ADP_I.EnableScrolling = EditorGUILayout.ToggleLeft(" Enable Scrolling", _ADP_I.EnableScrolling);
            //Undo.RecordObject(target, "Enable Scrolling Toggled");
            
            GUILayout.EndHorizontal();
        }

        private void _SetYearsRange()
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Set Years Range");

            _ADP_I.YearRangeMin = EditorGUILayout.IntField(_ADP_I.YearRangeMin);
            //Undo.RecordObject(target, "Year Range Minimum Changed");

            GUILayout.Label(" <<<  " + DateTime.Now.ToString("yyyy") + "  >>> ");

            _ADP_I.YearRangeMax = EditorGUILayout.IntField(_ADP_I.YearRangeMax);
            //Undo.RecordObject(target, "Year Range Maximum Changed");

            GUILayout.EndHorizontal();
        }

        private void _ManageDefaultDate()
        {
            GUILayout.BeginHorizontal();

            _SetTodaysDateAsDefaultDate();
            _SetYourOwnDefaultDate();

            GUILayout.EndHorizontal();
        }

        private void _SetTodaysDateAsDefaultDate()
        {
            GUILayout.BeginVertical();

            _ADP_I.TodayAsDefaultDay = EditorGUILayout.ToggleLeft(" Today's Date as Default Date", _ADP_I.TodayAsDefaultDay);
            //Undo.RecordObject(target, "Today As Default Day Toggled");

            if (!_ADP_I.TodayAsDefaultDay)
            {
                EditorGUILayout.LabelField(">> Set Custom Default Date >>>");
            }

            GUILayout.EndVertical();
        }

        private void _SetYourOwnDefaultDate()
        {
            if (!_ADP_I.TodayAsDefaultDay)
            {
                GUILayout.BeginVertical();

                GUILayout.Label("Month");
                _ADP_I.DefaultMonth = EditorGUILayout.IntField(_ADP_I.DefaultMonth);
                //Undo.RecordObject(target, "Default Month Changed");

                GUILayout.EndVertical();

                GUILayout.BeginVertical();

                GUILayout.Label("Day");
                _ADP_I.DefaultDay = EditorGUILayout.IntField(_ADP_I.DefaultDay);
                //Undo.RecordObject(target, "Default Day Changed");

                GUILayout.EndVertical();

                GUILayout.BeginVertical();

                GUILayout.Label("Year");
                _ADP_I.DefaultYear = EditorGUILayout.IntField(_ADP_I.DefaultYear);
                //Undo.RecordObject(target, "Default Year Changed");

                GUILayout.EndVertical();
            }
        }

        //--------
        #endregion

        #region LAYOUT_SETTINGS
        //---------------------

        private void _ProcessLayoutSettings()
        {
            _ADP_I._ExpandLayoutSettings = EditorGUILayout.BeginFoldoutHeaderGroup(_ADP_I._ExpandLayoutSettings, "Layout Settings");

            if (_ADP_I._ExpandLayoutSettings)
            {
                GUILayout.BeginVertical("Box");
                _GUILine(1, 0, 0);

                _ManageLayoutStyle();
                _ManageMonthStyle();
                _ManageShowLabels();

                _GUILine(1, 0, 0);
                GUILayout.EndVertical();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void _ManageLayoutStyle()
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Select Layout Style *");
            _ADP_I.ActiveLayoutStyle = (LayoutOptions)EditorGUILayout.EnumPopup(_ADP_I.ActiveLayoutStyle);
            //Undo.RecordObject(target, "Active Layout Style Changed");

            GUILayout.EndHorizontal();
        }

        private void _ManageMonthStyle()
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Select Month Style");
            _ADP_I.ActiveMonthStyle = (MonthOptions)EditorGUILayout.EnumPopup(_ADP_I.ActiveMonthStyle);
            //Undo.RecordObject(target, "Active Month Style Changed");

            GUILayout.EndHorizontal();
        }

        private void _ManageShowLabels()
        {
            GUILayout.BeginVertical();

            _ADP_I.ShowLabels = EditorGUILayout.ToggleLeft(" Show Labels", _ADP_I.ShowLabels);
            //Undo.RecordObject(target, "Show Labels Toggled");

            if (_ADP_I.ShowLabels)
            {
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Label Position");
                _ADP_I.ActiveLabelPosition = (LabelPositionOptions)EditorGUILayout.EnumPopup(_ADP_I.ActiveLabelPosition);
                //Undo.RecordObject(target, "Active Label Position Changed");

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        //--------
        #endregion

        #region VISUAL_SETTINGS
        //---------------------

        private void _ProcessVisualSettings()
        {
            _ADP_I._ExpandVisualSettings = EditorGUILayout.BeginFoldoutHeaderGroup(_ADP_I._ExpandVisualSettings, "Visual Settings");

            if (_ADP_I._ExpandVisualSettings)
            {
                GUILayout.BeginVertical("Box");
                _GUILine(1, 0, 0);

                _ProcessOverrideToggle();
                _ProcessVisualSettingsGroup();

                _GUILine(1, 0, 0);
                GUILayout.EndVertical();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void _ProcessOverrideToggle()
        {
            GUILayout.BeginHorizontal();

            _ADP_I.OverrideVisualSettings = EditorGUILayout.ToggleLeft(" Override Visual Settings **", _ADP_I.OverrideVisualSettings);
            //Undo.RecordObject(target, "Override Visual Settings Toggled");

            GUILayout.EndHorizontal();
        }

        private void _ProcessVisualSettingsGroup()
        {
            _GUILine(1, 0, 5);
            EditorGUI.BeginDisabledGroup(!_ADP_I.OverrideVisualSettings);

            _ProcessPanelColor();
            _ProcessTFBackgroundColor();
            _ProcessScrollerOverlayColor();
            _ProcessLabelColor();
            _ProcessTFTextColor();
            GUILayout.Space(_SmallSpace);
            _ProcessApplyVisualSettingsButton();

            EditorGUI.EndDisabledGroup();
        }

        private void _ProcessPanelColor()
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Panel Color");
            _ADP_I.MainPanelColor = EditorGUILayout.ColorField(_ADP_I.MainPanelColor);
            Undo.RecordObject(target, "Main Panel Color Changed");

            GUILayout.EndHorizontal();
        }

        private void _ProcessTFBackgroundColor()
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("TextField Background Color");
            _ADP_I.TFBackgroundColor = EditorGUILayout.ColorField(_ADP_I.TFBackgroundColor);
            Undo.RecordObject(target, "TextField Background Color Changed");

            GUILayout.EndHorizontal();
        }

        private void _ProcessScrollerOverlayColor()
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Scroller Overlay Color");
            _ADP_I.ScrollerOverlayColor = EditorGUILayout.ColorField(_ADP_I.ScrollerOverlayColor);
            Undo.RecordObject(target, "Scroller Overlay Color Changed");

            GUILayout.EndHorizontal();
        }

        private void _ProcessLabelColor()
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Label Color");
            _ADP_I.LabelColor = EditorGUILayout.ColorField(_ADP_I.LabelColor);
            Undo.RecordObject(target, "Label Color Changed");

            GUILayout.EndHorizontal();
        }

        private void _ProcessTFTextColor()
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("TextField Text Color");
            _ADP_I.TFTextColor = EditorGUILayout.ColorField(_ADP_I.TFTextColor);
            Undo.RecordObject(target, "TextField Text Color Changed");

            GUILayout.EndHorizontal();
        }

        private void _ProcessApplyVisualSettingsButton()
        {
            GUILayout.BeginHorizontal();

            _ADP_I.ApplyVisualSettingsButton = GUILayout.Button("Apply Visual Settings");

            _ADP_I.DefaultVisualSettingsButton = GUILayout.Button("Default Visual Settings");
            
            GUILayout.EndHorizontal();
        }

        //--------
        #endregion

        #region EVENT_MANAGEMENT
        //----------------------

        private void _ProcessOnDateSelectEvent()
        {
            _ADP_I._ExpandEventHandler = EditorGUILayout.BeginFoldoutHeaderGroup(_ADP_I._ExpandEventHandler, "Add Event Delegate/s ***");

            if (_ADP_I._ExpandEventHandler)
            {
                GUILayout.BeginVertical("Box");
                _GUILine(1, 0, 0);

                // EVENT HANDLER

                serializedObject.Update();
                _OnDateSelectProp = serializedObject.FindProperty("OnDateSelect");
                EditorGUILayout.PropertyField(_OnDateSelectProp, true);
                serializedObject.ApplyModifiedProperties();
                Undo.RecordObject(target, "Event Handler Updated");

                _GUILine(1, 0, 0);
                GUILayout.EndVertical();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        //--------
        #endregion

        #region INFO_BOX
        //--------------

        private void _ProcessInfoBox()
        {
            _ADP_I._ExpandInfoBoxes = EditorGUILayout.BeginFoldoutHeaderGroup(_ADP_I._ExpandInfoBoxes, "Info Box");

            if (_ADP_I._ExpandInfoBoxes)
            {
                GUILayout.BeginVertical("Box");
                _GUILine(1, 0, 0);

                StringBuilder _SB = new StringBuilder();
                _SB.Append("* Changes also apparent in Editor mode.");
                _SB.Append("\n");
                _SB.Append("** Overwites all the changes made to visual elements.");
                _SB.Append("\n");
                _SB.Append("*** Delegates must have an argument of type AMADate.");

                GUILayout.Space(_SmallSpace);
                EditorGUILayout.HelpBox(_SB.ToString(), MessageType.Info);
                GUILayout.Space(_SmallSpace);

                _GUILine(1, 0, 0);
                GUILayout.EndVertical();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        //--------
        #endregion

        #region GENERAL
        //-------------

        private void _GUILine(int _Height = 1, int _TopSpace = 5, int _BottomSpace = 5)
        {
            GUILayout.Space(_TopSpace);
            Rect _Rect = EditorGUILayout.GetControlRect(false, _Height);
            _Rect.height = _Height;
            EditorGUI.DrawRect(_Rect, new Color(0.5f, 0.5f, 0.5f, 1));
            GUILayout.Space(_BottomSpace);
        }

        //--------
        #endregion
    }
}
#endif