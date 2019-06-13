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
                        xmlWriter.WriteStartElement("block");
                        xmlWriter.WriteAttributeString("type", block.GetType().Name);
                        xmlWriter.WriteAttributeString("id", block.Id.ToString());
                        xmlWriter.WriteAttributeString("x", block.getPositionX().ToString());
                        xmlWriter.WriteAttributeString("y", block.getPositionY().ToString());
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

                            if (blockType.Equals("BeginBlock"))
                            {
                                block = _blockManager.StartBlock;
                            }
                            else if (blockType.Equals("CommandBlock"))
                            {
                                block = new CommandBlock(CanvasBlocks, _propertiesManager, id);
                            }
                            else if (blockType.Equals("EndBlock"))
                            {
                                block = new EndBlock(CanvasBlocks, _propertiesManager, id);
                            }
                            else if (blockType.Equals("IfBlock"))
                            {
                                block = new IfBlock(CanvasBlocks, _propertiesManager, id);
                            }
                            else if (blockType.Equals("InputBlock"))
                            {
                                block = new InputBlock(CanvasBlocks, _propertiesManager, id);
                            }
                            else if (blockType.Equals("OutputBlock"))
                            {
                                block = new OutputBlock(CanvasBlocks, _propertiesManager, id);
                            }

                            block.SetPosition(x, y);

                            _blockManager.AddBlock(block);
                        }
                        else if (reader.Name.Equals("connection"))
                        {

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
