function OnApplicationStarted() {}

function ShowGameListAll() {
    param($scriptMainMenuItemActionArgs)

    $preset = New-Object Playnite.SDK.Models.FilterPreset
    $preset.Settings = $PlayniteApi.MainView.GetCurrentFilterSettings()
    
    # Remove o filtro de instalação para listar tudo
    $preset.Settings.IsInstalled = $false
    $preset.Settings.IsUnInstalled = $false
    
    # Agrupa e ordena a interface por CompletionStatus
    $preset.GroupingOrder = [Playnite.SDK.Models.GroupableField]::CompletionStatus
    $preset.SortingOrder = [Playnite.SDK.Models.SortOrder]::CompletionStatus
    
    $PlayniteApi.MainView.ApplyFilterPreset($preset)
}

function ShowGameListInstalled() {
    param($scriptMainMenuItemActionArgs)

    $preset = New-Object Playnite.SDK.Models.FilterPreset
    $preset.Settings = $PlayniteApi.MainView.GetCurrentFilterSettings()
    
    # Adiciona o filtro para mostrar APENAS instalados
    $preset.Settings.IsInstalled = $true
    $preset.Settings.IsUnInstalled = $false
    
    # Agrupa e ordena a interface por CompletionStatus
    $preset.GroupingOrder = [Playnite.SDK.Models.GroupableField]::CompletionStatus
    $preset.SortingOrder = [Playnite.SDK.Models.SortOrder]::CompletionStatus
    
    $PlayniteApi.MainView.ApplyFilterPreset($preset)
}

function ToggleSortOrder() {
    param($scriptMainMenuItemActionArgs)
    
    $preset = New-Object Playnite.SDK.Models.FilterPreset
    $preset.Settings = $PlayniteApi.MainView.GetCurrentFilterSettings()
    
    # Mantém o agrupamento e ordenação atuais da tela, só muda o sentido
    $preset.GroupingOrder = $PlayniteApi.MainView.Grouping
    $preset.SortingOrder = $PlayniteApi.MainView.SortOrder

    if ($PlayniteApi.MainView.SortOrderDirection -eq [Playnite.SDK.Models.SortOrderDirection]::Ascending) {
        $preset.SortingOrderDirection = [Playnite.SDK.Models.SortOrderDirection]::Descending
    }
    else {
        $preset.SortingOrderDirection = [Playnite.SDK.Models.SortOrderDirection]::Ascending
    }
    
    $PlayniteApi.MainView.ApplyFilterPreset($preset)
}

function GetMainMenuItems() {
    param($getMainMenuItemsArgs)

    $item1 = New-Object Playnite.SDK.Plugins.ScriptMainMenuItem
    $item1.Description = "List Installed"
    $item1.FunctionName = "ShowGameListInstalled"
    $item1.MenuSection = "Organize Libary"

    $item2 = New-Object Playnite.SDK.Plugins.ScriptMainMenuItem
    $item2.Description = "List All"
    $item2.FunctionName = "ShowGameListAll"
    $item2.MenuSection = "Organize Libary"

    $item3 = New-Object Playnite.SDK.Plugins.ScriptMainMenuItem
    $item3.Description = "Sort Order"
    $item3.FunctionName = "ToggleSortOrder"
    $item3.MenuSection = "Organize Libary"
    
    return @($item1, $item2, $item3)
}
