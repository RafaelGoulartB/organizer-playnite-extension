using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
                },
                new MainMenuItem
                {
                    Description = "Add Numbers to Statuses (Force Custom Sort)",
                    MenuSection = "Organize Library",
                    Action = (a) => ApplySortNumbers()
                },
                new MainMenuItem
                {
                    Description = "Remove Numbers from Statuses",
                    MenuSection = "Organize Library",
                    Action = (a) => RemoveSortNumbers()
                }
            };
        }

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            if (Application.Current != null && Application.Current.MainWindow != null)
            {
                if (settings.ToggleListKey != Key.None)
                {
                    var cmdToggleList = new RelayCommand(() => ToggleList());
                    Application.Current.MainWindow.InputBindings.Add(new KeyBinding(cmdToggleList, settings.ToggleListKey, settings.ToggleListModifier));
                }

                if (settings.ToggleSortKey != Key.None)
                {
                    var cmdToggleSort = new RelayCommand(() => ToggleSortOrder());
                    Application.Current.MainWindow.InputBindings.Add(new KeyBinding(cmdToggleSort, settings.ToggleSortKey, settings.ToggleSortModifier));
                }
            }
        }

        private void ToggleList()
        {
            var preset = new FilterPreset();
            preset.Settings = PlayniteApi.MainView.GetCurrentFilterSettings();
            
            bool isCurrentlyInstalled = preset.Settings.IsInstalled;

            if (isCurrentlyInstalled)
            {
                preset.Settings.IsInstalled = false;
                preset.Settings.IsUnInstalled = false;
            }
            else
            {
                preset.Settings.IsInstalled = true;
                preset.Settings.IsUnInstalled = false;
            }
            
            preset.GroupingOrder = GroupableField.CompletionStatus;
            preset.SortingOrder = SortOrder.CompletionStatus;
            
            PlayniteApi.MainView.ApplyFilterPreset(preset);
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

        private readonly string[] statusOrder = new string[]
        {
            "Beaten",
            "Playing",
            "On Going",
            "Plan to Play",
            "Played",
            "On Hold",
            "Not Played",
            "Abandoned"
        };

        private void ApplySortNumbers()
        {
            try
            {
                foreach (var status in PlayniteApi.Database.CompletionStatuses)
                {
                    string cleanName = Regex.Replace(status.Name, @"^\d+\.\s*", "");
                    int index = Array.FindIndex(statusOrder, s => s.Equals(cleanName, StringComparison.OrdinalIgnoreCase));
                    
                    if (index >= 0)
                    {
                        status.Name = $"{index + 1}. {cleanName}";
                        PlayniteApi.Database.CompletionStatuses.Update(status);
                    }
                }
                PlayniteApi.Dialogs.ShowMessage("Done! Custom order numbers applied to Completion Statuses.", "Success");
            }
            catch (Exception ex)
            {
                PlayniteApi.Dialogs.ShowErrorMessage("Error updating statuses: " + ex.Message);
            }
        }

        private void RemoveSortNumbers()
        {
            try
            {
                foreach (var status in PlayniteApi.Database.CompletionStatuses)
                {
                    if (Regex.IsMatch(status.Name, @"^\d+\.\s*"))
                    {
                        status.Name = Regex.Replace(status.Name, @"^\d+\.\s*", "");
                        PlayniteApi.Database.CompletionStatuses.Update(status);
                    }
                }
                PlayniteApi.Dialogs.ShowMessage("Done! Custom order numbers removed from Completion Statuses.", "Success");
            }
            catch (Exception ex)
            {
                PlayniteApi.Dialogs.ShowErrorMessage("Error updating statuses: " + ex.Message);
            }
        }
    }
}
