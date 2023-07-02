using PolygonalSpace.Generators;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PolygonalSpace
{
    internal class PolygonalSpaceGenerator
    {
        private readonly Generator[] _generators;


        public PolygonalSpaceGenerator(Generator[] generators) => _generators = generators;

        public async void AsyncGenerate()
        {
            foreach (var generator in _generators)
            {
                generator.Generate();

                await Task.Delay(1);
            }
        }

        public async void AsyncClearSpaces()
        {
            foreach (var generator in _generators)
            {
                generator.Clear();

                await Task.Delay(1);
            }
        }

        public void SaveImage(Canvas renderTarget, int qualityMultiplier)
        {
            string fileName;

            using (System.Windows.Forms.SaveFileDialog dialog = new())
            {
                dialog.FileName = "Polygonal space";
                dialog.Filter = "PNG Files (*.png)|*.png;";
                dialog.DefaultExt = ".png";
                dialog.AddExtension = true;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;

                fileName = dialog.FileName;
            }

            RenderTargetBitmap renderTargetBitmap = new((int)renderTarget.Width * qualityMultiplier, (int)renderTarget.Height * qualityMultiplier,
                                                        96 * qualityMultiplier, 96 * qualityMultiplier, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(renderTarget);

            PngBitmapEncoder pngImage = new();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (Stream fileStream = File.Create(fileName))
            {
                pngImage.Save(fileStream);
            }

            MessageBox.Show("Сохранено!", "Polygonal space", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}