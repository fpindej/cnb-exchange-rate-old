## Exchange rate app

## Data source

CNB does not expose a public API

For the purpose of this project I was able to find an [XML file on CNB website](https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.xml)

## Solution structure

The solution follows a tree folder structure that corresponds with the namespaces of specific project files.

This means that every `.csproj` has a specified `RootNamespace` in order to create a default namespace.

_Example:_

```cs
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    ...
    <RootNamespace>ExchangeRate.Infrastructure.$(AssemblyName)</RootNamespace>
  </PropertyGroup>

</Project>

```