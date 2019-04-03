using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Comma_Dot_Visual_Language.Helpers
{
    static class ImageGenerator
    {
        private static string _assemblyName;

        private static string GetAssemblyName()
        {
            return _assemblyName ?? (_assemblyName = Assembly.GetExecutingAssembly().GetName().Name);
        }

        public static Image GenerateBlockImage(string filename, string extension = ".png")
        {
            var source = new BitmapImage(new Uri(@"pack://application:,,,/"
                + GetAssemblyName()
                + ";component/"
                + "images/"
                + filename
                + extension, UriKind.Absolute));

            return new Image()
            {
                Source = source,
            };
        }
    }
}
