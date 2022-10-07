# personal_finances

## Generate EF model from DB (scaffold databasefirst)
dotnet ef dbcontext scaffold Name="Invoices" Microsoft.EntityFrameworkCore.SqlServer --context InvoicesDBContext --output-dir DataAccess/EntityFramework --schema fin --no-pluralize --no-build --force
