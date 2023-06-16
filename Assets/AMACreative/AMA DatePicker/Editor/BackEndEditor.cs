#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AMADatePicker
{
    public class BackEndEditor : MonoBehaviour
    {
        #region VARIABLES
        //---------------

        private static string _ADPPrefabPath = "Assets/AMACreative/AMA DatePicker/Prefabs/AMA DatePicker.prefab";
        private static string _ADPTMPPrefabPath = "Assets/AMACreative/AMA DatePicker/Prefabs/AMA DatePicker (TMP).prefab";
        private static string _CanvasPrefabPath = "Assets/AMACreative/AMA DatePicker/Prefabs/Canvas.prefab";
        private static string _EventSystemPrefabPath = "Assets/AMACreative/AMA DatePicker/Prefabs/EventSystem.prefab";

        //--------
        #endregion

        #region METHODS
        //-------------

        [MenuItem("GameObject/UI/AMA DatePicker", false, 10)]
        public static void _SpawnADPInstance(MenuCommand _MC)
        {
            GameObject _ADPPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(_ADPPrefabPath, typeof(GameObject));
            GameObject _InstADPGO = Instantiate(_ADPPrefab);
            _InstADPGO.name = "AMA DatePicker";

            GameObject _SelectedGO = (GameObject)_MC.context;

            if (_SelectedGO != null)
            {
                if (_CheckForCanvasInParent(_SelectedGO))
                {
                    GameObjectUtility.SetParentAndAlign(_InstADPGO, _SelectedGO);
                }
                else
                {
                    _SearchForCanvasInScene(_InstADPGO);
                }
            }
            else
            {
                _SearchForCanvasInScene(_InstADPGO);
            }

            Undo.RegisterCreatedObjectUndo(_InstADPGO, "Create " + _InstADPGO.name);
            Selection.activeObject = _InstADPGO;
        }

        [MenuItem("GameObject/UI/AMA DatePicker - TextMeshPro", false, 10)]
        public static void _SpawnADPTMPInstance(MenuCommand _MC)
        {
            GameObject _ADPPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(_ADPTMPPrefabPath, typeof(GameObject));
            GameObject _InstADPGO = Instantiate(_ADPPrefab);
            _InstADPGO.name = "AMA DatePicker";
            
            GameObject _SelectedGO = (GameObject) _MC.context;

            if (_SelectedGO != null)
            {
                if (_CheckForCanvasInParent(_SelectedGO))
                {
                    GameObjectUtility.SetParentAndAlign(_InstADPGO, _SelectedGO);
                }
                else
                {
                    _SearchForCanvasInScene(_InstADPGO);
                }
            }
            else
            {
                _SearchForCanvasInScene(_InstADPGO);
            }

            Undo.RegisterCreatedObjectUndo(_InstADPGO, "Create " + _InstADPGO.name);
            Selection.activeObject = _InstADPGO;
        }

        private static bool _CheckForCanvasInParent(GameObject _SelectedGO)
        {
            Transform _Parent = _SelectedGO.transform;

            do
            {
                if (_Parent.gameObject.GetComponent<Canvas>())
                {
                    return true;
                }

                _Parent = _Parent.parent;
            }
            while (_Parent != null);

            return false;
        }

        private static void _SearchForCanvasInScene(GameObject _InstADPGO)
        {
            if (FindObjectOfType<Canvas>() != null)
            {
                GameObject _GOWithCanvas = FindObjectOfType<Canvas>().gameObject;
                GameObjectUtility.SetParentAndAlign(_InstADPGO, _GOWithCanvas);
            }
            else
            {
                GameObject _CanvasPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(_CanvasPrefabPath, typeof(GameObject));
                GameObject _InstCanvasGO = Instantiate(_CanvasPrefab);
                _InstCanvasGO.name = "Canvas";

                if (FindObjectOfType<EventSystem>() == null)
                {
                    GameObject _EventSystemPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(_EventSystemPrefabPath, typeof(GameObject));
                    GameObject _InstEventSystemGO = Instantiate(_EventSystemPrefab);
                    _InstEventSystemGO.name = "EventSystem";
                }

                GameObjectUtility.SetParentAndAlign(_InstADPGO, _InstCanvasGO);
            }
        }

        //--------
        #endregion
    }
}
#endif