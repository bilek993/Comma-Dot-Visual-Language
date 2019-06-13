using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using Comma_Dot_Visual_Language.Blocks;
using Comma_Dot_Visual_Language.Helpers;

namespace Comma_Dot_Visual_Language.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlockManager _blockManager;
        private PropertiesManager _propertiesManager;
        private Runner _runner;

        public MainWindow()
        {
            InitializeComponent();

            _propertiesManager = new PropertiesManager(LabelId, LabelOutputBlockPrimary, 
                LabelOutputBlockOptional, TextBoxCommand, VariableTypeComboBox, DockPanelVariableType, 
                PanelOptionalOutput, PanelPrimaryOutput);

            _blockManager = new BlockManager(CanvasBlocks, _propertiesManager);
            _propertiesManager.SelectedBlock = _blockManager.CreateBasicBlocks();
            _propertiesManager.Update();

            _runner = new Runner();
        }

        private void MenuCommandBlockClick(object sender, RoutedEventArgs e)
        {
            _blockManager.CreateCommandBlock();
        }

        private void MenuAddInputBlockClick(object sender, RoutedEventArgs e)
        {
            _blockManager.CreateInputBlock();
        }

        private void MenuAddOutputBlockClick(object sender, RoutedEventArgs e)
        {
            _blockManager.CreateOutputBlock();
        }

        private void MenuAddIfBlockClick(object sender, RoutedEventArgs e)
        {
            _blockManager.CreateIfBlock();
        }

        private void MenuAddEndBlockClick(object sender, RoutedEventArgs e)
        {
            _blockManager.CreateEndBlock();
        }

        private void MenuAddConnectionClick(object sender, RoutedEventArgs e)
        {
            BlockManager.IsAddConnectionMode = true;
            Mouse.OverrideCursor = Cursors.Pen;
        }

        private void TextBoxCommandTextChanged(object sender, TextChangedEventArgs e)
        {
            _propertiesManager.CommandChanged(TextBoxCommand.Text);
        }

        private void ButtonRemoveConnectionPrimaryClick(object sender, RoutedEventArgs e)
        {
            _blockManager.RemoveConnection(_propertiesManager.SelectedBlock, 0);
        }

        private void ButtonRemoveConnectionOptionalClick(object sender, RoutedEventArgs e)
        {
            _blockManager.RemoveConnection(_propertiesManager.SelectedBlock, 1);
        }

        private void MenuRunClick(object sender, RoutedEventArgs e)
        {
            _runner.SetStartBlock(_blockManager.StartBlock);

            var thread = new Thread(_runner.Run);
            thread.Start();
        }

        private void VariableTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _propertiesManager?.VariableTypeUpdate(VariableTypeComboBox.SelectedIndex);
        }

        private void MenuShowCommandsListClick(object sender, RoutedEventArgs e)
        {
            new CommandsListWindow().Show();
        }

        private void MenuCloseAppClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtnRemoveBlockClick(object sender, RoutedEventArgs e)
        {
            _blockManager.RemoveBlock(_propertiesManager.SelectedBlock);
        }

        private void MenuItemNewClick(object sender, RoutedEventArgs e)
        {
            CanvasBlocks.Children.Clear();
            Block.ResetBlocksCounter();

            _blockManager = new BlockManager(CanvasBlocks, _propertiesManager);
            _propertiesManager.SelectedBlock = _blockManager.CreateBasicBlocks();
            _propertiesManager.Update();

            _runner = new Runner();
        }

        private void MenuItemSaveClick(object sender, RoutedEventArgs e)
        {
            saveToFile("file.xml");
        }

        private void MenuItemOpenClick(object sender, RoutedEventArgs e)
        {
            MenuItemNewClick(sender, e);

            openFromFile("file.xml");
        }

        private void saveToFile(string fileName)
        {
            XmlWriter xmlWriter = XmlWriter.Create(fileName);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("program");
            {

                xmlWriter.WriteStartElement("blocks");
                {
                    foreach (var block in _blockManager.GetBlocks())
                    {
                        string blockType = block.GetType().Name;
                        xmlWriter.WriteStartElement("block");
                        xmlWriter.WriteAttributeString("type", blockType);
                        xmlWriter.WriteAttributeString("id", block.Id.ToString());
                        xmlWriter.WriteAttributeString("x", block.GetPositionX().ToString());
                        xmlWriter.WriteAttributeString("y", block.GetPositionY().ToString());

                        if (blockType.Equals("InputBlock"))
                        {
                            xmlWriter.WriteAttributeString("varType", ((InputBlock)block).VarType.ToString());
                        }

                        xmlWriter.WriteString(block.Command);
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("connections");
                {
                    foreach (var block in _blockManager.GetBlocks())
                    {
                        if (block.NextBlockPrimary != null)
                        {
                            xmlWriter.WriteStartElement("connection");
                            xmlWriter.WriteAttributeString("id1", block.Id.ToString());
                            xmlWriter.WriteAttributeString("id2", block.NextBlockPrimary.Id.ToString());
                            xmlWriter.WriteEndElement();
                        }
                        if (block.NextBlockOptional != null)
                        {
                            xmlWriter.WriteStartElement("connection");
                            xmlWriter.WriteAttributeString("id1", block.Id.ToString());
                            xmlWriter.WriteAttributeString("id2", block.NextBlockOptional.Id.ToString());
                            xmlWriter.WriteEndElement();
                        }
                    }
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        private void openFromFile(string fileName)
        {
            XmlTextReader reader = new XmlTextReader(fileName);

            Block block = null;
            string blockType;
            int id;
            double x;
            double y;
            string command;
            string varType;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name.Equals("block"))
                        {
                            block = null;

                            blockType = reader.GetAttribute("type");
                            id = Int32.Parse(reader.GetAttribute("id"));
                            x = Double.Parse(reader.GetAttribute("x"));
                            y = Double.Parse(reader.GetAttribute("y"));
                            varType = reader.GetAttribute("varType");

                            if (blockType.Equals("BeginBlock"))
                            {
                                block = _blockManager.StartBlock;
                            }
                            else if (blockType.Equals("CommandBlock"))
                            {
                                block = new CommandBlock(CanvasBlocks, _propertiesManager, id);
                                _blockManager.AddBlock(block);
                            }
                            else if (blockType.Equals("EndBlock"))
                            {
                                block = new EndBlock(CanvasBlocks, _propertiesManager, id);
                                _blockManager.AddBlock(block);
                            }
                            else if (blockType.Equals("IfBlock"))
                            {
                                block = new IfBlock(CanvasBlocks, _propertiesManager, id);
                                _blockManager.AddBlock(block);
                            }
                            else if (blockType.Equals("InputBlock"))
                            {
                                InputBlock b = new InputBlock(CanvasBlocks, _propertiesManager, id);
                                b.VarType = (VariableType)Enum.Parse(typeof(VariableType), varType);
                                block = b;
                                _blockManager.AddBlock(block);
                            }
                            else if (blockType.Equals("OutputBlock"))
                            {
                                block = new OutputBlock(CanvasBlocks, _propertiesManager, id);
                                _blockManager.AddBlock(block);
                            }

                            block.SetPosition(x, y);
                        }
                        else if (reader.Name.Equals("connection"))
                        {
                            int id1 = Int32.Parse(reader.GetAttribute("id1"));
                            int id2 = Int32.Parse(reader.GetAttribute("id2"));

                            Block block1 = _blockManager.FindBlockById(id1);
                            Block block2 = _blockManager.FindBlockById(id2);

                            block1.AddConnection(block2);
                        }

                        break;
                    case XmlNodeType.Text:
                        command = reader.Value;
                        block.Command = command;

                        break;
                    case XmlNodeType.EndElement:

                        break;
                }
            }

            reader.Close();
        }
    }
}
