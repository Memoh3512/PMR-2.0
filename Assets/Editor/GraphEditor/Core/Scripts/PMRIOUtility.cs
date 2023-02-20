using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PMR.GraphEditor.Utilities
{

    using Save;
    using ScriptableObjects;
    using Elements;
    
    public static class PMRIOUtility
    {
        private static PMRGraphView graphView;
        private static string graphFileName;
        private static string graphFolderName;
        private static string containerFolderPath;

        private static List<PMRGroup> groups;
        private static List<PMRNode> nodes;

        private static Dictionary<string, PMRGroupSO> createdGroups;
        private static Dictionary<string, PMRGraphSO> createdNodes;

        public static void Initialize(PMRGraphView pmrGraphView, string graphFolder, string graphName)
        {

            graphView = pmrGraphView; 
            
            graphFileName = graphName;
            graphFolderName = graphFolder;
            containerFolderPath = $"Assets/{graphFolderName}/Graphs/{graphFileName}";

            groups = new List<PMRGroup>();
            nodes = new List<PMRNode>();

            createdGroups = new Dictionary<string, PMRGroupSO>();
            createdNodes = new Dictionary<string, PMRGraphSO>();
        }
        
        #region Save
        
        public static void Save()
        {
            CreateStaticFolders();

            GetElementsFromGraphView();

            PMRGraphSaveDataSO graphData = CreateAsset<PMRGraphSaveDataSO>($"Assets/Editor/Graphs/{graphFolderName}", $"{graphFileName}Graph");
            graphData.Initialize(graphFileName);

            PMRContainerSO container = CreateAsset<PMRContainerSO>(containerFolderPath, graphFileName);
            container.Initialize(graphFileName);

            SaveGroups(graphData, container);
            SaveNodes(graphData, container);
            
            SaveAsset(graphData);
            SaveAsset(container);
        }

        #endregion
        
        #region Creation
        
        private static void CreateStaticFolders()
        {
            CreateFolder("Assets/Editor/", "Graphs");
            
            CreateFolder("Assets", graphFolderName);
            CreateFolder($"Assets/{graphFolderName}", "Graphs");
            
            CreateFolder($"Assets/{graphFolderName}/Graphs", graphFileName);
            CreateFolder(containerFolderPath, "Global");
            CreateFolder(containerFolderPath, "Groups");
        }

        #endregion
        
        #region Utility Methods
        private static void CreateFolder(string path, string folderName)
        {
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            {
                return;
            }
            AssetDatabase.CreateFolder(path, folderName);
        }
        
        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}";

            T asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, fullPath);   
            }
            return asset;
        }
        
        private static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        #endregion
        
        #region Fetch
        
        private static void GetElementsFromGraphView()
        {
            
            graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is PMRNode node)
                {
                    nodes.Add(node);
                    return;
                }

                if (graphElement is PMRGroup group)
                {
                    groups.Add(group);
                    return;
                }
                
            });
        }
        
        #endregion
        
        #region Groups
        
        private static void SaveGroups(PMRGraphSaveDataSO graphData, PMRContainerSO container)
        {
            foreach (PMRGroup group in groups)
            {
                SaveGroupToGraph(group, graphData);
                SaveGroupToScriptableObject(group, container);
            }
        }

        private static void SaveGroupToGraph(PMRGroup group, PMRGraphSaveDataSO graphData)
        {
            PMRGroupSaveData groupData = (PMRGroupSaveData) group.CreateEditorSaveData();
            graphData.Groups.Add(groupData);
        }
        
        private static void SaveGroupToScriptableObject(PMRGroup group, PMRContainerSO container)
        {
            string groupName = group.title;
            CreateFolder($"{containerFolderPath}/Groups", groupName);
            
            PMRGroupSO groupSO =  (PMRGroupSO)group.CreateRuntimeSaveData($"{containerFolderPath}/Groups/{groupName}", groupName);
            
            createdGroups.Add(group.ID, groupSO);
            
            container.Groups.Add(groupSO, new List<PMRGraphSO>());

            SaveAsset(groupSO);
        }
        
        #endregion
        
        #region Nodes
        
        private static void SaveNodes(PMRGraphSaveDataSO graphData, PMRContainerSO container)
        {
            
            //create scriptable objects for each nodes
            foreach (PMRNode node in nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node, container);
            }
            
            //update connections with created SOs
            UpdateNodeConnections();

        }

        private static void SaveNodeToGraph(PMRNode node, PMRGraphSaveDataSO graphData)
        {

            //TODO faire une fonction CreateSaveData dans chaque type de Node qui va save la data a place de faire le meme objet pour chacun (ep28 30:45)
            PMRNodeSaveData nodeData = node.CreateEditorSaveData();
            graphData.Nodes.Add(nodeData);

        }
        
        private static void SaveNodeToScriptableObject(PMRNode node, PMRContainerSO container)
        {
            PMRGraphSO graphSO;
            if (node.Group != null)
            {
                graphSO = node.CreateRuntimeSaveData($"{containerFolderPath}/Groups/{node.Group.title}", node.NodeName);
                
                container.Groups.AddItem(createdGroups[node.Group.ID], graphSO);
            }
            else
            {
                graphSO = node.CreateRuntimeSaveData($"{containerFolderPath}/Global/", node.NodeName);
                container.UngroupedNodes.Add(graphSO);
            }
            
            createdNodes.Add(node.ID, graphSO);
            
            SaveAsset(graphSO);
        }
        
        private static void UpdateNodeConnections()
        {
            foreach (PMRNode node in nodes)
            {
                PMRGraphSO nodeSO = createdNodes[node.ID];

                node.UpdateConnection(nodeSO, createdNodes);
                
                SaveAsset(nodeSO);
            }
        }

        #endregion
        
        #region Collection Utility

        public static void AddItem<K, V>(this SerializableDictionary<K, List<V>> serializableDictionary, K key, V value)
        {
            if (serializableDictionary.ContainsKey(key))
            {
                serializableDictionary[key].Add(value);
                return;
            }
            serializableDictionary.Add(key, new List<V>(){value});
        }
        
        #endregion
        
    }
}
