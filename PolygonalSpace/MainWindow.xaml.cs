using PolygonalSpace.GenerationComponents;
using PolygonalSpace.Generators;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PolygonalSpace
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static PolygonalSpaceGenerator _generator;
        private static readonly Dictionary<object, Regex> _allowedSymbolsByTag = new()
        {
            ["double"] = new Regex("[^0-9,]+"),
            ["int"] = new Regex("[^0-9]+"),
        };


        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateImage()
        {
            try
            {
                SectorsGenerator sectors = new(SectorsSpace, int.Parse(SectorSize.Text));
                DotsGenerator dots = new(DotsSpace, (Color)DotColor.SelectedColor, new Range(double.Parse(DotRadiusRangeMin.Text), double.Parse(DotRadiusRangeMax.Text)), sectors.GetStartPoint(), sectors.GetSectors());
                CirclesGenerator circles = new(CirclesSpace, dots.GetDots());
                LinesGenerator lines = new(LinesSpace, new Range(double.Parse(LineThicknessRangeMin.Text), double.Parse(LineThicknessRangeMax.Text)), dots.GetDots(), dots.GetRadiusRange());
                PolygonsGenerator polygons = new(PolygonsSpace, dots.GetDots());
                ParticlesGenerator particles = new(ParticlesSpace, int.Parse(NumberParticles.Text), new Range(double.Parse(ParticleRadiusRangeMin.Text), double.Parse(ParticleRadiusRangeMax.Text)), (Color)ParticleColor.SelectedColor);

                _generator = new PolygonalSpaceGenerator(new Generator[] { sectors, dots, circles, lines, polygons, particles });
                _generator.AsyncClearSpaces();
                _generator.AsyncGenerate();
            }
            catch (FormatException)
            {
                MessageBox.Show("Не все параметры введены корректно!", "Polygonal space", MessageBoxButton.OK, MessageBoxImage.Error);
            }      
        }

        private void SpaceVisibilityCheckbox_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, VisualContainer> nameMatching = new()
            {
                ["SectorsSpaceVisibility"] = SectorsSpace,
                ["DotsSpaceVisibility"] = DotsSpace,
                ["CirclesSpaceVisibility"] = CirclesSpace,
                ["LinesSpaceVisibility"] = LinesSpace,
                ["PolygonsSpaceVisibility"] = PolygonsSpace,
                ["ParticlesSpaceVisibility"] = ParticlesSpace
            };

            CheckBox checkBox = (CheckBox)sender;
            nameMatching[checkBox.Name].Visibility = (bool)checkBox.IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Viewbox.Width = MainSpace.Width * e.NewValue;
            Viewbox.Height = MainSpace.Height * e.NewValue;
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e) => GenerateImage();
        private void SaveButton_Click(object sender, RoutedEventArgs e) => _generator?.SaveImage(MainSpace, int.Parse(QualityMultiplier.Text));

        private void ZoomItButton_Click(object sender, RoutedEventArgs e) => Zoom.Value += 0.01;
        private void ZoomOutButton_Click(object sender, RoutedEventArgs e) => Zoom.Value -= 0.01;

        private void InputValidationTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) =>
            e.Handled = _allowedSymbolsByTag[((TextBox)sender).Tag].IsMatch(e.Text);
    }
}