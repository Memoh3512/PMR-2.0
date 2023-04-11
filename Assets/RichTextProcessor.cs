using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace PMR
{
    public class RichTextProcessor : MonoBehaviour
    {

        private TextMeshProUGUI textComp;
        public TextSettings textSettings;

        private Dictionary<int, Vector3> posBases = new Dictionary<int, Vector3>();

        private static readonly string[] CustomLinkTags =
        {
            "t",
            "g",
            "w",
            "star",
        };

        public static string ProcessTextTags(string text)
        {
            string tagStart, tagEnd, linkStart;
            foreach (string tag in CustomLinkTags)
            {
                tagStart = $"<{tag}>";
                tagEnd = $"</{tag}>";
                linkStart = $"<link=\"{tag}\">";
                text = text.Replace(tagStart, linkStart);
                text = text.Replace(tagEnd, "</link>");
            }
            return text;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            textComp = GetComponent<TextMeshProUGUI>();
            textComp.text = ProcessTextTags(textComp.text);
            textComp.ForceMeshUpdate();
            
            originalTextInfo = (TMP_MeshInfo[])textComp.textInfo.meshInfo.Clone();
        }

        private TMP_MeshInfo[] originalTextInfo;

        // Update is called once per frame
        void Update()
        {
            if (textComp is null || textSettings is null) return;

            //process custom link tags
            var info = textComp.textInfo;

            //TODO Build table avec valeurs originales selon le material index de couleurs et vertex positions
            
            foreach (var linkInfo in info.linkInfo)
            {
                switch (linkInfo.GetLinkID())
                {
                    case "star": Wave(linkInfo); break;
                }
            }
            
            textComp.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }
        
        #region Wave

        void Wave(TMP_LinkInfo linkInfo)
        {
            float time = Time.realtimeSinceStartup;
            // Loops all characters containing the rainbow link.
            for (int i = linkInfo.linkTextfirstCharacterIndex; i < linkInfo.linkTextfirstCharacterIndex + linkInfo.linkTextLength; i++)
            {
                TMP_CharacterInfo charInfo = textComp.textInfo.characterInfo[i]; // Gets info on the current character
                int materialIndex = charInfo.materialReferenceIndex; // Gets the index of the current character material

                Color32[] newColors = originalTextInfo[materialIndex].colors32;
                Vector3[] newVertices = originalTextInfo[materialIndex].vertices;

                float sin = Mathf.Sin((time * textSettings.waveLength) + i);
                Vector3 offset = new Vector2(0, sin) * textSettings.waveHeight;
                // Loop all vertexes of the current characters
                for (int j = 0; j < 4; j++)
                {
                    if (charInfo.character == ' ') continue; // Skips spaces
                    int vertexIndex = charInfo.vertexIndex + j;
                   
                    // Offset and Rainbow effects, replace it with any other effect you want.
                    Color32 rainbow = Color.HSVToRGB(((time) + (vertexIndex * (0.001f))) % 1f, 1f, 1f);

                    if (!posBases.ContainsKey(vertexIndex)) posBases.Add(vertexIndex, newVertices[vertexIndex]);
                    
                    // Sets the new effects
                    newColors[vertexIndex] = rainbow;
                    newVertices[vertexIndex] = posBases[vertexIndex] + offset;
                }

                textComp.textInfo.meshInfo[materialIndex].colors32 = newColors;
                
            }  
        }
        
        #endregion
    }
}
