using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Model
{
    internal class ButtonMetadataComparer : IEqualityComparer<Button>
    {
        public bool Equals(Button x, Button y)
        {
            var metaButtonX = x?.Tag as ButtonMetadata;
            var metaButtonY = y?.Tag as ButtonMetadata;

            if (metaButtonX == null || metaButtonY == null) return false;

            string nameButtonX = new DirectoryInfo(metaButtonX.Path!).Name;
            string nameButtonY = new DirectoryInfo(metaButtonY.Path!).Name;

            return nameButtonX == nameButtonY;
        }

        public int GetHashCode(Button obj)
        {
            var meta = obj?.Tag as ButtonMetadata;
            if (meta == null) return 0;

            string name = new DirectoryInfo(meta.Path!).Name;
            return name.GetHashCode();
        }
    }
}
