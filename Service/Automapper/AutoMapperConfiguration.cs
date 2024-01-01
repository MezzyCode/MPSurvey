using AutoMapper;
using Database.JsonModels.Setting;
using Model.JsonModels.Master;
using Model.JsonModels.Setting;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Automapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, JsonUser>();
            CreateMap<JsonUser, User>();

            CreateMap<Answer, JsonAnswer>(MemberList.None);
            CreateMap<JsonAnswer, Answer>(MemberList.None);

            CreateMap<Answer2, JsonAnswer2>(MemberList.None);
            CreateMap<JsonAnswer2, Answer2>(MemberList.None);

            CreateMap<HelperTable, JsonHelperTable>(MemberList.None);
            CreateMap<JsonHelperTable, HelperTable>(MemberList.None);
        }
    }
}
