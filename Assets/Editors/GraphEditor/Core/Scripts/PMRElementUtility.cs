using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMR.GraphEditor.Utilities
{
    
    using Elements;
    public static class PMRElementUtility
    {
        public static TextField CreateTextField(string value = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }

            return textField;
        }

        public static TextField CreateTextArea(string value = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, onValueChanged);

            textArea.multiline = true;

            return textArea;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };

            return foldout;
        }

        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text
            };
            return button;
        }

        public static Port CreatePort(this PMRNode node, string portName = "", Direction direction = Direction.Output,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(bool));
            port.portName = portName;

            return port;
        }
    }
}
