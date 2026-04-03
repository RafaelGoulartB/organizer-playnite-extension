using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PnExtension
{
    public class PnExtensionPlugin : GenericPlugin
    {
        private PnExtensionSettings settings { get; set; }

        public override Guid Id { get; } = Guid.Parse("fb1cefd6-9216-4af5-b04b-703649dd0cc8");

        public PnExtensionPlugin(IPlayniteAPI api) : base(api)
        {
            settings = new PnExtensionSettings(this);
            Properties = new GenericPluginProperties
            {
                HasSettings = true
            };
        }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return settings;
        }

        public override UserControl GetSettingsView(bool firstRunSettings)
        {
            return new PnExtensionSettingsView();
        }

        public override IEnumerable<MainMenuItem> GetMainMenuItems(GetMainMenuItemsArgs args)
        {
            return new List<MainMenuItem>
            {
                new MainMenuItem
                {
                    Description = "List Installed",
                    MenuSection = "Organize Library",
                    Action = (a) => ShowGameListInstalled()
                },
                new MainMenuItem
                {
                    Description = "List All",
                    MenuSection = "Organize Library",
                    Action = (a) => ShowGameListAll()
                },
                new MainMenuItem
                {
                    Description = "Sort Order",
                    MenuSection = "Organize Library",
                    Action = (a) => ToggleSortOrder()
                }
            };
        }

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            if (Application.Current != null && Application.Current.MainWindow != null)
            {
                if (settings.ListAllKey != Key.None)
                {
                    var cmdAll = new RelayCommand(() => ShowGameListAll());
                    Application.Current.MainWindow.InputBindings.Add(new KeyBinding(cmdAll, settings.ListAllKey, settings.ListAllModifier));
                }

                if (settings.ListInstalledKey != Key.None)
                {
                    var cmdInst = new RelayCommand(() => ShowGameListInstalled());
                    Application.Current.MainWindow.InputBindings.Add(new KeyBinding(cmdInst, settings.ListInstalledKey, settings.ListInstalledModifier));
                }
            }
        }

        private void ShowGameListAll()
        {
            var preset = new FilterPreset();
            preset.Settings = PlayniteApi.MainView.GetCurrentFilterSettings();
            preset.Settings.IsInstalled = false;
            preset.Settings.IsUnInstalled = false;
            
            preset.GroupingOrder = GroupableField.CompletionStatus;
            preset.SortingOrder = SortOrder.CompletionStatus;
            
            PlayniteApi.MainView.ApplyFilterPreset(preset);
        }

        private void ShowGameListInstalled()
        {
            var preset = new FilterPreset();
            preset.Settings = PlayniteApi.MainView.GetCurrentFilterSettings();
            preset.Settings.IsInstalled = true;
            preset.Settings.IsUnInstalled = false;
            
            preset.GroupingOrder = GroupableField.CompletionStatus;
            preset.SortingOrder = SortOrder.CompletionStatus;
            
            PlayniteApi.MainView.ApplyFilterPreset(preset);
        }

        private void ToggleSortOrder()
        {
            var preset = new FilterPreset();
            preset.Settings = PlayniteApi.MainView.GetCurrentFilterSettings();
            
            preset.GroupingOrder = PlayniteApi.MainView.Grouping;
            preset.SortingOrder = PlayniteApi.MainView.SortOrder;
            
            if (PlayniteApi.MainView.SortOrderDirection == SortOrderDirection.Ascending)
            {
                preset.SortingOrderDirection = SortOrderDirection.Descending;
            }
            else
            {
                preset.SortingOrderDirection = SortOrderDirection.Ascending;
            }
            
            PlayniteApi.MainView.ApplyFilterPreset(preset);
        }
    }
}
