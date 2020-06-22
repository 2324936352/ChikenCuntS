using LPFW.EntitiyModels.MusicUIEntity;
using System.Collections.Generic;

namespace LPFW.EntitiyModels.MusicUIEntity
{
    public interface IStudentRepository
    {
        
        /// <summary>
        /// 通过 Id 来获取学生信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MusicCore GetStudent(int id);
        /// <summary>
        /// 获取所有的学生信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<MusicCore> GetAllStudents();
        /// <summary>
        /// 添加一名新的学生信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        MusicCore Add(MusicCore student);
        /// <summary>
        /// 更新一名学生的信息
        /// </summary>
        /// <param name="updateStudent"></param>
        /// <returns></returns>
        MusicCore Update(MusicCore updateStudent);
        /// <summary>
        /// 删除一名学生的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MusicCore Delete(int id);



    }
}
