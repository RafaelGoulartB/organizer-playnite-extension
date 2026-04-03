function OnApplicationStarted() {
    $PlayniteApi.Dialogs.ShowMessage("A extensão PnExtension (Modo UI Filter Integrado) foi carregada!", "Setup da Extensão")
}

function ShowGameListAll() {
    param($scriptMainMenuItemActionArgs)

    $PlayniteApi.MainView.Grouping = [Playnite.SDK.Models.GroupableField]::CompletionStatus
    
    # Remove o filtro de instalação para listar tudo
    $settings = $PlayniteApi.MainView.GetCurrentFilterSettings()
    $settings.IsInstalled = $false
    $settings.IsUnInstalled = $false
}

function ShowGameListInstalled() {
    param($scriptMainMenuItemActionArgs)

    $PlayniteApi.MainView.Grouping = [Playnite.SDK.Models.GroupableField]::CompletionStatus
    
    # Adiciona o filtro para mostrar APENAS instalados
    $settings = $PlayniteApi.MainView.GetCurrentFilterSettings()
    $settings.IsInstalled = $true
    $settings.IsUnInstalled = $false
}

function ToggleSortOrder() {
    param($scriptMainMenuItemActionArgs)
    
    # Alterna a ordem de Z-A para A-Z ou vice-versa na interface
    if ($PlayniteApi.MainView.SortOrderDirection -eq [Playnite.SDK.Models.SortOrderDirection]::Ascending) {
        $PlayniteApi.MainView.SortOrderDirection = [Playnite.SDK.Models.SortOrderDirection]::Descending
    } else {
        $PlayniteApi.MainView.SortOrderDirection = [Playnite.SDK.Models.SortOrderDirection]::Ascending
    }
}

function GetMainMenuItems() {
    param($getMainMenuItemsArgs)
    
    $item1 = New-Object Playnite.SDK.Plugins.ScriptMainMenuItem
    $item1.Description = "UI: Listar Todos (Agrupar Status)"
    $item1.FunctionName = "ShowGameListAll"
    $item1.MenuSection = "PnExtension"
    
    $item2 = New-Object Playnite.SDK.Plugins.ScriptMainMenuItem
    $item2.Description = "UI: Listar Instalados (Agrupar Status)"
    $item2.FunctionName = "ShowGameListInstalled"
    $item2.MenuSection = "PnExtension"

    $item3 = New-Object Playnite.SDK.Plugins.ScriptMainMenuItem
    $item3.Description = "UI: Alternar Classificação (Ascendente / Descendente)"
    $item3.FunctionName = "ToggleSortOrder"
    $item3.MenuSection = "PnExtension"
    
    return @($item1, $item2, $item3)
}
