using System;
using UnityEngine;
using UnityEngine.Events;

namespace AMADatePicker
{
    public class ADP : MonoBehaviour
    {
        #region VARIABLES
        //---------------

        private GameObject _BackEndManager;
        private BackEndManager _BackEndManager_I;

        public int _UndoGI = 0;

        private bool _HasInit = false;

        public int _DefaultYearIndex = 0;

        // FOLDOUT BOOLS

        public bool _ExpandBasicSettings = true;
        public bool _ExpandLayoutSettings = true;
        public bool _ExpandVisualSettings = true;
        public bool _ExpandEventHandler = true;
        public bool _ExpandInfoBoxes = true;

        // ENABLE SCROLLING

        [SerializeField] private bool _EnableScrolling = true;
        [HideInInspector] public bool EnableScrolling
        {
            get { return _EnableScrolling; }
            set
            {
                if (_EnableScrolling != value)
                {
                    _EnableScrolling = value;
                    _BackEndManager_I._ToggleEnableScrolling(_EnableScrolling);
                }
            }
        }

        // YEAR RANGE MINIMUM

        [SerializeField] private int _YearRangeMin = 100;
        [HideInInspector] public int YearRangeMin
        {
            get { return _YearRangeMin; }
            set
            {
                if (_YearRangeMin != value)
                {
                    _YearRangeMin = value;

                    if (!_TodayAsDefaultDay)
                    {
                        _CheckAndSetDefaultDate();
                    }
                }
            }
        }

        // YEAR RANGE MAXIMUM

        [SerializeField] private int _YearRangeMax = 50;
        [HideInInspector] public int YearRangeMax
        {
            get { return _YearRangeMax; }
            set
            {
                if (_YearRangeMax != value)
                {
                    _YearRangeMax = value;

                    if (!_TodayAsDefaultDay)
                    {
                        _CheckAndSetDefaultDate();
                    }
                }
            }
        }

        // TODAY AS DEFAULT DAY

        [SerializeField] private bool _TodayAsDefaultDay = true;
        [HideInInspector] public bool TodayAsDefaultDay
        {
            get { return _TodayAsDefaultDay; }
            set
            {
                if (_TodayAsDefaultDay != value)
                {
                    _TodayAsDefaultDay = value;
                    _CheckAndSetDefaultDate();
                }
            }
        }

        // SET CUSTOM MONTH

        [SerializeField] private int _DefaultMonth = 0;
        [HideInInspector] public int DefaultMonth
        {
            get { return _DefaultMonth; }
            set
            {
                if (_DefaultMonth != value)
                {
                    _DefaultMonth = value;
                    _CheckAndSetDefaultDate();
                }
            }
        }

        // SET CUSTOM DAY

        [SerializeField] private int _DefaultDay = 0;
        [HideInInspector] public int DefaultDay
        {
            get { return _DefaultDay; }
            set
            {
                if (_DefaultDay != value)
                {
                    _DefaultDay = value;
                    _CheckAndSetDefaultDate();
                }
            }
        }

        // SET CUSTOM YEAR

        [SerializeField] private int _DefaultYear = 0;
        [HideInInspector] public int DefaultYear
        {
            get { return _DefaultYear; }
            set
            {
                if (_DefaultYear != value)
                {
                    _DefaultYear = value;
                    _CheckAndSetDefaultDate();
                }
            }
        }

        // LAYOUT STYLES

        [SerializeField] private LayoutOptions _ActiveLayoutStyle = LayoutOptions.Vertical;
        [HideInInspector] public LayoutOptions ActiveLayoutStyle
        {
            get { return _ActiveLayoutStyle; }
            set
            {
                if (_ActiveLayoutStyle != value)
                {
                    _ActiveLayoutStyle = value;
                    _BackEndManager_I._SetLayout(_ActiveLayoutStyle);
                }
            }
        }

        // MONTH STYLES

        [SerializeField] private MonthOptions _ActiveMonthStyle = MonthOptions.Words;
        [HideInInspector] public MonthOptions ActiveMonthStyle
        {
            get { return _ActiveMonthStyle; }
            set
            {
                if (_ActiveMonthStyle != value)
                {
                    _ActiveMonthStyle = value;
                    
                    if (Application.isPlaying)
                    {
                        _BackEndManager_I._SetMonthOptions(_ActiveMonthStyle);
                    }
                }
            }
        }

        // SHOW LABELS

        [SerializeField] private bool _ShowLabels = false;
        [HideInInspector] public bool ShowLabels
        {
            get { return _ShowLabels; }
            set
            {
                if (_ShowLabels != value)
                {
                    _ShowLabels = value;
                    _BackEndManager_I._ToggleLabelsVisibility(_ShowLabels);
                }
            }
        }

        // LABEL POSITION OPTIONS

        [SerializeField] private LabelPositionOptions _ActiveLabelPosition = LabelPositionOptions.Top;
        [HideInInspector] public LabelPositionOptions ActiveLabelPosition
        {
            get { return _ActiveLabelPosition; }
            set
            {
                if (_ActiveLabelPosition != value)
                {
                    _ActiveLabelPosition = value;
                    _BackEndManager_I._SetLabelPosition(_ActiveLabelPosition);
                }
            }
        }

        // OVERRIDE VISUAL SETTINGS

        [SerializeField] private bool _OverrideVisualSettings = false;
        [HideInInspector] public bool OverrideVisualSettings
        {
            get { return _OverrideVisualSettings; }
            set
            {
                if (_OverrideVisualSettings != value)
                {
                    _OverrideVisualSettings = value;
                }
            }
        }

        // CHANGE PANEL COLOR

        [SerializeField] private Color _MainPanelColor = new Color(1F, 1F, 1F, 0.349F);
        [HideInInspector] public Color MainPanelColor
        {
            get { return _MainPanelColor; }
            set
            {
                if (!_CompareColors(_MainPanelColor, value))
                {
                    _MainPanelColor = value;
                }
            }
        }

        // CHANGE LABEL COLOR

        [SerializeField] private Color _LabelColor = new Color(1F, 1F, 1F, 1F);
        [HideInInspector] public Color LabelColor
        {
            get { return _LabelColor; }
            set
            {
                if (!_CompareColors(_LabelColor, value))
                {
                    _LabelColor = value;
                }
            }
        }

        // CHANGE TEXT FIELD TEXT COLOR

        [SerializeField] private Color _TFTextColor = new Color(0.1960784F, 0.1960784F, 0.1960784F, 1F);
        [HideInInspector] public Color TFTextColor
        {
            get { return _TFTextColor; }
            set
            {
                if (!_CompareColors(_TFTextColor, value))
                {
                    _TFTextColor = value;
                }
            }
        }

        // CHANGE TEXT FIELD BACKGROUND COLOR

        [SerializeField] private Color _TFBackgroundColor = new Color(1F, 1F, 1F, 1F);
        [HideInInspector] public Color TFBackgroundColor
        {
            get { return _TFBackgroundColor; }
            set
            {
                if (!_CompareColors(_TFBackgroundColor, value))
                {
                    _TFBackgroundColor = value;
                }
            }
        }

        // CHANGE SCROLLER OVERLAY COLOR

        [SerializeField] private Color _ScrollerOverlayColor = new Color(0F, 0F, 0F, 1F);
        [HideInInspector] public Color ScrollerOverlayColor
        {
            get { return _ScrollerOverlayColor; }
            set
            {
                if (!_CompareColors(_ScrollerOverlayColor, value))
                {
                    _ScrollerOverlayColor = value;
                }
            }
        }

        // APPLY VISUAL SETTINGS BUTTON

        [SerializeField] private bool _ApplyVisualSettingsButton = false;
        [HideInInspector] public bool ApplyVisualSettingsButton
        {
            get { return _ApplyVisualSettingsButton; }
            set
            {
                if (value == true)
                {
                    value = false;
                    _BackEndManager_I._ApplyVisualSettings();
                    
                }
            }
        }

        // DEFAULT VISUAL SETTINGS BUTTON

        [SerializeField] private bool _DefaultVisualSettingsButton = false;
        [HideInInspector] public bool DefaultVisualSettingsButton
        {
            get { return _DefaultVisualSettingsButton; }
            set
            {
                if (value == true)
                {
                    value = false;
                    _BackEndManager_I._DefaultVisualSettings();
                }
            }
        }

        // Events

        [Serializable]
        public class DatePickerEvent : UnityEvent<AMADate> { }

        public DatePickerEvent OnDateSelect = new DatePickerEvent();

        //--------
        #endregion

        #region INIT
        //----------

        private void OnEnable()
        {
            _CheckInit();
        }

        private void _Init()
        {
            _BackEndManager = transform.GetChild(0).gameObject;
            _BackEndManager_I = _BackEndManager.GetComponent<BackEndManager>();

            _CheckAndSetDefaultDate();
        }

        public void _CheckInit()
        {
            if (!_HasInit) { _Init(); _HasInit = true; }
        }

        //--------
        #endregion

        #region PRIVATE_CALLBACKS
        //-----------------------

        private void _CheckAndSetDefaultDate()
        {
            if (_TodayAsDefaultDay)
            {
                _DefaultMonth = int.Parse(DateTime.Now.ToString("MM"));
                _DefaultDay = int.Parse(DateTime.Now.ToString("dd"));
                _DefaultYear = int.Parse(DateTime.Now.ToString("yyyy"));
            }
            else
            {
                _DefaultYear = _SetYearWithinRange(_DefaultYear);

                (int MM, int dd, int yyyy) _Results = Tools.ValidateAndFixDate(_DefaultMonth, _DefaultDay, _DefaultYear);

                _DefaultMonth = _Results.MM;
                _DefaultDay = _Results.dd;
                _DefaultYear = _Results.yyyy;
            }
            
            _DefaultYearIndex = _CalculateYearIndex(_DefaultYear);
        }

        private int _SetYearWithinRange(int _yyyy)
        {
            int _CurrentYear = int.Parse(DateTime.Now.ToString("yyyy"));

            if (_yyyy < _CurrentYear - YearRangeMin)
            {
                _yyyy = _CurrentYear - YearRangeMin;
            }
            else if (_yyyy > _CurrentYear + YearRangeMax)
            {
                _yyyy = _CurrentYear + YearRangeMax;
            }

            return _yyyy;
        }

        private int _CalculateYearIndex(int _yyyy)
        {
            int _YearIndex = 0;
            int _CurrentYear = int.Parse(DateTime.Now.ToString("yyyy"));

            if (_yyyy < _CurrentYear)
            {
                _YearIndex = YearRangeMin - (_CurrentYear - _yyyy) + 1;
            }
            else
            {
                _YearIndex = YearRangeMin + (_yyyy - _CurrentYear) + 1;
            }

            return _YearIndex;
        }

        //--------
        #endregion

        #region FOR_USER
        //--------------

        public void ApplyVisualSettings()
        {
            _BackEndManager_I._ApplyVisualSettings();
        }

        public void DefaultVisualSettings()
        {
            _BackEndManager_I._DefaultVisualSettings();
        }

        public AMADate GetDate()
        {
            return new AMADate(_BackEndManager_I._MonthSelected, _BackEndManager_I._DaySelected, _BackEndManager_I._YearSelected);
        }

        public void ScrollToDate(int MM, int dd, int yyyy)
        {
            yyyy = _SetYearWithinRange(yyyy);
            (int _MM, int _dd, int _yyyy) = Tools.ValidateAndFixDate(MM, dd, yyyy);
            int _YearIndex = _CalculateYearIndex(_yyyy);

            StartCoroutine(_BackEndManager_I._MonthScrollTo(_MM));
            StartCoroutine(_BackEndManager_I._DayScrollTo(_dd));
            StartCoroutine(_BackEndManager_I._YearScrollTo(_YearIndex));
        }

        //--------
        #endregion

        #region GENERAL
        //-------------

        private bool _CompareColors(Color _ColorA, Color _ColorB)
        {
            bool _AreSame = true;

            if (_ColorA.r != _ColorB.r) { _AreSame = false; }

            if (_ColorA.g != _ColorB.g) { _AreSame = false; }

            if (_ColorA.b != _ColorB.b) { _AreSame = false; }

            if (_ColorA.a != _ColorB.a) { _AreSame = false; }

            return _AreSame;
        }

        //--------
        #endregion
    }
}