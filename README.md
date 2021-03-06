# DotNet CEP Search

[![Nuget](https://img.shields.io/nuget/v/DotNetCEPSearch)](https://www.nuget.org/packages/DotNetCEPSearch/) ![Nuget](https://img.shields.io/nuget/dt/DotNetCEPSearch)

It is a .NET Standard library that helps you to get a CEP or Address from brazilian's postal service.
You can use both in .NET Framework 4.x and .NET Core 2.x applications.

## Notes
Version 1.0.2:

Fixed URI principal

## Installation

Use the package manager to install.

```bash
Install-Package DotNetCEPSearch -Version 1.0.2
```

## Usage

After install:
```C#
using DotNet.CEP.Search.App;
```
Get Addres by CEP
```C#
CepSearch cep = new CepSearch();
string jsonResultAsync = await cep.GetAddressByCepAsync("numberOfCep");

string jsonResult = GetAddressByCep("numberOfCep");
```
Get CEP by Address
```C#
CepSearch cep = new CepSearch();
string jsonResultAsync = await cep.GetCepByAddressAsync("address");

string jsonResult = cep.GetCepByAddress("address");
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)
