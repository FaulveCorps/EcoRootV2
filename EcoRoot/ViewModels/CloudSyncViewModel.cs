using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using EcoRoot.Resources;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace EcoRoot.ViewModels;

public sealed class CloudSyncViewModel : BaseViewModel
{
    const string SyncEnabledKey = "cloud_sync_enabled";
    const string SyncConsentKey = "cloud_sync_consent";
    const string LastSyncKey = "cloud_sync_last_sync";

    bool isSyncEnabled;
    bool isConsentGranted;
    bool isConsentRequired;
    bool suppressToggle;
    string statusMessage = string.Empty;
    string lastSyncText = string.Empty;
    DateTimeOffset? lastSyncAt;

    public CloudSyncViewModel()
    {
        Title = Strings.CloudSyncTitle;
        DataItems = new[]
        {
            Strings.CloudSyncDataItemProgress,
            Strings.CloudSyncDataItemQuizAttempts,
            Strings.CloudSyncDataItemLastSync
        };

        isConsentGranted = Preferences.Default.Get(SyncConsentKey, false);
        isSyncEnabled = Preferences.Default.Get(SyncEnabledKey, false);

        LoadLastSync();

        if (isSyncEnabled && !isConsentGranted)
        {
            isSyncEnabled = false;
        }

        UpdateStatus(isSyncEnabled);
        ProvideConsentCommand = new Command(ProvideConsent);
    }

    public string IntroText => Strings.CloudSyncIntro;

    public string DataTitle => Strings.CloudSyncDataTitle;

    public IReadOnlyList<string> DataItems { get; }

    public bool IsSyncEnabled
    {
        get => isSyncEnabled;
        set
        {
            if (SetProperty(ref isSyncEnabled, value))
            {
                HandleToggle(value);
            }
        }
    }

    public bool IsConsentRequired
    {
        get => isConsentRequired;
        private set => SetProperty(ref isConsentRequired, value);
    }

    public string StatusMessage
    {
        get => statusMessage;
        private set => SetProperty(ref statusMessage, value);
    }

    public string LastSyncText
    {
        get => lastSyncText;
        private set => SetProperty(ref lastSyncText, value);
    }

    public ICommand ProvideConsentCommand { get; }

    void HandleToggle(bool value)
    {
        if (suppressToggle)
        {
            return;
        }

        if (value && !isConsentGranted)
        {
            IsConsentRequired = true;
            StatusMessage = Strings.CloudSyncConsentRequired;
            suppressToggle = true;
            IsSyncEnabled = false;
            suppressToggle = false;
            return;
        }

        Preferences.Default.Set(SyncEnabledKey, value);
        if (value)
        {
            SetLastSync(DateTimeOffset.UtcNow);
        }
        UpdateStatus(value);
    }

    void ProvideConsent()
    {
        isConsentGranted = true;
        Preferences.Default.Set(SyncConsentKey, true);
        IsConsentRequired = false;
        suppressToggle = true;
        IsSyncEnabled = true;
        suppressToggle = false;
        Preferences.Default.Set(SyncEnabledKey, true);
        StatusMessage = Strings.CloudSyncEnabled;
        SetLastSync(DateTimeOffset.UtcNow);
    }

    void UpdateStatus(bool isEnabled)
    {
        StatusMessage = isEnabled ? Strings.CloudSyncEnabled : Strings.CloudSyncDisabled;
    }

    void LoadLastSync()
    {
        var raw = Preferences.Default.Get(LastSyncKey, string.Empty);
        if (DateTimeOffset.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var parsed))
        {
            lastSyncAt = parsed;
        }
        else
        {
            lastSyncAt = null;
        }

        UpdateLastSyncText();
    }

    void SetLastSync(DateTimeOffset? value)
    {
        lastSyncAt = value;
        if (value.HasValue)
        {
            Preferences.Default.Set(LastSyncKey, value.Value.ToString("o", CultureInfo.InvariantCulture));
        }
        else
        {
            Preferences.Default.Remove(LastSyncKey);
        }

        UpdateLastSyncText();
    }

    void UpdateLastSyncText()
    {
        LastSyncText = lastSyncAt.HasValue
            ? string.Format(CultureInfo.CurrentCulture, Strings.CloudSyncLastSyncFormat, lastSyncAt.Value.ToLocalTime().ToString("g", CultureInfo.CurrentCulture))
            : Strings.CloudSyncLastSyncEmpty;
    }
}
