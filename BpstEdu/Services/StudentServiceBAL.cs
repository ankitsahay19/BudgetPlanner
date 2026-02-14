using BpstEdu.Data;
using BpstEdu.Models;
using Microsoft.AspNetCore.Identity; 
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BpstEdu.Services
{
    public class StudentServiceBAL(AppDbContext dbContext, IUserServiceBAL userServiceBal) : IStudentServiceBAL
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IUserServiceBAL _userServiceBal = userServiceBal;
        //public async Task<bool> MoveApplicationToBatch(int applicationId, int batchId)
        //{
        //    bool result = false;
        //    var stuApplication = await _dbContext.Applications.FindAsync(applicationId);
        //    var batch = await _dbContext.Batchs.FindAsync(batchId);
        //    if (stuApplication != null && batch != null)
        //    {
        //        var newStudent = GetNewStudentObjFromApplication(stuApplication);
        //        await CreateStudent(newStudent);
        //        await AddStudentToBatch(newStudent, batch, stuApplication);
        //        var _stuLoginAccountResult = await AddStudentLoginDetails(newStudent);
        //        if (!_stuLoginAccountResult.Succeeded)
        //        {
        //            var sbErrorDetail = new StringBuilder();
        //            foreach (var error in _stuLoginAccountResult.Errors)
        //                sbErrorDetail.Append(error.Description + " | " + error.Description);
        //            newStudent.ErrorLogDuringLoginGenration = sbErrorDetail.ToString();
        //        }
        //        await _dbContext.SaveChangesAsync();
        //        result = true;
        //    }
        //    return result;
        //}
        //public async Task<bool> CreateStudent(Student student)
        //{
        //    var result = false;
        //    if (student.UniqueId == 0)
        //    {
        //        student.LastUpdatedDate = student.CreatedDate = DateTime.UtcNow;
        //        _dbContext.Add(student);
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        student.LastUpdatedDate = DateTime.UtcNow;
        //        _dbContext.Update(student);
        //    }
        //    await _dbContext.SaveChangesAsync();
        //    if (student.Aadhar != null && student.Aadhar.Length > 0)
        //    {
        //        student.AadharFileUrl = await Common.CommonFuntions.UploadFile(student.Aadhar, "Student", student.UniqueId, "Aadhar");
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    if (student.Pan != null && student.Pan.Length > 0)
        //    {
        //        student.PanFileUrl = await Common.CommonFuntions.UploadFile(student.Pan, "Student", student.UniqueId, "Pan");
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    return result;
        //}
        //public async Task<IdentityResult> AddStudentLoginDetails(Student student)
        //{
        //    var appUser = new AppUser()
        //    {
        //        UserName = student.Email,
        //        Email = student.Email,
        //        Password = Common.CommonFuntions.GetDefaultPassword(student.DateOfBirth),
        //        ConfirmPassword = Common.CommonFuntions.GetDefaultPassword(student.DateOfBirth),
        //        PhoneNumber = student.PhoneNumber
        //    };
        //    var result = await _userServiceBal.AddUser(appUser, ["Student"]);
        //    student.LoginIdGuid = appUser.Id;
        //    return result;
        //}
        //private static Student GetNewStudentObjFromApplication(StudentApplication app)
        //{
        //    return new Student()
        //    {
        //        ApplicationId = app.UniqueId,
        //        Email = app.EmailId,
        //        FirstName = app.FirstName,
        //        LastName = app.LastName,
        //        CreatedDate = DateTime.Now,
        //        PhoneNumber = app.MobileNumber
        //    };
        //}
        //private async Task<bool> AddStudentToBatch(Student student, Batch batch, StudentApplication stuApp, int bsUniqueId = 0)
        //{
        //    if (bsUniqueId > 0)
        //    {
        //        var bs = await _dbContext.BatchStudents.FindAsync(bsUniqueId);
        //        if (bs != null)
        //        {
        //            bs.StudentId = student.UniqueId;
        //            bs.BatchId = batch.UniqueId;
        //            bs.LastUpdatedDate = DateTime.Now;
        //        }
        //    }
        //    else if (student.UniqueId > 0 && batch.UniqueId > 0)
        //    {
        //        var ifExist = await _dbContext.BatchStudents.Where(
        //            b => b.BatchId == batch.UniqueId &&
        //            student.UniqueId == b.StudentId).AnyAsync();

        //        if (!ifExist)
        //            await _dbContext.BatchStudents.AddAsync(new BatchStudent()
        //            {
        //                BatchId = batch.UniqueId,
        //                StudentId = student.UniqueId,
        //                CreatedDate = DateTime.Now,
        //                LastUpdatedDate = DateTime.Now,
        //                DiscountedFeeAmount = 0,
        //            });
        //    }
        //    await _dbContext.SaveChangesAsync();
        //    stuApp.StatusId = 3;
        //    await _dbContext.SaveChangesAsync();
        //    return true;
        //}
    }

}
