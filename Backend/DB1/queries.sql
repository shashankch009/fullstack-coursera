-- Create Departments table
CREATE TABLE Departments (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
);

-- Create Employees table
CREATE TABLE Employees (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    HireDate DATE NOT NULL,
    DepartmentID INT NOT NULL,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(ID)
);

-- Insert sample data into Departments
INSERT INTO Departments (Name) VALUES 
('HR'), 
('IT'), 
('Finance');


-- Insert sample data into Employees
INSERT INTO Employees (FirstName, LastName, HireDate, DepartmentID) VALUES 
('John', 'Doe', '2020-01-15', 1), 
('Jane', 'Smith', '2019-03-22', 2), 
('Emily', 'Jones', '2021-07-30', 3), 
('Michael', 'Brown', '2018-11-05', 1), 
('Sarah', 'Davis', '2022-02-10', 2);

-- Select all employees with their department names
SELECT CONCAT(e.FirstName, ' ', e.LastName) as EmployeeName, d.Name as DepartmentName 
FROM Employees as e JOIN Departments as d ON e.DepartmentID = d.ID;


-- Select average salary from Employees for each department 
SELECT d.Name AS DepartmentName, AVG(e.Salary) AS AverageSalary 
FROM Employees e
JOIN Departments d ON e.DepartmentID = d.ID
GROUP BY d.Name
HAVING AVG(e.Salary) > 50000;


-- Select average salary for each department having more than 2 employees
SELECT d.Name AS DepartmentName, AVG(e.Salary) AS AverageSalary
FROM Employees e
JOIN Departments d ON e.DepartmentID = d.ID
GROUP BY d.Name
HAVING COUNT(e.ID) > 2;

-- Select all employees who were hired after 2020
SELECT CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName, e.HireDate 
FROM Employees e
WHERE e.HireDate > '2020-01-01';

-- select employees having salary greater than average salary of their department
SELECT CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName, e.Salary
FROM Employees e
JOIN Departments d ON e.DepartmentID = d.ID
WHERE e.Salary > (SELECT AVG(e2.Salary) FROM Employees e2 WHERE e2.DepartmentID = d.ID);

-- better way of above query 
WITH DepartmentAverageSalaries AS (
    SELECT DepartmentID, AVG(Salary) AS AverageSalary
    FROM Employees
    GROUP BY DepartmentID
)
SELECT CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName, e.Salary
FROM Employees e
JOIN DepartmentAverageSalaries das ON e.DepartmentID = das.DepartmentID
WHERE e.Salary > das.AverageSalary;