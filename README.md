# Core.EfBaseRepository

## Mongo Examples

### Abstract
```csharp
public interface IUserRepository : IMongoEntityRepository<Users>
{
}
```
### Concrete
```csharp
public class UserRepository : MongoEntityRepositoryBase<Users>, IUserRepository
{
    public UserRepository(MongodOptions options) : base(options)
    {
    }
}
```
Note: This approach allows you to perform CRUD operations for the User entity when using MongoDB.

## Ef Examples

### Abstract
```csharp
public interface IUserRepository : IEntityRepository<Users>
{
}

```
### Concrete
```csharp
public class UserRepository : EfEntityRepositoryBase<Users, ExampleContext>, IUserRepository
{
    public UserRepository(ExampleContext context) : base(context)
    {
    }
}
```
Note: This approach allows you to perform CRUD operations for the User entity when using Entity Framework.
