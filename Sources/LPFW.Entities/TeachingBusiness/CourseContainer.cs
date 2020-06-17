using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPFW.EntitiyModels.TeachingBusiness
{
    public class CourseContainer:Entity
    {
        public CourseContainer() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}
