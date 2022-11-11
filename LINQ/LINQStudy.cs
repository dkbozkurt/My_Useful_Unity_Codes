// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LINQ
{
    /// <summary>
    /// LINQ
    /// 
    /// Ref : https://www.c-sharpcorner.com/article/linq-for-beginners/
    ///       https://www.c-sharpcorner.com/UploadFile/72d20e/concept-of-linq-with-C-Sharp/
    /// </summary>
    public class LINQStudy : MonoBehaviour
    {
        private List<Employee> _employees = new List<Employee>();
        private List<Project> _projects= new List<Project>();
        
        private void Awake() // void Main(string[] args) 
        {
            InitializeEmployees();
            InitializeProjects();

            Operations();
        }

        #region Data Setup

        public void InitializeEmployees()  
        {  
            _employees.Add(new Employee  
            {  
                EmployeeId = 1,  
                EmployeeName = "Tuba",  
                ProjectId = 100  
            });  
  
            _employees.Add(new Employee  
            {  
                EmployeeId = 2,  
                EmployeeName = "Atul",  
                ProjectId = 101  
            });  
  
            _employees.Add(new Employee  
            {  
                EmployeeId = 3,  
                EmployeeName = "Theran",  
                ProjectId = 101  
            });  
        } 
        
        public void InitializeProjects()    
        {    
            _projects.Add(new Project    
            {    
                ProjectId = 100,    
                ProjectName = "ABC"    
    
            });    
    
            _projects.Add(new Project    
            {    
                ProjectId = 101,    
                ProjectName = "PQR"    
    
            });      
        }

        #endregion

        private void Operations()
        {
            
            #region WHERE & SELECT

            // // Query Syntax
            // var querySyntax1 = from employee in _employees
            //     where employee.EmployeeName.StartsWith("T")
            //     select employee.EmployeeName;
            //
            // Debug.Log("Where in querySyntax ----");
            // foreach (var item in querySyntax1)
            // {
            //     Debug.Log(item);
            // }
            // Debug.Log("\n");
            //
            // // Method Syntax
            // var methodSyntax1 = _employees.Where(e => e.EmployeeName.StartsWith("T"));
            // Debug.Log("Where in methodSyntax ----");
            //
            // foreach (var item in methodSyntax1)
            // {
            //     Debug.Log(item.EmployeeName);   
            // }
            //
            // Debug.Log("\n");
            #endregion

            #region ORDERBY

            // // Order by ascending
            //
            // var querySyntax2 = from employee in _employees
            //     orderby employee.EmployeeName
            //     select employee.EmployeeName;
            //
            // var methodSyntax2 = _employees.OrderBy(e => e.EmployeeName);
            //
            // Debug.Log("Order by ascending in querySyntax------");  
            // foreach (var item in querySyntax2)  
            // {  
            //     Debug.Log(item);  
            // }  
            //
            // Debug.Log("Order by ascending in methodSyntax------");  
            // foreach (var item in methodSyntax2)  
            // {  
            //     Debug.Log(item.EmployeeName);  
            // }  
            //
            // Debug.Log('\n');
            //
            // // Order by descending
            //
            // var querySyntax3 = from employee in _employees
            //     orderby employee.EmployeeName descending
            //     select employee.EmployeeName;
            //
            // var methodSyntax3 = _employees.OrderByDescending(e => e.EmployeeName);
            // Debug.Log("Order by descending in querySyntax ----");
            // foreach (var item in querySyntax3)
            // {
            //     Debug.Log(item);
            // }
            //
            // Debug.Log("Order by descending in methodSyntax ----");
            // foreach (var item in methodSyntax3)
            // {
            //     Debug.Log(item.EmployeeName);
            // }
            // Debug.Log('\n');
            #endregion

            #region THEN BY
            //
            // var querySyntax4 = from employee in _employees  
            //     orderby employee.ProjectId
            //     select employee;
            //
            // var methodSyntax4 = _employees.OrderBy(e => e.ProjectId);
            //
            // // Descending
            // // var querySyntax4 = from employee in _employees  
            // //     orderby employee.ProjectId, employee.EmployeeName descending  
            // //     select employee;  
            // //
            // // var methodSyntax4 = _employees.OrderBy(e => e.ProjectId).
            // //     ThenByDescending(e => e.EmployeeName);  
            //
            // Debug.Log("Then by in querySyntax------");  
            // foreach (var item in querySyntax4)  
            // {  
            //     Debug.Log(item.EmployeeName + ":" + item.ProjectId);  
            // }  
            //
            // Debug.Log("Then by in methodSyntax------");  
            // foreach (var item in methodSyntax4)  
            // {  
            //     Debug.Log(item.EmployeeName + ":" + item.ProjectId);  
            // }  
            //
            // Debug.Log('\n');  

            #endregion

            #region TAKE
            //
            // var querySyntax5 = (from employee in _employees  
            //     select employee).Take(2);  
            //
            //
            // var methodSyntax5 = _employees.Take(2);  
            //
            //
            // Debug.Log("Take in querySyntax------");  
            // foreach (var item in querySyntax5)  
            // {  
            //     Debug.Log(item.EmployeeName);  
            // }  
            //
            // Debug.Log("Take in methodSyntax------");  
            // foreach (var item in methodSyntax5)  
            // {  
            //     Debug.Log(item.EmployeeName);  
            // }  
            //
            // Debug.Log('\n');

            #endregion

            #region SKIP
            //
            // var querySyntax6 = (from employee in _employees  
            //     select employee).Skip(2);  
            //
            // var methodSyntax6 = _employees.Skip(2);  
            //
            // Debug.Log("Skip in querySyntax------");  
            // foreach (var item in querySyntax6)  
            // {  
            //     Debug.Log(item.EmployeeName);  
            // }  
            //
            // Debug.Log("Skip in methodSyntax------");  
            // foreach (var item in methodSyntax6)  
            // {  
            //     Debug.Log(item.EmployeeName);  
            // }  
            //
            // Debug.Log('\n');  

            #endregion

            #region GROUP
            //
            // var querySyntax7 = from employee in _employees  
            //     group employee by employee.ProjectId;  
            //
            //
            // var methodSyntax7 = _employees.GroupBy(e => e.ProjectId);  
            //
            // Debug.Log("Group in querySyntax------");  
            // foreach (var item in querySyntax7)  
            // {  
            //     Debug.Log(item.Key + ":" + item.Count());  
            // }  
            //
            // Debug.Log("Group in methodSyntax------");  
            // foreach (var item in methodSyntax7)  
            // {  
            //     Debug.Log(item.Key + ":" + item.Count());  
            // }  
            //
            // Debug.Log('\n');  

            #endregion

            #region FIRST
            //
            // var querySyntax8 = (from employee in _employees  
            //     //where employee.EmployeeName.StartsWith("Q")  
            //     select employee).First();  
            //
            // var methodSyntax8 = _employees  
            //     //.Where(e => e.EmployeeName.StartsWith("Q"))                   
            //     .First();  
            //
            // Debug.Log("First in querySyntax------");  
            // if (querySyntax8 != null)  
            // {  
            //     Debug.Log(querySyntax8.EmployeeName);  
            // }  
            //
            // Debug.Log("First in methodSyntax------");  
            // if (methodSyntax8 != null)  
            // {  
            //     Debug.Log(methodSyntax8.EmployeeName);  
            // }  
            //
            // Debug.Log('\n');  

            #endregion

            #region FIRST OR DEFAULT

            // var querySyntax9 = (from employee in _employees  
            //     //where employee.EmployeeName.StartsWith("Q")  
            //     select employee).FirstOrDefault();  
            //
            // var methodSyntax9 = _employees  
            //     //.Where(e => e.EmployeeName.StartsWith("Q"))  
            //     .FirstOrDefault();  
            //
            // Debug.Log("First or default in querySyntax------");  
            // if (querySyntax9 != null)  
            // {  
            //     Debug.Log(querySyntax9.EmployeeName);  
            // }  
            //
            // Debug.Log("First or default in methodSyntax------");  
            // if (methodSyntax9 != null)  
            // {  
            //     Debug.Log(methodSyntax9.EmployeeName);  
            // }  
            //
            // Debug.Log('\n');

            #endregion

            #region JOIN
            //
            // var querySyntax10 = from employee in _employees  
            //     join project in _projects on employee.ProjectId equals project.ProjectId  
            //     select new { employee.EmployeeName, project.ProjectName };  
            //
            // var methodSyntax10 = _employees.Join(_projects,  
            //     e => e.ProjectId,  
            //     p => p.ProjectId,  
            //     (e, p) => new { e.EmployeeName, p.ProjectName });  
            //
            // Debug.Log("Join in querySyntax------");  
            // foreach (var item in querySyntax10)  
            // {  
            //     Debug.Log(item.EmployeeName + ":" + item.ProjectName);  
            // }  
            // Debug.Log("Join in methodSyntax------");  
            // foreach (var item in methodSyntax10)  
            // {  
            //     Debug.Log(item.EmployeeName + ":" + item.ProjectName);  
            // }  
            //
            // Debug.Log('\n');  

            #endregion

            #region LEFT JOIN
            // var querySyntax11 = from employee in _employees  
            //     join project in _projects on employee.ProjectId equals project.ProjectId into group1  
            //     from project in group1.DefaultIfEmpty()  
            //     select new { employee.EmployeeName, ProjectName = project?.ProjectName ?? "NULL" };             
            //
            // Debug.Log("Left Join in querySyntax------");  
            // foreach (var item in querySyntax11)  
            // {  
            //     Debug.Log(item.EmployeeName + ":" + item.ProjectName);  
            // }  
            //
            // Debug.Log('\n');

            #endregion

            #region SIMPLE SELECT 
            
            // int[] numbers = { 5, 4, 1, 3, 9, 8};  
            // var numsPlusOne =from n in numbers select n;  
            // foreach (var i in numsPlusOne)  
            // {  
            //     Debug.Log(i.ToString());  
            // }  

            #endregion

            #region MULTIPLE SELECT

            // int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };  
            // int[] numbersB = { 1, 3, 5, 7, 8 };  
            // var pairs =from a in numbersA from b in numbersB where a < b select new { a, b };
            // Debug.Log("Pairs where a < b:");  
            // foreach (var pair in pairs)  
            // {  
            //     Debug.Log("{0} is less than {1}", pair.a, pair.b);  
            // } 

            #endregion

            #region COUNT FUNCTION

            // int[] factorsOf300 = { 2, 2, 3, 5, 5 };  
            // int uniqueFactors = factorsOf300.Distinct().Count();  
            // Debug.Log("There are {0} unique factors of 300.", uniqueFactors);

            #endregion

            #region OR
            //
            // int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            // int oddNumbers = numbers.Count(n => n % 2 == 1);
            // Debug.Log("There are "+oddNumbers+" odd numbers in the list.");

            #endregion

            #region ALL

            // bool isAllPlayerNumbersGreaterThanHundred = _projects.All(p => p.ProjectId > 100);
            // Debug.Log("All Project Numbers are greater than 100 " + isAllPlayerNumbersGreaterThanHundred);

            #endregion
        }
    }
    
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int ProjectId { get; set; }
    }

    public class Project
    {
        public int ProjectId { get; set; }  
        public string ProjectName { get; set; }  
    }
}