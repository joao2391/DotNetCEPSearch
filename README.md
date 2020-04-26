# DotNet CEP Search

[![Nuget](https://img.shields.io/nuget/v/DotNetCEPSearch)](https://www.nuget.org/packages/DotNetCEPSearch/) ![Nuget](https://img.shields.io/nuget/dt/DotNetCEPSearch)

.Net library for helping you to get CEP or Address.

## Notes
Version 1.0.1:

Fixed some issues

## Installation

Use the package manager to install.

```bash
Install-Package DotNetCEPSearch -Version 1.0.0
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
