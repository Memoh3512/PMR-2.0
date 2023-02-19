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

        public static void Initialize(PMRGraphView pmrGraphView, string graphFolder, string graphName)
        {

            graphView = pmrGraphView; 
            
            graphFileName = graphName;
            graphFolderName = graphFolder;
            containerFolderPath = $"Assets/{graphFolderName}/Graphs/{graphFileName}";

            groups = new List<PMRGroup>();
            nodes = new List<PMRNode>();
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
        
        private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
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
            PMRGroupSaveData groupData = new PMRGroupSaveData()
            {
                ID = group.ID,
                Name = group.title,
                Position = group.GetPosition().position
            };
            
            graphData.Groups.Add(groupData);
        }
        
        private static void SaveGroupToScriptableObject(PMRGroup group, PMRContainerSO container)
        {
            string groupName = group.title;
            CreateFolder($"{containerFolderPath}/Groups", groupName);

            PMRGroupSO groupSO = CreateAsset<PMRGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);
            groupSO.Initialize(groupName);
            
            container.Groups.Add(groupSO, new List<PMRGraphSO>());

            SaveAsset(groupSO);
        }
        
        #endregion
        
        #region Nodes
        
        private static void SaveNodes(PMRGraphSaveDataSO graphData, PMRContainerSO container)
        {
            foreach (PMRNode node in nodes)
            {
                SaveNodeToGraph(node, graphData);
            }
        }

        private static void SaveNodeToGraph(PMRNode node, PMRGraphSaveDataSO graphData)
        {
            //TODO faire une fonction CreateSaveData dans chaque type de Node qui va save la data a place de faire le meme objet pour chacun (ep28 30:45)
        }

        #endregion
        
    }
}
