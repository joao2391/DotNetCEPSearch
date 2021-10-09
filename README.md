# DotNet CEP Search

[![Nuget](https://img.shields.io/nuget/v/DotNetCEPSearch)](https://www.nuget.org/packages/DotNetCEPSearch/) ![Nuget](https://img.shields.io/nuget/dt/DotNetCEPSearch)

It is a .NET Standard library that helps you to get a CEP or Address from brazilian's postal service.
You can use both in .NET Framework 4.x and .NET Core 2.x applications.

## Notes
Version 1.1.0:

- Migrated to dotnet 5
- Changed return's type

## Installation

Use the package manager to install.

```bash
Install-Package DotNetCEPSearch -Version 1.1.0
```

## Usage

After install:
```C#
using DotNet.CEP.Search.App;
```
Get Addres by CEP
```C#
CepSearch cep = new CepSearch();
// Returns an object's array with 
//Rua, Bairro, Cidade, Cep, Uf
var addressAsync = await cep.GetAddressByCepAsync("CEP");

var address = GetAddressByCep("CEP");
```

Get CEP by Address
```C#
CepSearch cep = new CepSearch();
// Returns an object's array with 
//Rua, Bairro, Cidade, Cep, Uf
var cepAsync = await cep.GetCepByAddressAsync("address");

var cep = cep.GetCepByAddress("address");
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)
