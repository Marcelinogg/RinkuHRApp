### SPECIFICATIONS
- IDE's and tools used: **Visual Studio Code, SQL Server Management Studio 15.18**
- Programming languages, frameworks and versions used:
    - **MS SQL Server**
    - **C#**
    - **Javascript**
    - **HTML**
    - **bootstrap 5 (CSS)**
    - **.Net 6 (dotnet)**
    - **Entity Framework Core 6**
- Implemented architecture: **Model View Controller**

### LOAD DATA WITH ENTITYFRAMEWORK CORE

Add the next packages: 
 ```
 dotnet add package Microsoft.EntityFrameworkCore.SqlServer
 dotnet add package Microsoft.EntityFrameworkCore.Design
 ```

Run the next command:
```
dotnet ef dbcontext scaffold "Server=LAPTOP-VAA09C66\SQLEXPRESS;Database=dbHumanResources;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Data -c HumanResourcesContext -t Employees -t Positions -t Payrolls -t Periods -t Transactions -t vwPayrollConcepts -t Concepts -t EmployeeStatus -f
```

