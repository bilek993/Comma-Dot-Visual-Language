using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Text;
using System.Threading.Tasks;

namespace Comma_Dot_Visual_Language
{
    public class PropertiesManager
    {
        public Block SelectedBlock { private get; set; }

        private readonly Label _labelId;
        private readonly Label _labelOutputPrimary;
        private readonly Label _labelOutputOptional;
        private const string NoneValue = "None";

        public PropertiesManager(Label labelId, Label labelOutputPrimary, Label labelOutputOptional)
        {
            _labelId = labelId;
            _labelOutputPrimary = labelOutputPrimary;
            _labelOutputOptional = labelOutputOptional;

            _labelId.Content = "";
            _labelOutputPrimary.Content = "";
            _labelOutputOptional.Content = "";
        }

        private string GenerateOutputContent(string id, string command)
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
        }
    }
}
