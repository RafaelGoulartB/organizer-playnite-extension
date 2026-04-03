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

        private Key listAllKey = Key.None;
        public Key ListAllKey { get => listAllKey; set => SetValue(ref listAllKey, value); }

        private ModifierKeys listAllModifier = ModifierKeys.None;
        public ModifierKeys ListAllModifier { get => listAllModifier; set => SetValue(ref listAllModifier, value); }

        private Key listInstalledKey = Key.None;
        public Key ListInstalledKey { get => listInstalledKey; set => SetValue(ref listInstalledKey, value); }

        private ModifierKeys listInstalledModifier = ModifierKeys.None;
        public ModifierKeys ListInstalledModifier { get => listInstalledModifier; set => SetValue(ref listInstalledModifier, value); }

        public PnExtensionSettings()
        {
        }

        public PnExtensionSettings(PnExtensionPlugin plugin)
        {
            this.plugin = plugin;
            var savedSettings = plugin.LoadPluginSettings<PnExtensionSettings>();
            if (savedSettings != null)
            {
                ListAllKey = savedSettings.ListAllKey;
                ListAllModifier = savedSettings.ListAllModifier;
                ListInstalledKey = savedSettings.ListInstalledKey;
                ListInstalledModifier = savedSettings.ListInstalledModifier;
            }
        }

        public void BeginEdit()
        {
            editingClone = Serialization.GetClone(this);
        }

        public void CancelEdit()
        {
            ListAllKey = editingClone.ListAllKey;
            ListAllModifier = editingClone.ListAllModifier;
            ListInstalledKey = editingClone.ListInstalledKey;
            ListInstalledModifier = editingClone.ListInstalledModifier;
        }

        public void EndEdit()
        {
            plugin.SavePluginSettings(this);
            // Hinting to user that Playnite restart might be necessary for shortcut binding changes
            plugin.PlayniteApi.Dialogs.ShowMessage("Para aplicar completamente as alterações nos atalhos de teclado, você deve reiniciar o Playnite.", "Reinício Recomendado");
        }

        public bool VerifySettings(out List<string> errors)
        {
            errors = new List<string>();
            return true;
        }
    }
}
