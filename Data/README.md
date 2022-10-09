### LOAD DATA WITH ENTITYFRAMEWORK CORE

Add the next packages: 
 ´
 dotnet add package Microsoft.EntityFrameworkCore.SqlServer
 dotnet add package Microsoft.EntityFrameworkCore.Design
 ´

Run the next command:
`
dotnet ef dbcontext scaffold "Server=LAPTOP-VAA09C66\SQLEXPRESS;Database=dbHumanResources;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Data -c HumanResourcesContext -t Employees -t Positions -t Payrolls -t Periods -t Transactions -t vwPayrollConcepts -f
`

