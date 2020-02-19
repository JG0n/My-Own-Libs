# MyOwn.IoC

Simple auto mapper

## Run sample

The code that is commited should run like the following.

```diff
user@host:/workspace/My-Own-Libs/MyOwn.Mapper$ dotnet run
+LOG - Mapper for type 'Person' found
+LOG - Executing mapper function for property 'FirstName'
+LOG - Executing mapper function for property 'LastName'
+LOG - Setting property 'Age' from 'Person' to 'PersonDto'
+LOG - Setting property 'FavoriteMovie' from 'Person' to 'PersonDto'
!LOG - Complex property 'Address' of type 'AddressDto'
!LOG - Mapper not registered
+LOG - Setting property 'Street' from 'Address' to 'AddressDto'
+LOG - Setting property 'Street2' from 'Address' to 'AddressDto'
+LOG - Setting property 'City' from 'Address' to 'AddressDto'
+LOG - Setting property 'Country' from 'Address' to 'AddressDto'
!LOG - Property 'ZipCode' will not be mapped
Person >> PersonDto
FirstName: John         >>       John
MiddleName: F.  >>       <null>
LastName: Doe   >>       Doe
Age: 24         >>       24
FavoriteMovie: Top Gun  >>       Top Gun
Street: My own street   >>       My own street
Street2: N 13   >>       N 13
City: Tokyo     >>       Tokyo
Country: Japan  >>       Japan
PostalCode: 1234        >>       <null>
```