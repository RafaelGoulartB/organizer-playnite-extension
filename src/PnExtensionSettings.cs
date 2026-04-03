using Playnite.SDK;
using Playnite.SDK.Data;
using System.Collections.Generic;
using System.Windows.Input;

namespace PnExtension
{
    public class PnExtensionSettings : ObservableObject, ISettings
    {
        private readonly PnExtensionPlugin plugin;
        private PnExtensionSettings editingClone { get; set; }

        private Key toggleListKey = Key.L;
        public Key ToggleListKey { get => toggleListKey; set => SetValue(ref toggleListKey, value); }

        private ModifierKeys toggleListModifier = ModifierKeys.Control;
        public ModifierKeys ToggleListModifier { get => toggleListModifier; set => SetValue(ref toggleListModifier, value); }

        private Key toggleSortKey = Key.L;
        public Key ToggleSortKey { get => toggleSortKey; set => SetValue(ref toggleSortKey, value); }

        private ModifierKeys toggleSortModifier = ModifierKeys.Control | ModifierKeys.Shift;
        public ModifierKeys ToggleSortModifier { get => toggleSortModifier; set => SetValue(ref toggleSortModifier, value); }

        [DontSerialize]
        public Dictionary<ModifierKeys, string> AvailableModifiers { get; } = new Dictionary<ModifierKeys, string>
        {
            { ModifierKeys.None, "Nenhum" },
            { ModifierKeys.Control, "Ctrl" },
            { ModifierKeys.Shift, "Shift" },
            { ModifierKeys.Alt, "Alt" },
            { ModifierKeys.Control | ModifierKeys.Shift, "Ctrl + Shift" },
            { ModifierKeys.Control | ModifierKeys.Alt, "Ctrl + Alt" }
        };

        public PnExtensionSettings()
        {
        }

        public PnExtensionSettings(PnExtensionPlugin plugin)
        {
            this.plugin = plugin;
            var savedSettings = plugin.LoadPluginSettings<PnExtensionSettings>();
            if (savedSettings != null)
            {
                ToggleListKey = savedSettings.ToggleListKey;
                ToggleListModifier = savedSettings.ToggleListModifier;
                ToggleSortKey = savedSettings.ToggleSortKey;
                ToggleSortModifier = savedSettings.ToggleSortModifier;
            }
        }

        public void BeginEdit()
        {
            editingClone = Serialization.GetClone(this);
        }

        public void CancelEdit()
        {
            ToggleListKey = editingClone.ToggleListKey;
            ToggleListModifier = editingClone.ToggleListModifier;
            ToggleSortKey = editingClone.ToggleSortKey;
            ToggleSortModifier = editingClone.ToggleSortModifier;
        }

        public void EndEdit()
        {
            plugin.SavePluginSettings(this);
            plugin.PlayniteApi.Dialogs.ShowMessage("Para aplicar completamente as alterações nos atalhos de teclado, você deve reiniciar o Playnite.", "Reinício Recomendado");
        }

        public bool VerifySettings(out List<string> errors)
        {
            errors = new List<string>();
            return true;
        }
    }
}
