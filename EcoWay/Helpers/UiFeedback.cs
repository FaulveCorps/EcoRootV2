using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace EcoWay.Helpers;

public static class UiFeedback
{
    public static async Task ShowToastAsync(string message, double fontSize = 14)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        var toast = Toast.Make(message, ToastDuration.Short, fontSize);
        await toast.Show();
    }

    public static void TryHaptic(HapticFeedbackType feedbackType = HapticFeedbackType.Click)
    {
        try
        {
            HapticFeedback.Default.Perform(feedbackType);
        }
        catch
        {
            // Some platforms/devices may not support haptics.
        }
    }
}