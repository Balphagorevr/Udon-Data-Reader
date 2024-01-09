using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using VRC.Udon;
using System.Linq;
using System.Collections.Generic;

namespace Balphagore.UdonDataReader.Editor
{
    public class UdonDataReaderConsole : EditorWindow
    {
        private VisualTreeAsset m_VisualTreeAsset = default;

        private UdonSymbol[] loadedMethods;
        private UdonSymbol[] loadedVariables;

        private Dictionary<int, int> matchingSymbols = new Dictionary<int, int>();
        UdonProgramData programData;

        TextField variableSearchField;
        TextField methodSearchField;
        TextField lblProgramName;
        TextField lblSourceCompiler;
        TextField lblSerializedAsset;
        TextField lblSyncMode;
        TextField lblUpdateOrder;
        ObjectField objUdonBehaviour;
        VisualElement variableTable;
        VisualElement methodTable;
        MultiColumnListView mclvVariables;
        MultiColumnListView mclvMethods;
        Image programIcon;

        [MenuItem("Balphagore/Udon Data Reader/Show Reader")]
        public static UdonDataReaderConsole ShowWindow()
        {
            UdonDataReaderConsole wnd = GetWindow<UdonDataReaderConsole>(title: "Udon Data Reader", focus: true);
            wnd.titleContent = new GUIContent("Udon Data Reader");
            wnd.maxSize = new Vector2(1000f, 850f);
            wnd.minSize = wnd.maxSize;
            wnd.titleContent.image = Resources.Load<Texture>("Icons/icon_programSearch");
            return wnd;
        }

        public void CreateGUI()
        {
            if (m_VisualTreeAsset == null)
            {
                m_VisualTreeAsset = Resources.Load<VisualTreeAsset>("UdonDataReaderConsole");
            }

            VisualElement root = rootVisualElement;
            VisualElement uXML = m_VisualTreeAsset.Instantiate();
            root.Add(uXML);

            lblProgramName = root.Query<TextField>("lblProgramName");
            lblSourceCompiler = root.Query<TextField>("lblSourceCompiler");
            lblSerializedAsset = root.Query<TextField>("lblSerializedAsset");
            lblSyncMode = root.Query<TextField>("lblSyncMode");
            lblUpdateOrder = root.Query<TextField>("lblUpdateOrder");
            objUdonBehaviour = root.Query<ObjectField>("objUdonBehaviour");
            variableSearchField = root.Query<TextField>("variableSearchField");
            methodSearchField = root.Query<TextField>("methodSearchField");
            variableTable = root.Query<VisualElement>("variableTable");
            methodTable = root.Query<VisualElement>("methodTable");

            VisualElement iconElement = root.Query<VisualElement>("programIcon"); //root.Query<Image>("programIcon");
            programIcon = new Image();
            iconElement.Add(programIcon);

            #region MultiColumnListView Prep

            loadedMethods = new UdonSymbol[0];
            loadedVariables = new UdonSymbol[0];

            SetupMethodListview();
            SetupVariableListView();
            #endregion

            #region Register Events

            objUdonBehaviour.RegisterValueChangedCallback(e =>
            {
                if (e.newValue == null)
                {
                    ClearView();
                } else
                {
                    ClearView();
                    DisplayProgramData((UdonBehaviour)e.newValue);
                }
            });

            lblSerializedAsset.RegisterCallback<MouseDownEvent>(e => PingAsset(lblSerializedAsset.value));

            variableSearchField.RegisterValueChangedCallback(e =>
            {
                if (!string.IsNullOrEmpty(e.newValue))
                {
                    FilterVariableListviews(e.newValue);
                } else
                {
                    UpdateVariableListview(loadedVariables);
                }
                
            });
            methodSearchField.RegisterValueChangedCallback(e =>
            {
                if (!string.IsNullOrEmpty(e.newValue))
                {
                    FilterMethodListviews(e.newValue);
                } else
                {
                    UpdateMethodListView(loadedMethods);
                }
            });
            #endregion
        }

        private void FindMatchingCells()
        {
            matchingSymbols.Clear();
            // Each method must check for a corresponding return symbol if applicable.
            for (int i = 0; i < mclvMethods.itemsSource.Count; i++)
            {
                var method = (UdonSymbol)mclvMethods.itemsSource[i];

                if (method.returnSymbol == null) continue;

                // Locate the variable that matches the return symbol.
                for (int j = 0; j < mclvVariables.itemsSource.Count; j++)
                {
                    var variable = (UdonSymbol)mclvVariables.itemsSource[j];

                    if (variable.symbolName == method.returnSymbol.symbolName)
                    {
                        if (!matchingSymbols.ContainsKey(j))
                        {
                            matchingSymbols.Add(j, i);
                            break;
                        }
                    }
                }
            }
        }

        private void FocusOnVariable(int variableIndex)
        {
            mclvVariables.ScrollToItem(variableIndex);
            mclvVariables.selectedIndex = variableIndex;
        }

        private void FocusOnMethod(int index)
        {
            mclvMethods.ScrollToItem(index);
            mclvMethods.selectedIndex = index;
        }

        private int FindVariableIndexForMethod(int methodIndex)
        {
            foreach(var pair in matchingSymbols)
            {
                if (pair.Value == methodIndex) return pair.Key;
            }

            return -1;
        }

        private int FindMethodIndexForVariable(int variableIndex)
        {
            if (matchingSymbols.TryGetValue(variableIndex, out int methodIndex))
            {
                return methodIndex;
            }

            return -1;
        }

        private void SetupVariableListView()
        {
            mclvVariables = new MultiColumnListView()
            {
                showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly,
                selectionType = SelectionType.Single
            };
            mclvVariables.style.maxHeight = 250;
            variableTable.Add(mclvVariables);

            // Variable Symbol
            mclvVariables.columns.Add(new Column()
            {
                title = "Symbol Name",
                width= 250,
                bindCell = (e, i) =>
                {
                    VisualElement symbolBody = e.Q<VisualElement>();
                    Image img = new Image();
                    Label lbl = e.Q<Label>();

                    if (symbolBody == null)
                    {
                        symbolBody = new VisualElement();
                        symbolBody.name = "returnSymbolBody";
                        symbolBody.style.flexDirection = FlexDirection.Row;
                        symbolBody.style.flexGrow = 1;

                        lbl = new Label();
                        img = new Image();

                        symbolBody.Add(img);
                        symbolBody.Add(lbl);
                        e.Add(symbolBody);
                    }

                    lbl.text = loadedVariables[i].symbolName;

                    if (matchingSymbols.ContainsKey(i))
                    {

                        img.image = Resources.Load<Texture>("Icons/icon_link");

                        int methodIndex = FindMethodIndexForVariable(i);
                        lbl.RegisterCallback<ClickEvent>(ce => FocusOnMethod(methodIndex));
                    }
                }
            });

            mclvVariables.columns.Add(new Column()
            {
                title = "Data Type",
                width = 100,
                bindCell = (e, i) =>
                {
                    Label lbl = e.Q<Label>();
                    if (lbl == null)
                    {
                        lbl = new Label();
                        e.Add(lbl);
                    };
                    lbl.text = loadedVariables[i].dataType;
                }
            });

            mclvVariables.columns.Add(new Column()
            {
                title = "Access Modifier",
                width = 110,
                bindCell = (e, i) =>
                {
                    Label lbl  = e.Q<Label>();

                    if (lbl == null)
                    {
                        lbl = new Label();
                        e.Add(lbl);
                    };
                    lbl.text = loadedVariables[i].accessModifier.ToString();
                }
            });

            mclvVariables.columns.Add(new Column()
            {
                title = "Address",
                width = 75,
                optional = true,
                bindCell = (e, i) =>
                {
                    IntegerField intField = e.Q<IntegerField>();
                    if (intField == null)
                    {
                        intField = new IntegerField();
                        e.Add(intField);
                    }
                    intField.SetEnabled(false);
                    intField.value = (int)loadedVariables[i].address;
                }
            });

            mclvVariables.columns.Add(new Column()
            {
                title = "Network Sync",
                width = 100,
                bindCell = (e, i) =>
                {
                    Toggle t = e.Q<Toggle>();
                    if (t == null)
                    {
                        t = new Toggle();
                        e.style.justifyContent = Justify.Center;
                        e.Add(t);
                    }
                    t.SetEnabled(false);
                    t.value = loadedVariables[i].synced.Value;
                }
            });

            mclvVariables.columns.Add(new Column()
            {
                title = "Exported",
                width = 100,
                bindCell = (e, i) =>
                {
                    Toggle t = e.Q<Toggle>();
                    if (t == null)
                    {
                        t = new Toggle();
                        e.style.justifyContent = Justify.Center;
                        e.Add(t);
                    }
                    t.SetEnabled(false);
                    t.value = loadedVariables[i].isExported;
                },
            });

        }

        private void SetupMethodListview()
        {
            mclvMethods = new MultiColumnListView()
            {
                showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly,
                selectionType = SelectionType.Single
            };

            mclvMethods.style.maxHeight = 250;
            methodTable.Add(mclvMethods);

            mclvMethods.columns.Add(new Column()
            {
                title = "Symbol Name",
                width = 250,
                bindCell = (e, i) =>
                {
                    Label lbl = e.Q<Label>();
                    if (lbl == null)
                    {
                        lbl = new Label();
                        e.Add(lbl);
                    }
                    lbl.text = loadedMethods[i].symbolName;
                    SpawnContextOption(e, loadedMethods[i].symbolName);
                }
            });

            mclvMethods.columns.Add(new Column()
            {
                title = "Access Modifier",
                width = 110,
                bindCell = (e, i) =>
                {
                    Label lbl = e.Q<Label>();
                    if (lbl == null)
                    {
                        lbl = new Label();
                        e.Add(lbl);
                    }
                    lbl.text = loadedMethods[i].accessModifier.ToString();
                }
            });

            mclvMethods.columns.Add(new Column()
            {
                title = "Address",
                width = 75,
                optional = true,
                bindCell = (e, i) =>
                {
                    IntegerField intField = e.Q<IntegerField>();
                    if (intField == null)
                    {
                        intField = new IntegerField();
                        e.Add(intField);
                    }
                    intField.SetEnabled(false);
                    intField.value = (int)loadedMethods[i].address;
                }
            });

            mclvMethods.columns.Add(new Column()
            {
                title = "Exported",
                width = 75,
                bindCell = (e, i) =>
                {
                    Toggle t = e.Q<Toggle>();
                    if (t == null)
                    {
                        t = new Toggle();
                        e.Add(t);
                    }
                    t.SetEnabled(false);
                    t.value = loadedMethods[i].isExported;
                },
            });

            mclvMethods.columns.Add(new Column()
            {
                // Return Symbol
                title = "Return Symbol",
                width = 250,
                bindCell = (e, i) =>
                {
                    string returnSymbolName = loadedMethods[i].returnSymbol == null ? "void" : loadedMethods[i].returnSymbol.symbolName;

                    VisualElement symbolBody = e.Q<VisualElement>("returnSymbolBody");
                    Image img = new Image();
                    Label lbl = e.Q<Label>();

                    if (symbolBody == null)
                    {
                        symbolBody = new VisualElement();
                        symbolBody.name = "returnSymbolBody";
                        symbolBody.style.flexDirection = FlexDirection.Row;
                        symbolBody.style.flexGrow = 1;

                        lbl = new Label();
                        img = new Image();

                        symbolBody.Add(img);
                        symbolBody.Add(lbl);
                        e.Add(symbolBody);
                    }

                    lbl.text = returnSymbolName;

                    if (matchingSymbols.ContainsValue(i))
                    {
                        if (img != null)
                        {
                            img.image = Resources.Load<Texture>("Icons/icon_link");
                        }

                        int variableIndex = FindVariableIndexForMethod(i);
                        lbl.RegisterCallback<ClickEvent>(ce => FocusOnVariable(variableIndex));
                    }
                }
            });

            mclvMethods.columns.Add(new Column()
            {
                title = "Return Symbol Type",
                width = 130,
                bindCell = (e, i) =>
                {
                    string returnSymbolType = loadedMethods[i].returnSymbol == null ? "void" : loadedMethods[i].returnSymbol.dataType;

                    Label lbl = e.Q<Label>();
                    if (lbl == null)
                    {
                        lbl = new Label();
                        e.Add(lbl);
                    }
                    lbl.text = returnSymbolType;
                }
            });
        }

        private void FilterVariableListviews(string text)
        {
            var filteredVariables = loadedVariables.Where(v => v.symbolName.ToLower().Contains(text.ToLower())).ToArray();
            FindMatchingCells();
            UpdateVariableListview(filteredVariables);
            
        }

        public void FilterMethodListviews(string text)
        {
            var filteredMethods = loadedMethods.Where(v => v.symbolName.ToLower().Contains(text.ToLower())).ToArray();
            FindMatchingCells();
            UpdateMethodListView(filteredMethods);
            FindMatchingCells();
        }

        private void UpdateMethodListView(UdonSymbol[] methods)
        {
            mclvMethods.itemsSource = methods;
            mclvMethods.Rebuild();
        }

        private void UpdateVariableListview(UdonSymbol[] variables)
        {
            mclvVariables.itemsSource = variables;
            mclvVariables.Rebuild();
        }

        private void ClearView()
        {
            lblProgramName.value = default;
            lblSourceCompiler.value = default;
            lblSerializedAsset.value = default;
            lblSyncMode.value = default;
            lblUpdateOrder.value = default;
        }

        private void PingAsset(string assetName)
        {
            string[] guids = AssetDatabase.FindAssets(assetName);

            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));

                if (asset != null) EditorGUIUtility.PingObject(asset);
            }
        }

        private void DisplayProgramData(UdonBehaviour udonBehaviour)
        {
            programData = UdonDataReader.ReadUdonProgram(udonBehaviour);

            lblProgramName.value = programData.programName;
            lblSourceCompiler.value = programData.sourceCompiler;
            lblSerializedAsset.value = programData.serializedAssetName;
            lblSyncMode.value = programData.syncMode.ToString();
            lblUpdateOrder.value = programData.updateOrder.ToString();


            switch(programData.sourceCompiler)
            {
                case "UdonSharpProgramAsset":
                    programIcon.image = AssetDatabase.LoadAssetAtPath<Texture>("Packages/com.vrchat.worlds/Integrations/UdonSharp/Editor/Resources/UdonSharpProgramAsset icon.png"); //Resources.Load<Texture>("UdonSharpProgramAsset icon");
                    break;
                case "UdonGraphProgramAsset":
                    programIcon.image = AssetDatabase.LoadAssetAtPath<Texture>("Packages/com.vrchat.worlds/Editor/Udon/Resources/UdonLogoAlpha.png");
                    break;
            }

            loadedMethods = programData.GetMethods(true).ToArray();
            loadedVariables = programData.GetVariables(true).ToArray();

            UpdateMethodListView(loadedMethods);
            UpdateVariableListview(loadedVariables);
            FindMatchingCells();
        }

        private void SpawnContextOption(VisualElement cell, string symbolName)
        {
            cell.RegisterCallback<ContextClickEvent>(e =>
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Copy Symbol Name"), false, () => EditorGUIUtility.systemCopyBuffer = symbolName);
                menu.ShowAsContext();
                e.StopPropagation();
            });
        }
    }
}

