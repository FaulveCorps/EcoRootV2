using Microsoft.Maui.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.System;
using WinUIWindow = Microsoft.UI.Xaml.Window;

namespace EcoWay.Platforms.Windows;

internal static class BackspaceNavigationHandler
{
	private static bool _attached;

	public static void Attach(WinUIWindow window)
	{
		if (_attached)
		{
			return;
		}

		if (window.Content is FrameworkElement root)
		{
			_attached = true;
			root.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(OnKeyDown), true);
		}
	}

	private static async void OnKeyDown(object sender, KeyRoutedEventArgs e)
	{
		if (e.Key != VirtualKey.Back)
		{
			return;
		}

		if (sender is FrameworkElement root && IsTextInputFocused(root.XamlRoot))
		{
			return;
		}

		var shell = Shell.Current;
		if (shell is null)
		{
			return;
		}

		if (shell.Navigation.ModalStack.Count > 0)
		{
			e.Handled = true;
			await shell.Navigation.PopModalAsync();
			return;
		}

		if (shell.Navigation.NavigationStack.Count > 1)
		{
			e.Handled = true;
			await shell.GoToAsync("..");
		}
	}

	private static bool IsTextInputFocused(XamlRoot? root)
	{
		if (root is null)
		{
			return false;
		}

		var focused = FocusManager.GetFocusedElement(root);
		return focused is TextBox
			|| focused is PasswordBox
			|| focused is AutoSuggestBox
			|| focused is RichEditBox;
	}
}
