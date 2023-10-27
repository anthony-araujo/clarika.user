# ClarikaAppService
### Application Developer Interview Test
### Class Diagram proposed 

```mermaid
classDiagram
  class UserApp {
    + Id : Guid
    + Email: String
    + FirstName : String
    + LastName : String
    + DateBirth : Date
    + Age : Integer
    + UserLocations : List~UserLocation~
    + Country : Country
    + PasswordHash: String
    + SecurityStamp: String
    + ConcurrencyStamp: String

  }

  class UserLocation {
    + Id : Guid
    + Address : String
    + ZipCode : String
    + Province : String
    + UserAppId : Guid
    + CountryId : Integer
    + StateId : State
    + CityId : Integer
  }

  class Country {
    + Id : Integer
    + Name : String
    + States : List~State~
    + UserLocations : List~UserLocation~
  }

  class State {
    + Id : Integer
    + Name : String
    + CountryId: Integer
    + Cities : List~City~
    + UserLocations : List~UserLocation~
  }

    class LocationType {
    + Id : Integer
    + Name : String
    + UserLocations : List~UserLocation~
  }

  class City {
    + Id : Integer
    + Name : String
    + StateId: Integer
    + UserLocations : List~UserLocation~
  }

  UserApp "1" -- "1..*" UserLocation 
  Country "1" -- "1..*" State
  Country "1" -- "1..*" UserApp
  State "1" -- "1..*" City 
  City "1" -- "1..*" UserLocation 
  State "1" -- "1..*" UserLocation 
  Country "1" -- "1..*" UserLocation
  LocationType "1" -- "1..*" UserLocation
  
```

### 🌍 Countries States Cities Database Integration **API**
- Reference: [API Documentation](https://countrystatecity.in/)
- Demo : https://dr5hn.github.io/countries-states-cities-database/

## Development

Before you can build this project, you must install and configure the following dependencies on your machine:

1. Node.js: We use Node to run a development web server and build the project.
1. 
   
In ./src/ClarikaAppService/ClientApp run

    npm install

### Using angular-cli

You can also use [Angular CLI] to generate some custom client code.

For example, the following command:

    ng generate component my-component

will generate few files:

    create ClarikaAppService/ClientApp/src/app/my-component/my-component.component.html
    create ClarikaAppService/ClientApp/src/app/my-component/my-component.component.ts
    update ClarikaAppService/ClientApp/src/app/app.module.ts

## Code style / formatting

To format the dotnet code, run

    dotnet format

## Testing

To launch your application's tests, run:

    dotnet test --verbosity normal

### Client tests

In ClientApp folder run :

    npm test
