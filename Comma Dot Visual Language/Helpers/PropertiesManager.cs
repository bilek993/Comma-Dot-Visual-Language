using System.Windows;
using System.Windows.Controls;
using Comma_Dot_Visual_Language.Blocks;

namespace Comma_Dot_Visual_Language.Helpers
{
    public class PropertiesManager
    {
        public Block SelectedBlock { get; set; }

        private readonly Label _labelId;
        private readonly Label _labelOutputPrimary;
        private readonly Label _labelOutputOptional;
        private readonly TextBox _textBoxCommand;
        private readonly ComboBox _typesComboBox;
        private readonly DockPanel _variableDockPanel;
        private readonly DockPanel _optionalOutputDockPanel;
        private readonly DockPanel _primaryOutputDockPanel;
        private const string NoneValue = "None";

        public PropertiesManager(Label labelId, Label labelOutputPrimary, Label labelOutputOptional,
            TextBox textBoxCommand, ComboBox typesComboBox, DockPanel variableDockPanel, DockPanel optionalOutputDockPanel,
            DockPanel primaryOutputDockPanel)
        {
            _labelId = labelId;
            _labelOutputPrimary = labelOutputPrimary;
            _labelOutputOptional = labelOutputOptional;
            _textBoxCommand = textBoxCommand;
            _typesComboBox = typesComboBox;
            _variableDockPanel = variableDockPanel;
            _optionalOutputDockPanel = optionalOutputDockPanel;
            _primaryOutputDockPanel = primaryOutputDockPanel;

            _labelId.Content = "";
            _labelOutputPrimary.Content = "";
            _labelOutputOptional.Content = "";
        }

        private static string GenerateOutputContent(string id, string command)
        {
            return "Block " + id + " (" + command + ")";
        }

        public void Update()
        {
            if (SelectedBlock == null)
                return;

            _labelId.Content = SelectedBlock.Id;

            if (SelectedBlock.NextBlockPrimary != null)
                _labelOutputPrimary.Content = GenerateOutputContent(SelectedBlock.NextBlockPrimary.Id.ToString(),
                    SelectedBlock.NextBlockPrimary.Command);
            else
                _labelOutputPrimary.Content = NoneValue;

            if (SelectedBlock.NextBlockOptional != null)
                _labelOutputOptional.Content = GenerateOutputContent(SelectedBlock.NextBlockOptional.Id.ToString(),
                    SelectedBlock.NextBlockOptional.Command);
            else
                _labelOutputOptional.Content = NoneValue;

            if (SelectedBlock.GetType() == typeof(BeginBlock) || SelectedBlock.GetType() == typeof(EndBlock))
                _textBoxCommand.IsEnabled = false;
            else
                _textBoxCommand.IsEnabled = true;

            if (SelectedBlock.GetType() != typeof(InputBlock))
            {
                _variableDockPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                _variableDockPanel.Visibility = Visibility.Visible;
                InputBlock inputBlock = (InputBlock) SelectedBlock;
                _typesComboBox.SelectedIndex = (int) inputBlock.VarType;

            }

            _optionalOutputDockPanel.Visibility = SelectedBlock.GetType() == typeof(IfBlock) ? Visibility.Visible : Visibility.Collapsed;
            _primaryOutputDockPanel.Visibility = SelectedBlock.GetType() == typeof(EndBlock) ? Visibility.Collapsed : Visibility.Visible;

            _textBoxCommand.Text = SelectedBlock.Command;
        }

        public void CommandChanged(string newCommand)
        {
            if (SelectedBlock != null)
                SelectedBlock.Command = newCommand;
        }

        public void VariableTypeUpdate(int value)
        {
            if (SelectedBlock != null && SelectedBlock.GetType() != typeof(InputBlock))
                return;

            var inputBlock = (InputBlock) SelectedBlock;
            inputBlock.VarType = (VariableType)value;
        }
    }
}
