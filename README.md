# DariuszLabaj.MaterialTheme.Wpf

A WPF resource library providing a Material Design 3-inspired visual theme, control styles, color schemes, typography, dimensions, icons, and a small set of custom input controls.

The library is intended to provide a consistent visual foundation for WPF applications without requiring application-specific control templates to be maintained in every project.

>**Note** that the Path icons used within this library are conversions sourced from [Google Fonts Icons](https://fonts.google.com/icons).
These converted icons are used for convenience, and I do not claim any rights to the original designs.

## Themes

You can easily create custom color themes based on Material IO themes using this website: [Material Theme to Xaml](https://dariuszlabaj.github.io/MaterialThemeToXaml/)

## Source of Icons

The icons used in this repository are sourced and converted from Google Fonts Icons. You can browse the original icon set [here](https://fonts.google.com/icons).

## Features

- Material Design 3-inspired WPF styling.
- Light and dark color schemes.
- Medium-contrast and high-contrast variants.
- Material-style color palettes:
  - Primary
  - Secondary
  - Tertiary
  - Neutral
  - Neutral Variant
- Material typography based on the bundled Roboto variable fonts.
- Centralized dimensions and typography resources.
- Styled standard WPF controls.
- Custom input controls:
  - `TextInput`
  - `PasswordInput`
  - `SearchBox`
- Material-style cards.
- Material Icons in multiple variants:
  - Outlined
  - Filled
  - Rounded
  - Sharp
  - Two Tone
- Custom converters and helpers used by the control templates.
- Default styles can be applied globally by merging a single resource dictionary.

## Target Framework

The project targets:

- **.NET Framework 4.8**
- **WPF**
- Nullable reference types enabled.
- Implicit usings enabled.
- C# `latestMajor`.
- Warnings treated as errors.

## Dependencies

The library has one external NuGet dependency:

```xml
<PackageReference Include="System.Windows.Interactivity.WPF" Version="2.0.20525" />
```

The library also embeds the following fonts:

- `Roboto-VariableFont_wdth,wght.ttf`
- `Roboto-Italic-VariableFont_wdth,wght.ttf`

## Installation

### Project reference

Add the project to the solution and reference:

```text
DariuszLabaj.MaterialTheme.Wpf
```

The assembly namespace is:

```text
DariuszLabaj.MaterialTheme.Wpf
```

### NuGet

If the library is distributed as a NuGet package, add the corresponding package reference to the application.

The source project currently contains the WPF library itself but does not contain NuGet package metadata.

## Basic Usage

Merge `MaterialTheme.xaml` into the application's resources.

### `App.xaml`

```xml
<Application x:Class="MyApplication.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="/DariuszLabaj.MaterialTheme.Wpf;component/MaterialTheme.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>

</Application>
```

After merging `MaterialTheme.xaml`, the library supplies default styles for the supported WPF controls.

For example:

```xml
<Button Content="Start" />
<TextBox />
<CheckBox Content="Enabled" />
<ComboBox />
<ProgressBar Value="65" />
```

No explicit style is required for these controls when the global theme is active.

## Theme Architecture

The theme is divided into several resource layers.

```text
MaterialTheme.xaml
│
├── Typography.xaml
├── Dimensions.xaml
├── Icons/MaterialIcons.xaml
│
├── Themes/Palettes/neutral.xaml
├── Themes/Schemes/light.xaml
│
└── Controls/*.xaml
```

`MaterialTheme.xaml` is the convenient entry point for normal application usage.

The individual dictionaries can also be used directly when an application needs more control over the resource composition.

## Color Palettes

The library provides five reference palettes:

```text
Themes/Palettes/
├── primary.xaml
├── secondary.xaml
├── tertiary.xaml
├── neutral.xaml
└── neutral-variant.xaml
```

Each palette exposes tonal values from `0` to `100` and corresponding brushes.

For example:

```xml
<SolidColorBrush x:Key="50Brush" ... />
<SolidColorBrush x:Key="60Brush" ... />
<SolidColorBrush x:Key="70Brush" ... />
```

The palette resources are used by the color schemes to construct semantic Material-style colors.

## Color Schemes

The library contains six schemes:

```text
Themes/Schemes/
├── light.xaml
├── light-medium-contrast.xaml
├── light-high-contrast.xaml
├── dark.xaml
├── dark-medium-contrast.xaml
└── dark-high-contrast.xaml
```

The schemes expose semantic resources such as:

```text
primary
primaryBrush

onPrimary
onPrimaryBrush

primaryContainer
primaryContainerBrush

onPrimaryContainer
onPrimaryContainerBrush

secondary
secondaryBrush

tertiary
tertiaryBrush

background
backgroundBrush

onBackground
onBackgroundBrush

surface
surfaceBrush

onSurface
onSurfaceBrush

surfaceContainer
surfaceContainerBrush

outline
outlineBrush
```

Using semantic resources instead of hard-coded colors is recommended:

```xml
<Border Background="{DynamicResource surfaceContainerBrush}"
        BorderBrush="{DynamicResource outlineBrush}">
```

This allows controls to follow the active color scheme.

## Switching Between Light and Dark Themes

The default `MaterialTheme.xaml` loads the light scheme:

```xml
<ResourceDictionary Source="/DariuszLabaj.MaterialTheme.Wpf;component/Themes/Schemes/light.xaml"/>
```

To use the dark scheme, replace it with:

```xml
<ResourceDictionary Source="/DariuszLabaj.MaterialTheme.Wpf;component/Themes/Schemes/dark.xaml"/>
```

The same approach can be used for the medium-contrast and high-contrast variants.

When implementing runtime theme switching, replace the corresponding scheme dictionary in the application's `MergedDictionaries`. Because the controls use `DynamicResource`, they can resolve the updated semantic resources without requiring individual controls to be recreated.

## Supported Standard Controls

`MaterialTheme.xaml` defines global styles for:

- `Window`
- `Page`
- `TextBlock`
- `TextBox`
- `RichTextBox`
- `Ribbon text controls`
- `Label`
- `Button`
- `ToggleButton`
- `CheckBox`
- `RadioButton`
- `ComboBox`
- `ProgressBar`
- `Thumb`
- `ScrollBar`
- `ScrollViewer`
- `TabControl`
- `DataGrid`
- `GroupBox`
- `ListBox`
- `SearchBox`
- `TextInput`
- `PasswordInput`

Additional resource dictionaries are available for:

- `ListView`
- `TreeView`
- `Expander`
- `Slider`

These dictionaries are present in the library but are not currently merged by the main `MaterialTheme.xaml`.

## Buttons

The button resource dictionary provides four variants:

```text
md.button.filled
md.button.elevated
md.button.outlined
md.button.text
```

Example:

```xml
<Button Content="Filled"
        Style="{StaticResource md.button.filled}" />

<Button Content="Outlined"
        Style="{StaticResource md.button.outlined}" />

<Button Content="Text"
        Style="{StaticResource md.button.text}" />

<Button Content="Elevated"
        Style="{StaticResource md.button.elevated}" />
```

When `MaterialTheme.xaml` is used, the default `Button` style is:

```text
md.button.filled
```

## Toggle Buttons and Switches

The library provides:

```text
md.toggle-button.filled
md.toggle-button.elevated
md.toggle-button.outlined
```

The default global `ToggleButton` style is:

```text
md.toggle-button.filled
```

Example:

```xml
<ToggleButton Content="Automatic mode"
              IsChecked="{Binding IsAutomaticMode}" />
```

## Cards

Cards are implemented as styles for `Border`.

Available styles:

```text
ElevatedCard
FilledCard
OutlinedCard
```

Example:

```xml
<Border Style="{StaticResource FilledCard}">
    <StackPanel>
        <TextBlock Text="Machine status" />
        <TextBlock Text="Running" />
    </StackPanel>
</Border>
```

The predefined cards use the semantic surface and outline resources from the active color scheme.

## Input Controls

### TextInput

`TextInput` is a custom `UserControl` intended for Material-style text entry.

Important dependency properties include:

```text
Text
Label
SupportingText
AccentColor
Background
TextWrapping
SupportingTextForeground
SupportingTextBackground
AcceptsReturn
FontSize
```

Example:

```xml
<md:TextInput Label="Operator name"
              SupportingText="Enter the name of the current operator."
              Text="{Binding OperatorName, UpdateSourceTrigger=PropertyChanged}" />
```

Namespace:

```xml
xmlns:md="clr-namespace:DariuszLabaj.MaterialTheme.Wpf.Controls;assembly=DariuszLabaj.MaterialTheme.Wpf"
```

`Text` uses two-way binding by default.

### PasswordInput

`PasswordInput` provides a Material-style password field.

Important dependency properties include:

```text
Password
Label
SupportingText
AccentColor
Background
SupportingTextForeground
SupportingTextBackground
FontSize
```

The password value is represented as:

```csharp
SecureString
```

Example:

```xml
<md:PasswordInput Label="Password"
                  Password="{Binding Password, Mode=TwoWay}" />
```

`Password` uses two-way binding by default.

### SearchBox

`SearchBox` is a compact search input control.

Properties include:

```text
Text
SearchPrompt
Background
BorderThickness
FontSize
```

Example:

```xml
<md:SearchBox Text="{Binding SearchText, UpdateSourceTrigger=PropertyChanged}"
              SearchPrompt="Search..." />
```

## TextBoxHelper

The library contains an attached property helper:

```text
DariuszLabaj.MaterialTheme.Wpf.Helpers.TextBoxHelper
```

It provides a `Label` attached property for standard WPF `TextBox` controls.

Example:

```xml
<TextBox
    md:TextBoxHelper.Label="Serial number"
    Text="{Binding SerialNumber}" />
```

This can be useful when the custom `TextInput` control is not appropriate and the application needs to retain a standard `TextBox`.

## ProgressBar

The library provides Material-style progress bar templates for:

```text
md.ProgressBar
md.ProgressBar-WihtoutStop
md.ProgressBar.Circular
```

The default `ProgressBar` style is:

```text
md.ProgressBar
```

A circular progress bar can be explicitly selected:

```xml
<ProgressBar
    Style="{StaticResource md.ProgressBar.Circular}"
    Minimum="0"
    Maximum="100"
    Value="65" />
```

The templates support both determinate and indeterminate states.

For an indeterminate progress bar:

```xml
<ProgressBar IsIndeterminate="True" />
```

## Tabs

The library provides:

```text
md.TabControl
md.TabControl.Filled
md.TabControl.Elevated
```

The default global style is:

```text
md.TabControl
```

Example:

```xml
<TabControl SelectedIndex="{Binding SelectedPage}">
    <TabItem Header="Overview">
        <!-- content -->
    </TabItem>

    <TabItem Header="Diagnostics">
        <!-- content -->
    </TabItem>
</TabControl>
```

## DataGrid

The library provides a Material-style `DataGrid` including custom styling for:

- `DataGrid`
- `DataGridCell`
- `DataGridColumnHeader`

Example:

```xml
<DataGrid ItemsSource="{Binding Items}"
          AutoGenerateColumns="False">
    <DataGrid.Columns>
        <DataGridTextColumn Header="Name"
                            Binding="{Binding Name}" />
        <DataGridTextColumn Header="Status"
                            Binding="{Binding Status}" />
    </DataGrid.Columns>
</DataGrid>
```

## ListBox

The default `ListBox` and `ListBoxItem` styles are supplied by:

```text
md.ListBox
md.ListBoxItem
```

The templates include list-item-specific corner radius handling.

## Typography

`Typography.xaml` defines Material-style type-scale resources.

Examples include:

```text
md.sys.typescale.display-large-size
md.sys.typescale.display-medium-size
md.sys.typescale.display-small-size

md.sys.typescale.headline-large-size
md.sys.typescale.headline-medium-size
md.sys.typescale.headline-small-size

md.sys.typescale.title-large-size
md.sys.typescale.title-medium-size
md.sys.typescale.title-small-size

md.sys.typescale.body-large-size
md.sys.typescale.body-medium-size
md.sys.typescale.body-small-size

md.sys.typescale.label-large-size
md.sys.typescale.label-medium-size
md.sys.typescale.label-small-size
```

Roboto variable fonts are embedded in the assembly and referenced by the typography resources.

The global `TextBlock`, `TextBox`, `RichTextBox`, `Label`, and related text styles use these resources.

## Icons

`Icons/MaterialIcons.xaml` is the main icon resource dictionary and currently loads the Outlined icon set.

Icon sets are provided as separate dictionaries:

```text
Icons/
├── MaterialIcons.xaml
├── Outlined.xaml
├── Filled.xaml
├── Rounded.xaml
├── Sharp.xaml
└── TwoTone.xaml
```

Icons use keys in the form:

```text
md.icon.<icon_name>
```

For example:

```xml
<Path Data="{StaticResource md.icon.search}" />
```

The exact rendering mechanism depends on the icon resource definition, so applications should generally consume the supplied icon resources rather than recreate the icon geometries.

## Dimensions

`Dimensions.xaml` contains shared dimensional resources used by the theme.

Keeping dimensions in a central resource dictionary allows application-level adjustments without modifying individual control templates.

## Custom Resource Composition

For applications requiring only selected parts of the library, resource dictionaries can be merged individually.

For example:

```xml
<ResourceDictionary.MergedDictionaries>
    <ResourceDictionary Source="/DariuszLabaj.MaterialTheme.Wpf;component/Typography.xaml" />
    <ResourceDictionary Source="/DariuszLabaj.MaterialTheme.Wpf;component/Dimensions.xaml" />

    <ResourceDictionary Source="/DariuszLabaj.MaterialTheme.Wpf;component/Themes/Palettes/neutral.xaml" />
    <ResourceDictionary Source="/DariuszLabaj.MaterialTheme.Wpf;component/Themes/Schemes/dark.xaml" />

    <ResourceDictionary Source="/DariuszLabaj.MaterialTheme.Wpf;component/Controls/Button.xaml" />
    <ResourceDictionary Source="/DariuszLabaj.MaterialTheme.Wpf;component/Controls/DataGrid.xaml" />
</ResourceDictionary.MergedDictionaries>
```

This is useful when an application has its own theme infrastructure and only wants to reuse selected control templates or design tokens.

## Resource Naming Convention

The library generally follows this naming convention:

```text
md.ref.*
```

Reference design tokens, such as fonts.

```text
md.sys.*
```

System-level design tokens and control templates.

```text
md.<component>*
```

Component styles.

```text
md.icon.*
```

Icon resources.

Semantic color resources, such as:

```text
primaryBrush
onPrimaryBrush
surfaceBrush
onSurfaceBrush
backgroundBrush
onBackgroundBrush
outlineBrush
```

should be preferred in application XAML over direct palette tones.

## Example Application

A minimal application can use the theme as follows:

```xml
<Window x:Class="Demo.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:md="clr-namespace:DariuszLabaj.MaterialTheme.Wpf.Controls;assembly=DariuszLabaj.MaterialTheme.Wpf"
        Title="Material Theme Demo"
        Width="800"
        Height="500">

    <Window.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="/DariuszLabaj.MaterialTheme.Wpf;component/MaterialTheme.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Window.Resources>

    <Grid Margin="24">
        <StackPanel Width="360"
                    HorizontalAlignment="Center"
                    VerticalAlignment="Center">

            <TextBlock Text="Machine login"
                       Style="{StaticResource md.headline-TextBlock}"
                       Margin="0,0,0,16" />

            <md:TextInput Label="Operator"
                          Text="{Binding OperatorName}" />

            <md:PasswordInput Label="Password"
                              Password="{Binding Password}" />

            <Button Content="Login"
                    Margin="4"
                    HorizontalAlignment="Right" />

            <ProgressBar Value="70"
                         Margin="4,16,4,0" />

        </StackPanel>
    </Grid>
</Window>
```

## Project Structure

```text
DariuszLabaj.MaterialTheme.Wpf/
├── Assets/
│   └── Fonts/
├── Controls/
│   ├── Button.xaml
│   ├── Cards.xaml
│   ├── CheckBox.xaml
│   ├── ComboBox.xaml
│   ├── DataGrid.xaml
│   ├── Expander.xaml
│   ├── InputControls.xaml
│   ├── ListBox.xaml
│   ├── ListView.xaml
│   ├── PasswordInput.xaml
│   ├── ProgressBar.xaml
│   ├── RadioButton.xaml
│   ├── ScrollBar.xaml
│   ├── SearchBox.xaml
│   ├── Slider.xaml
│   ├── TabControl.xaml
│   ├── TextBox.xaml
│   ├── TextInput.xaml
│   ├── ToggleButton.xaml
│   └── TreeView.xaml
├── Converters/
├── Helpers/
├── Icons/
├── Themes/
│   ├── Layers/
│   ├── Palettes/
│   └── Schemes/
├── Dimensions.xaml
├── MaterialTheme.xaml
├── Typography.xaml
└── LICENSE.txt
```

## Design Principles

The library is structured around three levels of resources:

1. **Reference tokens** — fonts, typography and base palette values.
2. **System/semantic tokens** — semantic colors, surfaces, outlines and component design tokens.
3. **Component styles** — WPF control templates and default styles.

Applications should generally consume the semantic layer rather than hard-coded colors. This keeps the UI consistent when switching between light, dark, and contrast variants.

## License

See [`LICENSE.txt`](LICENSE.txt) included with the library for the applicable license terms.

## Status

This README documents the library based on the contents of the current source distribution. The project is a WPF resource/theme library rather than a complete application framework.

API and resource keys should be considered subject to change unless explicitly treated as part of the public API.
