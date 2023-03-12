using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
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

        //save
        private static Dictionary<string, PMRGroupSO> createdGroups;
        private static Dictionary<string, PMRGraphSO> createdNodes;

        //load
        private static Dictionary<string, PMRGroup> loadedGroups;
        private static Dictionary<string, PMRNode> loadedNodes;

        private static JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public static void Initialize(PMRGraphView pmrGraphView, string graphFolder, string graphName)
        {
            graphView = pmrGraphView;

            graphFileName = graphName;
            graphFolderName = graphFolder;
            containerFolderPath = $"Assets/{graphFolderName}/Graphs/{graphFileName}";

            groups = new List<PMRGroup>();
            nodes = new List<PMRNode>();

            //save
            createdGroups = new Dictionary<string, PMRGroupSO>();
            createdNodes = new Dictionary<string, PMRGraphSO>();

            //load
            loadedGroups = new Dictionary<string, PMRGroup>();
            loadedNodes = new Dictionary<string, PMRNode>();
        }

        #region Save

        public static void Save()
        {
            CreateStaticFolders();

            GetElementsFromGraphView();

            PMRGraphSaveDataSO graphData =
                CreateAsset<PMRGraphSaveDataSO>($"Assets/Editor/Graphs/{graphFolderName}", $"{graphFileName}Graph");
            graphData.Initialize(graphFileName);

            PMRContainerSO container = CreateAsset<PMRContainerSO>(containerFolderPath, graphFileName);
            container.Initialize(graphFileName);

            SaveGroups(graphData, container);
            SaveNodes(graphData, container);

            SaveAsset(graphData);
            SaveAsset(container);
        }

        #endregion

        #region Load

        public static void Load()
        {
            PMRGraphSaveDataSO graphData =
                LoadAsset<PMRGraphSaveDataSO>($"Assets/Editor/Graphs/{graphFolderName}", graphFileName);

            if (graphData == null)
            {
                EditorUtility.DisplayDialog(
                    "Could not load the file.",
                    "The file at the following path could not be found:\n\n" +
                    $"Assets/Editor/Graphs/{graphFileName}\n\n" +
                    "Make sure you chose the right file and it's placed at the folder path above.",
                    "Ok"
                );
                return;
            }

            PMRGraphEditorWindow.UpdateFileName(graphData.FileName);

            LoadGroups(graphData.Groups);
            LoadNodes(graphData.Nodes);
            LoadNodesConnections();
        }

        private static void LoadNodes(List<string> graphNodes)
        {
            foreach (string nodeJson in graphNodes)
            {
                if (string.IsNullOrEmpty(nodeJson))
                {
                    Debug.LogError("node JSON string is null or empty while loading!!! This is not supposed to happen");
                    continue;
                }
                
                PMRNodeSaveData nodeData = JsonConvert.DeserializeObject<PMRNodeSaveData>(nodeJson, serializerSettings);
                if (nodeData == null)
                {
                    Debug.LogError("Unknown JSON format while deserializing Node!");
                    continue;
                }
                Debug.Log($"TYPE: {nodeData.GetType()}");

                PMRNode node = nodeData.LoadData(graphView);
                node.Draw();
                graphView.AddElement(node);
                
                loadedNodes.Add(node.ID, node);
                
                if (string.IsNullOrEmpty(nodeData.GroupID)) continue;
                
                PMRGroup group = loadedGroups[nodeData.GroupID];
                node.Group = group;
                
                group.AddElement(node);
            }
        }

        private static void LoadGroups(List<PMRGroupSaveData> graphGroups)
        {
            foreach (PMRGroupSaveData groupData in graphGroups)
            {
                PMRGroup group = graphView.CreateGroup(groupData.Name, groupData.Position.ToVector2());
                group.ID = groupData.ID;

                loadedGroups.Add(group.ID, group);
            }
        }

        private static void LoadNodesConnections()
        {
            foreach (PMRNode loadedNode in loadedNodes.Values)
            {
                loadedNode.LoadConnections(loadedNodes);
                loadedNode.RefreshPorts();
            }
        }

        #endregion

        #region Creation

        private static void CreateStaticFolders()
        {
            CreateFolder("Assets/Editor", "Graphs");
            CreateFolder("Assets/Editor/Graphs", graphFolderName);

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
            string fullPath = $"{path}/{assetName}.asset";

            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }

        private static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        private static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void RemoveFolder(string fullPath)
        {
            FileUtil.DeleteFileOrDirectory($"{fullPath}.meta");
            FileUtil.DeleteFileOrDirectory($"{fullPath}/");
        }

        private static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
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
            List<string> groupIDs = new List<string>();

            foreach (PMRGroup group in groups)
            {
                SaveGroupToGraph(group, graphData);
                SaveGroupToScriptableObject(group, container);

                groupIDs.Add(group.ID);
            }

            UpdateOldGroups(groupIDs, graphData);
        }

        private static void SaveGroupToGraph(PMRGroup group, PMRGraphSaveDataSO graphData)
        {
            PMRGroupSaveData groupData = (PMRGroupSaveData)group.CreateEditorSaveData();
            graphData.Groups.Add(groupData);
        }

        private static void SaveGroupToScriptableObject(PMRGroup group, PMRContainerSO container)
        {
            string groupFileName = group.ID;
            CreateFolder($"{containerFolderPath}/Groups", groupFileName);

            PMRGroupSO groupSO =
                (PMRGroupSO)group.CreateRuntimeSaveData($"{containerFolderPath}/Groups/{groupFileName}", groupFileName);

            createdGroups.Add(group.ID, groupSO);

            container.Groups.Add(groupSO, new List<PMRGraphSO>());

            SaveAsset(groupSO);
        }

        private static void UpdateOldGroups(List<string> currentGroupIDs, PMRGraphSaveDataSO graphData)
        {
            if (graphData.OldGroupIDs != null && graphData.OldGroupIDs.Count > 0)
            {
                List<string> groupsToRemove = graphData.OldGroupIDs.Except(currentGroupIDs).ToList();

                foreach (string groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
                }
            }

            graphData.OldGroupIDs = new List<string>(currentGroupIDs);
        }

        #endregion

        #region Nodes

        private static void SaveNodes(PMRGraphSaveDataSO graphData, PMRContainerSO container)
        {
            SerializableDictionary<string, List<string>> groupedNodeIDs =
                new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeIDs = new List<string>();

            //create scriptable objects for each nodes
            foreach (PMRNode node in nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node, container);

                if (node.Group != null)
                {
                    groupedNodeIDs.AddItem(node.Group.ID, node.ID);
                    continue;
                }

                ungroupedNodeIDs.Add(node.ID);
            }

            //update connections with created SOs
            UpdateNodeConnections();
            UpdateOldGroupedNodes(groupedNodeIDs, graphData);
            UpdateOldUngroupedNodes(ungroupedNodeIDs, graphData);
        }

        private static void SaveNodeToGraph(PMRNode node, PMRGraphSaveDataSO graphData)
        {
            PMRNodeSaveData nodeData = node.CreateEditorSaveData();
            string nodeJson = JsonConvert.SerializeObject(nodeData, Formatting.None, serializerSettings);
            graphData.Nodes.Add(nodeJson);
        }

        private static void SaveNodeToScriptableObject(PMRNode node, PMRContainerSO container)
        {
            PMRGraphSO graphSO;
            if (node.Group != null)
            {
                graphSO = node.CreateRuntimeSaveData($"{containerFolderPath}/Groups/{node.Group.ID}", node.ID);

                container.Groups.AddItem(createdGroups[node.Group.ID], graphSO);
            }
            else
            {
                graphSO = node.CreateRuntimeSaveData($"{containerFolderPath}/Global/", node.ID);
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

                node.SaveConnections(nodeSO, createdNodes);

                SaveAsset(nodeSO);
            }
        }

        private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeIDs, PMRGraphSaveDataSO graphData)
        {
            if (graphData.OldNodeIDs != null && graphData.OldNodeIDs.Count > 0)
            {
                List<string> nodesToRemove = graphData.OldNodeIDs.Except(currentUngroupedNodeIDs).ToList();

                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Global/", nodeToRemove);
                }
            }

            graphData.OldNodeIDs = new List<string>(currentUngroupedNodeIDs);
        }

        private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeIDs,
            PMRGraphSaveDataSO graphData)
        {
            if (graphData.OldGroupedNodeIDs != null && graphData.OldGroupedNodeIDs.Count > 0)
            {
                foreach (KeyValuePair<string, List<string>> oldGroupedNode in graphData.OldGroupedNodeIDs)
                {
                    List<string> nodesToRemove = new List<string>();

                    if (currentGroupedNodeIDs.ContainsKey(oldGroupedNode.Key))
                    {
                        nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNodeIDs[oldGroupedNode.Key]).ToList();
                    }

                    foreach (string nodeToRemove in nodesToRemove)
                    {
                        RemoveAsset($"{containerFolderPath}/Groups/{oldGroupedNode.Key}", nodeToRemove);
                    }
                }
            }

            graphData.OldGroupedNodeIDs = new SerializableDictionary<string, List<string>>(currentGroupedNodeIDs);
        }

        #endregion

        #region Collection Utility

        private static void AddItem<K, V>(this SerializableDictionary<K, List<V>> serializableDictionary, K key, V value)
        {
            if (serializableDictionary.ContainsKey(key))
            {
                serializableDictionary[key].Add(value);
                return;
            }

            serializableDictionary.Add(key, new List<V>() { value });
        }

        #endregion
    }
}