using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace PMR
{
    public class RichTextProcessor : MonoBehaviour
    {

        struct CustomTag
        {
            public string tag;
            public string entryTag;
            public string exitTag;
            public CustomTag(string _tag, string _entryTag, string _exitTag)
            {
                tag = _tag;
                entryTag = _entryTag;
                exitTag = _exitTag;
            }
        }
        
        private TextMeshProUGUI textComp;
        public TextSettings textSettings;

        private Dictionary<int, Vector3> posBases = new Dictionary<int, Vector3>();

        private static readonly CustomTag[] CustomTags =
        {
            new CustomTag("w", "<b><font=\"pmdialog SDF\" material=\"pmdialog_Rainbow\"><w>", "</b></w></font>"),
        };

        private static readonly string[] CustomLinkTags =
        {
            "t",
            "g",
            "w",
            "star",
        };
        
        private static string ProcessTags(string text)
        {
            foreach (CustomTag tag in CustomTags)
            {
                text = text.Replace($"<{tag.tag}>", tag.entryTag);
                text = text.Replace($"</{tag.tag}>", tag.exitTag);
            }
            return text;
        }

        private static string ProcessLinkTags(string text)
        {
            foreach (string tag in CustomLinkTags)
            {
                text = text.Replace($"<{tag}>", $"<link=\"{tag}\">");
                text = text.Replace($"</{tag}>", "</link>");
            }
            return text;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            textComp = GetComponent<TextMeshProUGUI>();
            
            //process tags
            string text = textComp.text;
            text = ProcessTags(text);
            textComp.text = ProcessLinkTags(text);
            
            textComp.ForceMeshUpdate();

            if (textSettings == null)
            {
                Debug.LogError("No settings attached to text processor!");
                gameObject.SetActive(false);
                return;
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            if (textComp is null || textSettings is null) return;

            //process custom link tags
            var info = textComp.textInfo;
            
            foreach (var linkInfo in info.linkInfo)
            {
                switch (linkInfo.GetLinkID())
                {
                    case "w": Wave(linkInfo); break;
                }
            }
            
            textComp.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }
        
        #region Wave

        void Wave(TMP_LinkInfo linkInfo)
        {

            if (textSettings.waveMaterial == null)
            {
                Debug.LogError("No material set in settings for wave effect!");
                return;
            }
            
            float time = Time.realtimeSinceStartup;
            // Loops all characters containing the rainbow link.
            for (int i = linkInfo.linkTextfirstCharacterIndex; i < linkInfo.linkTextfirstCharacterIndex + linkInfo.linkTextLength; i++)
            {
                TMP_CharacterInfo charInfo = textComp.textInfo.characterInfo[i]; // Gets info on the current character
                
                int materialIndex = charInfo.materialReferenceIndex; // Gets the index of the current character material

                Vector3[] vertices = textComp.textInfo.meshInfo[materialIndex].vertices;
                float sin = Mathf.Sin((time * textSettings.waveLength) + i);
                Vector3 offset = new Vector2(0, sin) * textSettings.waveHeight;

                // Loop all vertexes of the current characters
                for (int j = 0; j < 4; j++)
                {
                    if (charInfo.character == ' ') continue; // Skips spaces
                    int vertexIndex = charInfo.vertexIndex + j;
                    if (!posBases.ContainsKey(vertexIndex)) posBases.Add(vertexIndex, vertices[vertexIndex]);
                    
                    // Sets the new effects
                    vertices[vertexIndex] = posBases[vertexIndex] + offset;
                }
            }  
        }
        
        #endregion
    }
}
