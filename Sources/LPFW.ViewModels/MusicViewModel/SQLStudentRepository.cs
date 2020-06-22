using LPFW.EntitiyModels.MusicUIEntity;
using LPFW.ORM;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.ViewModels.MusicViewModel
{
    public  class SQLStudentRepository: IStudentRepository
    {
        private readonly ILogger logger;
        private readonly LpDbContext context;

        public SQLStudentRepository(LpDbContext context, ILogger<SQLStudentRepository> logger)
        {
            this.logger = logger;
            this.context = context;
        }


        public MusicCore Add(MusicCore student)
        {
            context.MusicCores.Add(student);
            context.SaveChanges();
            return student;
        }

        public MusicCore Delete(int id)
        {
            MusicCore student = context.MusicCores.Find(id);
            if (student != null)
            {
                context.MusicCores.Remove(student);
                context.SaveChanges();
            }
            return student;

        }

        public IEnumerable<MusicCore> GetAllStudents()
        {
            logger.LogTrace("学生信息 Trace(跟踪) Log");
            logger.LogDebug("学生信息 Debug(调试) Log");
            logger.LogInformation("学生信息 信息(Information) Log");
            logger.LogWarning("学生信息 警告(Warning) Log");
            logger.LogError("学生信息 错误(Error) Log");
            logger.LogCritical("学生信息 严重(Critical) Log");


            return context.MusicCores;
        }

        public MusicCore GetStudent(int id)
        {
            return context.MusicCores.Find(id);
        }

        public MusicCore Update(MusicCore updateStudent)
        {

            var student = context.MusicCores.Attach(updateStudent);

            student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            context.SaveChanges();

            return updateStudent;

        }
    }
}
