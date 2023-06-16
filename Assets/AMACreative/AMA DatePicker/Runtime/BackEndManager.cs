using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace AMADatePicker
{
    [RequireComponent(typeof(TMP_Text))]
    public class BackEndManager : MonoBehaviour
    {
        #region VARIABLES
        //---------------

        [HideInInspector] public int _MonthSelected;
        [HideInInspector] public int _DaySelected;
        [HideInInspector] public int _YearSelected;

        private bool _HasInit = false;
        private bool _IsTMPText = false;

        private List<RectTransform> _RTs = new List<RectTransform>(); 

        /* --- MONTH --- */

        [NonSerialized] public Text[] _MonthTexts = new Text[12];
        [NonSerialized] public TMP_Text[] _MonthTMPTexts = new TMP_Text[12];

        private Coroutine _MonthActiveCoroutine;
        private float _MonthDivFactor = 0F;
        private float _MonthAnimStep = 0F;

        /* --- DAY --- */

        private GameObject[] _LastThreeDays = new GameObject[3];

        private Coroutine _DayActiveCoroutine;
        private float _DayDivFactor = 0F;
        private float _DayAnimStep = 0F;
        private int _DayActiveChildCount = 0;

        private int _29th = 0;
        private int _30th = 1;
        private int _31th = 2;

        /* --- YEAR --- */

        private Coroutine _YearActiveCoroutine;
        private float _YearDivFactor = 0F;
        private float _YearAnimStep = 0F;

        private int[] _YearsArray;

        [Header("--- PLACEHOLDERS ---")]

        [SerializeField] private GameObject _MonthPlaceholder;
        [SerializeField] private GameObject _DayPlaceholder;
        [SerializeField] private GameObject _YearPlaceholder;

        [Header("--- INTERIOR PANELS ---")]

        public Transform _MonthPanelT;
        public Transform _DayPanelT;
        public Transform _YearPanelT;

        [Header("--- SCROLL PAGES ---")]

        public Transform _MonthScrollPageT;
        public Transform _DayScrollPageT;
        public Transform _YearScrollPageT;

        [Header("--- SCROLL FIELDS ---")]

        public Transform _MonthScrollField;
        public Transform _DayScrollField;
        public Transform _YearScrollField;

        [Header("--- SCROLL FIELD IMAGES ---")]

        public Image _MonthScrollerOverlayI;
        public Image _DayScrollerOverlayI;
        public Image _YearScrollerOverlayI;

        [Header("--- SCROLL RECTS ---")]

        public ScrollRect _MonthScrollRect;
        public ScrollRect _DayScrollRect;
        public ScrollRect _YearScrollRect;

        [Header("--- LABELS ---")]

        public Transform _MonthLabel;
        public Transform _DayLabel;
        public Transform _YearLabel;

        [Header("--- GENERAL ---")]

        public GameObject _SecondRowPrefab;
        [SerializeField] private RectTransform _ADP_RT;
        [SerializeField] private ADP _ADP_I;

        private float _AnimStepDivider = 40F;

        //--------
        #endregion

        #region START
        //-----------

        private void OnEnable()
        {
            if (_HasInit) { return; } else { _HasInit = true; }

            _CheckTextType();

            _MonthSetup();
            _DaySetup();
            _YearSetup();

            Invoke("_ApplyVisualSettings", 0.05F);

            Invoke("_MatchScrollerSizes", 0.1F);
            Invoke("_SetDefaultDate", 0.1F);
        }

        //--------
        #endregion

        #region GENERAL_PROCESSING
        //------------------------

        private void _CheckTextType()
        {
            if (_MonthPlaceholder.GetComponent<Text>())
            {
                _IsTMPText = false;
            }
            else
            {
                _IsTMPText = true;
            }
        }

        private void _UpdateCalander()
        {
            for (int i = 0; i < _LastThreeDays.Length; i++)
            {
                _LastThreeDays[i].SetActive(true);
            }

            for (int i = 0; i < Tools._MonthWithOnlyThirties.Length; i++)
            {
                if (_MonthSelected == Tools._MonthWithOnlyThirties[i])
                {
                    _LastThreeDays[_31th].SetActive(false);

                    if (_DaySelected == 31)
                    {
                        _DaySelected = 30;
                    }
                }
            }

            if (_MonthSelected == 2)
            {
                _LastThreeDays[_30th].SetActive(false);
                _LastThreeDays[_31th].SetActive(false);

                if (_YearSelected % 4 != 0)
                {
                    _LastThreeDays[_29th].SetActive(false);

                    if (_DaySelected > 28)
                    {
                        _DaySelected = 28;
                    }
                }
                else
                {
                    if (_DaySelected > 29)
                    {
                        _DaySelected = 29;
                    }
                }
            }

            _DayRefactor();

            _ADP_I.OnDateSelect.Invoke(new AMADate(_MonthSelected, _DaySelected, _YearSelected));
        }

        private void _SetDefaultDate()
        {
            _MonthActiveCoroutine = StartCoroutine(_MonthScrollTo(_ADP_I.DefaultMonth));
            _MonthSelected = _ADP_I.DefaultMonth;

            _DayActiveCoroutine = StartCoroutine(_DayScrollTo(_ADP_I.DefaultDay));
            _DaySelected = _ADP_I.DefaultDay;

            _YearActiveCoroutine = StartCoroutine(_YearScrollTo(_ADP_I._DefaultYearIndex));
            _YearSelected = _ADP_I.DefaultYear;

            _UpdateCalander();
        }
        
        public IEnumerator _MonthScrollTo(int _CurrentMonth)
        {
            yield return new WaitForEndOfFrame();

            float _VertPosPrimary = _MonthScrollRect.verticalNormalizedPosition;
            float _VertPosFinal = (_GetActiveChildCount(_MonthScrollPageT) - _CurrentMonth) * _MonthDivFactor;

            float _Duration = 1F;
            float _CurrentTime = 0F;
            float _TopSpeed = _Duration * 2F;
            float _Speed = _TopSpeed;

            while (_CurrentTime < _Duration)
            {
                float _NormalizedTime = _CurrentTime / _Duration;
                _MonthScrollRect.verticalNormalizedPosition = Mathf.Lerp(_VertPosPrimary, _VertPosFinal, _NormalizedTime);

                _Speed = Mathf.Lerp(_TopSpeed, 0.05F, _NormalizedTime);
                _CurrentTime += Time.deltaTime * _Speed;

                yield return null;
            }

            _MonthScrollRect.verticalNormalizedPosition = _VertPosFinal;
        }

        public IEnumerator _DayScrollTo(int _CurrentDay)
        {
            yield return new WaitForEndOfFrame();

            float _VertPosPrimary = _MonthScrollRect.verticalNormalizedPosition;
            float _VertPosFinal = (_GetActiveChildCount(_DayScrollPageT) - _CurrentDay) * _DayDivFactor;

            float _Duration = 1F;
            float _CurrentTime = 0F;
            float _TopSpeed = _Duration * 2F;
            float _Speed = _TopSpeed;

            while (_CurrentTime < _Duration)
            {
                float _NormalizedTime = _CurrentTime / _Duration;
                _DayScrollRect.verticalNormalizedPosition = Mathf.Lerp(_VertPosPrimary, _VertPosFinal, _NormalizedTime);

                _Speed = Mathf.Lerp(_TopSpeed, 0.05F, _NormalizedTime);
                _CurrentTime += Time.deltaTime * _Speed;

                yield return null;
            }

            _DayScrollRect.verticalNormalizedPosition = _VertPosFinal;
        }

        public IEnumerator _YearScrollTo(int _YearIndex)
        {
            yield return new WaitForEndOfFrame();

            float _VertPosPrimary = _MonthScrollRect.verticalNormalizedPosition;
            float _VertPosFinal = (_GetActiveChildCount(_YearScrollPageT) - _YearIndex) * _YearDivFactor;

            float _Duration = 1F;
            float _CurrentTime = 0F;
            float _TopSpeed = _Duration * 2F;
            float _Speed = _TopSpeed;

            while (_CurrentTime < _Duration)
            {
                float _NormalizedTime = _CurrentTime / _Duration;
                _YearScrollRect.verticalNormalizedPosition = Mathf.Lerp(_VertPosPrimary, _VertPosFinal, _NormalizedTime);

                _Speed = Mathf.Lerp(_TopSpeed, 0.05F, _NormalizedTime);
                _CurrentTime += Time.deltaTime * _Speed;

                yield return null;
            }

            _YearScrollRect.verticalNormalizedPosition = _VertPosFinal;
        }

        private int _GetActiveChildCount(Transform _ReceivedTransform)
        {
            int _ActiveChildCount = 0;

            for (int i = 0; i < _ReceivedTransform.childCount; i++)
            {
                if (_ReceivedTransform.GetChild(i).gameObject.activeSelf)
                {
                    _ActiveChildCount++;
                }
            }

            return _ActiveChildCount;
        }

        //--------
        #endregion

        #region LAYOUT_UPDATING
        //---------------------

        private void _MatchScrollerSizes()
        {
            Vector2 _SizeDelta = new Vector2();

            _SizeDelta.x = _MonthScrollPageT.GetComponent<RectTransform>().sizeDelta.x;
            _SizeDelta.y = _MonthScrollPageT.GetComponent<RectTransform>().sizeDelta.y / _GetActiveChildCount(_MonthScrollPageT);

            _MonthScrollRect.GetComponent<RectTransform>().sizeDelta = _SizeDelta;
            _DayScrollRect.GetComponent<RectTransform>().sizeDelta = _SizeDelta;
            _YearScrollRect.GetComponent<RectTransform>().sizeDelta = _SizeDelta;

            RectTransform _RT = GetComponent<RectTransform>();
            float _ADPScale = _ADP_RT.transform.localScale.x;
            Vector2 _ADPRTSizeDelta = new Vector2(_RT.sizeDelta.x * _ADPScale, _RT.sizeDelta.y * _ADPScale);
            _ADP_RT.sizeDelta = _ADPRTSizeDelta;

            _ForceRebuildLayoutForAll();
        }

        public void _ForceRebuildLayoutForAll()
        {
            if (Application.isPlaying)
            {
                _ForceRebuildCached();
            }
            else
            {
                _ForceRebuildUncached();
            }
        }

        private void _ForceRebuildCached()
        {
            if (_RTs.Count == 0)
            {
                _RTs.Add(GetComponent<RectTransform>());

                RectTransform[] _RectTransforms = GetComponentsInChildren<RectTransform>(true);

                foreach (RectTransform _RT in _RectTransforms)
                {
                    if (_RT.GetComponent<VerticalLayoutGroup>() || _RT.GetComponent<HorizontalLayoutGroup>())
                    {
                        _RTs.Add(_RT);
                    }
                }
            }

            foreach (RectTransform _RT in _RTs)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(_RT);
            }
        }

        private void _ForceRebuildUncached()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

            Transform[] _Transforms = GetComponentsInChildren<Transform>(true);

            foreach (Transform _Transform in _Transforms)
            {
                RectTransform _RT = _Transform.GetComponent<RectTransform>();

                if (_RT != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(_RT);
                }
            }
        }

        //--------
        #endregion

        #region MONTH_PROCESSING
        //----------------------

        public void _MonthSetup()
        {
            if (!_IsTMPText)
            {
                for (int i = 0; i < 12; i++)
                {
                    Text _InstText = Instantiate(_MonthPlaceholder, _MonthScrollPageT).GetComponent<Text>();
                    _MonthTexts[i] = _InstText;

                    if (_ADP_I.ActiveMonthStyle == MonthOptions.Words)
                    {
                        _InstText.text = Tools.Months[i];
                    }
                    else
                    {
                        _InstText.text = (i + 1).ToString();
                    }

                    _InstText.name = "MONTH_" + (i + 1).ToString();
                }
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    TMP_Text _InstText = Instantiate(_MonthPlaceholder, _MonthScrollPageT).GetComponent<TMP_Text>();
                    _MonthTMPTexts[i] = _InstText;

                    if (_ADP_I.ActiveMonthStyle == MonthOptions.Words)
                    {
                        _InstText.text = Tools.Months[i];
                    }
                    else
                    {
                        _InstText.text = (i + 1).ToString();
                    }

                    _InstText.name = "MONTH_" + (i + 1).ToString();
                }
            }

            Destroy(_MonthPlaceholder);

            Invoke("_MonthRefactor", 0.05F);
        }

        private void _MonthRefactor()
        {
            _MonthDivFactor = 1F / (_MonthScrollPageT.childCount - 1F);
            _MonthAnimStep = _MonthDivFactor / _AnimStepDivider;
        }

        public void _MonthScrolled()
        {
            if (_MonthActiveCoroutine != null)
            {
                StopCoroutine(_MonthActiveCoroutine);
            }

            _MonthActiveCoroutine = StartCoroutine(_MonthSnap());
        }

        private IEnumerator _MonthSnap()
        {
            while (Mathf.Abs(_MonthScrollRect.velocity.y) > 100F)
            {
                yield return null;
            }

            _MonthScrollRect.StopMovement();

            float _DownSector = _MonthScrollRect.verticalNormalizedPosition - (_MonthScrollRect.verticalNormalizedPosition % _MonthDivFactor);

            if (_DownSector < 0F) { _DownSector = 0F; }

            float _UpSector = _DownSector + _MonthDivFactor;

            if (_UpSector > 1F) { _UpSector = 1F; }

            float _MiddlePoint = _MonthScrollRect.verticalNormalizedPosition;

            if ((_UpSector - _MiddlePoint) < (_MiddlePoint - _DownSector))
            {
                while (_MonthScrollRect.verticalNormalizedPosition < _UpSector)
                {
                    _MonthScrollRect.verticalNormalizedPosition += _MonthAnimStep;

                    yield return null;
                }

                _MonthScrollRect.verticalNormalizedPosition = _UpSector;
            }
            else
            {
                while (_MonthScrollRect.verticalNormalizedPosition > _DownSector)
                {
                    _MonthScrollRect.verticalNormalizedPosition -= _MonthAnimStep;

                    yield return null;
                }

                _MonthScrollRect.verticalNormalizedPosition = _DownSector;
            }

            int _MonthChildCount = _MonthScrollPageT.childCount;
            float _ChosenMonthInverted = _MonthScrollRect.verticalNormalizedPosition * _MonthChildCount;
            float _ChosenMonth = _MonthChildCount - _ChosenMonthInverted;
            _MonthSelected = (int)_ChosenMonth + 1;

            if (_MonthSelected > _MonthChildCount) { _MonthSelected = _MonthChildCount; }

            _UpdateCalander();
        }

        public void _SetMonthOptions(MonthOptions _ReceivedMO)
        {
            if (_ReceivedMO == MonthOptions.Words)
            {
                if (!_IsTMPText)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        _MonthTexts[i].text = Tools.Months[i];
                    }
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        _MonthTMPTexts[i].text = Tools.Months[i];
                    }
                }                
            }
            else
            {
                if (!_IsTMPText)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        _MonthTexts[i].text = (i + 1).ToString();
                    }
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                    {
                        _MonthTMPTexts[i].text = (i + 1).ToString();
                    }
                }
                
            }
        }

        //--------
        #endregion

        #region DAY_PROCESSING
        //--------------------

        public void _DaySetup()
        {
            if (!_IsTMPText)
            {
                for (int i = 1; i <= 31; i++)
                {
                    Text _InstText = Instantiate(_DayPlaceholder, _DayScrollPageT).GetComponent<Text>();

                    if (i < 10)
                    {
                        _InstText.text = "0" + (i).ToString();
                    }
                    else
                    {
                        _InstText.text = i.ToString();

                        if (i >= 29)
                        {
                            _LastThreeDays[i - 29] = _InstText.gameObject;
                        }
                    }

                    _InstText.name = "DAY_" + _InstText.text;
                }
            }
            else
            {
                for (int i = 1; i <= 31; i++)
                {
                    TMP_Text _InstText = Instantiate(_DayPlaceholder, _DayScrollPageT).GetComponent<TMP_Text>();

                    if (i < 10)
                    {
                        _InstText.text = "0" + (i).ToString();
                    }
                    else
                    {
                        _InstText.text = i.ToString();

                        if (i >= 29)
                        {
                            _LastThreeDays[i - 29] = _InstText.gameObject;
                        }
                    }

                    _InstText.name = "DAY_" + _InstText.text;
                }
            }

            Destroy(_DayPlaceholder);

            Invoke("_DayRefactor", 0.05F);
        }

        private void _DayRefactor()
        {
            _DayActiveChildCount = _GetActiveChildCount(_DayScrollPageT);
            _DayDivFactor = 1F / (_DayActiveChildCount - 1F);
            _DayAnimStep = _DayDivFactor / _AnimStepDivider;
        }

        public void _DayScrolled()
        {
            if (_DayActiveCoroutine != null)
            {
                StopCoroutine(_DayActiveCoroutine);
            }

            _DayActiveCoroutine = StartCoroutine(_DaySnap());
        }

        private IEnumerator _DaySnap()
        {
            while (Mathf.Abs(_DayScrollRect.velocity.y) > 100F)
            {
                yield return null;
            }

            _DayScrollRect.StopMovement();

            float _DownSector = _DayScrollRect.verticalNormalizedPosition - (_DayScrollRect.verticalNormalizedPosition % _DayDivFactor);

            if (_DownSector < 0F) { _DownSector = 0F; }

            float _UpSector = _DownSector + _DayDivFactor;

            if (_UpSector > 1F) { _UpSector = 1F; }

            float _MiddlePoint = _DayScrollRect.verticalNormalizedPosition;

            if ((_UpSector - _MiddlePoint) < (_MiddlePoint - _DownSector))
            {
                while (_DayScrollRect.verticalNormalizedPosition < _UpSector)
                {
                    _DayScrollRect.verticalNormalizedPosition += _DayAnimStep;

                    yield return null;
                }

                _DayScrollRect.verticalNormalizedPosition = _UpSector;
            }
            else
            {
                while (_DayScrollRect.verticalNormalizedPosition > _DownSector)
                {
                    _DayScrollRect.verticalNormalizedPosition -= _DayAnimStep;

                    yield return null;
                }

                _DayScrollRect.verticalNormalizedPosition = _DownSector;
            }

            _DayActiveChildCount = _GetActiveChildCount(_DayScrollPageT);

            float _ChosenDayInverted = _DayScrollRect.verticalNormalizedPosition * _DayActiveChildCount;
            float _ChosenDay = _DayActiveChildCount - _ChosenDayInverted;
            _DaySelected = (int)_ChosenDay + 1;

            if (_DaySelected > _DayActiveChildCount) { _DaySelected = _DayActiveChildCount; }

            _UpdateCalander();
        }

        //--------
        #endregion

        #region YEAR_PROCESSING
        //---------------------

        public void _YearSetup()
        {
            int _CurrentYear = int.Parse(DateTime.Now.ToString("yyyy"));
            int _Count = 0;

            _YearsArray = new int[_ADP_I.YearRangeMin + _ADP_I.YearRangeMax + 1];

            if (!_IsTMPText)
            {
                for (int i = -_ADP_I.YearRangeMin; i <= _ADP_I.YearRangeMax; i++)
                {
                    Text _InstText = Instantiate(_YearPlaceholder, _YearScrollPageT).GetComponent<Text>();
                    _InstText.text = (_CurrentYear + i).ToString();
                    _YearsArray[_Count] = _CurrentYear + i;
                    _Count++;

                    _InstText.name = "YEAR_" + _InstText.text;
                }
            }
            else
            {
                for (int i = -_ADP_I.YearRangeMin; i <= _ADP_I.YearRangeMax; i++)
                {
                    TMP_Text _InstText = Instantiate(_YearPlaceholder, _YearScrollPageT).GetComponent<TMP_Text>();
                    _InstText.text = (_CurrentYear + i).ToString();
                    _YearsArray[_Count] = _CurrentYear + i;
                    _Count++;

                    _InstText.name = "YEAR_" + _InstText.text;
                }
            }

            Destroy(_YearPlaceholder);

            Invoke("_YearRefactor", 0.05F);
        }

        private void _YearRefactor()
        {
            _YearDivFactor = 1F / (_YearScrollPageT.childCount - 1F);
            _YearAnimStep = _YearDivFactor / _AnimStepDivider;
        }

        public void _YearScrolled()
        {
            if (_YearActiveCoroutine != null)
            {
                StopCoroutine(_YearActiveCoroutine);
            }

            _YearActiveCoroutine = StartCoroutine(_YearSnap());
        }

        private IEnumerator _YearSnap()
        {
            while (Mathf.Abs(_YearScrollRect.velocity.y) > 100F)
            {
                yield return null;
            }

            _YearScrollRect.StopMovement();

            float _DownSector = _YearScrollRect.verticalNormalizedPosition - (_YearScrollRect.verticalNormalizedPosition % _YearDivFactor);

            if (_DownSector < 0F) { _DownSector = 0F; }

            float _UpSector = _DownSector + _YearDivFactor;

            if (_UpSector > 1F) { _UpSector = 1F; }

            float _MiddlePoint = _YearScrollRect.verticalNormalizedPosition;

            if ((_UpSector - _MiddlePoint) < (_MiddlePoint - _DownSector))
            {
                while (_YearScrollRect.verticalNormalizedPosition < _UpSector)
                {
                    _YearScrollRect.verticalNormalizedPosition += _YearAnimStep;

                    yield return null;
                }

                _YearScrollRect.verticalNormalizedPosition = _UpSector;
            }
            else
            {
                while (_YearScrollRect.verticalNormalizedPosition > _DownSector)
                {
                    _YearScrollRect.verticalNormalizedPosition -= _YearAnimStep;

                    yield return null;
                }

                _YearScrollRect.verticalNormalizedPosition = _DownSector;
            }

            int _YearChildCount = _YearScrollPageT.childCount;
            float _ChosenYearInverted = _YearScrollRect.verticalNormalizedPosition * _YearScrollPageT.childCount;
            float _ChosenYear = _YearChildCount - _ChosenYearInverted;
            int _ChosenYearIndex = (int)_ChosenYear;

            if (_ChosenYearIndex >= _YearsArray.Length)
            {
                _ChosenYearIndex = _YearsArray.Length - 1;
            }
            else if (_ChosenYearIndex < 0)
            {
                _ChosenYearIndex = 0;
            }

            _YearSelected = _YearsArray[_ChosenYearIndex];

            _UpdateCalander();
        }

        //--------
        #endregion

        #region LAYOUT_PROCESSING
        //-----------------------

        public void _ToggleEnableScrolling(bool _EnableScrolling)
        {
            if (_EnableScrolling)
            {
                _MonthScrollerOverlayI.raycastTarget = true;
                _DayScrollerOverlayI.raycastTarget = true;
                _YearScrollerOverlayI.raycastTarget = true;
            }
            else
            {
                _MonthScrollerOverlayI.raycastTarget = false;
                _DayScrollerOverlayI.raycastTarget = false;
                _YearScrollerOverlayI.raycastTarget = false;
            }
        }

        public void _SetLayout(LayoutOptions _ReceivedLO)
        {
            ContentSizeFitter _CSF = GetComponent<ContentSizeFitter>();

            if (_CSF != null) { DestroyImmediate(_CSF); }

            if (_ReceivedLO == LayoutOptions.Vertical)
            {
                _UnsetLayoutFromCompact();
                _SetLayoutToVertical();
            }
            else if (_ReceivedLO == LayoutOptions.Horizontal)
            {
                _UnsetLayoutFromCompact();
                _SetLayoutToHorizontal();
            }
            else
            {
                _SetLayoutToCompact();
            }

            ContentSizeFitter _NewCSF = gameObject.AddComponent<ContentSizeFitter>();
            _NewCSF.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            _NewCSF.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            Invoke("_MatchScrollerSizes", 0.05F);
        }

        public void _SetLayoutToVertical()
        {
            HorizontalLayoutGroup _HLG = GetComponent<HorizontalLayoutGroup>();
            VerticalLayoutGroup _VLG = GetComponent<VerticalLayoutGroup>();

            if (_HLG != null) { DestroyImmediate(_HLG); }

            if (_VLG != null)
            {
                return;
            }

            _SetupVerticalLayoutGroup(gameObject.AddComponent<VerticalLayoutGroup>());
        }

        public void _SetLayoutToHorizontal()
        {
            VerticalLayoutGroup _VLG = GetComponent<VerticalLayoutGroup>();
            HorizontalLayoutGroup _HLG = GetComponent<HorizontalLayoutGroup>();

            if (_VLG != null) { DestroyImmediate(_VLG); }

            if (_HLG != null)
            {
                return;
            }

            _SetupHorizontalLayoutGroup(gameObject.AddComponent<HorizontalLayoutGroup>());
        }

        public void _SetLayoutToCompact()
        {
            if (transform.childCount == 2)
            {
                return;
            }

            _SetLayoutToVertical();

            GameObject _SecondRowGO = Instantiate(_SecondRowPrefab, transform);
            Transform _SecondRowT = _SecondRowGO.transform;

            _DayPanelT.transform.SetParent(_SecondRowT);
            _YearPanelT.transform.SetParent(_SecondRowT);
        }

        public void _UnsetLayoutFromCompact()
        {
            if (transform.childCount == 3)
            {
                return;
            }

            Transform _SecondRowT = transform.GetChild(1);

            _DayPanelT.transform.SetParent(transform);
            _YearPanelT.transform.SetParent(transform);

            _DayPanelT.transform.SetSiblingIndex(1);
            _YearPanelT.transform.SetSiblingIndex(2);

            DestroyImmediate(_SecondRowT.gameObject);
        }

        private void _SetupVerticalLayoutGroup(VerticalLayoutGroup _VLG)
        {
            _VLG.padding.left = 75;
            _VLG.padding.right = 75;
            _VLG.padding.top = 75;
            _VLG.padding.bottom = 75;
            _VLG.spacing = 30;
            _VLG.childAlignment = TextAnchor.UpperCenter;
            _VLG.childControlWidth = false;
            _VLG.childControlHeight = false;
            _VLG.childScaleWidth = true;
            _VLG.childScaleHeight = true;
            _VLG.childForceExpandWidth = true;
            _VLG.childForceExpandHeight = true;
        }

        private void _SetupHorizontalLayoutGroup(HorizontalLayoutGroup _HLG)
        {
            _HLG.padding.left = 75;
            _HLG.padding.right = 75;
            _HLG.padding.top = 75;
            _HLG.padding.bottom = 75;
            _HLG.spacing = 30;
            _HLG.childAlignment = TextAnchor.UpperCenter;
            _HLG.childControlWidth = false;
            _HLG.childControlHeight = false;
            _HLG.childScaleWidth = true;
            _HLG.childScaleHeight = true;
            _HLG.childForceExpandWidth = true;
            _HLG.childForceExpandHeight = true;
        }

        //--------
        #endregion

        #region LABELS_PROCESSING
        //-----------------------

        public void _ToggleLabelsVisibility(bool _Enable)
        {
            if (_Enable)
            {
                _MonthLabel.gameObject.SetActive(true);
                _DayLabel.gameObject.SetActive(true);
                _YearLabel.gameObject.SetActive(true);
            }
            else
            {
                _MonthLabel.gameObject.SetActive(false);
                _DayLabel.gameObject.SetActive(false);
                _YearLabel.gameObject.SetActive(false);
            }

            Invoke("_MatchScrollerSizes", 0.05F);
        }

        public void _SetLabelPosition(LabelPositionOptions _ReceivedLPO)
        {
            if (_ReceivedLPO == LabelPositionOptions.Top || _ReceivedLPO == LabelPositionOptions.Bottom)
            {
                _SetLabelPositionVertical(_MonthPanelT, _ReceivedLPO);
                _SetLabelPositionVertical(_DayPanelT, _ReceivedLPO);
                _SetLabelPositionVertical(_YearPanelT, _ReceivedLPO);
            }
            else if (_ReceivedLPO == LabelPositionOptions.Left || _ReceivedLPO == LabelPositionOptions.Right)
            {
                _SetLabelPositionHorizontal(_MonthPanelT, _ReceivedLPO);
                _SetLabelPositionHorizontal(_DayPanelT, _ReceivedLPO);
                _SetLabelPositionHorizontal(_YearPanelT, _ReceivedLPO);
            }

            Invoke("_MatchScrollerSizes", 0.05F);
        }

        private void _SetLabelPositionVertical(Transform _ReceivedPanel, LabelPositionOptions _ReceivedLPO)
        {
            ContentSizeFitter _CSF = _ReceivedPanel.GetComponent<ContentSizeFitter>();

            if (_CSF != null) { DestroyImmediate(_CSF); }

            VerticalLayoutGroup _VLG = _ReceivedPanel.GetComponent<VerticalLayoutGroup>();
            HorizontalLayoutGroup _HLG = _ReceivedPanel.GetComponent<HorizontalLayoutGroup>();

            if (_HLG != null) { DestroyImmediate(_HLG); }

            if (_VLG == null)
            {
                _VLG = _ReceivedPanel.gameObject.AddComponent<VerticalLayoutGroup>();
            }

            if (_ReceivedLPO == LabelPositionOptions.Top)
            {
                _SetupVerticalLayoutGroup(_VLG, false);
            }
            else
            {
                _SetupVerticalLayoutGroup(_VLG, true);
            }

            ContentSizeFitter _NewCSF = _ReceivedPanel.gameObject.AddComponent<ContentSizeFitter>();
            _NewCSF.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            _NewCSF.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        private void _SetLabelPositionHorizontal(Transform _ReceivedPanel, LabelPositionOptions _ReceivedLPO)
        {
            ContentSizeFitter _CSF = _ReceivedPanel.GetComponent<ContentSizeFitter>();

            if (_CSF != null) { DestroyImmediate(_CSF); }

            HorizontalLayoutGroup _HLG = _ReceivedPanel.GetComponent<HorizontalLayoutGroup>();
            VerticalLayoutGroup _VLG = _ReceivedPanel.GetComponent<VerticalLayoutGroup>();
            
            if (_VLG != null) { DestroyImmediate(_VLG); }

            if (_HLG == null)
            {
                _HLG = _ReceivedPanel.gameObject.AddComponent<HorizontalLayoutGroup>();
            }

            if (_ReceivedLPO == LabelPositionOptions.Left)
            {
                _SetupHorizontalLayoutGroup(_HLG, false);
            }
            else
            {
                _SetupHorizontalLayoutGroup(_HLG, true);
            }

            ContentSizeFitter _NewCSF = _ReceivedPanel.gameObject.AddComponent<ContentSizeFitter>();
            _NewCSF.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            _NewCSF.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        private void _SetupVerticalLayoutGroup(VerticalLayoutGroup _VLG, bool _IsBottom)
        {
            _VLG.padding.left = 0;
            _VLG.padding.right = 0;
            _VLG.padding.top = 0;
            _VLG.padding.bottom = 0;
            _VLG.spacing = 30;
            _VLG.childAlignment = TextAnchor.MiddleCenter;
            _VLG.reverseArrangement = _IsBottom;
            _VLG.childControlWidth = false;
            _VLG.childControlHeight = false;
            _VLG.childScaleWidth = true;
            _VLG.childScaleHeight = true;
            _VLG.childForceExpandWidth = true;
            _VLG.childForceExpandHeight = true;
        }

        private void _SetupHorizontalLayoutGroup(HorizontalLayoutGroup _HLG, bool _IsRight)
        {
            if (_IsRight)
            {
                _HLG.padding.left = 0;
                _HLG.padding.right = 50;
            }
            else
            {
                _HLG.padding.left = 50;
                _HLG.padding.right = 0;
            }
            
            _HLG.padding.top = 0;
            _HLG.padding.bottom = 0;
            _HLG.spacing = 100;
            _HLG.childAlignment = TextAnchor.MiddleCenter;
            _HLG.reverseArrangement = _IsRight;
            _HLG.childControlWidth = false;
            _HLG.childControlHeight = false;
            _HLG.childScaleWidth = true;
            _HLG.childScaleHeight = true;
            _HLG.childForceExpandWidth = true;
            _HLG.childForceExpandHeight = true;
        }

        //--------
        #endregion

        #region VISUAL_SETTINGS_PROCESSING
        //--------------------------------

        public void _ApplyVisualSettings()
        {
            Transform[] _Transforms = GetComponentsInChildren<Transform>(true);

            foreach (Transform _Transform in _Transforms)
            {
                if (_Transform.CompareTag("ADPMainPanel"))
                {
                    _Transform.GetComponent<Image>().color = _ADP_I.MainPanelColor;
                }

                if (_Transform.CompareTag("ADPBackground"))
                {
                    _Transform.GetComponent<Image>().color = _ADP_I.TFBackgroundColor;
                }

                if (_Transform.CompareTag("ADPScrollerOverlay"))
                {
                    _Transform.GetComponent<Image>().color = _ADP_I.ScrollerOverlayColor;
                }

                if (_Transform.CompareTag("ADPLabel"))
                {
                    if (_Transform.gameObject.activeSelf)
                    {
                        _Transform.GetComponent<Text>().color = _ADP_I.LabelColor;
                    }
                    else
                    {
                        _Transform.gameObject.SetActive(true);
                        _Transform.GetComponent<Text>().color = _ADP_I.LabelColor;
                        _Transform.gameObject.SetActive(false);
                    }
                }

                if (_Transform.CompareTag("ADPText"))
                {
                    _Transform.GetComponent<Text>().color = _ADP_I.TFTextColor;
                }

                if (_Transform.CompareTag("ADPTextTMP"))
                {
                    _Transform.GetComponent<TMP_Text>().color = _ADP_I.TFTextColor;
                }

                if (_Transform.CompareTag("ADPLabelTMP"))
                {
                    if (_Transform.gameObject.activeSelf)
                    {
                        _Transform.GetComponent<TMP_Text>().color = _ADP_I.LabelColor;
                    }
                    else
                    {
                        _Transform.gameObject.SetActive(true);
                        _Transform.GetComponent<TMP_Text>().color = _ADP_I.LabelColor;
                        _Transform.gameObject.SetActive(false);
                    }
                }
            }
        }

        public void _DefaultVisualSettings()
        {
            _ADP_I.MainPanelColor = new Color(1F, 1F, 1F, 0.349F);
            _ADP_I.LabelColor = new Color(1F, 1F, 1F, 1F);
            _ADP_I.TFTextColor = new Color(0.1960784F, 0.1960784F, 0.1960784F, 1F);
            _ADP_I.TFBackgroundColor = new Color(1F, 1F, 1F, 1F);
            _ADP_I.ScrollerOverlayColor = new Color(0F, 0F, 0F, 1F);

            _ApplyVisualSettings();
        }

        //--------
        #endregion
    }
}