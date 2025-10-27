using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Model
{
    internal class RowItems
    {
        public Button NameButton { get; private set; }
        public Button ExtensionButton { get; private set; }
        public Label SizeLabel { get; private set; }

        public RowItems(Button nameButton, Button extensionButton, Label sizeLabel)
        {
            NameButton = nameButton;
            ExtensionButton = extensionButton;
            SizeLabel = sizeLabel;
        }
    }
}
