using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    class Program
    {
        private enum MenuState { Browsing, StepIn, StepOut }
        private static MenuState state;
        private static int highlightedNodeChild = 0;
        private static Stack<MenuNode> nodePath = new Stack<MenuNode>(); 
        private static MenuNode currentNode = new MenuNode
            (new MenuNode[1]
            {
                new MenuNode(new MenuNode[3]
                {
                    new MenuNode(new MenuNode[4]
                    {
                        new ButtonNode("Breads"),
                        new ButtonNode("Meats"),
                        new ButtonNode("Veg"),
                        new ButtonNode("Drinks")
                    }, "Subway"),
                    new MenuNode(new MenuNode[2]
                    {
                        new ButtonNode("Chickens"),
                        new ButtonNode("Other Stuff")
                    }, "KFC"),
                    new MenuNode(new MenuNode[5]
                    {
                        new ButtonNode("Slimes"),
                        new ButtonNode("Burgers"),
                        new ButtonNode("Chickens"),
                        new ButtonNode("Fries"),
                        new ButtonNode("Drinks")
                    }, "McDonalds") }, 
                "Restaurants")
            }, "Root");

        static void Main(string[] args)
        {
            nodePath.Push(currentNode);
            while (true)
            {
                DrawChildren(highlightedNodeChild);
                Console.WriteLine("\n Q -> BACK | E -> ENTER");

                ConsoleKeyInfo key = Console.ReadKey(true);
                Console.Clear();
                state = MenuState.Browsing;

                switch (key.Key)
                {
                    case ConsoleKey.W:
                        if (currentNode.children.Length == 0)
                        {
                            highlightedNodeChild = 0;
                            continue;
                        }
                        highlightedNodeChild = (highlightedNodeChild-- <= 0) ? currentNode.children.Length - 1 : highlightedNodeChild;
                        break;
                    case ConsoleKey.S:
                        if (currentNode.children.Length == 0)
                        {
                            highlightedNodeChild = 0;
                            continue;
                        }
                        highlightedNodeChild = (highlightedNodeChild++ >= currentNode.children.Length - 1) ? 0 : highlightedNodeChild;
                        break;
                    case ConsoleKey.E:
                        if(currentNode.children.Length == 0)
                            break;
                        state = MenuState.StepIn;
                        break;
                    case ConsoleKey.Q:
                        state = MenuState.StepOut;
                        break;
                }
            }
        }

        static void DrawChildren(int _highlightIndex)
        {
            switch (state)
            {
                case MenuState.Browsing:
                    //is just here incase somehing needs to happen
                    break;
                case MenuState.StepIn:
                    if (currentNode.children[_highlightIndex].children.Length == 0)
                    {
                        if (currentNode.children[_highlightIndex] is IButtonNode)
                            (currentNode.children[_highlightIndex] as IButtonNode).Interact();
                        break;
                    }

                    nodePath.Push(currentNode);
                    currentNode = currentNode.children[_highlightIndex];
                    highlightedNodeChild = 0;
                    break;
                case MenuState.StepOut:
                    currentNode = nodePath.Count == 0 ? currentNode : nodePath.Pop();
                    highlightedNodeChild = 0;
                    break;
            }

            if (currentNode.children.Length > 0)
            {
                for (int i = 0; i < currentNode.children.Length; i++) //this is basically showing what is selected then drawing
                {
                    if (i == _highlightIndex)
                        Console.Write("// ");
                    currentNode.children[i].DrawSelf();
                }
            }
        }
    }

    class MenuNode
    {
        public MenuNode[] children;

        public string displayText; //this would be a gameobject or something

        public virtual void DrawSelf()
        {
            Console.WriteLine(displayText);
        }

        public MenuNode(string _text)
        {
            children = new MenuNode[0];
            displayText = _text;
        }

        public MenuNode(MenuNode[] _children, string _text)
        {
            children = _children;
            displayText = _text;
        }
    }

    interface IButtonNode 
    {
        void Interact();
    }

    class ButtonNode : MenuNode, IButtonNode
    {
        public void Interact()
        {
            Console.WriteLine($"Clicked the {displayText} button!");
        }

        public ButtonNode(string _text): base(_text) { }
    }
}
