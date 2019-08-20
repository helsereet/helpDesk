using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace UsersSupport.Data
{
    public class DataAccess
    {
        public event EventHandler PositionExistException;

        public event EventHandler DepartmentExistException;

        public event EventHandler RequestThemeExistException;

        public event EventHandler RequestTypeExistException;

        public event EventHandler UserNameExistException;

        public event EventHandler EmailExistException;

        public event EventHandler RequestSettingExistException;

        public event EventHandler DBConnectionException;

        public event EventHandler SuccessRecordDelete;

        public event EventHandler SuccessRecordUpdate;

        public event EventHandler SuccessRecordInsert;

        public event EventHandler UndefinedException;

        public event EventHandler RequestAlreadyTaken;

        public event EventHandler SuccessTakenRequest;

        public event EventHandler RequestAlreadyClosed;

        public event EventHandler SuccessClosedRequest;

        public event EventHandler SuccessUpdatedPassword;

        public event EventHandler PasswordDoesnotMatch;

        public User GetUser(string userName, string password)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT e.Id, e.DepartmentId, e.PositionId, e.RoleId, e.Password, e.FirstName, e.LastName, e.SurName, " +
                             "d.DepartmentName, p.PositionName, r.RoleName, e.UserName, e.Email, e.PhoneNumber, e.RoomNumber, " +
                             "e.Address, e.WorkingState " +
                             "FROM EMPLOYEE e " +
                             "INNER JOIN DEPARTMENT d ON e.DepartmentId = d.Id " +
                             "INNER JOIN POSITION p ON e.PositionId = p.Id " +
                             "INNER JOIN ROLE r ON e.RoleId = r.Id " +
                             "WHERE e.UserName = @UserName AND e.Password = @Password";
                return connection.Query<User>(sql, new { UserName = userName, Password = password }).SingleOrDefault();
            }
        }

        public void SolveRequest(Hashtable hashtable)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "dbo.SolveRequest @RequestId, @SolvedByEmployeeId, @Solution";
                connection.Execute(sql, new
                {
                    RequestId = hashtable["requestId"],
                    SolvedByEmployeeId = hashtable["solvedByEmployeeId"],
                    Solution = hashtable["solution"]
                });
            }
        }

        public void SimpleDelete(string tableName, int id)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = $"DELETE {tableName} WHERE Id = @Id";
                connection.Execute(sql, new { Id = id });
            }
            SuccessRecordDelete?.Invoke(this, null);
        }

        public void SimpleUpdate(string tableName, string columnName, string value, int id)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = $"UPDATE {tableName} SET {columnName} = @Value WHERE Id = @Id";
                var exception = false;
                try
                {
                    connection.Execute(sql, new { Value = value, Id = id });
                }
                catch (SqlException ex)
                {
                    exception = true;
                    if (ex.Message.Contains("UQ_POSITION_PositionName"))
                    {
                        PositionExistException?.Invoke(this, null);
                    }
                    else if (ex.Message.Contains("UQ_DEPARTMENT_DepartmentName"))
                    {
                        DepartmentExistException?.Invoke(this, null);
                    }
                    else if (ex.Message.Contains("UQ_REQUEST_THEME_ThemeName"))
                    {
                        RequestThemeExistException?.Invoke(this, null);
                    }
                    else if (ex.Message.Contains("UQ_REQUEST_TYPE_TypeName"))
                    {
                        RequestTypeExistException?.Invoke(this, null);
                    }
                }
                if (!exception)
                    SuccessRecordUpdate?.Invoke(this, null);
            }
        }

        public List<Request> GetRequests()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT r.Id, " +
                                "rty.TypeName, " +
                                "rth.ThemeName, " +
                                "rtrt.Description, " +
                                "r.ShortDescription, " +
                                "r.FullDescription, " +
                                "r.Solution, " +
                                "r.CustomerMark " +
                                "FROM REQUEST r " +
                                "LEFT JOIN REQUEST_TYPE_REQUEST_THEME rtrt ON r.RequestTypeRequestThemeId = rtrt.Id " +
                                "LEFT JOIN REQUEST_TYPE rty ON rtrt.RequestTypeId = rty.Id " +
                                "LEFT JOIN REQUEST_THEME rth ON rtrt.RequestThemeId = rth.Id";
                var result = connection.Query<Request>(sql).ToList();
                return result;
            }
        }

        public void SimpleInsert(string tableName, string value)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = $"INSERT INTO {tableName} VALUES(@Value)";
                var exception = false;
                try
                {
                    connection.Execute(sql, new { Value = value });
                }
                catch (SqlException ex)
                {
                    exception = true;
                    if (ex.Message.Contains("UQ_POSITION_PositionName"))
                    {
                        PositionExistException?.Invoke(this, null);
                    }
                    else if (ex.Message.Contains("UQ_DEPARTMENT_DepartmentName"))
                    {
                        DepartmentExistException?.Invoke(this, null);
                    }
                    else if (ex.Message.Contains("UQ_REQUEST_THEME_ThemeName"))
                    {
                        RequestThemeExistException?.Invoke(this, null);
                    }
                    else if (ex.Message.Contains("UQ_REQUEST_TYPE_TypeName"))
                    {
                        RequestTypeExistException?.Invoke(this, null);
                    }
                }
                if (!exception)
                    SuccessRecordInsert?.Invoke(this, null);
            }
        }

        public void AddRequestSetting(Hashtable hashtable)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "INSERT INTO REQUEST_TYPE_REQUEST_THEME VALUES" +
                             "(@TypeId, @ThemeId, @Description, @NormExecutionTimeInSeconds, @NormExecutionTimeReadable)";
                var exception = false;

                try
                {
                    connection.Execute(sql, new
                    {
                        TypeId = hashtable["requestType"],
                        ThemeId = hashtable["requestTheme"],
                        Description = hashtable["description"],
                        NormExecutionTimeInSeconds = hashtable["normExecutionTimeInSeconds"],
                        NormExecutionTimeReadable = hashtable["normExecutionTimeReadable"]
                    });
                }
                catch (SqlException ex)
                {
                    exception = true;
                    if (ex.Message.Contains("UQ_REQUEST_TYPE_REQUEST_THEME"))
                        RequestSettingExistException?.Invoke(this, null);
                }
                if (!exception)
                    SuccessRecordInsert?.Invoke(this, null);
            }
        }

        public void UpdateRequestSetting(Hashtable hashtable)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "UPDATE REQUEST_TYPE_REQUEST_THEME SET " +
                             "RequestTypeId = @TypeId, " +
                             "RequestThemeId = @ThemeId, " +
                             "Description = @Description, " +
                             "NormExecutionTimeInSeconds = @NormExecutionTimeInSeconds, " +
                             "NormExecutionTimeReadable = @NormExecutionTimeReadable " +
                             "WHERE Id = @Id";
                var exception = false;
                try
                {
                    connection.Execute(sql, new
                    {
                        TypeId = hashtable["requestType"],
                        ThemeId = hashtable["requestTheme"],
                        Description = hashtable["description"],
                        NormExecutionTimeInSeconds = hashtable["normExecutionTimeInSeconds"],
                        NormExecutionTimeReadable = hashtable["normExecutionTimeReadable"],
                        Id = hashtable["id"]
                    });
                }
                catch (SqlException ex)
                {
                    exception = true;
                    if (ex.Message.Contains("UQ_REQUEST_TYPE_REQUEST_THEME"))
                        RequestSettingExistException?.Invoke(this, null);
                }
                if (!exception)
                    SuccessRecordUpdate?.Invoke(this, null);
            }
        }

        public List<Employee> GetEmployees()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT e.Id, e.DepartmentId, e.PositionId, e.RoleId, e.Password, e.FirstName, e.LastName, e.SurName, " +
                             "d.DepartmentName, p.PositionName, r.RoleName, e.UserName, e.Email, e.PhoneNumber, e.RoomNumber, e.Address, e.WorkingState " +
                             "FROM EMPLOYEE e " +
                             "LEFT JOIN DEPARTMENT d ON e.DepartmentId = d.Id " +
                             "LEFT JOIN POSITION p ON e.PositionId = p.Id " +
                             "LEFT JOIN ROLE r ON e.RoleId = r.Id";
                return connection.Query<Employee>(sql).ToList();
            }
        }

        public List<Position> GetPositions()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM POSITION ORDER BY PositionName";
                return connection.Query<Position>(sql).ToList();
            }
        }

        public List<Department> GetDepartments()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM DEPARTMENT ORDER BY DepartmentName";
                return connection.Query<Department>(sql).ToList();
            }
        }

        public List<Role> GetRoles()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM ROLE";
                return connection.Query<Role>(sql).ToList();
            }
        }

        public List<RequestTheme> GetRequestThemes()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM REQUEST_THEME ORDER BY ThemeName";
                return connection.Query<RequestTheme>(sql).ToList();
            }
        }

        public List<RequestTheme> GetRequestThemes(int requestTypeId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT  rt.Id, rt.ThemeName FROM REQUEST_TYPE_REQUEST_THEME rtrt " +
                             "INNER JOIN REQUEST_THEME rt ON rtrt.RequestThemeId = rt.Id " +
                             "WHERE rtrt.RequestTypeId = @RequestTypeId";
                return connection.Query<RequestTheme>(sql, new { RequestTypeId = requestTypeId }).ToList();
            }
        }

        public List<RequestTypeRequestTheme> GetRequestTypeRequestThemes(int requestTypeId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT  rtrt.Id, rtrt.RequestThemeId, rtrt.RequestTypeId, rt.ThemeName, rtrt.Description FROM REQUEST_TYPE_REQUEST_THEME rtrt " +
                             "INNER JOIN REQUEST_THEME rt ON rtrt.RequestThemeId = rt.Id " +
                             "WHERE rtrt.RequestTypeId = @RequestTypeId";
                var result = connection.Query<RequestTypeRequestTheme>(sql, new { RequestTypeId = requestTypeId }).ToList();
                return result;
            }
        }

        public void AddRequest(Hashtable hashtable)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "INSERT INTO REQUEST(RequestTypeRequestThemeId, ShortDescription, FullDescription, CreatedAt, CreatedByEmployeeId) " +
                             "VALUES ( @RequestTypeRequestThemeId, @ShortDescription, @FullDescription, GETUTCDATE(), @CreatedByEmployeeId )";
                var exception = false;

                try
                {
                    connection.Execute(sql, new
                    {
                        RequestTypeRequestThemeId = hashtable["requestTypeRequestThemeId"],
                        ShortDescription = hashtable["shortDescription"],
                        FullDescription = hashtable["fullDescription"],
                        CreatedByEmployeeId = hashtable["createdByEmployeeId"]
                    });
                }
                catch (Exception)
                {
                    exception = true;
                }

                if (exception)
                    UndefinedException?.Invoke(this, null);
                else
                    SuccessRecordInsert?.Invoke(this, null);
            }
        }

        public List<Request> GetOpenedRequests(int currentUserId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM REQUEST " +
                             "WHERE CreatedByEmployeeId = @CreatedByEmployeeId AND Closed = 0";
                return connection.Query<Request>(sql, new { CreatedByEmployeeId = currentUserId }).ToList();
            }
        }

        public List<Request> GetOpenedNotTakenRequests(int currentUserId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM REQUEST " +
                             "WHERE CreatedByEmployeeId = @CreatedByEmployeeId AND Taken = 0 AND Closed = 0";
                return connection.Query<Request>(sql, new { CreatedByEmployeeId = currentUserId }).ToList();
            }
        }

        public List<Request> GetAllOpenedTasks()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM REQUEST WHERE Closed = 0";
                return connection.Query<Request>(sql).ToList();
            }
        }

        public List<RequestType> GetRequestTypes()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM REQUEST_TYPE ORDER BY TypeName";
                return connection.Query<RequestType>(sql).ToList();
            }
        }

        public void AddEmployee(Dictionary<string, string> txtBoxes,
                                  Dictionary<string, int> cmbBoxes,
                                  Dictionary<string, bool> chkBoxes)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "INSERT INTO EMPLOYEE(PositionId, DepartmentId, RoleId, UserName, Password, FirstName, " +
                             "LastName, SurName, RoomNumber, Address, Email, PhoneNumber, WorkingState) VALUES " +
                             "(@PositionId, @DepartmentId, @RoleId, @UserName, @Password, @FirstName, @LastName, " +
                             "@SurName, @RoomNumber, @Address, @Email, @PhoneNumber, @WorkingState)";
                var exception = false;
                try
                {
                    connection.Execute(sql, new
                    {
                        PositionId = cmbBoxes["positionsCmbBox"],
                        DepartmentId = cmbBoxes["departmentsCmbBox"],
                        RoleId = cmbBoxes["rolesCmbBox"],
                        UserName = txtBoxes["userNameTxtBox"],
                        Password = txtBoxes["passwordTxtBox"],
                        FirstName = txtBoxes["firstNameTxtBox"],
                        LastName = txtBoxes["lastNameTxtBox"],
                        SurName = txtBoxes["surNameTxtBox"],
                        RoomNumber = txtBoxes["roomNumberTxtBox"],
                        Address = txtBoxes["addressTxtBox"],
                        Email = txtBoxes["emailTxtBox"],
                        PhoneNumber = txtBoxes["phoneNumberTxtBox"],
                        WorkingState = chkBoxes["workingStateChkBox"]
                    });
                }
                catch (SqlException ex)
                {
                    exception = true;
                    if (ex.Message.Contains("UserName"))
                    {
                        UserNameExistException?.Invoke(this, null);
                    }
                    else if (ex.Message.Contains("Email"))
                    {
                        EmailExistException?.Invoke(this, null);
                    }
                    else
                    {
                        DBConnectionException?.Invoke(this, null);
                    }
                }
                if (!exception)
                    SuccessRecordInsert?.Invoke(this, null);
            }
        }

        public void UpdateEmployee(Dictionary<string, string> txtBoxes,
                                     Dictionary<string, int> cmbBoxes,
                                     Dictionary<string, bool> chkBoxes,
                                     int employeeId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "UPDATE EMPLOYEE " +
                             "SET  PositionId = @PositionId, " +
                                  "DepartmentId = @DepartmentId, " +
                                  "RoleId = @RoleId, " +
                                  "UserName = @UserName, " +
                                  "Password = @Password, " +
                                  "FirstName = @FirstName, " +
                                  "LastName = @LastName, " +
                                  "SurName = @SurName, " +
                                  "RoomNumber = @RoomNumber, " +
                                  "Address = @Address, " +
                                  "Email = @Email, " +
                                  "PhoneNumber = @PhoneNumber, " +
                                  "WorkingState = @WorkingState " +
                             "WHERE Id = @Id";
                var exception = false;
                try
                {
                    connection.Execute(sql, new
                    {
                        PositionId = cmbBoxes["positionsCmbBox"],
                        DepartmentId = cmbBoxes["departmentsCmbBox"],
                        RoleId = cmbBoxes["rolesCmbBox"],
                        UserName = txtBoxes["userNameTxtBox"],
                        Password = txtBoxes["passwordTxtBox"],
                        FirstName = txtBoxes["firstNameTxtBox"],
                        LastName = txtBoxes["lastNameTxtBox"],
                        SurName = txtBoxes["surNameTxtBox"],
                        RoomNumber = txtBoxes["roomNumberTxtBox"],
                        Address = txtBoxes["addressTxtBox"],
                        Email = txtBoxes["emailTxtBox"],
                        PhoneNumber = txtBoxes["phoneNumberTxtBox"],
                        WorkingState = chkBoxes["workingStateChkBox"],
                        Id = employeeId
                    });
                }
                catch (SqlException ex)
                {
                    exception = true;
                    if (ex.Message.Contains("UserName"))
                    {
                        UserNameExistException?.Invoke(this, null);
                    }
                    else if (ex.Message.Contains("Email"))
                    {
                        EmailExistException?.Invoke(this, null);
                    }
                }
                if (!exception)
                    SuccessRecordUpdate?.Invoke(this, null);
            }
        }

        public List<RequestSetting> GetRequestSettings()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT rr.Id, rr.RequestTypeId, rr.RequestThemeId, rtp.TypeName, " +
                             "rth.ThemeName, rr.Description, rr.NormExecutionTimeInSeconds, " +
                             "rr.NormExecutionTimeReadable " +
                             "FROM REQUEST_TYPE_REQUEST_THEME rr " +
                             "INNER JOIN REQUEST_TYPE rtp ON rr.RequestTypeId = rtp.Id " +
                             "INNER JOIN REQUEST_THEME rth ON rr.RequestThemeId = rth.Id " +
                             "ORDER BY rtp.TypeName, rth.ThemeName";
                return connection.Query<RequestSetting>(sql).ToList();
            }
        }

        public void DeleteEmployee(int employeeId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "DELETE EMPLOYEE WHERE Id = @Id";
                connection.Execute(sql, new { Id = employeeId });
            }
        }

        public string GetCurrentRoleName(int currentRoleId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT RoleName FROM ROLE WHERE Id = @Id";
                var role = connection.Query<Role>(sql, new { Id = currentRoleId }).SingleOrDefault();
                return role?.RoleName;
            }
        }

        public List<Request> GetSolvedNotClosedTasks(int currentUserId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM REQUEST " +
                             "WHERE LastTakenByEmployeeId = @LastTakenByEmployeeId AND Solved = 1 AND Closed = 0";
                var result = connection.Query<Request>(sql, new { LastTakenByEmployeeId = currentUserId }).ToList();
                return result;
            }
        }

        public List<Request> GetSolvedNotClosedRequests(int currentUserId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM REQUEST " +
                             "WHERE CreatedByEmployeeId = @CreatedByEmployeeId AND Solved = 1 AND Closed = 0";
                var result = connection.Query<Request>(sql, new { CreatedByEmployeeId = currentUserId }).ToList();
                return result;
            }
        }

        public List<Request> GetAllOpenedAtWorkTasks()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM REQUEST " +
                             "WHERE Taken = 1 AND Closed = 0";
                return connection.Query<Request>(sql).ToList();
            }
        }

        public List<Request> GetAllOpenedNotTakenTasks()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM REQUEST WHERE Taken = 0 AND Closed = 0";
                return connection.Query<Request>(sql).ToList();
            }
        }

        public List<Request> GetClosedRequests(int currentUserId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM REQUEST " +
                             "WHERE CreatedByEmployeeId = @CreatedByEmployeeId AND Closed = 1";
                return connection.Query<Request>(sql, new { CreatedByEmployeeId = currentUserId }).ToList();
            }
        }

        public List<Request> GetClosedTasks(int currentUserId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM REQUEST " +
                             "WHERE LastTakenByEmployeeId = @LastTakenByEmployeeId AND Closed = 1";
                return connection.Query<Request>(sql, new { LastTakenByEmployeeId = currentUserId }).ToList();
            }
        }

        public List<Request> GetTakenTasks(int currentUserId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM REQUEST " +
                             "WHERE LastTakenByEmployeeId = @LastTakenByEmployeeId AND Taken = 1 AND Closed = 0";
                var result = connection.Query<Request>(sql, new { LastTakenByEmployeeId = currentUserId }).ToList();
                return result;
            }
        }

        public List<Request> GetAllClosedTasks()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT * FROM REQUEST " +
                             "WHERE Closed = 1";
                var result = connection.Query<Request>(sql).ToList();
                return result;
            }
        }

        public Request GetRequest(int requestId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                string sql = "SELECT rty.TypeName, " +
                    "rth.ThemeName, " +
                    "rtrt.Description, " +
                    "r.ShortDescription, " +
                    "r.FullDescription, " +
                    "r.Solution, " +
                    "e.LastName + ' ' + e.FirstName + ' ' + e.SurName as CustomerFullName, " +
                    "ee.LastName + ' ' + ee.FirstName + ' ' + ee.SurName as PerformerFullName, " +
                    "r.CreatedAt, " +
                    "r.LastTakenByEmployeeId, " +
                    "r.Taken, " +
                    "r.Solved, " +
                    "r.SolvedAt, " +
                    "r.Closed, " +
                    "r.ClosedAt, " +
                    "r.CustomerMark, " +
                    "r.SpentOnSolutionInSeconds, " +
                    "rtrt.NormExecutionTimeInSeconds " +
                    "FROM REQUEST r " +
                    "LEFT JOIN REQUEST_TYPE_REQUEST_THEME rtrt ON r.RequestTypeRequestThemeId = rtrt.Id " +
                    "LEFT JOIN REQUEST_TYPE rty ON rtrt.RequestTypeId = rty.Id " +
                    "LEFT JOIN REQUEST_THEME rth ON rtrt.RequestThemeId = rth.Id " +
                    "LEFT JOIN EMPLOYEE e ON r.CreatedByEmployeeId = e.Id " +
                    "LEFT JOIN EMPLOYEE ee ON r.LastTakenByEmployeeId = ee.Id " +
                    "WHERE r.Id = @Id";
                var result = connection.Query<Request>(sql, new { Id = requestId }).FirstOrDefault();
                return result;
            }
        }

        public void TakeRequest(int performerId, int requestId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                // -1 = need to update form(request already taken); 1 = not need to update form(success taken)
                var output = connection.Execute("dbo.TakeRequest @LastTakenByEmployeeId, @Id", new { LastTakenByEmployeeId = performerId, Id = requestId });
                if (output == -1)
                    RequestAlreadyTaken?.Invoke(this, null);
                else
                    SuccessTakenRequest?.Invoke(this, null);
            }
        }

        public void ReturnRequest(int requestId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "UPDATE REQUEST SET Taken = 0, TakenAt = NULL WHERE Id = @Id";
                connection.Execute(sql, new { Id = requestId });
            }
        }

        public void CloseRequest(int requestId, int customerMark, int points)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var output = connection.Execute("dbo.CloseRequest @Id, @CustomerMark, @Points", new
                {
                    Id = requestId,
                    CustomerMark = customerMark,
                    Points = points
                });
                if (output == -1)
                    RequestAlreadyClosed?.Invoke(this, null);
                else
                    SuccessClosedRequest?.Invoke(this, null);
            }
        }

        public int GetOpenedRequestsCount(int employeeId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "SELECT COUNT(*) FROM REQUEST WHERE Closed = 0 AND CreatedByEmployeeId = @CreatedByEmployeeId";
                var result = connection.ExecuteScalar<int>(sql, new { CreatedByEmployeeId = employeeId });
                return result;
            }
        }

        public int GetSolvedNotClosedRequestsCount(int employeeId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "SELECT COUNT(*) FROM REQUEST WHERE Solved = 1 AND Closed = 0 AND CreatedByEmployeeId = @CreatedByEmployeeId";
                var result = connection.ExecuteScalar<int>(sql, new { CreatedByEmployeeId = employeeId });
                return result;
            }
        }

        public int GetClosedRequestsCount(int employeeId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "SELECT COUNT(*) FROM REQUEST WHERE Closed = 1 AND CreatedByEmployeeId = @CreatedByEmployeeId";
                var result = connection.ExecuteScalar<int>(sql, new { CreatedByEmployeeId = employeeId });
                return result;
            }
        }

        public int GetOpenedTasksCount(int employeeId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "SELECT COUNT(*) FROM REQUEST WHERE Closed = 0 AND LastTakenByEmployeeId = @LastTakenByEmployeeId";
                var result = connection.ExecuteScalar<int>(sql, new { LastTakenByEmployeeId = employeeId });
                return result;
            }
        }

        public int GetSolvedNotClosedTasksCount(int employeeId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "SELECT COUNT(*) FROM REQUEST WHERE Solved = 1 AND Closed = 0 AND LastTakenByEmployeeId = @LastTakenByEmployeeId";
                var result = connection.ExecuteScalar<int>(sql, new { LastTakenByEmployeeId = employeeId });
                return result;
            }
        }

        public int GetClosedTasksCount(int employeeId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "SELECT COUNT(*) FROM REQUEST WHERE Closed = 1 AND LastTakenByEmployeeId = @LastTakenByEmployeeId";
                var result = connection.ExecuteScalar<int>(sql, new { LastTakenByEmployeeId = employeeId });
                return result;
            }
        }

        public void UpdatePassword(string userName, string currentPassword, string newPassword)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "dbo.UpdatePassword @UserName, @CurrentPassword, @NewPassword";
                var result = connection.ExecuteScalar<int>(sql, new
                {
                    UserName = userName,
                    CurrentPassword = currentPassword,
                    NewPassword = newPassword
                });

                if (result == 1)
                    SuccessUpdatedPassword?.Invoke(this, null);
                else
                    PasswordDoesnotMatch?.Invoke(this, null);
            }
        }

        public PerformerStatistics GetPerformerStatisticsForCurrentDay(int solvedByEmployeeId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "dbo.GetPerformerStatisticsForCurrentDay @SolvedByEmployeeId";
                var result = connection.Query<PerformerStatistics>(sql, new { SolvedByEmployeeId = solvedByEmployeeId }).SingleOrDefault();
                return result;
            }
        }

        public PerformerStatistics GetPerformerStatisticsForCurrentWeek(int solvedByEmployeeId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "dbo.GetPerformerStatisticsForCurrentWeek @SolvedByEmployeeId";
                var result = connection.Query<PerformerStatistics>(sql, new { SolvedByEmployeeId = solvedByEmployeeId }).SingleOrDefault();
                return result;
            }
        }

        public PerformerStatistics GetPerformerStatisticsForCurrentMonth(int solvedByEmployeeId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "dbo.GetPerformerStatisticsForCurrentMonth @SolvedByEmployeeId";
                var result = connection.Query<PerformerStatistics>(sql, new { SolvedByEmployeeId = solvedByEmployeeId }).SingleOrDefault();
                return result;
            }
        }

        public List<PerformerStatistics> GetAllPerformersStatisticsForCurrentDay()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "dbo.GetAllPerformersStatisticsForCurrentDay";
                var result = connection.Query<PerformerStatistics>(sql).ToList();
                return result;
            }
        }

        public List<PerformerStatistics> GetAllPerformersStatisticsForCurrentWeek()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "dbo.GetAllPerformersStatisticsForCurrentWeek";
                var result = connection.Query<PerformerStatistics>(sql).ToList();
                return result;
            }
        }

        public List<PerformerStatistics> GetAllPerformersStatisticsForCurrentMonth()
        {
            using (IDbConnection connection = new SqlConnection(Helper.GetConnectionString("UserSupportDB")))
            {
                var sql = "dbo.GetAllPerformersStatisticsForCurrentMonth";
                var result = connection.Query<PerformerStatistics>(sql).ToList();
                return result;
            }
        }
    }
}
