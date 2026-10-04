using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Windows.ApplicationModel.Resources;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LanTest
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : Window
	{
		private static readonly string SettingsPath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
			"LanTest",
			"language.txt");

		private readonly ResourceManager _resourceManager = new();
		private readonly string _language = LoadLanguage();

		public MainWindow()
		{
			InitializeComponent();

			LanguageSelector.SelectedIndex = _language == "de-DE" ? 1 : 0;
			UpdateLocalizedText();
		}

		private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (LanguageSelector.SelectedIndex < 0)
				return;

			// Persisted only; the choice is applied the next time the app starts.
			SaveLanguage(LanguageSelector.SelectedIndex == 1 ? "de-DE" : "en-US");
		}

		// Todo :: MHas :: This is for testing
		// Updates the UI text based on the selected language.
		private void UpdateLocalizedText()
		{
			// MRT Core resolves resources without package identity, unlike the UWP ResourceLoader.
			var context = _resourceManager.CreateResourceContext();
			context.QualifierValues["Language"] = _language;

			var strings = _resourceManager.MainResourceMap.GetSubtree("Resources");
			Title = strings.GetValue("WindowTitle", context).ValueAsString;
			HeaderText.Text = strings.GetValue("Header", context).ValueAsString;
			DescriptionText.Text = strings.GetValue("Description", context).ValueAsString;
			LanguageLabel.Text = strings.GetValue("LanguageLabel", context).ValueAsString;
		}

		private static string LoadLanguage()
		{
			try
			{
				return File.ReadAllText(SettingsPath).Trim() == "de-DE" ? "de-DE" : "en-US";
			}
			catch (IOException)
			{
				return "en-US";
			}
		}

		private static void SaveLanguage(string language)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
			File.WriteAllText(SettingsPath, language);
		}
	}
}
